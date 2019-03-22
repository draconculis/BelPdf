﻿using Dek.Bel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.ModelExtensions
{
    public static class ReferenceExtension
    {
        public static string StartString(this Reference me)
        {
            return $"{me} / {me}";
        }
    }
}
