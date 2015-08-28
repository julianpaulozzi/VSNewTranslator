using System.Windows;

namespace NewTranslator.Adornment
{
    public class ItemCommandParameter
    {
        public object ItemSource { get; set; }
        public UIElement ElementSource { get; set; }
        public string Tag { get; set; }
    }
}