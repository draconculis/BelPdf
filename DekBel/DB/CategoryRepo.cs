﻿using Dek.Bel.Categories;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.DB
{
    [Export(typeof(CategoryRepo))]
    public class CategoryRepo
    {
        [Import] IDBService dBService { get; set; }

        private string TableName => dBService.TableCategoryName;
        private const string ColCode = "Code";
        private const string ColName = "Name";
        private const string ColDescription = "Description";

        public void AddOrUpdateCategory(Category cat)
        {
            bool isUpdate = dBService.ValueExists(TableName, ColCode, cat.Code);

            if(string.IsNullOrWhiteSpace(cat.Name))
                throw new Exception("Name cannot be empty");

            if (isUpdate)
            {
                dBService.Update(TableName,
                    $"{ColCode} = {cat.Code}",
                    $"{ColName} = {cat.Name}, {ColDescription} = {cat.Description}");
            }
            else
            {
                if (dBService.ValueExists(TableName, "Name", cat.Name))
                    throw new Exception("Name must be unique");

                dBService.Insert(TableName, 
                    $"{ColCode},{ColName},{ColDescription}", 
                    $"'{cat.Code}','{cat.Name}','{cat.Description}'");
            }
        }
    }
}
