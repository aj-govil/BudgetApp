using Budget;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZAMWPFHomeBudget
{

    /// <summary>
    /// Interaction logic for Form.xaml
    /// </summary>
    public partial class Form : Window, ExpenseViewInterface
    {
        Presenter presenter;
        public Form()
        {
            InitializeComponent();
            presenter = new Presenter(this);
            calExpenseDate.SelectedDate = DateTime.Now;
            Total.Visibility= Visibility.Hidden;
            IdData.Visibility= Visibility.Hidden;
            MonthData.Visibility= Visibility.Hidden;
            BalanceData.Visibility = Visibility.Visible;
            PopulateExpenses();
        }

        public void PopulateExpenses()
        {
            bool filterByCat = false;
            string filterCat = "";

            List<BudgetItem> budgetItems;
            if(FilterByCategory.IsChecked == true)
            {
                filterByCat = true;
            }

            if(cmbExpenseFilterCat.SelectedItem != null)
            {
                filterCat = (string)cmbExpenseFilterCat.SelectedItem;
            }
            else
            {
                filterByCat = false;
            }
            budgetItems = presenter.GetExpenses(filterExpenseStartDate.SelectedDate, filterExpenseEndDate.SelectedDate, filterByCat, filterCat);
            myDataGrid.ItemsSource = budgetItems;
            myDataGrid.Columns.Clear();
            var column = new DataGridTextColumn();
            column.Header = "Date";
            column.Binding = new System.Windows.Data.Binding("Date") { StringFormat = "yyyy-MM-dd" };
            myDataGrid.Columns.Add(column);
            column = new DataGridTextColumn();
            column.Header = "Category";
            column.Binding = new System.Windows.Data.Binding("Category");
            myDataGrid.Columns.Add(column);
            column = new DataGridTextColumn();
            column.Header = "Description";
            column.Binding = new System.Windows.Data.Binding("ShortDescription");
            myDataGrid.Columns.Add(column);
            column = new DataGridTextColumn();
            column.Header = "Amount";
            column.Binding = new System.Windows.Data.Binding("Amount") { StringFormat = "-{0:F2}" };
            myDataGrid.Columns.Add(column);
            column = new DataGridTextColumn();
            column.Header = "Balance";
            column.Binding = new System.Windows.Data.Binding("Balance") { StringFormat = "-{0:F2}" };
            myDataGrid.Columns.Add(column);
        }

        public void PopulateCategories()
        {
            cmbExpenseCat.Items.Clear();
            cmbExpenseFilterCat.Items.Clear();

            string[] categories = presenter.GetCategories();

            foreach (string category in categories)
            {
                cmbExpenseCat.Items.Add(category);
                cmbExpenseFilterCat.Items.Add(category);
            }
        }
        public void FileWorkingOn(string file)
        {
            fileWorking.Text = file;
        }

        public void ClearField()
        {
            txbExpenseDesc.Text = string.Empty;
            txbExpenseAmt.Text = string.Empty;
            txbCatDesc.Text = string.Empty;
            cmbCatType.Text = string.Empty;
            cmbExpenseCat.Text = string.Empty;
        }
        public void OutputMessage(string message)
        {
            System.Windows.MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txbExpenseDesc.Text = string.Empty;
            cmbExpenseCat.Text = string.Empty;
            txbExpenseAmt.Text = string.Empty;
            calExpenseDate.SelectedDate = DateTime.Now;
        }

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            presenter.AddExpense(txbExpenseDesc.Text, txbExpenseAmt.Text, calExpenseDate.SelectedDate, cmbExpenseCat.Text);
            PopulateExpenses();
        }

        private void btnAddCat_Click(object sender, RoutedEventArgs e)
        {
            presenter.createCategory(txbCatDesc.Text, cmbCatType.Text);
            PopulateCategories();
        }

        private void btnFilterCat_Click(object sender, RoutedEventArgs e)
        {
            PopulateExpenses();
        }

        private void cmbFilterCat_Click(object sender, RoutedEventArgs e)
        {

            PopulateExpenses();
        }

        private void dpFilterDate_Click(object sender, RoutedEventArgs e)
        {
            PopulateExpenses();
        }

        private void ExpenseGridModify_Click(object sender, RoutedEventArgs e)
        {
            BudgetItem? selectedBudgetItem = myDataGrid.SelectedItem as BudgetItem;
            if (selectedBudgetItem != null)
            {
                UpdateForm update = new UpdateForm(selectedBudgetItem, this);
                update.Show();
                PopulateExpenses();
            }
        }

        private void ExpenseGridDelete_Click(object sender, RoutedEventArgs e)
        {
            BudgetItem? selectedItem = myDataGrid.SelectedItem as BudgetItem;
            if (selectedItem != null)
            {
                presenter.deleteExpense(selectedItem);
                PopulateExpenses();
            }
        }

        private void ExpenseGridCancel_Click(object sender, RoutedEventArgs e)
        {
            cmExpense.IsOpen = false;
        }

        private void ExpenseGridByMonth_Click(object sender, RoutedEventArgs e)
        {
            presenter.FormatSwitch((bool)ByCategoryCheckbox.IsChecked, (bool)ByMonthCheckBox.IsChecked);

        }

     
        private void ByCategoryChecked(object sender, RoutedEventArgs e)
        {
            presenter.FormatSwitch((bool)ByCategoryCheckbox.IsChecked, (bool)ByMonthCheckBox.IsChecked);
        }
        public void Dictionnary()
        {
            bool filterByCat = false;
            string filterCat = "";
            if (FilterByCategory.IsChecked == true)
            {
                filterByCat = true;
            }

            if (cmbExpenseFilterCat.SelectedItem != null)
            {
                filterCat = (string)cmbExpenseFilterCat.SelectedItem;
            }
            else
            {
                filterByCat = false;
            }
            List<Dictionary<string, object>> expenseList = presenter.GetBudgetDictionaryByCategoryAndMonth(filterExpenseStartDate.SelectedDate, filterExpenseEndDate.SelectedDate, filterByCat, filterCat);
            myDataGrid.Columns.Clear();
            myDataGrid.ItemsSource = expenseList;
            presenter.CreateDictionnay(expenseList);
            DataGridColumn secondColumn = myDataGrid.Columns[1]; // Get the second column
            myDataGrid.Columns.Remove(secondColumn); // Remove the second column
            myDataGrid.Columns.Add(secondColumn);

        }
        public void AddColumn(DataGridTextColumn newCollumn)
        {
            myDataGrid.Columns.Add(newCollumn);
        }

        public void GroupByCategoryFormat()
        {
            bool filterByCat = false;
            string filterCat = "";
            if (FilterByCategory.IsChecked == true)
            {
                filterByCat = true;
            }

            if (cmbExpenseFilterCat.SelectedItem != null)
            {
                filterCat = (string)cmbExpenseFilterCat.SelectedItem;
            }
            else
            {
                filterByCat = false;
            }
            List<BudgetItemsByCategory> expensesList = presenter.GetExpensesByCategory(filterExpenseStartDate.SelectedDate, filterExpenseEndDate.SelectedDate, filterByCat, filterCat);
            myDataGrid.Columns.Clear();
            myDataGrid.ItemsSource = expensesList;
            var column = new DataGridTextColumn();
            column.Header = "Category";
            column.Binding = new System.Windows.Data.Binding("Category");
            myDataGrid.Columns.Add(column);
            column = new DataGridTextColumn();
            column.Header = "Total";
            column.Binding = new System.Windows.Data.Binding("Total") { StringFormat = "-{0:F2}" };
            myDataGrid.Columns.Add(column);
        }

        public void GroupByCategoryAndExpenseFormat()
        {
            Dictionnary();
        }

        public void StandardFormat()
        {
            PopulateExpenses();
        }


        public void GroupByExpenseFormat()
        {
            bool filterByCat = false;
            string filterCat = "";
            if (FilterByCategory.IsChecked == true)
            {
                filterByCat = true;
            }

            if (cmbExpenseFilterCat.SelectedItem != null)
            {
                filterCat = (string)cmbExpenseFilterCat.SelectedItem;
            }
            else
            {
                filterByCat = false;
            }
            myDataGrid.Columns.Clear();
            myDataGrid.ItemsSource = presenter.GetByMonthList(filterExpenseStartDate.SelectedDate, filterExpenseEndDate.SelectedDate, filterByCat, filterCat);
            var column = new DataGridTextColumn();
            column.Header = "Month";
            column.Binding = new System.Windows.Data.Binding("Month");
            myDataGrid.Columns.Add(column);
            column = new DataGridTextColumn();
            column.Header = "Total";
            column.Binding = new System.Windows.Data.Binding("Total") { StringFormat = "-{0:F2}" };
            myDataGrid.Columns.Add(column);
        }
    }
}
