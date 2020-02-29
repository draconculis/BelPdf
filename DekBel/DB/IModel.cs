using Dek.Bel.Cls;

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
