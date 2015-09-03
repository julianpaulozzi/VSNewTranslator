using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NewTranslator.Core.Translation
{
    public class GoogleTranslator : BaseTranslator
    {
        private List<TranslationLanguage> _sourceLanguages;
        private List<TranslationLanguage> _targetLanguages;

        public GoogleTranslator()
        {
            EnableCache = true;
        }

        public override string Name
        {
            get { return Constants.TranslatorNames.Google; }
        }

        public override string AccessibleName
        {
            get { return "Google Translate"; }
        }

        public override bool NeedCredentials { get { return true; } }

        public override List<TranslationLanguage> SourceLanguages
        {
            get
            {
                if (_sourceLanguages == null)
                {
                    _sourceLanguages = new TranslationLanguageCollection { { "", "Auto-detect" } };
                    _sourceLanguages.AddRange(TargetLanguages);
                }
                return _sourceLanguages;
            }
            protected set { _sourceLanguages = value; }
        }

        public override List<TranslationLanguage> TargetLanguages
        {
            get
            {
                if (_targetLanguages == null)
                {
                    _targetLanguages = Constants.GoogleTranslator.AvailableLanguages.Copy();
                }
                return _targetLanguages;
            }
            protected set { _targetLanguages = value; }
        }

        protected override async Task<TranslationResult> GetRemoteTranslationAsync(string text, string sourceLang, string destinationLang)
        {
            /* {
					"sentences":[{"trans":"текст","orig":"text","translit":"tekst","src_translit":""}],
					"dict":
					[
							{"pos":"существительное","terms":["текст","текстовый файл","оригинал","руководство","тема","подлинный текст","цитата из библии"]},
							{"pos":"глагол","terms":["пис `ать крупным почерком"]}
					],
					"src":"en",
					"server_time":12
				}
			* */
            
            var data = Utils.CreateQuerystring(new Dictionary<string, string>()
            {
                {"client", "f"},
                {"otf", "1"},
                {"pc", "0"},
                {"sl", sourceLang},
                {"tl", destinationLang},
                {"hl", destinationLang},
                {"text", text}
            });

            string response = await Utils.GetHttpResponseAsync(Constants.GoogleTranslator.BaseUrl, data);
            JObject json = JObject.Parse(response);
            var result = ParseResponse(json);
            result.DestinationLanguage = destinationLang;
            result.RequestSourceLanguage = sourceLang;
            result.OriginalText = text;
            result.TranslationSource = Name;
            
            return result;
        }

        private TranslationResult ParseResponse(JObject json)
        {
            TranslationResult res = new TranslationResult();

            JToken sentences = json["sentences"];
            List<string> sentenceTexts = new List<string>();
            foreach (JToken sent in sentences.Children())
            {
                sentenceTexts.Add(sent.Value<string>("trans"));
            }
            string fullText = string.Join(" ", sentenceTexts);
            if (!string.IsNullOrWhiteSpace(fullText))
            {
                res.Sentences.Add(fullText);
            }

            JToken dictionary = json["dict"];
            if (dictionary != null)
            {
                foreach (JToken dToken in dictionary.Children())
                {
                    DictionaryItem d = new DictionaryItem();
                    d.Title = dToken.Value<string>("pos");
                    JToken terms = dToken["terms"];
                    foreach (JToken term in terms.Children())
                    {
                        d.Terms.Add(term.Value<string>());
                    }
                    res.DictionaryItems.Add(d);
                }
            }

            res.SourceLanguage = json.Value<string>("src");
            return res;
        }
    }
}