using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using NewTranslator.Core.Translation;

namespace NewTranslator.Adornment
{
    internal sealed class TranslationAdornmentManager
    {
        private readonly ITextBuffer _buffer;
        private readonly IAdornmentLayer _layer;
        private readonly IWpfTextView _view;
        private TranslationAdornment _translationAdornmentFocused;
        internal bool m_adorned;
        internal TranslationCommandFilter TranslationCommandFilter;
        public IList<TranslationAdornment> Translations = new List<TranslationAdornment>();

        private TranslationAdornmentManager(IWpfTextView view)
        {
            _view = view;
            _view.LayoutChanged += OnLayoutChanged;
            _view.Closed += OnClosed;
            _buffer = _view.TextBuffer;
            _buffer.Changed += OnBufferChanged;

            _layer = view.GetAdornmentLayer("TranslationAdornmentLayer");
        }

        public static TranslationAdornmentManager Create(IWpfTextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(() => new TranslationAdornmentManager(view));
        }

        public bool HandleMouseLeftButton(MouseButtonEventArgs e)
        {
            if (!m_adorned)
                return false;

            var translationAdornment = e.Source as TranslationAdornment;
            if (translationAdornment != null)
            {
                _translationAdornmentFocused = translationAdornment;
                return false;
            }

            ClearTranslations();

            return false;
        }

        public bool HandleMouseRightButton(MouseButtonEventArgs e)
        {
            if (!m_adorned)
                return false;

            var translationAdornment = e.Source as TranslationAdornment;
            if (translationAdornment != null)
            {
                _translationAdornmentFocused = translationAdornment;
                return true;
            }

            ClearTranslations();

            return false;
        }

        public bool HandleCommand(VSConstants.VSStd2KCmdID key)
        {
            if (!m_adorned)
                return false;

            if(_translationAdornmentFocused != null && _translationAdornmentFocused.MenuIsOpen)
                return true;

            // Debug.WriteLine(">> VSStd2KCmdID: {0}", key);

            switch (key)
            {
                case VSConstants.VSStd2KCmdID.RETURN:
                case VSConstants.VSStd2KCmdID.TYPECHAR:
                    if (_translationAdornmentFocused != null)
                        _translationAdornmentFocused.TryReplaceWithCurrentItem();
                    return true;
                case VSConstants.VSStd2KCmdID.UP:
                    if (_translationAdornmentFocused != null)
                        _translationAdornmentFocused.MoveCurrentItem(true);
                    return true;
                case VSConstants.VSStd2KCmdID.DOWN:
                    if (_translationAdornmentFocused != null)
                        _translationAdornmentFocused.MoveCurrentItem(false);
                    return true;
                case VSConstants.VSStd2KCmdID.LEFT:
                    if (_translationAdornmentFocused != null)
                        _translationAdornmentFocused.CloseCurrentItemMenu();
                    return true;
                case VSConstants.VSStd2KCmdID.RIGHT:
                case VSConstants.VSStd2KCmdID.SHOWCONTEXTMENU:
                    if (_translationAdornmentFocused != null)
                        _translationAdornmentFocused.OpenCurrentItemMenu();
                    return true;
                case VSConstants.VSStd2KCmdID.CANCEL:
                    ClearTranslations();
                    return true;
                case VSConstants.VSStd2KCmdID.COPY:
                    if (_translationAdornmentFocused != null)
                        _translationAdornmentFocused.CopyCurrentItemText();
                    return true;
                case VSConstants.VSStd2KCmdID.INSERT:
                case VSConstants.VSStd2KCmdID.DELETE:
                case VSConstants.VSStd2KCmdID.BACKSPACE:
                case VSConstants.VSStd2KCmdID.TAB:
                case VSConstants.VSStd2KCmdID.END:
                case VSConstants.VSStd2KCmdID.HOME:
                case VSConstants.VSStd2KCmdID.PAGEDN:
                case VSConstants.VSStd2KCmdID.PAGEUP:
                case VSConstants.VSStd2KCmdID.PASTE:
                case VSConstants.VSStd2KCmdID.PASTEASHTML:
                case VSConstants.VSStd2KCmdID.BOL:
                case VSConstants.VSStd2KCmdID.EOL:
                case VSConstants.VSStd2KCmdID.BACKTAB:
                    return true;
            }

            return false;
        }

        public bool HandleCommand(VSConstants.VSStd97CmdID key)
        {
            if (!m_adorned)
                return false;

            if (_translationAdornmentFocused != null && _translationAdornmentFocused.MenuIsOpen)
                return true;

            // Debug.WriteLine(">> VSStd2KCmdID: {0}", key);

            switch (key)
            {
                case VSConstants.VSStd97CmdID.Copy:
                    if (_translationAdornmentFocused != null)
                        _translationAdornmentFocused.CopyCurrentItemText();
                    return true;
            }

            return false;
        }

        private void OnBufferChanged(object sender, TextContentChangedEventArgs e)
        {
            ClearTranslations();
            _view.VisualElement.Focus();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            _buffer.Changed -= OnBufferChanged;
            _view.LayoutChanged -= OnLayoutChanged;
            _view.Closed -= OnClosed;
            ClearTranslations();
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            var newPosts = new HashSet<TranslationAdornment>();

            foreach (Span span in e.NewOrReformattedSpans)
            {
                var adornments = GetTranslations(new SnapshotSpan(_view.TextSnapshot, span));
                foreach (var adornment in adornments)
                {
                    if (!newPosts.Contains(adornment))
                    {
                        newPosts.Add(adornment);
                        RenderTranslation(adornment);
                    }
                }
            }
        }

        private void ClearTranslations()
        {
            for (var i = Translations.Count - 1; i >= 0; i--)
            {
                RemoveTranslation(Translations[i]);
            }
        }

        private void RenderTranslation(TranslationAdornment translation)
        {
            var span = translation.Span.GetSpan(_view.TextSnapshot);
            var g = _view.TextViewLines.GetMarkerGeometry(span);

            if (g != null)
            {
                translation.Margin = new Thickness(g.Bounds.BottomLeft.X, g.Bounds.BottomLeft.Y, 0, 0);
                _layer.AddAdornment(span, null, translation);
            }
        }

        public void AddTranslation(SnapshotSpan span, TranslationRequest request)
        {
            var viewportSize = new Size(_view.ViewportWidth, _view.ViewportHeight);
            ClearTranslations();
            var a = new TranslationAdornment(span, request, viewportSize);
            Translations.Add(a);
            a.AdornmentClosed += OnTranslationClosed;
            RenderTranslation(a);
            InvalidateState();
            a.Focus();
            _translationAdornmentFocused = a;
        }

        private void RemoveTranslation(TranslationAdornment a)
        {
            a.DetachRequest();
            Translations.Remove(a);
            _layer.RemoveAdornment(a);
            if (Equals(_translationAdornmentFocused, a))
                _translationAdornmentFocused = null;
            InvalidateState();
        }

        /// <summary>
        ///     gets translations attached to this SnapshotSpan
        /// </summary>
        public List<TranslationAdornment> GetTranslations(SnapshotSpan span)
        {
            var res = new List<TranslationAdornment>();
            foreach (var t in Translations)
            {
                if (t.Span.GetSpan(span.Snapshot).OverlapsWith(span))
                    res.Add(t);
            }
            return res;
        }

        private void OnTranslationClosed(object sender, EventArgs e)
        {
            RemoveTranslation((TranslationAdornment) sender);
        }

        private void InvalidateState()
        {
            m_adorned = Translations != null && Translations.Any();

            if (!m_adorned && TranslationCommandFilter != null)
            {
                TranslationCommandFilter.RemoveCommandFilter(_view, TranslationCommandFilter);
                TranslationCommandFilter = null;
                return;
            }

            if (m_adorned && TranslationCommandFilter == null)
            {
                TranslationCommandFilter = new TranslationCommandFilter(_view);
                TranslationCommandFilter.AddCommandFilter(_view, TranslationCommandFilter);
            }
        }
    }
}