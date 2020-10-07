using Dek.Cls;
using Dek.DB;
using Dek.Bel.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;

namespace Dek.Bel.Core.Services
{
    [Export(typeof(ICategoryService))]
    public class CategoryService : ICategoryService
    {
        public IEnumerable<Category> Categories => m_DBService.Select<Category>();
        private IDBService m_DBService;

        [ImportingConstructor]
        CategoryService(IDBService dBService)
        {
            m_DBService = dBService;
        }

        private EventHandler<CategoryEventArgs> CategoryUpdatedEventHandler;
        public event EventHandler<CategoryEventArgs> CategoryUpdated
        {
            add { CategoryUpdatedEventHandler += value; }
            remove { CategoryUpdatedEventHandler -= value; }
        }

        private void FireCategoryUpdated(Category cat = null)
        {
            CategoryUpdatedEventHandler?.Invoke(this, new CategoryEventArgs {Category = cat});
        }

        /**************************************************************
          Categories
        */

        public Category InsertOrUpdate(string code, string name, string desc) => InsertOrUpdate(new Category { Name = name, Code = code, Description = desc });

        /// <summary>
        /// Add a new category. If Id not provided, generate new. Returns cat (with new Id).
        /// </summary>
        /// <param name="cat"></param>
        /// <exception cref="ArgumentException">Throws arg exception if code not unique</exception>
        public Category InsertOrUpdate(Category cat)
        {
            Category existingCat = Categories.FirstOrDefault(x => x.Code.ToLower() == cat.Code.ToLower());

            if (cat.Id == Id.Null)
                cat.Id =
                    (existingCat?.Id != null)
                    ? cat.Id = existingCat.Id
                    : cat.Id = Id.NewId();

            m_DBService.InsertOrUpdate(cat);

            FireCategoryUpdated(cat);

            return cat;
        }

        /// <summary>
        /// Add a new category. Returns null if category with code already exists.
        /// </summary>
        /// <param name="cat"></param>
        /// <exception cref="ArgumentException">Throws arg exception if code not unique</exception>
        public Category CreateNewCategory(string code, string name, string description = null)
        {
            Category existingCat = Categories.FirstOrDefault(x => x.Code == code);
            if (existingCat != null)
                return null;

            Category cat = new Category
            {
                Id = Id.NewId(),
                Code = code,
                Name = name,
                Description = description,
            };
            
            m_DBService.InsertOrUpdate(cat);

            FireCategoryUpdated(cat);

            return cat;
        }


        /// <summary>
        /// Delete a category. 
        /// If force and in use, then remove all references to this category, otherwise throw.
        /// </summary>
        public void Remove(Category cat, bool force = false)
        {
            if (cat.Id.IsNull)
                throw new Exception($"The id of category {cat.Code} is zero. This nil-category cannot be removed.");

            List<CitationCategory> referencedCitations = CitationCategoriesByCategory(cat.Id);
            if (referencedCitations.Any())
            {
                if (force) 
                { 
                    foreach(CitationCategory citCat in referencedCitations)
                        m_DBService.Delete(citCat);
                }
                else
                {
                    IEnumerable<Id> ids = referencedCitations.Select(x => x.CitationId).OrderBy(x => x).ToList();
                    string idString = string.Join($"{Environment.NewLine}", ids.Select(x => x.ToString()).ToArray());
                    throw new Exception($"The following {referencedCitations.Count} citations reference this category: " + idString);
                }
            }

            m_DBService.Delete(cat);
            FireCategoryUpdated(cat);
        }

        public Category this[string code]
        {
            get => Categories.FirstOrDefault(c => c.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase));
        }

        public void AddCategoryToCitation(Id citationId, Id categoryId, int weight, bool isMain)
        {   
            CitationCategory cg = new CitationCategory
            {
                CategoryId = categoryId,
                CitationId = citationId,
                Weight = weight,
                IsMain = isMain,
            };

            m_DBService.InsertOrUpdate(cg);

            if(categoryId != Id.Null)
                RemoveUncategorizedForCitation(citationId);
        }

        //public void SetMainCategory(Id citationId, Id categoryId)
        //{
        //    var cgs = GetCitationCategories(citationId);

        //    // There can be only one
        //    foreach (var cg in cgs)
        //    {
        //        cg.IsMain = false;
        //        m_DBService.InsertOrUpdate(cg);
        //    }

        //    CitationCategory maincg = m_DBService.Select<CitationCategory>($"`CitationId` = '{citationId}' AND `CategoryId` = '{cat.Id}'").FirstOrDefault();
        //    if (maincg == null)
        //        maincg = new CitationCategory
        //        {
        //            CategoryId = categoryId,
        //            CitationId = citationId,
        //            Weight = 3,
        //        };

        //    maincg.IsMain = true;

        //    m_DBService.InsertOrUpdate(maincg);
        //}

        public void SetMainCategory(CitationCategory citationCategory)
        {
            var cgs = CitationCategoriesByCitation(citationCategory.CitationId);
            if (cgs == null || cgs.Count < 1)
                return;

            // There can be only one
            foreach (var cg in cgs)
            {
                cg.IsMain = false;
                m_DBService.InsertOrUpdate(cg);
            }

            citationCategory.IsMain = true;
            m_DBService.InsertOrUpdate(citationCategory);

            // We have now set a main category. If this category != Uncategorized, remove refs to Uncategorized
            RemoveUncategorizedForCitation(citationCategory.CitationId);
        }

        /// <summary>
        /// Please note - will return uncategorized if not found, never null
        /// </summary>
        public Category GetMainCategory(string citationId) => GetMainCategory(new Id(citationId));
        public Category GetMainCategory(Id citationId)
        {
            CitationCategory mainCitCat = GetMainCitationCategory(citationId);
            return Categories.Single(x => x.Id == mainCitCat.CategoryId);
        }

        /// <summary>
        /// Please note - will return uncategorized if not found, never null
        /// </summary>
        public CitationCategory GetMainCitationCategory(string citationId) => GetMainCitationCategory(new Id(citationId));
        public CitationCategory GetMainCitationCategory(Id citationId)
        {
            var cgs = CitationCategoriesByCitation(citationId);
            var mainCitCat = cgs.SingleOrDefault(x => x.IsMain);
            if (mainCitCat == null)
            {
                mainCitCat = new CitationCategory
                {
                    CategoryId = Id.Empty,
                    IsMain = true,
                    CitationId = citationId,
                    Weight = 0,
                };
                m_DBService.InsertOrUpdate(mainCitCat);
            }
            return mainCitCat;
        }

        public void SetWeight(Id citationId, Id categoryId, int weight)
        {
            var cg = CitationCategoriesByCitation(citationId).SingleOrDefault(x => x.CitationId == citationId && x.CategoryId == categoryId);
            cg.Weight = weight;

            m_DBService.InsertOrUpdate(cg);
        }

        public List<CitationCategory> CitationCategoriesByCitation(Id citationId) =>
            m_DBService.Select<CitationCategory>($"`CitationId` = '{citationId}'");
        
        public List<CitationCategory> CitationCategoriesByCategory(Id categoryId) =>
            m_DBService.Select<CitationCategory>($"`CategoryId`='{categoryId}'");

        private void RemoveUncategorizedForCitation(Id citationId)
        {
            if (CitationCategoriesByCitation(citationId).Any(x => x.CategoryId == Id.Null))
                m_DBService.Delete<CitationCategory>($"`CitationId`='{citationId}' and `CategoryId`='{Id.Null}'");
        }

        public bool CitationHasNullCategory(Id citationId)
        {
            return CitationCategoriesByCitation(citationId).Any(x => x.CitationId == Id.Null);
        }

        public bool CitationHasMainCategory(Id citationId)
        {
            return CitationCategoriesByCitation(citationId).Any(x => x.IsMain);
        }
    }
}
