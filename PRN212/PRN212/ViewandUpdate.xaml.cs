using PRN212.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PRN212
{
    /// <summary>
    /// Interaction logic for ViewandUpdate.xaml
    /// </summary>
    public partial class ViewandUpdate : Window
    {
        private Prn212Context context;
        private Models.Action action;
        private Customer currentCustomer;

        public ViewandUpdate(Prn212Context context, Models.Action action, Customer currentCustomer)
        {
            InitializeComponent(); // Đảm bảo rằng các thành phần giao diện người dùng được khởi tạo

            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.currentCustomer = currentCustomer ?? throw new ArgumentNullException(nameof(currentCustomer));


            // Hiển thị thông tin của action lên giao diện người dùng
            txtID.Text = action.ActionId.ToString();
            txtName.Text = action.ActionName ?? string.Empty;
            txtDescription.Text = action.ActionDescription ?? string.Empty;
            
            if (!string.IsNullOrEmpty(action.DateAction) && DateTime.TryParse(action.DateAction, out DateTime date))
            {
                dtpDate.SelectedDate = date;
            }

            if (!string.IsNullOrEmpty(action.TimeAction) && TimeSpan.TryParse(action.TimeAction, out TimeSpan time))
            {
                cbHours.SelectedItem = cbHours.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == time.Hours.ToString("00"));
                cbMinutes.SelectedItem = cbMinutes.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == time.Minutes.ToString("00"));
            }

            var status = context.Statuses.FirstOrDefault(s => s.StatusId == action.StatusId);
            if (status != null)
            {
                cboStatus.SelectedItem = cboStatus.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == status.StatusName);
            }
            if (action.StatusId == 3)
            {
                txtName.IsEnabled = false;
                txtDescription.IsEnabled = false;
                dtpDate.IsEnabled = false;
                cbHours.IsEnabled = false;
                cbMinutes.IsEnabled = false;
                cboStatus.IsEnabled = false;
                btupdate.Visibility = Visibility.Collapsed;
            }

        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy giá trị từ giao diện người dùng
                action.ActionName = txtName.Text;
                action.ActionDescription = txtDescription.Text;
                if (dtpDate.SelectedDate.HasValue)
                {
                    action.DateAction = dtpDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }

                if (cbHours.SelectedItem != null && cbMinutes.SelectedItem != null)
                {
                    string selectedTime = $"{(cbHours.SelectedItem as ComboBoxItem).Content}:{(cbMinutes.SelectedItem as ComboBoxItem).Content}";
                    action.TimeAction = TimeSpan.Parse(selectedTime).ToString(@"hh\:mm");
                }

                if (cboStatus.SelectedItem != null)
                {
                    var selectedStatus = cboStatus.SelectedItem as ComboBoxItem;
                    var status = context.Statuses.FirstOrDefault(s => s.StatusName == selectedStatus.Content.ToString());
                    if (status != null)
                    {
                        action.StatusId = status.StatusId;
                    }
                }

                // Cập nhật vào cơ sở dữ liệu
                context.Update(action);
                context.SaveChanges();

                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                // Đóng cửa sổ sau khi cập nhật thành công
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cập nhật thất bại: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            // Đóng cửa sổ hiện tại
            this.Close();
        }

        private void btndelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Hiển thị hộp thoại xác nhận
                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa tác vụ này không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Xóa action từ context
                    context.Actions.Remove(action);
                    context.SaveChanges();

                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Đóng cửa sổ sau khi xóa thành công
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Xóa thất bại: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
