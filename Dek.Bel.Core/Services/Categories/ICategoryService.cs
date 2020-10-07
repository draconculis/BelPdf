using Dek.Cls;
using Dek.Bel.Core.Models;
using System;
using System.Collections.Generic;

namespace Dek.Bel.Core.Services
{
    public interface ICategoryService
    {
        // When set of categories change
        event EventHandler<CategoryEventArgs> CategoryUpdated;

        IEnumerable<Category> Categories { get; }

        // Citation 
        List<CitationCategory> CitationCategoriesByCitation(Id citationId);
        List<CitationCategory> CitationCategoriesByCategory(Id categoryId);
        bool CitationHasNullCategory(Id citationId);
        bool CitationHasMainCategory(Id citationId);

        Category CreateNewCategory(string code, string name, string description = null);
        Category InsertOrUpdate(Category cat);
        Category InsertOrUpdate(string code, string name, string desc);
        void Remove(Category cat, bool force = true);

        void AddCategoryToCitation(Id citationId, Id categoryId, int weight, bool isMain);
        void SetMainCategory(CitationCategory citationCategory);
        Category GetMainCategory(string citationId);
        Category GetMainCategory(Id citationId);
        CitationCategory GetMainCitationCategory(string citationId);
        CitationCategory GetMainCitationCategory(Id citationId);
        void SetWeight(Id citationId, Id categoryId, int weight);
    }
}