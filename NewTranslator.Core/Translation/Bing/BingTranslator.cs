using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewTranslator.Core.Translation.Bing
{
	public class BingTranslator : BaseTranslator
	{
	    internal bool InitLanguagesFromApi = false;

        private readonly BingConnector _connector;
        private readonly object _syncRoot = new object();

        public BingTranslator(BingConnector connector)
        {
            EnableCache = true;
            _connector = connector;
		}	

		public override string Name
		{
			get { return Constants.TranslatorNames.Bing; }
		}
        
	    public override string AccessibleName
		{
			get { return "Bing Translator"; }
		}

        public override bool NeedCredentials { get { return true; } }

        private List<TranslationLanguage> _sourceLanguages;
        public override List<TranslationLanguage> SourceLanguages
        {
            get
            {
                if (_sourceLanguages == null) {
                    InitLanguages();                    
                }
                return _sourceLanguages;                
            }
            protected set { _sourceLanguages = value; }
        }

        private List<TranslationLanguage> _targetLanguages;
        public override List<TranslationLanguage> TargetLanguages
        {
            get
            {
                if (_targetLanguages == null) {
                    InitLanguages();
                }
                return _targetLanguages;
            }
            protected set { _targetLanguages = value; }
        }

        private void InitLanguages()
        {
            if (!InitLanguagesFromApi)
            {
                TargetLanguages = Constants.BingTranslator.AvailableLanguages.Copy();
                SourceLanguages = new TranslationLanguageCollection { { "", "Auto-detect" } };
                SourceLanguages.AddRange(TargetLanguages);
                return;
            }
            
            lock (_syncRoot)
            {
                if (_sourceLanguages == null || _targetLanguages == null)
                {
                    string[] codes = null;
                    string[] names = null;
                    try
                    {
                        codes = JsonConvert.DeserializeObject<string[]>(_connector.GetLanguagesForTranslate());
                        names = JsonConvert.DeserializeObject<string[]>(_connector.GetLanguageNames("en", codes));
                    }
                    catch
                    {
                        codes = new string[0];
                        names = new string[0];
                    }
                    var languages = new List<TranslationLanguage>(codes.Length);
                    for (int i = 0; i < codes.Length; i++)
                    {
                        languages.Add(new TranslationLanguage(codes[i], names[i]));
                    }
                    languages.Sort((a, b) => string.CompareOrdinal(a.Name, b.Name));
                    _targetLanguages = languages;
                    _sourceLanguages = new List<TranslationLanguage> { new TranslationLanguage("", "Auto-detect") };
                    _sourceLanguages.AddRange(_targetLanguages);
                }
            }
        }

	    protected override async Task<TranslationResult> GetRemoteTranslationAsync(string text, string sourceLang, string destinationLang)
		{
            var translations = await _connector.GetTranslationsAsync(text, sourceLang, destinationLang);
		    JObject json = JObject.Parse(translations);
            var result = ParseResponse(json);
            result.DestinationLanguage = destinationLang;
	        result.RequestSourceLanguage = sourceLang;
            result.OriginalText = text;
            result.TranslationSource = Name;
            
            return result;
		}
		
		private	TranslationResult ParseResponse(JObject json)
		{
            /* {
                 "From": "en",
                 "Translations": [
                   {
                     "Count": 0,
                     "MatchDegree": 100,
                     "MatchedOriginalText": "",
                     "Rating": 5,
                     "TranslatedText": "\u043f\u0435\u0440\u0435\u0439\u0442\u0438"
                   },
                   {
                     "Count": 0,
                     "MatchDegree": 100,
                     "MatchedOriginalText": "go",
                     "Rating": 0,
                     "TranslatedText": "\u0438\u0434\u0442\u0438"
                   }
                 ]
               }           
           * */
            
			TranslationResult res = new TranslationResult();            
            JToken jTranslations = json["Translations"];

            IEnumerable<string> translations = jTranslations
                .Select(t => new BingTranslation{
                    MatchDegree = t["MatchDegree"].Value<int>(),
                    Rating = t["Rating"].Value<int>(),
                    Text = t["TranslatedText"].Value<string>()
                })
                .OrderByDescending(t => t.MatchDegree)
                .ThenByDescending(t => t.Rating)
                .Select(t => t.Text);

            //filtering duplicates
            //LINQ Distinct is not guaranteed to preserve order
            HashSet<string> distinctValues = new HashSet<string>();
            foreach (string t in translations) {
                if (!distinctValues.Contains(t)) { 
                    res.Sentences.Add(t);
                    distinctValues.Add(t);
                }
            }
            
			res.SourceLanguage = json.Value<string>("From");
			return res;
		}		
	}
}
