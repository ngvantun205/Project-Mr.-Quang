using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TDEduEnglish.Behaviors {
    public static class TextSelectionBehavior {
        public static readonly DependencyProperty SelectedTextCommandProperty =
            DependencyProperty.RegisterAttached(
                "SelectedTextCommand",
                typeof(ICommand),
                typeof(TextSelectionBehavior),
                new PropertyMetadata(null, OnSelectedTextCommandChanged));

        public static void SetSelectedTextCommand(DependencyObject obj, ICommand value)
            => obj.SetValue(SelectedTextCommandProperty, value);

        public static ICommand GetSelectedTextCommand(DependencyObject obj)
            => (ICommand)obj.GetValue(SelectedTextCommandProperty);

        private static void OnSelectedTextCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is RichTextBox rtb) {
                rtb.SelectionChanged -= Rtb_SelectionChanged;
                rtb.SelectionChanged += Rtb_SelectionChanged;
            }
        }

        private static void Rtb_SelectionChanged(object sender, RoutedEventArgs e) {
            var rtb = sender as RichTextBox;
            var command = GetSelectedTextCommand(rtb);

            if (command == null)
                return;

            var selectedText = rtb.Selection?.Text?.Trim();

            if (!string.IsNullOrEmpty(selectedText) && command.CanExecute(selectedText))
                command.Execute(selectedText);
        }
    }
}
