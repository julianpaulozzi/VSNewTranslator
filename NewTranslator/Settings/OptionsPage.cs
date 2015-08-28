using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using NewTranslator.Core.Translation;

namespace NewTranslator.Settings
{
	[Guid(GuidList.OptionsPageGuid)]
	[ComVisible(true)]
	public partial class OptionsPage : DialogPage//, System.Windows.Forms.IWin32Window
	{
		private OptionsPageControl _optionsControl;

		public OptionsPage()
		{
		    // InitializeComponent();
		    TranslationService = TranslatorRegistry.AllTranslationServicesToken; //"Google";
            SourceLanguage = "";
			TargetLanguage = "en";
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public string TranslationService { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public string SourceLanguage { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public string TargetLanguage { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string BingClientId { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string BingClientSecret { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected override IWin32Window Window
		{
			get
			{
			    _optionsControl = new OptionsPageControl
			    {
			        Location = new Point(0, 0),
			        OptionsPage = this
			    };
			    return _optionsControl;
			}
		}

		public override void SaveSettingsToStorage()
		{
			if (_optionsControl != null)
			{
				_optionsControl.UpdateOptions();
				Global.Options.Translators = TranslatorRegistry.GetTranslators(TranslationService);
				Global.Options.SourceLanguage = SourceLanguage;
				Global.Options.TargetLanguage = TargetLanguage;
			    Global.Options.BingClientCredential = new ClientCredential
			    {
                    ClientId = BingClientId,
			        ClientSecret = BingClientSecret
			    };
				base.SaveSettingsToStorage();
			}
		}
	}
}
