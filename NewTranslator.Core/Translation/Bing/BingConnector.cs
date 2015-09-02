using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewTranslator.Core.Translation.Bing
{
    public class BingConnector
    {
        private DateTime _tokenExpires;
        private string _currentToken;
        private ClientCredential _lastClientCredential;

        private ClientCredential ClientCredential
        {
            get
            {
                var clientCredential = ClientCredentialRepository.Current.GetCredential(Constants.TranslatorNames.Bing);
                if (clientCredential != _lastClientCredential)
                {
                    _tokenExpires = DateTime.Now;
                }
                _lastClientCredential = clientCredential;
                return _lastClientCredential;
            }
        }

        public string GetCurrentToken()
        {
            if ((_tokenExpires - DateTime.Now).TotalMinutes < 1) { 
                RenewToken();                
            }
            return _currentToken;
        }

        private void RenewToken()
        {
            if(ClientCredential == null || !ClientCredential.IsValid)
                return;

            var data = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", ClientCredential.ClientId },
                { "client_secret", ClientCredential.ClientSecret },
                { "scope", Constants.BingTranslator.ClientScope }                
            };

            string response = Utils.GetHttpResponse(Constants.BingTranslator.TokenUrl, Utils.CreateQuerystring(data));
            JObject jToken = JObject.Parse(response);
            _currentToken = jToken["access_token"].Value<string>();
            _tokenExpires = DateTime.Now.AddSeconds(int.Parse(jToken["expires_in"].Value<string>()));
        }

        private Task<string> GetDataAsync(string method, Dictionary<string, string> data = null)
        {
            if (ClientCredential == null || !ClientCredential.IsValid)
            {
                throw new InvalidOperationException("To use the Bing translator you must enter the credentials in 'Tools > Options... > Translator'");
            }

            data = data ?? new Dictionary<string, string>();
            var client = new WebClient { Headers = {["Authorization"] = "Bearer " + GetCurrentToken() } };
            var uri = new Uri(string.Format("{0}{1}?{2}", Constants.BingTranslator.BaseUrl, method, Utils.CreateQuerystring(data)));
            return client.DownloadStringTaskAsync(uri);
        }

        public virtual Task<string> GetLanguageNamesAsync(string locale, IEnumerable<string> codes)
        {            
            return GetDataAsync("GetLanguageNames", new Dictionary<string, string>{
                { "locale", "en" },
                { "languageCodes", JsonConvert.SerializeObject(codes) }
            });            
        }

        public virtual Task<string> GetLanguagesForTranslateAsync()
        {
            return GetDataAsync("GetLanguagesForTranslate");            
        }

        public virtual Task<string> GetTranslationsAsync(string text, string sourceLang, string destLang)
        {
            var cleanText = text.Trim();
            return GetDataAsync("GetTranslations", new Dictionary<string, string> {                
                {"text", cleanText},
				{"from", sourceLang},
				{"to", destLang},
				{"maxTranslations", "20"}
            });            
        }


        private string GetData(string method, Dictionary<string, string> data = null)
        {
            if (ClientCredential == null || !ClientCredential.IsValid)
                return null;

            data = data ?? new Dictionary<string, string>();
            WebClient client = new WebClient { Headers = {["Authorization"] = "Bearer " + GetCurrentToken() } };
            return client.DownloadString(Constants.BingTranslator.BaseUrl + method + "?" + Utils.CreateQuerystring(data));
        }

        public virtual string GetLanguageNames(string locale, IEnumerable<string> codes)
        {
            return GetData("GetLanguageNames", new Dictionary<string, string>{
                { "locale", "en" },
                { "languageCodes", JsonConvert.SerializeObject(codes) }
            });
        }

        public virtual string GetLanguagesForTranslate()
        {
            return GetData("GetLanguagesForTranslate");
        }
    }
}
