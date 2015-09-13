using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
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
    // [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [ProvideAutoLoad(UIContextGuids80.CodeWindow)]
    [ProvideAutoLoad("f1536ef8-92ec-443c-9ed7-fdadf150da82")]
    // [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    public sealed class NewTranslatorPackage : Microsoft.VisualStudio.Shell.Package
    {
        private IVsStatusbar _vsStatusbar;

        /// <summary>
        /// This read-only property returns the package instance
        /// </summary>
        internal static NewTranslatorPackage Instance { get; private set; }

        private IVsStatusbar StatusBar
        {
            get { return _vsStatusbar ?? (_vsStatusbar = GetService(typeof (SVsStatusbar)) as IVsStatusbar); }
        }

        protected override void Initialize()
        {
            base.Initialize();

            Instance = this;

            // Add our command handlers for menu (commands must exist in the .vsct file)
            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                var translateCommandId = new CommandID(GuidList.guidTranslatorCmdSet, (int)PkgCmdIDList.Translate);
                var menuItemTranslate = new OleMenuCommand(TranslateMenu_Clicked, translateCommandId);
                menuItemTranslate.BeforeQueryStatus += MenuItemTranslateOnBeforeQueryStatus;
                mcs.AddCommand(menuItemTranslate);
            }
        }

        private void MenuItemTranslateOnBeforeQueryStatus(object sender, EventArgs eventArgs)
        {
            var oleMenuCommand = sender as OleMenuCommand;
            if (oleMenuCommand != null)
            {
                var view = GetActiveTextView();
                oleMenuCommand.Enabled = view != null && view.Selection != null && !view.Selection.IsEmpty;
                if (oleMenuCommand.Enabled)
                {
                    var span = view.Selection.SelectedSpans[0];
                    var selectedText = span.GetText();
                    oleMenuCommand.Text = string.Format("Translate '{0}'", selectedText.Truncate(26));
                }
                else
                {
                    oleMenuCommand.Text = "Translate Selection";
                }
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

            var userData = vTextView as IVsUserData;
            if (null == userData) return null;

            object holder;
            var guidViewHost = DefGuidList.guidIWpfTextViewHost;
            userData.GetData(ref guidViewHost, out holder);
            var viewHost = (IWpfTextViewHost)holder;
            return viewHost.TextView;
        }

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void TranslateMenu_Clicked(object sender, EventArgs e)
        {
            ClearStatusBar();
            IWpfTextView view = GetActiveTextView();
            ITextSelection selection = view.Selection;
            if (selection != null && !selection.IsEmpty)
            {
                SnapshotSpan span = view.Selection.SelectedSpans[0];
                var selectedText = span.GetText();
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
                    NotifyNothingToTranslate();
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
            else
            {
                NotifyNothingToTranslate();
            }
        }

        private void ClearStatusBar()
        {
            StatusBar.FreezeOutput(0);
            StatusBar.Clear();
        }

        private void NotifyNothingToTranslate()
        {
            int frozen;
            StatusBar.IsFrozen(out frozen);
            if (frozen == 0)
            {
                StatusBar.SetText("Nothing to translate.");
            }
        }
    }
}