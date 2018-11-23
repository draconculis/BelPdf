﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Categories
{
    public class Category
    {
        public string Code { get; }
        public string Name { get; }
        public string Description { get; }

        private string FullName => $"{Code} - {Name}";

        public Category(string code, string name, string desc)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(code))
                throw new ArgumentException();

            Name = name;
            Code = code;
            Description = desc;
        }

        public override string ToString() => FullName;
    }
}
