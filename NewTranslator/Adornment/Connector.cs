using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using NewTranslator.Core.Translation;

namespace NewTranslator.Adornment
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class Connector : IWpfTextViewCreationListener
    {
        public void TextViewCreated(IWpfTextView textView)
        {
            TranslationAdornmentManager.Create(textView);
            // AddCommandFilter(textView, new TranslationCommandFilter(textView));
        }

        public static void Execute(IWpfTextView view, TranslationRequest request)
        {
            TranslationAdornmentManager manager = null;
            try
            {
                manager = view.Properties.GetProperty<TranslationAdornmentManager>(typeof(TranslationAdornmentManager));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            if (manager == null) return;

            manager.AddTranslation(view.Selection.SelectedSpans[0], request);
            request.GetTranslationAsync();
        }

        [Export(typeof(AdornmentLayerDefinition))]
        [Name("TranslationAdornmentLayer")]
        [Order(After = PredefinedAdornmentLayers.Caret)]
		[Order(After = PredefinedAdornmentLayers.Outlining)]
		[Order(After = PredefinedAdornmentLayers.Selection)]
		[Order(After = PredefinedAdornmentLayers.Squiggle)]
		[Order(After = PredefinedAdornmentLayers.Text)]
		[Order(After = PredefinedAdornmentLayers.TextMarker)]
        public AdornmentLayerDefinition TranslationLayerDefinition;

        /*[Import(typeof(IVsEditorAdaptersFactoryService))]
        internal IVsEditorAdaptersFactoryService editorFactory = null;*/

        /*void AddCommandFilter(IWpfTextView textView, TranslationCommandFilter commandFilter)
        {
            if (commandFilter.m_added == false)
            {
                //get the view adapter from the editor factory
                IOleCommandTarget next;
                IVsTextView view = editorFactory.GetViewAdapter(textView);

                int hr = view.AddCommandFilter(commandFilter, out next);
                if (hr == VSConstants.S_OK)
                {
                    commandFilter.m_added = true;
                    //you'll need the next target for Exec and QueryStatus 
                    if (next != null)
                        commandFilter.m_nextTarget = next;
                }
            }
        }*/
    }
}
