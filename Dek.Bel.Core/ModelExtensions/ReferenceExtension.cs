using Dek.Bel.Core.Models;

namespace Dek.Bel.Core.ModelExtensions
{
    public static class ReferenceExtension
    {
        public static string StartString(this Reference me)
        {
            return $"{me} / {me}";
        }
    }
}
