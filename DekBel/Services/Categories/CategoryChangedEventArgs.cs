using System;
using Dek.Bel.Core.Models;

namespace Dek.Bel.Services.Categories
{
    public class CategoryChangedEventArgs : EventArgs
    {
        public CategoryChangedEventArgs(CitationCategory citationCategory)
        {
            CitationCategory = citationCategory;
        }

        public CitationCategory CitationCategory { get; set; }
    }
}