using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace Dek.Cls
{
    public static class Mef
    {
        public static void Initialize(object target) => InitializeInternal(target);
        public static void Initialize(object target, List<object> classes) => InitializeInternal(target, classes);
        public static void Initialize(object target, List<Type> types) => InitializeInternal(target, types);

        static AggregateCatalog TheCatalog;
        static CompositionContainer TheContainer;

        public static void Compose(object target)
        {
            try
            {
                TheContainer.ComposeParts(target);
            }
            catch (CompositionException compositionException)
            {
                throw compositionException;
            }
        }

        private static void InitializeInternal(object target, IEnumerable<object> classes)
        {
            TheCatalog = new AggregateCatalog();

            foreach (object obj in classes)
                TheCatalog.Catalogs.Add(new AssemblyCatalog(obj.GetType().Assembly));

            TheContainer = new CompositionContainer(TheCatalog);

            try
            {
                TheContainer.ComposeParts(target);
            }
            catch (CompositionException compositionException)
            {
                throw compositionException;
            }

        }

        private static void InitializeInternal(object target, IEnumerable<Type> types)
        {
            TheCatalog = new AggregateCatalog();

            foreach (Type t in types)
                TheCatalog.Catalogs.Add(new AssemblyCatalog(t.Assembly));

            TheContainer = new CompositionContainer(TheCatalog);

            try
            {
                TheContainer.ComposeParts(target);
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

        public static IEnumerable<string> CatalogParts => TheCatalog.Catalogs.FirstOrDefault()?.Select(x => x.ToString());
    }
}
