using Microsoft.VisualBasic.Devices;
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
    /// Interaction logic for Action.xaml
    /// </summary>
    public partial class Action : Window
    {
        private readonly Prn212Context context;
        private readonly Customer currentCustomer;

        public Action(Prn212Context context, Customer customer)
        {
            InitializeComponent();
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.currentCustomer = customer ?? throw new ArgumentNullException(nameof(customer));
            LoadTasks();
        }

        public void LoadTasks()
        {
            try
            {
                var tasks = context.Actions
                    .Where(t => t.CustomerId == currentCustomer.CustomerId)
                    .Join(context.Statuses, // Join with the Status table
                          action => action.StatusId,
                          status => status.StatusId,
                          (action, status) => new
                          {
                              action.ActionId,
                              action.ActionName,
                              action.ActionDescription,
                              action.DateAction,
                              action.TimeAction,
                              StatusName = status.StatusName // Get StatusName instead of StatusID
                          })
                    .ToList()
                    .Select(a => new
                    {
                        a.ActionId,
                        a.ActionName,
                        a.ActionDescription,
                        DateAction = DateTime.Parse(a.DateAction).ToString("yyyy-MM-dd"),
                        TimeAction = TimeSpan.Parse(a.TimeAction).ToString(@"hh\:mm"),
                        a.StatusName
                    })
                    .ToList();

                dataGridTasks.ItemsSource = tasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load failed: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddAction addActionWindow = new AddAction(context, currentCustomer, this);
            addActionWindow.Show();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedAction = dataGridTasks.SelectedItem as dynamic;
            if (selectedAction != null)
            {
                int ActionId = selectedAction.ActionId;
                Models.Action action = context.Actions.FirstOrDefault(a => a.ActionId == ActionId);
                if (action != null)
                {
                    ViewandUpdate viewAction = new ViewandUpdate(context, action, currentCustomer);
                    viewAction.Closed += ViewandUpdate_Close;
                    viewAction.Show();
                }
                else
                {
                    MessageBox.Show("Hãy chọn action bạn muốn xem chi tiết hoặc cập nhật");
                }
            }
        }

        private void ViewandUpdate_Close(object sender, EventArgs e)
        {
            // Làm mới lại danh sách các hành động nếu cần
            LoadTasks();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }

    }
}