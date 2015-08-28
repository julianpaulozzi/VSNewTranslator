using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NewTranslator.Core.Translation;

namespace NewTranslator.Settings
{
    public partial class OptionsPageControl : UserControl
	{
		private OptionsPage _optionsPage;
        private IEnumerable<TranslatorOptionLookup> _optionLookups;
        private List<TranslationLanguage> _sourceLanguages;
        private List<TranslationLanguage> _targetLanguages;

        public OptionsPageControl()
        {
            InitializeComponent();

            microsofttranslatorLinkLabel.Text = "See how to.";
            microsofttranslatorLinkLabel.Links.Add(3, 8, "http://blogs.msdn.com/b/translation/p/gettingstarted1.aspx");
            microsofttranslatorLinkLabel.LinkClicked += (sender, args) =>
            {
                System.Diagnostics.Process.Start(args.Link.LinkData.ToString());
            };
        }

        /// <summary>
		/// Gets or Sets the reference to the underlying OptionsPage object.
		/// </summary>
		public OptionsPage OptionsPage
		{
			get
			{
				return _optionsPage;
			}
			set
			{
				_optionsPage = value;
				InitView();
			}
		}

		private TranslatorOptionLookup SelectedOptionLookup
		{
			get { return (TranslatorOptionLookup)cmbService.SelectedItem; }
		}

        private IEnumerable<TranslatorOptionLookup> OptionLookups
        {
            get
            {
                if (_optionLookups == null)
                {
                    var optionLookups = new List<TranslatorOptionLookup>
                    {
                        new TranslatorOptionLookup(TranslatorRegistry.Translators)
                    };
                    var translatorOptionLookups = TranslatorRegistry.Translators
                        .Select(baseTranslator => new TranslatorOptionLookup(baseTranslator));
                    optionLookups.AddRange(translatorOptionLookups);
                    _optionLookups = optionLookups;
                }
                return _optionLookups;
            }
        }
        
		private void InitView()
		{
            txtBingClientId.Text = OptionsPage.BingClientId;
            txtBingClientSecret.Text = OptionsPage.BingClientSecret;

            foreach (var option in OptionLookups)
		    {
                cmbService.Items.Add(option);
            }

		    var selectedOptionLookup = OptionLookups.FirstOrDefault(p=> p.Name.Equals(OptionsPage.TranslationService));
		    cmbService.SelectedItem = selectedOptionLookup;
            
			FillLanguages();
		}

		private void FillLanguages()
		{
		    var name = SelectedOptionLookup.Name;
		    var baseTranslators = TranslatorRegistry.GetTranslators(name).ToList();
            if (baseTranslators.Count() == 1)
		    {
                var firstTranslator = baseTranslators[0];
		        _sourceLanguages = firstTranslator.SourceLanguages;
		        _targetLanguages = firstTranslator.TargetLanguages;
                FillLanguageList(cmbSourceLanguage, _sourceLanguages, OptionsPage.SourceLanguage);
                FillLanguageList(cmbTargetLanguage, _targetLanguages, OptionsPage.TargetLanguage);
                return;
            }
            
		    _sourceLanguages = baseTranslators.Select(p=> p.SourceLanguages).Aggregate((xs, ys) => xs.Intersect(ys).ToList());
            _targetLanguages = baseTranslators.Select(p=> p.TargetLanguages).Aggregate((xs, ys) => xs.Intersect(ys).ToList());

            FillLanguageList(cmbSourceLanguage, _sourceLanguages, OptionsPage.SourceLanguage);
            FillLanguageList(cmbTargetLanguage, _targetLanguages, OptionsPage.TargetLanguage);
        }

		private void FillLanguageList(ComboBox cb, List<TranslationLanguage> langs, string selectedCode)
		{
			cb.Items.Clear();
            foreach (TranslationLanguage l in langs) {
                cb.Items.Add(l);
            }
            if (langs.Count != 0) {
                cb.SelectedItem = langs.Find(l => l.Code == selectedCode) ?? langs[0];
            }
			UpdateSwap();
		}

		private void UpdateSwap()
		{
			var source = (TranslationLanguage)cmbSourceLanguage.SelectedItem;
			var target = (TranslationLanguage)cmbTargetLanguage.SelectedItem;
		    var canSwap = _sourceLanguages.Contains(target) && _targetLanguages.Contains(source);

            cmdSwap.Enabled = source != null && target != null && canSwap;
        }

		public void UpdateOptions()
		{
			OptionsPage.TranslationService = SelectedOptionLookup.Name;
			OptionsPage.SourceLanguage = (cmbSourceLanguage.SelectedItem as TranslationLanguage).Code;
			OptionsPage.TargetLanguage = (cmbTargetLanguage.SelectedItem as TranslationLanguage).Code;
		    OptionsPage.BingClientId = txtBingClientId.Text;
		    OptionsPage.BingClientSecret = txtBingClientSecret.Text;
        }

		//events
		private void cmbService_SelectedIndexChanged(object sender, EventArgs e)
		{
			FillLanguages();
		}	

		private void cmdSwap_Click(object sender, EventArgs e)
		{
			var source = (TranslationLanguage)cmbSourceLanguage.SelectedItem;
			var target = (TranslationLanguage)cmbTargetLanguage.SelectedItem;

		    cmbSourceLanguage.SelectedItem = _sourceLanguages.FirstOrDefault(p => p == target);
		    cmbTargetLanguage.SelectedItem = _targetLanguages.FirstOrDefault(p => p == source);
		}

		private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateSwap();
		}

	}
}
