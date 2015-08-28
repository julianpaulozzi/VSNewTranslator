using System.Collections.Generic;
using NewTranslator.Core;
using NewTranslator.Core.Translation;

namespace NewTranslator.Settings
{
    internal class Options
    {
        public IEnumerable<BaseTranslator> Translators { get; set; }

        public string SourceLanguage { get; set; }

        public string TargetLanguage { get; set; }
        
        public ClientCredential BingClientCredential { get; set; }

        private Options()
        {
        }
        
        public static Options Get()
        {
            var customOptions = Global.DTE.Properties["NewTranslator", "Options"];
            
            ClientCredentialRepository.Current
                .TryAddOrUpdateCredential(Constants.TranslatorNames.Bing,
                    new ClientCredential
                    {
                        ClientId = Constants.BingTranslator.TokenClientID,
                        ClientSecret = Constants.BingTranslator.TokenClientSecret
                    });

            var res = new Options
            {
                Translators = TranslatorRegistry.GetTranslators((string) customOptions.Item("TranslationService").Value),
                SourceLanguage = (string) customOptions.Item("SourceLanguage").Value,
                TargetLanguage = (string) customOptions.Item("TargetLanguage").Value,
                BingClientCredential = new ClientCredential
                {
                    ClientId = (string)customOptions.Item("BingClientId").Value,
                    ClientSecret = (string)customOptions.Item("BingClientSecret").Value
                }
            };
            return res;
        }
    }
}