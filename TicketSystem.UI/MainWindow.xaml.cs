using System.Windows;
using System.Windows.Controls;
using TicketSystem.Models;

namespace TicketSystem.UI
{
    public partial class MainWindow : Window
    {
        private readonly ApiClient _apiClient;

        public MainWindow()
        {
            InitializeComponent();
            _apiClient = new ApiClient();

            // Загружаем данные сразу при открытии окна
            LoadData();
        }

        // Общий метод для получения данных с сервера
        private async void LoadData()
        {
            var requests = await _apiClient.GetRequestsAsync();
            RequestsGrid.ItemsSource = requests;
        }

        // Кнопка обновления
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        // Добавление реальной заявки из полей ввода
        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            // Простая проверка, чтобы не отправлять пустые поля
            if (string.IsNullOrWhiteSpace(TitleInput.Text) || string.IsNullOrWhiteSpace(AuthorInput.Text))
            {
                MessageBox.Show("Пожалуйста, заполните тему и автора.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newRequest = new UserRequest
            {
                Title = TitleInput.Text,
                AuthorName = AuthorInput.Text,
                Description = DescriptionInput.Text,
                // Берем выбранный текст из выпадающего списка
                Priority = (PriorityInput.SelectedItem as ComboBoxItem)?.Content.ToString()
            };

            await _apiClient.CreateRequestAsync(newRequest);

            // Очищаем поля ввода после успешного добавления
            TitleInput.Clear();
            AuthorInput.Clear();
            DescriptionInput.Clear();
            PriorityInput.SelectedIndex = 1;

            // Обновляем таблицу
            LoadData();
        }

        // Удаление выделенной строки
        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выделена ли строка в таблице
            if (RequestsGrid.SelectedItem is UserRequest selectedRequest)
            {
                var result = MessageBox.Show($"Удалить заявку '{selectedRequest.Title}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await _apiClient.DeleteRequestAsync(selectedRequest.Id);
                    LoadData(); // Обновляем таблицу
                }
            }
            else
            {
                MessageBox.Show("Сначала выделите строку в таблице, которую хотите удалить.", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}