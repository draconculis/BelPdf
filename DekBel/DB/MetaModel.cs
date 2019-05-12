using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    public class MetaModel<T>
    {
        public T MainModel { get; set; }
        public List<object> Models {get;set;}

        MetaModel(T mainModel, List<object> models)
        {
            MainModel = mainModel;
            Models.AddRange(models);
        }

        public Type MainModelType()
        {
            return typeof(T);
        }

        public List<Type> ModelTypes()
        {
            return Models
                .Select(x => x.GetType())
                .ToList();
        }

    }


}
