using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using NewTranslator.Services;

namespace NewTranslator.Adornment
{
    internal sealed class TranslationCommandFilter : IOleCommandTarget
    {
        private readonly TranslationAdornmentManager _translationAdornmentManager;
        private readonly IWpfTextView m_textView;
        internal bool m_added;
        internal IOleCommandTarget m_nextTarget;
        
        public TranslationCommandFilter(IWpfTextView tv)
        {
            m_textView = tv;
            _translationAdornmentManager =
                m_textView.Properties.GetProperty<TranslationAdornmentManager>(typeof (TranslationAdornmentManager));
        }
        
        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (_translationAdornmentManager != null && _translationAdornmentManager.m_adorned)
            {
                if (pguidCmdGroup == typeof (VSConstants.VSStd2KCmdID).GUID)
                {
                    var handled = _translationAdornmentManager.HandleCommand((VSConstants.VSStd2KCmdID) nCmdID);
                    if (handled)
                    {
                        // Avoid text change
                        return VSConstants.S_FALSE;
                    }
                }
                else if (pguidCmdGroup == VSConstants.GUID_VSStandardCommandSet97)
                {
                    var handled = _translationAdornmentManager.HandleCommand((VSConstants.VSStd97CmdID) nCmdID);
                    if (handled)
                    {
                        // Avoid text change
                        return VSConstants.S_FALSE;
                    }
                }
            }

            return m_nextTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            if (_translationAdornmentManager != null && _translationAdornmentManager.m_adorned)
            {
                if (pguidCmdGroup == typeof (VSConstants.VSStd2KCmdID).GUID)
                {
                    for (int i = 0; i < cCmds; i++)
                    {
                        switch (prgCmds[i].cmdID)
                        {
                            case ((uint)VSConstants.VSStd2KCmdID.RETURN):
                            case ((uint)VSConstants.VSStd2KCmdID.TYPECHAR):
                            case ((uint)VSConstants.VSStd2KCmdID.CANCEL):
                                prgCmds[i].cmdf = VSConstants.S_OK;
                                break;
                        }
                    }
                }
            }
                /*if (pguidCmdGroup == typeof(VSConstants.VSStd2KCmdID).GUID)
                {
                    for (int i = 0; i < cCmds; i++)
                    {
                        switch (prgCmds[i].cmdID)
                        {
                            case ((uint)VSConstants.VSStd2KCmdID.TYPECHAR):
                            case ((uint)VSConstants.VSStd2KCmdID.BACKSPACE):
                            case ((uint)VSConstants.VSStd2KCmdID.TAB):
                            case ((uint)VSConstants.VSStd2KCmdID.LEFT):
                            case ((uint)VSConstants.VSStd2KCmdID.RIGHT):
                            case ((uint)VSConstants.VSStd2KCmdID.UP):
                            case ((uint)VSConstants.VSStd2KCmdID.DOWN):
                            case ((uint)VSConstants.VSStd2KCmdID.END):
                            case ((uint)VSConstants.VSStd2KCmdID.HOME):
                            case ((uint)VSConstants.VSStd2KCmdID.PAGEDN):
                            case ((uint)VSConstants.VSStd2KCmdID.PAGEUP):
                            case ((uint)VSConstants.VSStd2KCmdID.PASTE):
                            case ((uint)VSConstants.VSStd2KCmdID.PASTEASHTML):
                            case ((uint)VSConstants.VSStd2KCmdID.BOL):
                            case ((uint)VSConstants.VSStd2KCmdID.EOL):
                            case ((uint)VSConstants.VSStd2KCmdID.RETURN):
                            case ((uint)VSConstants.VSStd2KCmdID.BACKTAB):
                                prgCmds[i].cmdf = (uint)(OLECMDF.OLECMDF_ENABLED | OLECMDF.OLECMDF_SUPPORTED);
                                return VSConstants.S_OK;
                        }
                    }
                }*/

                return m_nextTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        internal static bool AddCommandFilter(IWpfTextView textView, TranslationCommandFilter commandFilter)
        {
            if (commandFilter == null)
                return false;

            if (commandFilter.m_added == false)
            {
                var vsEditorAdaptersFactoryService =
                    ServicesHelper.GetServiceFromComponentModel<IVsEditorAdaptersFactoryService>(false);

                //get the view adapter from the editor factory
                if (vsEditorAdaptersFactoryService != null)
                {
                    var view = vsEditorAdaptersFactoryService.GetViewAdapter(textView);
                    IOleCommandTarget next;

                    var hr = view.AddCommandFilter(commandFilter, out next);
                    if (hr == VSConstants.S_OK)
                    {
                        commandFilter.m_added = true;
                        //you'll need the next target for Exec and QueryStatus 
                        if (next != null)
                            commandFilter.m_nextTarget = next;
                    }
                    return true;
                }
            }
            return false;
        }

        internal static bool RemoveCommandFilter(IWpfTextView textView, TranslationCommandFilter commandFilter)
        {
            if (commandFilter == null)
                return false;

            if (commandFilter.m_added)
            {
                var vsEditorAdaptersFactoryService =
                    ServicesHelper.GetServiceFromComponentModel<IVsEditorAdaptersFactoryService>(false);

                //get the view adapter from the editor factory
                if (vsEditorAdaptersFactoryService != null)
                {
                    var view = vsEditorAdaptersFactoryService.GetViewAdapter(textView);
                    var hr = view.RemoveCommandFilter(commandFilter);
                    if (hr == VSConstants.S_OK)
                    {
                        commandFilter.m_added = false;
                    }
                    return true;
                }
            }

            return false;
        }
    }
}