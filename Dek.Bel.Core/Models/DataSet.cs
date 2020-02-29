using System.ComponentModel;

namespace Dek.Bel.Core.Models
{


    public class DataSet<T>
    {
        public BindingList<T> Data { get; set; }

        public T this[int idx] => Data[idx];

    }
}
