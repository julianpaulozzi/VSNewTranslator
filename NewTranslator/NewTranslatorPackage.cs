using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using NewTranslator.Adornment;
using NewTranslator.Core.Translation;
using NewTranslator.Settings;
using Constants = NewTranslator.Core.Constants;

namespace NewTranslator
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    //options
    [ProvideOptionPage(typeof(OptionsPage), "NewTranslator", "Options", 110, 111, true)]
    [ProvideProfile(typeof(OptionsPage), "NewTranslator", "Options", 110, 111, true, DescriptionResourceID = 110)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.TranslatorPackageGuid)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    public sealed class NewTranslatorPackage : Microsoft.VisualStudio.Shell.Package
    {
        /// <summary>
        /// This read-only property returns the package instance
        /// </summary>
        internal static NewTranslatorPackage Instance { get; private set; }

        public NewTranslatorPackage()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            Instance = this;

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                CommandID translateCommandID = new CommandID(GuidList.guidTranslatorCmdSet, (int)PkgCmdIDList.Translate);
                MenuCommand menuItemTranslate = new MenuCommand(TranslateMenu_Clicked, translateCommandID);
                mcs.AddCommand(menuItemTranslate);
            }
        }

        protected override void Dispose(bool disposing)
        {
            Instance = null;
            base.Dispose(disposing);
        }

        // get the active WpfTextView, if there is one.
        private IWpfTextView GetActiveTextView()
        {
            IVsTextView vTextView = null;

            IVsTextManager txtMgr = (IVsTextManager)GetService(typeof(SVsTextManager));
            txtMgr.GetActiveView(1, null, out vTextView);

            IVsUserData userData = vTextView as IVsUserData;
            if (null != userData)
            {
                IWpfTextViewHost viewHost;
                object holder;
                Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
                userData.GetData(ref guidViewHost, out holder);
                viewHost = (IWpfTextViewHost)holder;
                return viewHost.TextView;
            }
            return null;
        }

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void TranslateMenu_Clicked(object sender, EventArgs e)
        {
            IWpfTextView view = GetActiveTextView();
            ITextSelection selection = view.Selection;
            if (selection != null)
            {
                SnapshotSpan span = view.Selection.SelectedSpans[0];
                String selectedText = span.GetText();
                //nothing is selected - taking the entire line
                if (string.IsNullOrEmpty(selectedText))
                {
                    ITextSnapshotLine line = span.Start.GetContainingLine();
                    selectedText = span.Start.GetContainingLine().GetText();
                    view.Selection.Select(new SnapshotSpan(line.Start, line.End), false);
                }
                //still no selection
                if (string.IsNullOrWhiteSpace(selectedText))
                {
                    MessageBox.Show("Nothing to translate", "Translator", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    var opt = Global.Options;
                    if (opt.BingClientCredential != null && opt.BingClientCredential.IsValid)
                    {
                        ClientCredentialRepository.Current.TryAddOrUpdateCredential(Constants.TranslatorNames.Bing, opt.BingClientCredential);
                    }
                    else
                    {
                        ClientCredentialRepository.Current
                        .TryAddOrUpdateCredential(Constants.TranslatorNames.Bing,
                            new ClientCredential
                            {
                                ClientId = Constants.BingTranslator.TokenClientID,
                                ClientSecret = Constants.BingTranslator.TokenClientSecret
                            });
                    }

                    var request = new TranslationRequest(selectedText, opt.SourceLanguage, opt.TargetLanguage,
                        opt.Translators.ToArray());
                    Connector.Execute(view, request);
                }
            }
        }
    }
}