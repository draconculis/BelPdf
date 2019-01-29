using Dek.Bel.Cls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.DB
{
    public interface IModel
    {
    }

    public interface IModelWithId : IModel
    {
        Id Id { get; set; }
    }
}
