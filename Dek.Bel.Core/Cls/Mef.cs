using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Dek.Cls
{
    public class Mef
    {
        public static void Initialize(object target) => InitializeInternal(target);
        public static void Initialize(object target, List<object> classes) => InitializeInternal(target, classes);
        public static void Initialize(object target, List<Type> types) => InitializeInternal(target, types);

        private static void InitializeInternal(object target, IEnumerable<object> classes)
        {
            var catalog = new AggregateCatalog();

            foreach (object obj in classes)
                catalog.Catalogs.Add(new AssemblyCatalog(obj.GetType().Assembly));

            var container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(target);
            }
            catch (CompositionException compositionException)
            {
                throw compositionException;
            }

        }

        private static void InitializeInternal(object target, IEnumerable<Type> types)
        {
            var catalog = new AggregateCatalog();

            foreach (Type t in types)
                catalog.Catalogs.Add(new AssemblyCatalog(t.Assembly));

            var container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(target);
            }
            catch (CompositionException compositionException)
            {
                throw compositionException;
            }

        }

        private static void InitializeInternal(object target)
        {
            InitializeInternal(target, new List<object> { target });
        }
    }
}
