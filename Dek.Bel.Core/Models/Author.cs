using Dek.Cls;
using Dek.DB;
using System;

namespace Dek.Bel.Core.Models
{
    public class Author : IModelWithId
    {
        public Id Id { get; set; }
        public string Name { get; set; }
        public string Born { get; set; }
        public string Dead { get; set; }
        public string Notes { get; set; }

    }
}
