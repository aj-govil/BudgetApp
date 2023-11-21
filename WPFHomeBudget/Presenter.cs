using Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Windows;
using System.IO;
using System.Windows.Controls;

namespace ZAMWPFHomeBudget
{
    public class Presenter
    {
        private readonly ViewInterface view;
        private readonly ExpenseViewInterface expenseView;
        private static HomeBudget homeBudgetModel;
        private static string _fileName;

        public Presenter(ViewInterface v)
        {
            view = v;
        }

        public Presenter(ViewInterface v, string fileName, bool newDB)
        {
            view = v;
            homeBudgetModel = new HomeBudget(fileName, newDB);
            _fileName = fileName;
        }

        public Presenter(ExpenseViewInterface v)
        {
            expenseView = v;
        }

        public string[] GetCategories()
        {
            List<Category> categoriesList = homeBudgetModel.categories.List();
            string[] categories = new string[categoriesList.Count];

            for (int i = 0; i < categoriesList.Count; i++)
            {
                categories[i] = categoriesList[i].ToString();
            }

            return categories;
        }

        public List<BudgetItem> GetExpenses(DateTime? startDate, DateTime? endDate, bool filterByCat, string catDesc)
        {
            int catId = -1;

            List<Expense> expensesList = homeBudgetModel.expenses.List();
            List<Category> categoriesList = homeBudgetModel.categories.List();

            foreach(Category cat in categoriesList)
            {
                if(cat.Description == catDesc)
                {
                    catId = cat.Id;
                }
            }

            List<BudgetItem> budgetList = homeBudgetModel.GetBudgetItems(startDate, endDate, filterByCat, catId);

            return budgetList;
        }
        public List<BudgetItemsByCategory> GetExpensesByCategory(DateTime? startDate, DateTime? endDate, bool filterByCat, string catDesc)
        {
            int catId = -1;
            List<Category> categoriesList = homeBudgetModel.categories.List();

            foreach (Category cat in categoriesList)
            {
                if (cat.Description == catDesc)
                {
                    catId = cat.Id;
                }
            }
            List<BudgetItemsByCategory> expensesList = homeBudgetModel.GetBudgetItemsByCategory(startDate, endDate, filterByCat, catId);

            return expensesList;
        }

        public bool createCategory(string categoryName, string type)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(categoryName)) { expenseView.OutputMessage("Description field can not be empty"); return false; }
                if (String.IsNullOrWhiteSpace(type)) { expenseView.OutputMessage("type field can not be empty"); return false; }

                homeBudgetModel.categories.Add(categoryName, (Category.CategoryType)Enum.Parse(typeof(Category.CategoryType), type));
                expenseView.PopulateCategories();
                expenseView.ClearField();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void CreateDictionnay(List<Dictionary<string, object>> expenseList)
        {
            int counter = 0;
            int index = 0;
            string[] allTheCategories = GetCategories();
            string[] myList = new string[allTheCategories.Length];
            foreach (string key in expenseList[0].Keys)
            {
                if (counter == 0)
                {
                    var column = new DataGridTextColumn();
                    column.Header = key;
                    myList[index] = key;
                    column.Binding = new System.Windows.Data.Binding($"[{key}]");
                    expenseView.AddColumn(column);
                    index++;
                }
                else if (counter % 2 != 0)
                {
                    var column = new DataGridTextColumn();
                    column.Header = key;
                    myList[index] = key;
                    column.Binding = new System.Windows.Data.Binding($"[{key}]") { StringFormat = "-{0:F2}" };
                    expenseView.AddColumn(column);
                    index++;
                }

                counter++;
            }
            for (int i = 0; i < allTheCategories.Length; i++)
            {
                string collumnName = allTheCategories[i];

                if (myList.Contains(collumnName) == false)
                {
                    var column = new DataGridTextColumn();
                    column.Header = collumnName;
                    expenseView.AddColumn(column);
                }

            }

        }

        public bool AddExpense(string description, string amount, DateTime? date, string category)
        {
            try
            {
                if (!Double.TryParse(amount, out double parsedAmount))
                    throw new InvalidCastException();

                if (String.IsNullOrEmpty(description)) { expenseView.OutputMessage("Description field cannot be empty"); return false; }
                else if (String.IsNullOrEmpty(category)) { expenseView.OutputMessage("Category field cannot be empty"); return false; ; }

                else
                {
                    int categoryId = -1;
                    List<Category> categories = homeBudgetModel.categories.List();
                    foreach (Category categoryInList in categories)
                    {
                        if (categoryInList.Description == category)
                        {
                            categoryId = categoryInList.Id;
                        }
                    }
                    homeBudgetModel.expenses.Add((DateTime)date, categoryId, parsedAmount, description);
                    expenseView.ClearField();
                    return true;
                }
            }
            catch (InvalidCastException)
            {
                expenseView.OutputMessage("Amount field is incorrect");
                return false;
            }
            catch (Exception e) { expenseView.OutputMessage(e.Message); return false; }
        }

        /// <summary>
        /// Updates an expense based on the passed in one if all the information is valid.
        /// </summary>
        /// <param name="expenseToUpdate">The original expense to be modified.</param>
        /// <param name="newDescription">The description for the updated expense.</param>
        /// <param name="newAmount">The amount for the updated expense.</param>
        /// <param name="newDate">The date for the updated expense.</param>
        /// <param name="newCategory">The category for the updated expense.</param>
        /// <returns>True if the expense was updated, false otherwise.</returns>
        public bool UpdateExpense(int expenseToUpdate, string newDescription, string newAmount, DateTime? newDate, string newCategory)
        {
            try
            {
                if (!Double.TryParse(newAmount, out double parsedAmount))
                    throw new InvalidCastException();

                if (String.IsNullOrEmpty(newDescription)) { expenseView.OutputMessage("Description field cannot be empty"); return false; }
                else if (String.IsNullOrEmpty(newCategory)) { expenseView.OutputMessage("Category field cannot be empty"); return false; ; }

                else
                {
                    int categoryId = -1;
                    List<Category> categories = homeBudgetModel.categories.List();
                    foreach (Category categoryInList in categories)
                    {
                        if (categoryInList.Description == newCategory)
                        {
                            categoryId = categoryInList.Id;
                        }
                    }
                    homeBudgetModel.expenses.UpdateProperties(expenseToUpdate, (DateTime)newDate, categoryId, parsedAmount, newDescription);
                    expenseView.ClearField();
                    return true;
                }
            }
            catch (InvalidCastException)
            {
                expenseView.OutputMessage("Amount field is incorrect");
                return false;
            }
            catch (Exception e) { expenseView.OutputMessage(e.Message); return false; }

            
        }



        public void OpenDatabaseForm(string databaseFile)
        {
            try
            {
                Form expenseForm = new Form();
                string[] authorsList = databaseFile.Split('\\');
                File.WriteAllText("./initialize.ini", databaseFile);
                expenseForm.FileWorkingOn("Working on: " + authorsList[authorsList.Length - 1]);
                expenseForm.Show();
                expenseForm.PopulateCategories();
                view.ShowMessage("Opened: " + authorsList[authorsList.Length - 1]);
                
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void OpenDatabaseForm(string databaseFile, string workingOnFile)
        {
            try
            {
                Form expenseForm = new Form();
                File.WriteAllText("./initialize.ini", databaseFile);
                expenseForm.FileWorkingOn("Working on: " + workingOnFile);
                expenseForm.Show();
                expenseForm.PopulateCategories();
                view.ShowMessage("Opened: " + workingOnFile);
                
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<BudgetItemsByMonth> GetByMonthList(DateTime? startDate, DateTime? endDate, bool filterByCat, string catDesc)
        {
            int catId = -1;
            List<Category> categoriesList = homeBudgetModel.categories.List();

            foreach (Category cat in categoriesList)
            {
                if (cat.Description == catDesc)
                {
                    catId = cat.Id;
                }
            }
            List<BudgetItemsByMonth> budgetItems = homeBudgetModel.GetBudgetItemsByMonth(startDate, endDate, filterByCat, catId);
            return budgetItems;
        }
        public List<Dictionary<string, object>> GetBudgetDictionaryByCategoryAndMonth(DateTime? startDate, DateTime? endDate, bool filterByCat, string catDesc)
        {
            int catId = -1;
            List<Category> categoriesList = homeBudgetModel.categories.List();

            foreach (Category cat in categoriesList)
            {
                if (cat.Description == catDesc)
                {
                    catId = cat.Id;
                }
            }
            List<Dictionary<string, object>> budgetItemsByCategoryAndMonth = homeBudgetModel.GetBudgetDictionaryByCategoryAndMonth(startDate, endDate, filterByCat, catId);
            return budgetItemsByCategoryAndMonth;
        }

        public string GetLastFile()
        {
            return File.ReadAllText("./initialize.ini");
        }

        public void deleteExpense(BudgetItem budgetItem)
        {
            homeBudgetModel.expenses.Delete(budgetItem.ExpenseID);
        }

        public void FormatSwitch(bool groupByCategory, bool groupByExpense)
        {
            if(groupByCategory && groupByExpense)
            {
                expenseView.GroupByCategoryAndExpenseFormat();
            }
            else if(groupByCategory)
            {
                expenseView.GroupByCategoryFormat();
            }
            else if(groupByExpense)
            {
                expenseView.GroupByExpenseFormat();
            }
            else
            {
                expenseView.StandardFormat();
            }
        }


    }
}
