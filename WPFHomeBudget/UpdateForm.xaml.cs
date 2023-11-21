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
using Budget;

namespace ZAMWPFHomeBudget
{
    /// <summary>
    /// Interaction logic for UpdateForm.xaml
    /// </summary>
    public partial class UpdateForm : Window, ExpenseViewInterface
    {
        Presenter presenter;
        BudgetItem selectedItem;
        Form form;
        public UpdateForm(BudgetItem selectedItem, Form form)
        {
            InitializeComponent();
            presenter = new Presenter(this);
            this.selectedItem = selectedItem;
            this.form = form;
            PopulateCategories();
            SetBoxesOriginalValues();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if(presenter.UpdateExpense(selectedItem.ExpenseID, txbExpenseDesc.Text, txbExpenseAmt.Text, calExpenseDate.SelectedDate ,cmbExpenseCat.Text))
            {
                form.PopulateExpenses();
                Close();
            }

        }

        private void SetBoxesOriginalValues()
        {
            txbExpenseDesc.Text = selectedItem.ShortDescription;
            cmbExpenseCat.Text = selectedItem.Category;
            txbExpenseAmt.Text = selectedItem.Amount.ToString();
            calExpenseDate.SelectedDate = selectedItem.Date;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txbExpenseDesc.Text = string.Empty;
            cmbExpenseCat.Text = string.Empty;
            txbExpenseAmt.Text = string.Empty;
            calExpenseDate.SelectedDate = DateTime.Now;
        }


        public void ClearField()
        {
            txbExpenseDesc.Text = string.Empty;
            txbExpenseAmt.Text = string.Empty;
            cmbExpenseCat.Text = string.Empty;
        }

        public void FileWorkingOn(string file)
        {
            fileWorking.Text = file;
        }

        public void OutputMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void PopulateCategories()
        {
            cmbExpenseCat.Items.Clear();
            string[] categories = presenter.GetCategories();

            foreach (string category in categories)
            {
                cmbExpenseCat.Items.Add(category);
            }
        }

        public void AddColumn(DataGridTextColumn newCollumn)
        {
            throw new NotImplementedException();
        }

        public void GroupByExpenseFormat()
        {
            throw new NotImplementedException();
        }

        public void GroupByCategoryFormat()
        {
            throw new NotImplementedException();
        }

        public void GroupByCategoryAndExpenseFormat()
        {
            throw new NotImplementedException();
        }

        public void StandardFormat()
        {
            throw new NotImplementedException();
        }
    }
}
