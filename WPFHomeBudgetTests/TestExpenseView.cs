using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ZAMWPFHomeBudget;

namespace ZAMWPFHomeBudgetTests
{
    internal class TestExpenseView : ExpenseViewInterface
    {
        public bool outputMessageCalled = false;
        public bool populateCategoriesCalled = false;

        public void AddColumn(DataGridTextColumn newColumn)
        {
            
        }

        public void ClearField()
        {

        }

        public void FileWorkingOn(string file)
        {

        }

        public void GroupByCategoryAndExpenseFormat()
        {
            throw new NotImplementedException();
        }

        public void GroupByCategoryFormat()
        {
            throw new NotImplementedException();
        }

        public void GroupByExpenseFormat()
        {
            throw new NotImplementedException();
        }

        public void OutputMessage(string message)
        {
            outputMessageCalled= true;
        }

        public void PopulateCategories()
        {
            populateCategoriesCalled= true;
        }

        public void StandardFormat()
        {
            throw new NotImplementedException();
        }
    }
}
