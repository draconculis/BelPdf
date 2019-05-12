using Dek.Bel.DB;
using Dek.Bel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel.Services
{
    [Export(typeof(ICategoryService))]
    public class CategoryService : ICategoryService
    {
        public IEnumerable<Category> Categories => m_DBService.Select<Category>();
        private IDBService m_DBService;

        private BorderStyle m_DefaultBorderStyle;

        [ImportingConstructor]
        CategoryService(IDBService dBService)
        {
            m_DefaultBorderStyle = new Label().BorderStyle;

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

        public List<Category> LoadCategoriesFromDb()
        {
            return m_DBService.Select<Category>();
        }

        public void Add(string name, string code, string desc) => Add(new Category { Name = name, Code = code, Description = desc });

        /// <summary>
        /// Add a new category. If Id not provided, generate new. Returns cat (with new Id).
        /// </summary>
        /// <param name="cat"></param>
        /// <exception cref="ArgumentException">Throws arg exception if code not unique</exception>
        public Category Add(Category cat)
        {
            if (cat.Id == Id.Null)
                cat.Id = Id.NewId();

            if (Categories.Any(c => c.Code.Equals(cat.Code, StringComparison.CurrentCultureIgnoreCase)))
                throw new ArgumentException($"Code {cat.Code} not unique.");

            m_DBService.InsertOrUpdate(cat);

            FireCategoryUpdated(cat);

            return cat;
        }


        public void Remove(Category cat)
        {
            List<CitationCategory> referencedCitations = m_DBService.Select<CitationCategory>($"`CategoryId`='{cat.Id}'");
            if (referencedCitations.Any())
            {
                List<Id> ids = referencedCitations.Select(x => x.CitationId).ToList();
                string idString = string.Join(";", ids.Select(x => x.ToString()).ToArray());
                throw new Exception("The following citations refgerence this category: " + idString);
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
            var cgs = GetCitationCategories(citationCategory.CitationId);
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
        }

        public void SetWeight(Id citationId, Id categoryId, int weight)
        {
            var cg = GetCitationCategories(citationId).SingleOrDefault(x => x.CitationId == citationId && x.CategoryId == categoryId);
            cg.Weight = weight;

            m_DBService.InsertOrUpdate(cg);
        }

        public List<CitationCategory> GetCitationCategories(Id citationId)
        {
            List<CitationCategory> cgs = m_DBService.Select<CitationCategory>($"`CitationId` = '{citationId}'");

            return cgs;
        }

        /**************************************************************
          Labels 
        */

        Color labelColor = Color.Moccasin;
        Color labelColorMouseOver = Color.Orange;
        public Label CreateCategoryLabelControl(CitationCategory citCat, Category cat, ContextMenuStrip menu, ToolTip toolTip)
        {
            Label l = new Label();
            Font newFont = new Font("Times New Roman", 10, FontStyle.Regular);
            l.Font = newFont;
            l.MouseHover += L_MouseHover;
            l.MouseEnter += L_MouseEnter;
            l.MouseLeave += L_MouseLeave;
            l.AutoSize = true;
            l.BackColor = labelColor;
            l.Text = $"{cat.Code} [{citCat.Weight}]";
            l.ContextMenuStrip = menu;
            if (citCat.IsMain)
                SetMainStyleOnLabel(l);
            l.Tag = citCat;
            toolTip.SetToolTip(l, cat.Name);
            return l;
        }

        private void L_MouseLeave(object sender, EventArgs e)
        {
            if (!(sender is Label l))
                return;

            l.BackColor = labelColor;
        }

        private void L_MouseEnter(object sender, EventArgs e)
        {
            if (!(sender is Label l))
                return;

            l.BackColor = labelColorMouseOver;
        }

        private void L_MouseHover(object sender, EventArgs e)
        {
            
        }

        public void ClearMainStyleFromLabels(IEnumerable<Label> labels)
        {
            foreach(Label label in labels)
            {
                ClearMainStyleOnLabel(label);
            }
        }

        public void SetMainStyleOnLabel(Label label)
        {
            label.BorderStyle = BorderStyle.FixedSingle;
        }

        public void ClearMainStyleOnLabel(Label label)
        {
            label.BorderStyle = m_DefaultBorderStyle;
        }
    }
}
