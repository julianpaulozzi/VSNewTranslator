using System.Collections.Concurrent;

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
    }
}