using System.Collections.Generic;
using System.Linq;
using NewTranslator.Core.Translation;

namespace NewTranslator.Settings
{
    public class TranslatorOptionLookup
    {
        public TranslatorOptionLookup(BaseTranslator translator)
        {
            Name = translator.Name;
            AccessibleName = translator.AccessibleName;
        }

        public TranslatorOptionLookup(IEnumerable<BaseTranslator> translators)
        {
            var baseTranslators = translators as IList<BaseTranslator> ?? translators.ToList();
            Name = baseTranslators.GetTranslationServicesToken();
            AccessibleName = string.Join(" and ", baseTranslators.Select(p=> p.AccessibleName));
        }

        public string Name { get; set; }
        public string AccessibleName { get; set; }
    }
}