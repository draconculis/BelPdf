using Dek.Cls;

namespace Dek.DB
{
    public interface IModel
    {
    }

    public interface IModelWithId : IModel
    {
        Id Id { get; set; }
    }
}
