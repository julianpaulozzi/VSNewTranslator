using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NewTranslator.Adornment
{
    /// <summary>
    ///     Interaction logic for TranslationItemControl.xaml
    /// </summary>
    public partial class TranslationItemControl : UserControl
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof (ICommand), typeof (TranslationItemControl), new PropertyMetadata(default(ICommand)));
        
        public static readonly DependencyProperty OptionsCommandProperty = DependencyProperty.Register(
            "OptionsCommand", typeof (ICommand), typeof (TranslationItemControl),
            new PropertyMetadata(default(ICommand)));
        
        public TranslationItemControl()
        {
            InitializeComponent();
            
            ContentContainer.MouseUp += OnContentContainerMouseUp;
            OptionsButton.Click += OnOptionsButtonClick;
        }
        
        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        
        public ICommand OptionsCommand
        {
            get { return (ICommand) GetValue(OptionsCommandProperty); }
            set { SetValue(OptionsCommandProperty, value); }
        }

        private void OnOptionsButtonClick(object sender, RoutedEventArgs e)
        {
            var itemCommandParameter = new ItemCommandParameter
            {
                ItemSource = DataContext,
                ElementSource = OptionsButton,
                Tag = "menu"
            };

            if (OptionsCommand != null && OptionsCommand.CanExecute(itemCommandParameter))
                OptionsCommand.Execute(itemCommandParameter);
        }

        private void OnContentContainerMouseUp(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton != MouseButton.Left)
                return;

            var itemCommandParameter = new ItemCommandParameter
            {
                ItemSource = DataContext,
                ElementSource = ContentContainer,
                Tag = "content"
            };

            if (Command != null && Command.CanExecute(itemCommandParameter))
                Command.Execute(itemCommandParameter);
        }
    }
}