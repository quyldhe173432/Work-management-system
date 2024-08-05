using PRN212.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PRN212
{
    /// <summary>
    /// Interaction logic for AddAction.xaml
    /// </summary>
    public partial class AddAction : Window
    {
        private readonly Prn212Context context;
        private readonly Customer currentCustomer;
        private readonly Action actionWindow; // Thêm tham chiếu đến cửa sổ Action

        public AddAction(Prn212Context context, Customer currentCustomer, Action actionWindow)
        {
            InitializeComponent(); // Đảm bảo rằng các thành phần giao diện người dùng được khởi tạo

            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.currentCustomer = currentCustomer ?? throw new ArgumentNullException(nameof(currentCustomer));
            this.actionWindow = actionWindow ?? throw new ArgumentNullException(nameof(actionWindow));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem các trường dữ liệu có trống không
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                dtpDate.SelectedDate == null ||
                cbHours.SelectedItem == null ||
                cbMinutes.SelectedItem == null ||
                cboStatus.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra xem Date có hợp lệ không
            if (!dtpDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Ngày không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra xem Time có hợp lệ không
            string timeAction = $"{(cbHours.SelectedItem as ComboBoxItem).Content}:{(cbMinutes.SelectedItem as ComboBoxItem).Content}";
            if (!TimeSpan.TryParse(timeAction, out _))
            {
                MessageBox.Show("Thời gian không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Lấy giá trị Status
            var selectedStatus = cboStatus.SelectedItem as ComboBoxItem;
            var status = context.Statuses.FirstOrDefault(s => s.StatusName == selectedStatus.Content.ToString());
            if (status == null)
            {
                MessageBox.Show("Trạng thái không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dateActionString = dtpDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            var selectedHour = (cbHours.SelectedItem as ComboBoxItem).Content.ToString();

            // Kiểm tra nếu có hành động khác cùng ngày và giờ với trạng thái là 2
            if (context.Actions.Any(a => a.CustomerId == currentCustomer.CustomerId
                                         && a.DateAction == dateActionString
                                         && a.TimeAction == timeAction
                                         && a.StatusId == 2))
            {
                MessageBox.Show("Không thể thêm hành động cùng ngày và giờ với một hành động khác có trạng thái là 2.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (status.StatusId == 2)
            {
                // Tìm và xóa các công việc cũ có trạng thái là 1 cùng ngày và giờ
                var actionsToDelete = context.Actions
                    .Where(a => a.CustomerId == currentCustomer.CustomerId
                                && a.DateAction == dateActionString
                                && a.TimeAction == timeAction
                                && a.StatusId == 1)
                    .ToList();

                if (actionsToDelete.Any())
                {
                    context.Actions.RemoveRange(actionsToDelete);
                }
            }
            else
            {
                // Kiểm tra xem TimeAction đã tồn tại hay chưa, chỉ kiểm tra các hành động không có trạng thái là 1 hoặc 2
                if (context.Actions.Any(a => a.CustomerId == currentCustomer.CustomerId
                                             && a.DateAction == dateActionString
                                             && a.TimeAction == timeAction
                                             && a.StatusId != 1
                                             && a.StatusId != 2))
                {
                    MessageBox.Show("Thời gian cho hành động này đã tồn tại trong ngày đã chọn.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // Tạo một đối tượng Action mới
            var newAction = new Models.Action
            {
                ActionName = txtName.Text,
                ActionDescription = txtDescription.Text,
                DateAction = dtpDate.SelectedDate.Value.ToString("yyyy-MM-dd"),
                TimeAction = timeAction,
                StatusId = status.StatusId,
                CustomerId = currentCustomer.CustomerId
            };

            // Thêm vào cơ sở dữ liệu
            context.Actions.Add(newAction);
            context.SaveChanges();

            MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            // Tải lại danh sách hành động trong cửa sổ Action
            actionWindow.LoadTasks();

            // Đóng cửa sổ sau khi thêm mới thành công
            this.Close();
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            // Đóng cửa sổ hiện tại
            this.Close();
        }

    }
}
