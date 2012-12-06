using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Configuration;

namespace StockD
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string[] Outputarr = {"Amibroker", "Metastock", "Ninja", "MetaTrader4", "MetaTrader5", "FCharts", "AdvanceGet" };
            ObservableNodeList itemSource = new ObservableNodeList();


            Node firstnode = new Node("StdCSV");
            firstnode.IsSelected = true;
  


            string strOutputFormat = ConfigurationManager.AppSettings["OutputFormat"];
            string[] words = null;
            if (strOutputFormat != null)
            {
                words = strOutputFormat.Split(',');
                firstnode.IsSelected = false;    // First node may NOT be selected
            }

            itemSource.Add(firstnode);

            foreach (string item in Outputarr)
            {
                Node tmp = new Node(item);
                tmp.IsSelected = false;
                itemSource.Add(tmp);

            }

            foreach (string obj in words)
            {
                for (int i = 0; i < itemSource.Count; i++)
                {
                    Node tmpnode = itemSource[i];
                    if (tmpnode.Title == obj)
                        itemSource[i].IsSelected = true;
                }
                
            }

  
            this.Tools.ItemsSource = itemSource;


            InitCheckedboxValues();


        }

        public void InitCheckedboxValues()
        {
            string chktmp = ConfigurationManager.AppSettings["ChkBseEquity"];
            bool btemp = false;
            if(chktmp != null)
                btemp = bool.Parse(chktmp);
            this.ChkBSEEquity.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["ChkBseFo"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.ChkBseFo.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["ChkIndiaIndices"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.ChkIndiaIndices.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["chkYahooEOD"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.chkYahooEOD.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["chkYahooFundamental"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.chkYahooFundamental.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["ChkYahooIEOD1"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.ChkYahooIEOD1.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["ChkYahooIEOD5"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.ChkYahooIEOD5.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["ChkAddOIE"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.ChkAddOIE.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["chkNseEquity"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.chkNseEquity.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["chkNseFo"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.chkNseFo.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["chkNseForex"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.chkNseForex.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["chkNseNcdex"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.chkNseNcdex.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["chkNseBulkdeal"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.chkNseBulkdeal.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["chkNseBlockdeal"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.chkNseBlockdeal.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["chkNseFIIFutures"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.chkNseFIIFutures.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["ChkFOP"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.ChkFOP.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["chkCombinedReport"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.chkCombinedReport.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["ChkMcxEquity"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.ChkMcxEquity.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["ChkMcxCom"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.ChkMcxCom.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["ChkMcxForex"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.ChkMcxForex.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["ChkMutualFund"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.ChkMutualFund.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["ChkUseForex"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.ChkUseForex.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["ChkNatStoExc"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.ChkNatStoExc.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["ChkGoogleEOD"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.ChkGoogleEOD.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["ChkGoogleIEOD"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.ChkGoogleIEOD.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["chkSelectAll"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.chkSelectAll.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["chkIgnoreSaturday"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.chkIgnoreSaturday.IsChecked = btemp;

            chktmp = ConfigurationManager.AppSettings["chkIgnoreSunday"];
            btemp = false;
            if (chktmp != null)
                btemp = bool.Parse(chktmp);
            this.chkIgnoreSunday.IsChecked = btemp;

        }
        
     }
}
