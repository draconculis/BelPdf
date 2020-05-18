using Dek.Bel.Core.Cls;

namespace Dek.Bel.Core.Models
{
    public enum AuthorType
    {
        [EnumStringValue("Other")]
        Other,
        [EnumStringValue("Author")]
        Author,
        [EnumStringValue("Editor")]
        Editor,
        [EnumStringValue("Translator")]
        Translator,
        [EnumStringValue("Author and Editor")]
        AuthorAndEditor,
        [EnumStringValue("Author and Tranlator")]
        AuthorAndTranslator,
    }
}
