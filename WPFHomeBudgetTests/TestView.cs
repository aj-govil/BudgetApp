using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZAMWPFHomeBudget;

namespace ZAMWPFHomeBudgetTests
{
    internal class TestView : ViewInterface
    {
        public bool ShowMessageCalled;

        public void ShowMessage(string message)
        {
            ShowMessageCalled = true;
        }
    }
}
