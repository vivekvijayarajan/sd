using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockD
{
    class StartUp
    {

        [STAThread]
        static void Main()
        {
            App app = new App();
            StockDViewModel sm = new StockDViewModel();

            app.InitializeComponent();

            
            
            app.Run();
  
        }
    }
}
