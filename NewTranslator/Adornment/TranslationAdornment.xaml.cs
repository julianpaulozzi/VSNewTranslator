using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.VisualStudio.Text;
using NewTranslator.Core.Translation;
using NewTranslator.Presentation;

namespace NewTranslator.Adornment
{
    public partial class TranslationAdornment : UserControl
    {
        private readonly ContextMenu _menu;
        private bool _closeMenuRequested;
        private bool _ignoreItemCommand;
        private ObservableCollection<TranslationItem> _resultsCollection;

        public TranslationAdornment(SnapshotSpan span, TranslationRequest request, Size viewportSize)
        {
            Span = span.Snapshot.CreateTrackingSpan(span, SpanTrackingMode.EdgeExclusive);
            Request = request;
            Request.TranslationComplete += request_TranslationComplete;

            InitializeComponent();
            DataContext = this;

            SetMaxSize(viewportSize);
            
            _menu = spListBox.ContextMenu;
            _menu.KeyUp += (sender, args) =>
            {
                if (args.Key == Key.Left || args.Key == Key.Escape)
                {
                    CloseMenu();
                }
            };
            _menu.Opened += (sender, args) => { _closeMenuRequested = false; };
            _menu.Closed += (sender, args) => { _ignoreItemCommand = !_closeMenuRequested; };

            ItemCommand = new RelayCommand<ItemCommandParameter>(ItemCommandExecute);
            ItemOptionsCommand = new RelayCommand<ItemCommandParameter>(ItemOptionsCommandExecute);
            MenuCommand = new RelayCommand<MenuItem>(MenuCommandExecute);
        }

        internal bool MenuIsOpen
        {
            get { return _menu.IsOpen; }
        }

        public RelayCommand<MenuItem> MenuCommand { get; }
        public RelayCommand<ItemCommandParameter> ItemOptionsCommand { get; }
        public RelayCommand<ItemCommandParameter> ItemCommand { get; }
        public TranslationRequest Request { get; set; }
        public ITrackingSpan Span { get; set; }

        private void MenuCommandExecute(MenuItem menuItem)
        {
            var tag = menuItem?.Tag as string;
            if (string.IsNullOrEmpty(tag))
                return;

            var translationItem = menuItem.DataContext as TranslationItem;
            if (translationItem == null)
                return;

            var textWithOverflow = translationItem.TextWithOverflow;
            switch (tag.ToLower())
            {
                case "replace":
                    if(!string.IsNullOrEmpty(textWithOverflow))
                        ReplaceSnapshotSpanText(textWithOverflow);
                    break;
                case "copy":
                    Clipboard.SetText(textWithOverflow);
                    break;
                case "edit":
                    Visibility = Visibility.Hidden;
                    var editTranslationDialog = new EditTranslationDialog(translationItem,
                        item =>
                        {
                            if (item != null)
                            {
                                ReplaceSnapshotSpanText(item.Text);
                                TranslationCache.AddUserEditedItem(item.TranslationSource, item.RequestSourceLanguage, 
                                    item.DestinationLanguage, item.OriginalText, item.Text);
                            }

                        }, this);
                    editTranslationDialog.ShowModal();
                    Visibility = Visibility.Visible;
                    break;
                case "copyall":
                    var s1 = spListBox.Items.Cast<object>()
                        .Aggregate("", (current, item) => current + (item + Environment.NewLine));
                    Clipboard.SetText(s1);
                    break;
                case "removeedit":
                    var removed = TranslationCache.RemoveUserEditedItem(translationItem.TranslationSource, translationItem.RequestSourceLanguage,
                                    translationItem.DestinationLanguage, translationItem.OriginalText, translationItem.Text);
                    if (removed)
                    {
                        _resultsCollection.Remove(translationItem);
                        MoveCurrentItem(true);
                    }
                    break;
            }
        }

        private void ItemOptionsCommandExecute(ItemCommandParameter parameter)
        {
            if (parameter != null)
            {
                var item = parameter.ItemSource as TranslationItem;
                spListBox.SelectedItem = item;
                OpenMenuOnTarget(parameter.ElementSource, parameter.ItemSource, false);
            }
        }

        private void OpenMenuOnTarget(UIElement placementTarget, object dataContext, bool moveFocus)
        {
            CloseMenu();

            _menu.Visibility = Visibility.Visible;
            _menu.DataContext = dataContext;
            _menu.IsOpen = true;
            _menu.PlacementTarget = placementTarget;
            _menu.VerticalOffset = -2;
            _menu.Placement = PlacementMode.Right;
            if (moveFocus)
            {
                var menuItem = _menu.Items[0] as MenuItem;
                menuItem?.Focus();
            }
        }

        private void CloseMenu()
        {
            _closeMenuRequested = true;
            _menu.PlacementTarget = null;
            _menu.Placement = PlacementMode.Mouse;
            _menu.IsOpen = false;
            _menu.Visibility = Visibility.Collapsed;
        }

        private void ItemCommandExecute(ItemCommandParameter parameter)
        {
            if (_menu.IsOpen || _ignoreItemCommand)
            {
                _ignoreItemCommand = false;
                return;
            }

            if (parameter != null)
            {
                var item = parameter.ItemSource as TranslationItem;
                if (item != null && !string.IsNullOrEmpty(item.TextWithOverflow))
                    ReplaceSnapshotSpanText(item.TextWithOverflow);
            }
        }

        public event EventHandler AdornmentClosed;

        private void SetMaxSize(Size viewportSize)
        {
            var maxHeight = viewportSize.Height/2.0d;
            if (maxHeight > 600)
                maxHeight = 600;
            if (maxHeight < 150)
                maxHeight = 150;
            spListBox.MaxHeight = maxHeight;
        }
        
        private void UserControl_Initialized(object sender, EventArgs e)
        {
        }

        private void RenderTranslation(IEnumerable<TranslationResult> translationResults)
        {
            var results = new List<TranslationItem>();
            foreach (var translationResult in translationResults)
            {
                var originalText = translationResult.OriginalText;
                var translationSource = translationResult.TranslationSource;
                var sourceLanguage = translationResult.SourceLanguage;
                var destinationLanguage = translationResult.DestinationLanguage;
                var requestSourceLanguage = translationResult.RequestSourceLanguage;
                lblDirection.Text = string.Format("{0} - {1}", sourceLanguage.ToUpper(), destinationLanguage.ToUpper());
                if (translationResult.Sentences != null)
                {
                    results.AddRange(translationResult.Sentences
                        .Select(s => new TranslationItem
                        {
                            Text = s,
                            OriginalText = originalText,
                            TranslationSource = translationSource,
                            RequestSourceLanguage = requestSourceLanguage,
                            DestinationLanguage = destinationLanguage
                        }));
                }

                foreach (var di in translationResult.DictionaryItems)
                {
                    results.AddRange(di.Terms
                        .Select(term => new TranslationItem
                        {
                            Header = di.Title,
                            Text = term,
                            OriginalText = originalText,
                            TranslationSource = translationSource,
                            RequestSourceLanguage = requestSourceLanguage,
                            DestinationLanguage = destinationLanguage
                        }));
                }
            }

            _resultsCollection = new ObservableCollection<TranslationItem>(results);
            var view = CollectionViewSource.GetDefaultView(_resultsCollection);
            view.GroupDescriptions.Add(new PropertyGroupDescription("Header"));
            spListBox.ItemsSource = view;
        }

        internal void MoveCurrentItem(bool previous)
        {
            if (spListBox.Items == null)
                return;

            var indexCount = spListBox.Items.Count - 1;
            if (indexCount == -1 || indexCount == 0)
                return;

            var moveIndex = previous ? -1 : 1;
            var newIndex = spListBox.SelectedIndex + moveIndex;
            if (newIndex < 0)
                newIndex = indexCount;
            if (newIndex > indexCount)
                newIndex = 0;

            spListBox.SelectedIndex = newIndex;
            spListBox.ScrollIntoView(spListBox.SelectedItem);
        }

        internal void OpenCurrentItemMenu()
        {
            var selectedItem = spListBox.SelectedItem;
            var translationItemControl = GetCurrentItemControl();
            if (translationItemControl != null) OpenMenuOnTarget(translationItemControl, selectedItem, true);
        }

        internal void CloseCurrentItemMenu()
        {
            CloseMenu();
        }

        private TranslationItemControl GetCurrentItemControl()
        {
            var index = spListBox.SelectedIndex;
            if (index > -1)
            {
                var listBoxItem = spListBox.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem;
                if (listBoxItem != null)
                {
                    var translationItemControl = listBoxItem.FindFirstElementInVisualTree<TranslationItemControl>();
                    return translationItemControl;
                }
            }
            return null;
        }

        internal bool TryReplaceWithCurrentItem()
        {
            var translationItem = spListBox.SelectedItem as TranslationItem;
            if (translationItem != null && !string.IsNullOrEmpty(translationItem.TextWithOverflow))
            {
                ReplaceSnapshotSpanText(translationItem.TextWithOverflow);
                return true;
            }
            return false;
        }

        internal bool CopyCurrentItemText()
        {
            var translationItem = spListBox.SelectedItem as TranslationItem;
            if (translationItem != null && !string.IsNullOrEmpty(translationItem.TextWithOverflow))
            {
                Clipboard.SetText(translationItem.TextWithOverflow);
                return true;
            }
            return false;
        }

        internal void ReplaceSnapshotSpanText(string newText)
        {
            var buffer = Span.TextBuffer;
            Span sp = Span.GetSpan(buffer.CurrentSnapshot);
            buffer.Replace(sp, newText);
        }

        private void RenderException(IEnumerable<Exception> ex)
        {
            lblError.Visibility = Visibility.Visible;
            lblError.Text = string.Join(Environment.NewLine + " - ", ex.Select(p=> p.Message));
        }

        private void OnAdornmentClosed()
        {
            if (AdornmentClosed != null)
                AdornmentClosed(this, EventArgs.Empty);
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            OnAdornmentClosed();
        }

        private void cmdSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.DTE.ExecuteCommand("Tools.Options", GuidList.OptionsPageGuid);
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open configuration.\nYou can find them in 'Tools > Options... > Translator'");
            }
        }

        private void request_TranslationComplete(object sender, EventArgs e)
        {
            var request = (TranslationRequest) sender;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (request.Results != null)
                    RenderTranslation(request.Results);

                if(request.Exceptions != null)
                    RenderException(request.Exceptions);

                ProgressRing.IsActive = false;
            }));
        }

        public void DetachRequest()
        {
            Request.TranslationComplete -= request_TranslationComplete;
        }
    }
}