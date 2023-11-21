using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ZAMWPFHomeBudget
{
    public interface ExpenseViewInterface
    {
        void PopulateCategories();
        
        void FileWorkingOn(string file);

        void ClearField();

        void OutputMessage(string message);

        void GroupByExpenseFormat();

        void GroupByCategoryFormat();

        void GroupByCategoryAndExpenseFormat();

        void StandardFormat();

        void AddColumn(DataGridTextColumn newColumn);
    }
}
