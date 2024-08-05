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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        Prn212Context context = new Prn212Context();
        public Register()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra nếu các trường bắt buộc không được nhập
                if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Password) || cboGender.SelectedItem == null || !dpBirthdate.SelectedDate.HasValue)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                    return;
                }

                // Kiểm tra nếu CustomerName đã tồn tại
                if (context.Customers.Any(c => c.CustomerName == txtUsername.Text))
                {
                    MessageBox.Show("Tên người dùng đã tồn tại.");
                    return;
                }

                // Tạo đối tượng Customer mới
                Customer newCustomer = new Customer
                {
                    CustomerName = txtUsername.Text,
                    Password = txtPassword.Password,
                    Gender = (cboGender.SelectedItem as ComboBoxItem).Content.ToString() == "Male" ? true : false,
                    Address = txtAddress.Text,
                    Birthdate = dpBirthdate.SelectedDate.Value
                };

                // Thêm đối tượng mới vào context và lưu vào cơ sở dữ liệu
                context.Customers.Add(newCustomer);
                context.SaveChanges();

                MessageBox.Show("Registration Successful");

                // Chuyển đến trang đăng nhập sau khi đăng ký thành công
                Login loginWindow = new Login();
                loginWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Registration failed: " + ex.Message);
            }
        }
    }
}
