using System;
using System.Windows;
using NewTranslator.Presentation;

namespace NewTranslator.Adornment
{
    /// <summary>
    /// Interaction logic for EditTranslationDialog.xaml
    /// </summary>
    public partial class EditTranslationDialog : ExDialogWindow
    {
        private readonly string _originalText;
        private readonly string _title;

        public EditTranslationDialog() : this(null, null, null)
        {
        }

        public EditTranslationDialog(TranslationItem translationItem, Action<TranslationItem> callback, FrameworkElement owinElement = null)
        {
            InitializeComponent();
            
            if (translationItem == null || callback == null)
            {
                DialogResult = null;
                Close();
                return;
            }

            _title = Title;
            
            if (owinElement != null)
            {
                try
                {
                    var pointFromScreen = owinElement.PointToScreen(new Point(-5, 0));
                    WindowStartupLocation = WindowStartupLocation.Manual;
                    var top = pointFromScreen.Y;
                    var left = pointFromScreen.X;
                    Top = top > 0 ? top : Top;
                    Left = left > 0 ? left : Left;
                    MaxHeight = SystemParameters.PrimaryScreenHeight - Top - 50;
                    Width = owinElement.ActualWidth + 50;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            _originalText = translationItem.TextWithOverflow;
            TranslationTextBox.Text = translationItem.TextWithOverflow;
            TranslationTextBox.TextChanged += (sender, args) =>
            {
                var changed = string.Compare(_originalText, TranslationTextBox.Text, StringComparison.InvariantCulture) != 0;
                Title = changed ? _title + "*" : _title;
            };

            TranslationTextBox.Focus();
            var caretIndex = TranslationTextBox.Text.Length;
            if(caretIndex > -1)
                TranslationTextBox.CaretIndex = caretIndex;
            TranslationTextBox.ScrollToEnd();

            CancelButton.Click += (sender, args) =>
            {
                DialogResult = false;
                Close();
            };

            ReplaceButton.Click += (sender, args) =>
            {
                var item = new TranslationItem(translationItem) {Text = TranslationTextBox.Text};
                DialogResult = true;
                callback(item);
                Close();
            };
        }
    }
}
