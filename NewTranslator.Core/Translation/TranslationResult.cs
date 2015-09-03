using System;
using System.Collections.Generic;

namespace NewTranslator.Core.Translation
{
    public class TranslationResult
    {
        public TranslationResult()
        {
            CreatedAt = DateTime.UtcNow;
            Sentences = new List<string>();
            DictionaryItems = new List<DictionaryItem>();
        }

        public List<string> Sentences { get; set; }
        public List<DictionaryItem> DictionaryItems { get; set; }
        public string SourceLanguage { get; set; }
        public string DestinationLanguage { get; set; }

        public string OriginalText { get; protected internal set; }
        public string RequestSourceLanguage { get; protected internal set; }
        public string TranslationSource { get; protected internal set; }
        public DateTime CreatedAt { get; protected set; }
        public bool FromCache { get; protected internal set; }
    }
}