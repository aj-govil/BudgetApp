using Budget;
using ZAMWPFHomeBudget;

namespace ZAMWPFHomeBudgetTests
{
    public class PresenterTests
    {
        [Fact]
        public void TestGetCategories_AllDescriptionsMatch()
        {
            TestView testView = new TestView();
            Presenter presenter = new Presenter(testView, "testDb1", true);
            HomeBudget homeBudget = new HomeBudget("testDb1", false);
            List<Category> categories = homeBudget.categories.List();

            string[] returnedCategories = presenter.GetCategories();

            
            for(int i=0;i<returnedCategories.Length;i++)
            {
                Assert.True(returnedCategories[i] == categories[i].Description);
            }
            
        }

        [Fact]
        public void TestCreateCategory_ValidCategory() 
        { 
            TestView testView = new TestView();
            Presenter presenter1 = new Presenter(testView, "testDB2", true);
            TestExpenseView testExpenseView = new TestExpenseView();
            Presenter presenter2 = new Presenter(testExpenseView);

            bool status = presenter2.createCategory("New Category", "Expense");
            Assert.True(status);

            HomeBudget homeBudget = new HomeBudget("testDb2", false);

            List<Category> categories = homeBudget.categories.List();
            string[] returnedCategories = presenter2.GetCategories();

            for (int i = 0; i < returnedCategories.Length; i++)
            {
                Assert.True(returnedCategories[i] == categories[i].Description);
            }

            Assert.True(returnedCategories[returnedCategories.Length - 1] == "New Category");
            Assert.True(categories[returnedCategories.Length - 1].Description == "New Category");
            Assert.True(categories[returnedCategories.Length - 1].Type == Category.CategoryType.Expense);
        }

        [Fact]
        public void TestCreateCategory_EmptyName()
        {
            TestView testView = new TestView();
            Presenter presenter = new Presenter(testView, "testDb3", true);
            HomeBudget homeBudget = new HomeBudget("testDb3", false);

            bool status = presenter.createCategory("", "Expense");
            Assert.False(status);

            List<Category> categories = homeBudget.categories.List();
            string[] returnedCategories = presenter.GetCategories();

            for (int i = 0; i < returnedCategories.Length; i++)
            {
                Assert.True(returnedCategories[i] == categories[i].Description);
            }

            Assert.False(returnedCategories[returnedCategories.Length - 1] == "");
            Assert.False(categories[returnedCategories.Length - 1].Description == "");
        }

        [Fact]
        public void TestCreateCategory_EmptyType()
        {
            TestView testView = new TestView();
            Presenter presenter = new Presenter(testView, "testDb4", true);
            HomeBudget homeBudget = new HomeBudget("testDb4", false);

            bool status = presenter.createCategory("New Category", "");
            Assert.False(status);

            List<Category> categories = homeBudget.categories.List();
            string[] returnedCategories = presenter.GetCategories();

            for (int i = 0; i < returnedCategories.Length; i++)
            {
                Assert.True(returnedCategories[i] == categories[i].Description);
            }

            Assert.False(returnedCategories[returnedCategories.Length - 1] == "New Category");
            Assert.False(categories[returnedCategories.Length - 1].Description == "New Category");
        }

        [Fact]
        public void TestCreateCategory_InvalidType()
        {
            TestView testView = new TestView();
            Presenter presenter = new Presenter(testView, "testDb5", true);
            HomeBudget homeBudget = new HomeBudget("testDb5", false);

            bool status = presenter.createCategory("New Category", "Random");
            Assert.False(status);

            List<Category> categories = homeBudget.categories.List();
            string[] returnedCategories = presenter.GetCategories();

            for (int i = 0; i < returnedCategories.Length; i++)
            {
                Assert.True(returnedCategories[i] == categories[i].Description);
            }

            Assert.False(returnedCategories[returnedCategories.Length - 1] == "New Category");
            Assert.False(categories[returnedCategories.Length - 1].Description == "New Category");
        }

        [Fact]
        public void TestAddExpense_ValidExpense()
        {
            TestView testView = new TestView();
            Presenter presenter = new Presenter(testView, "testDb6", true);
            HomeBudget homeBudget = new HomeBudget("testDb6", false);
            TestExpenseView testExpenseView = new TestExpenseView();
            Presenter presenter2 = new Presenter(testExpenseView);
            DateTime dateTime= DateTime.Now;

            presenter2.createCategory("Tester Category", "Expense");
            int catId = presenter2.GetCategories().Count();
            Assert.True(homeBudget.categories.GetCategoryFromId(catId).Description == "Tester Category");

            bool status = presenter2.AddExpense("New Expense", "12.99", dateTime, "Tester Category");

            Assert.True(status);
            
            List<Expense> expenses = homeBudget.expenses.List();

            Assert.True(expenses.Count == 1);
            Assert.True(expenses[0].Description == "New Expense");
            Assert.True(expenses[0].Date.DayOfYear == dateTime.DayOfYear);
            Assert.True(expenses[0].Amount == 12.99);
            Assert.True(expenses[0].Category == catId);
            
        }

        [Fact]
        public void TestAddExpense_NonDoubleAmount()
        {
            TestView testView = new TestView();
            Presenter presenter = new Presenter(testView, "testDb7", true);
            HomeBudget homeBudget = new HomeBudget("testDb7", false);
            TestExpenseView testExpenseView = new TestExpenseView();
            Presenter presenter2 = new Presenter(testExpenseView);
            DateTime dateTime = DateTime.Now;

            presenter2.createCategory("Tester Category", "Expense");
            int catId = presenter2.GetCategories().Count();
            Assert.True(homeBudget.categories.GetCategoryFromId(catId).Description == "Tester Category");

            bool status = presenter2.AddExpense("New Expense", "Red", dateTime, "Tester Category");

            Assert.False(status);
            Assert.True(testExpenseView.outputMessageCalled);
            List<Expense> expenses = homeBudget.expenses.List();

            Assert.True(expenses.Count == 0);

        }

        [Fact]
        public void TestAddExpense_NoDescription()
        {
            TestView testView = new TestView();
            Presenter presenter = new Presenter(testView, "testDb8", true);
            HomeBudget homeBudget = new HomeBudget("testDb8", false);
            TestExpenseView testExpenseView = new TestExpenseView();
            Presenter presenter2 = new Presenter(testExpenseView);
            DateTime dateTime = DateTime.Now;

            presenter2.createCategory("Tester Category", "Expense");
            int catId = presenter2.GetCategories().Count();
            Assert.True(homeBudget.categories.GetCategoryFromId(catId).Description == "Tester Category");

            bool status = presenter2.AddExpense("", "12.99", dateTime, "Tester Category");

            Assert.False(status);
            Assert.True(testExpenseView.outputMessageCalled);
            List<Expense> expenses = homeBudget.expenses.List();

            Assert.True(expenses.Count == 0);

        }

        [Fact]
        public void OpenDatabaseForm_InvalidFile()
        {
            TestView testView = new TestView();
            Presenter presenter = new Presenter(testView, "testDb9", true);

            Assert.Throws<InvalidOperationException>(() => presenter.OpenDatabaseForm(""));

        }

        [Fact]
        public void GetLastFile_NoLastFile()
        {
            TestView testView = new TestView();
            Presenter presenter = new Presenter(testView, "testDb10", true);

            Assert.Throws<FileNotFoundException>(() => presenter.GetLastFile());

        }

        [Fact]
        public void TestUpdateExpense_ValidExpense()
        {
            TestView testView = new TestView();
            Presenter presenter = new Presenter(testView, "testDb11", true);
            HomeBudget homeBudget = new HomeBudget("testDb11", false);
            TestExpenseView testExpenseView = new TestExpenseView();
            Presenter presenter2 = new Presenter(testExpenseView);
            DateTime dateTime = DateTime.Now;

            presenter2.createCategory("Tester Category", "Expense");
            int catId = presenter2.GetCategories().Count();

            presenter2.AddExpense("New Expense", "12.99", dateTime, "Tester Category");

            List<Expense> expenses = homeBudget.expenses.List();

            dateTime = dateTime.AddDays(2);

            presenter2.createCategory("Other Category", "Expense");
            int catId2 = presenter2.GetCategories().Count();

            bool status = presenter2.UpdateExpense(expenses[0].Id, "Old Expense", "13", dateTime, "Other Category");
            Assert.True(status);
            Assert.False(testExpenseView.outputMessageCalled);

            expenses = homeBudget.expenses.List();

            Assert.True(expenses.Count == 1);
            Assert.True(expenses[0].Description == "Old Expense");
            Assert.True(expenses[0].Date.DayOfYear == dateTime.DayOfYear);
            Assert.True(expenses[0].Amount == 13);
            Assert.True(expenses[0].Category == catId2);
        }


        [Fact]
        public void TestUpdateExpense_InvalidName()
        {
            TestView testView = new TestView();
            Presenter presenter = new Presenter(testView, "testDb12", true);
            HomeBudget homeBudget = new HomeBudget("testDb12", false);
            TestExpenseView testExpenseView = new TestExpenseView();
            Presenter presenter2 = new Presenter(testExpenseView);
            DateTime dateTime = DateTime.Now;

            presenter2.createCategory("Tester Category", "Expense");
            int catId = presenter2.GetCategories().Count();

            presenter2.AddExpense("New Expense", "12.99", dateTime, "Tester Category");

            List<Expense> expenses = homeBudget.expenses.List();

            DateTime newDate = dateTime.AddDays(2);

            presenter2.createCategory("Other Category", "Expense");

            bool status = presenter2.UpdateExpense(expenses[0].Id, "", "13", newDate, "Other Category");
            Assert.False(status);
            Assert.True(testExpenseView.outputMessageCalled);

            expenses = homeBudget.expenses.List();

            Assert.True(expenses.Count == 1);
            Assert.True(expenses[0].Description == "New Expense");
            Assert.True(expenses[0].Date.DayOfYear == dateTime.DayOfYear);
            Assert.True(expenses[0].Amount == 12.99);
            Assert.True(expenses[0].Category == catId);
        }

        [Fact]
        public void TestUpdateExpense_InvalidAmount()
        {
            TestView testView = new TestView();
            Presenter presenter = new Presenter(testView, "testDb13", true);
            HomeBudget homeBudget = new HomeBudget("testDb13", false);
            TestExpenseView testExpenseView = new TestExpenseView();
            Presenter presenter2 = new Presenter(testExpenseView);
            DateTime dateTime = DateTime.Now;

            presenter2.createCategory("Tester Category", "Expense");
            int catId = presenter2.GetCategories().Count();

            presenter2.AddExpense("New Expense", "12.99", dateTime, "Tester Category");

            List<Expense> expenses = homeBudget.expenses.List();

            DateTime newDate = dateTime.AddDays(2);

            presenter2.createCategory("Other Category", "Expense");

            bool status = presenter2.UpdateExpense(expenses[0].Id, "Old Expense", "abc", newDate, "Other Category");
            Assert.False(status);
            Assert.True(testExpenseView.outputMessageCalled);

            expenses = homeBudget.expenses.List();

            Assert.True(expenses.Count == 1);
            Assert.True(expenses[0].Description == "New Expense");
            Assert.True(expenses[0].Date.DayOfYear == dateTime.DayOfYear);
            Assert.True(expenses[0].Amount == 12.99);
            Assert.True(expenses[0].Category == catId);
        }

        [Fact]
        public void TestUpdateExpense_InvalidCategory()
        {
            TestView testView = new TestView();
            Presenter presenter = new Presenter(testView, "testDb14", true);
            HomeBudget homeBudget = new HomeBudget("testDb14", false);
            TestExpenseView testExpenseView = new TestExpenseView();
            Presenter presenter2 = new Presenter(testExpenseView);
            DateTime dateTime = DateTime.Now;

            presenter2.createCategory("Tester Category", "Expense");
            int catId = presenter2.GetCategories().Count();

            presenter2.AddExpense("New Expense", "12.99", dateTime, "Tester Category");

            List<Expense> expenses = homeBudget.expenses.List();

            DateTime newDate = dateTime.AddDays(2);

            bool status = presenter2.UpdateExpense(expenses[0].Id, "Old Expense", "12.99", newDate, "");
            Assert.False(status);
            Assert.True(testExpenseView.outputMessageCalled);

            expenses = homeBudget.expenses.List();

            Assert.True(expenses.Count == 1);
            Assert.True(expenses[0].Description == "New Expense");
            Assert.True(expenses[0].Date.DayOfYear == dateTime.DayOfYear);
            Assert.True(expenses[0].Amount == 12.99);
            Assert.True(expenses[0].Category == catId);
        }


    }
}