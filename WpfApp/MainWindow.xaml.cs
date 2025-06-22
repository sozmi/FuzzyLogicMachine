using System.Windows;
using System.Windows.Controls;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string? lastPage = null;
        public MainWindow()
        {
            InitializeComponent();

            // Устанавливаем начальную страницу
            frame.Navigate(new Uri($"Pages/Operations.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Обновляет навигацию на основе текущего значения lastPage.
        /// </summary>
        private void UpdatePageNavigation()
        {
            if (frame != null && lastPage != null)
            {
                frame.Navigate(new Uri($"/Pages/{lastPage}", UriKind.Relative));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button rb)
            {
                lastPage = rb.Tag.ToString();
                UpdatePageNavigation();
            }
        }
    }
}