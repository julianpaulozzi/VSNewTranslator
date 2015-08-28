using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewTranslator.Core.Translation
{
	public abstract class BaseTranslator
	{
	    public virtual async Task<TranslationResult> GetTranslationAsync(string text, string sourceLang, string destinationLang,
	        bool forceRemote = false)
	    {

            var cacheKey = new TranslationCacheKey(Name, sourceLang, destinationLang, text);
            TranslationResult result;
            if (!forceRemote && EnableCache && TranslationCache.TryGetValue(cacheKey, out result))
            {
                if (result != null)
                {
                    result.OriginalText = text;
                    result.FromCache = true;
                    return result;
                }
            }

	        result = await GetRemoteTranslationAsync(text, sourceLang, destinationLang);

            if (EnableCache)
                TranslationCache.AddOrUpdate(cacheKey, result);

            return result;
	    }
        
        public virtual List<TranslationLanguage> SourceLanguages { get; protected set; }
        public virtual List<TranslationLanguage> TargetLanguages { get; protected set; }

		public abstract string AccessibleName { get; }
		public abstract string Name { get; }

        public abstract bool NeedCredentials { get; }

        public bool EnableCache { get; set; }

        protected abstract Task<TranslationResult> GetRemoteTranslationAsync(string text, string sourceLang, string destinationLang);

        public bool CanSwap(string sourceLang, string targetLang)
		{
			return TargetLanguages.Find(l => l.Code == sourceLang) != null
				&& SourceLanguages.Find(l => l.Code == targetLang) != null;
		}
	}
}
