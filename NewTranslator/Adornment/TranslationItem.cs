namespace NewTranslator.Adornment
{
    public class TranslationItem
    {
        private string _textWithOverflow;

        public TranslationItem()
        {
        }

        public TranslationItem(TranslationItem copyItem)
        {
            Header = copyItem.Header;
            Text = copyItem.Text;
            OriginalText = copyItem.OriginalText;
            TranslationSource = copyItem.TranslationSource;
            RequestSourceLanguage = copyItem.RequestSourceLanguage;
            DestinationLanguage = copyItem.DestinationLanguage;
            _textWithOverflow = null;
        }

        public string Header { get; set; }
        public string Text { get; set; }
        public string OriginalText { get; set; }
        public string TranslationSource { get; set; }
        public string RequestSourceLanguage { get; set; }
        public string DestinationLanguage { get; set; }

        public string TextWithOverflow
        {
            get
            {
                if (string.IsNullOrEmpty(_textWithOverflow))
                    _textWithOverflow = Text.TrimRestore(OriginalText);
                return _textWithOverflow;
            }
        }
        
        public override string ToString()
        {
            return Text;
        }
    }
}