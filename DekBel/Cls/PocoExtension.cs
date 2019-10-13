using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Cls
{
    public static class PocoExtension
    {
        public static void Load(this object me, object obj)
        {
            if (me == null)
                return;

            var otherProps = obj.GetType().GetProperties();

            foreach (var prop in me.GetType().GetProperties())
            {
                Type type = prop.GetType();
                if (type.IsArray)
                    continue;

                //object attributes = prop.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault();
                var otherProp = otherProps.SingleOrDefault(p => p.Name == prop.Name && p.PropertyType.FullName == prop.PropertyType.FullName);
                if (otherProp == null)
                    continue;
                try
                {
                    prop.SetValue(me, otherProp.GetValue(obj));
                }
                catch {}
            }


        }

    }
}
