using System.Windows;
using System.Windows.Controls;
using TicketSystem.Models;

namespace TicketSystem.UI
{
    public partial class MainWindow : Window
    {
        private readonly ApiClient _apiClient;

        private UserRequest _editingRequest = null;

        public MainWindow()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
            LoadData();
        }

        private async void LoadData()
        {
            var requests = await _apiClient.GetRequestsAsync();
            RequestsGrid.ItemsSource = requests;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private async void AddOrUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleInput.Text) || string.IsNullOrWhiteSpace(AuthorInput.Text))
            {
                MessageBox.Show("Пожалуйста, заполните тему и автора.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_editingRequest == null)
            {
                var newRequest = new UserRequest
                {
                    Title = TitleInput.Text,
                    AuthorName = AuthorInput.Text,
                    Description = DescriptionInput.Text,
                    Priority = (PriorityInput.SelectedItem as ComboBoxItem)?.Content.ToString()
                };
                await _apiClient.CreateRequestAsync(newRequest);
            }
            else
            {
                _editingRequest.Title = TitleInput.Text;
                _editingRequest.AuthorName = AuthorInput.Text;
                _editingRequest.Description = DescriptionInput.Text;
                _editingRequest.Priority = (PriorityInput.SelectedItem as ComboBoxItem)?.Content.ToString();

                await _apiClient.UpdateRequestAsync(_editingRequest);

                CancelEdit_Click(sender, e);
            }

            ClearForm();
            LoadData();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsGrid.SelectedItem is UserRequest selectedRequest)
            {
                _editingRequest = selectedRequest;

                TitleInput.Text = selectedRequest.Title;
                AuthorInput.Text = selectedRequest.AuthorName;
                DescriptionInput.Text = selectedRequest.Description;

                foreach (ComboBoxItem item in PriorityInput.Items)
                {
                    if (item.Content.ToString() == selectedRequest.Priority)
                    {
                        PriorityInput.SelectedItem = item;
                        break;
                    }
                }

                AddOrUpdateButton.Content = "Сохранить изменения";
                AddOrUpdateButton.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(33, 150, 243)); // Синий цвет
                CancelEditButton.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Сначала выделите строку в таблице, которую хотите отредактировать.", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CancelEdit_Click(object sender, RoutedEventArgs e)
        {
            _editingRequest = null;
            AddOrUpdateButton.Content = "Добавить заявку";
            AddOrUpdateButton.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(76, 175, 80)); // Возвращаем зеленый цвет
            CancelEditButton.Visibility = Visibility.Collapsed;
            ClearForm();
        }
        
        private void ClearForm()
        {
            TitleInput.Clear();
            AuthorInput.Clear();
            DescriptionInput.Clear();
            PriorityInput.SelectedIndex = 1;
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsGrid.SelectedItem is UserRequest selectedRequest)
            {
                var result = MessageBox.Show($"Удалить заявку '{selectedRequest.Title}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await _apiClient.DeleteRequestAsync(selectedRequest.Id);

                    // Если мы удалили ту же заявку, которую сейчас редактировали, нужно сбросить форму
                    if (_editingRequest != null && _editingRequest.Id == selectedRequest.Id)
                    {
                        CancelEdit_Click(sender, e);
                    }

                    LoadData();
                }
            }
        }
    }
}