using Dek.Cls;
using Dek.DB;
using System;

namespace Dek.Bel.Core.Models
{
    public class Author : IModelWithId
    {
        public Id Id { get; set; }
        public string Name { get; set; }
        public DateTime Born { get; set; }
        public DateTime Dead { get; set; }
        public string Notes { get; set; }

    }
}
