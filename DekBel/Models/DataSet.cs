using Dek.Bel.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{


    public class DataSet<T>
    {
        public BindingList<T> Data { get; set; }

        public T this[int idx] => Data[idx];

    }
}
