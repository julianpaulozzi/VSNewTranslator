using System;
using System.Collections.Concurrent;
using System.Linq;

namespace NewTranslator.Core.Translation
{
    internal sealed class TranslationCache
    {
        private static readonly ConcurrentDictionary<TranslationCacheKey, TranslationResult> _translationResultsCache =
            new ConcurrentDictionary<TranslationCacheKey, TranslationResult>();

        internal static TranslationResult AddOrUpdate(TranslationCacheKey key, TranslationResult result)
        {
            if (result == null)
                return null;

            return _translationResultsCache.AddOrUpdate(key, result, (cacheKey, translationResult) => result);
        }

        internal static bool TryGetValue(TranslationCacheKey key, out TranslationResult result)
        {
            return _translationResultsCache.TryGetValue(key, out result);
        }

        internal static bool AddUserEditedItem(string translationSourceName, string sourceLang, string destinationLang, string originalText, string editedText)
        {
            if(string.IsNullOrEmpty(translationSourceName) || string.IsNullOrEmpty(destinationLang) || string.IsNullOrEmpty(originalText) || string.IsNullOrEmpty(editedText))
                return false;

            var originalTextClean = originalText.Trim();
            var editedTextClean = editedText.Trim();

            if (originalTextClean.Equals(editedTextClean, StringComparison.Ordinal))
                return false;

            var translationCacheKey = new TranslationCacheKey(translationSourceName, sourceLang, destinationLang, originalText);
            TranslationResult translationResult;
            if (!TryGetValue(translationCacheKey, out translationResult))
            {
                translationResult = AddOrUpdate(translationCacheKey, new TranslationResult
                {
                    TranslationSource = translationSourceName,
                    SourceLanguage = sourceLang,
                    DestinationLanguage = destinationLang,
                    OriginalText = originalText,
                    FromCache = true
                });
            }

            var dictionaryItem = translationResult.DictionaryItems.FirstOrDefault(p => p.Title.Equals(Constants.TranslationCache.UserEditedItemHeader, StringComparison.OrdinalIgnoreCase));
            if (dictionaryItem == null)
            {
                dictionaryItem = new DictionaryItem
                {
                    Title = Constants.TranslationCache.UserEditedItemHeader
                };
                translationResult.DictionaryItems.Add(dictionaryItem);
            }
            
            var alreadyHasItem = dictionaryItem.Terms.Any(p=> p.Equals(editedTextClean, StringComparison.Ordinal));
            if (alreadyHasItem)
                return false;

            dictionaryItem.Terms.Add(editedTextClean);
            
            return true;
        }

        internal static bool RemoveUserEditedItem(string translationSourceName, string sourceLang, string destinationLang,
            string originalText, string editedText)
        {

            if (string.IsNullOrEmpty(translationSourceName) || string.IsNullOrEmpty(destinationLang) || string.IsNullOrEmpty(originalText) || string.IsNullOrEmpty(editedText))
                return false;

            var originalTextClean = originalText.Trim();
            var editedTextClean = editedText.Trim();

            if (originalTextClean.Equals(editedTextClean, StringComparison.Ordinal))
                return false;

            var translationCacheKey = new TranslationCacheKey(translationSourceName, sourceLang, destinationLang, originalText);
            TranslationResult translationResult;
            if (!TryGetValue(translationCacheKey, out translationResult))
            {
                return false;
            }

            var dictionaryItem = translationResult.DictionaryItems.FirstOrDefault(p => p.Title.Equals(Constants.TranslationCache.UserEditedItemHeader, StringComparison.OrdinalIgnoreCase));
            if (dictionaryItem == null)
            {
                return false;
            }

            var result = dictionaryItem.Terms.Remove(editedTextClean);

            if (!dictionaryItem.Terms.Any())
                translationResult.DictionaryItems.Remove(dictionaryItem);

            return result;
        }
    }
}