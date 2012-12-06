using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Collections.ObjectModel;



namespace StockD
{
    
    class StockDViewModel:INotifyPropertyChanged
    {
        #region Members
            private Settings objSettings;
            private ICommand mUpdater;
            Configuration config;
        #endregion

        #region Construction
            public StockDViewModel()
            {
                objSettings = new Settings();
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                StartDate = DateTime.Today;
                EndDate = DateTime.Today; ;


                   
            }
        #endregion

        #region Properties

            public string AppendText
            {
                get
                {
                    return Utility.strLog;
                }

                set
                {
                    Utility.strLog += value;
                    RaisePropertyChanged("AppendText");
                }
            }

            public string TargetFolder
            {
                get
                {
                    return objSettings.TargetFolder;
                }

                set
                {
                    
                    objSettings.TargetFolder = value;

                    RaisePropertyChanged("TargetFolder");
                    config.AppSettings.Settings.Remove("TargetFolder");
                    config.AppSettings.Settings.Add("TargetFolder", TargetFolder);
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");


                }
            }

            public string OutputFolder
            {
                get
                {
                    return objSettings.OutputFolder;
                }

                set
                {
                    objSettings.OutputFolder = value;

                    RaisePropertyChanged("OutputFolder");
                }
            }

            public string AmibrokerExeFolder
            {
                get
                {
                    return objSettings.AmibrokerExeFolder;
                }

                set
                {
                    objSettings.AmibrokerExeFolder = value;

                    RaisePropertyChanged("AmibrokerExeFolder");

                    UpdateAmibrokerExeFolderInConfig();
                }
            }

            public string DatabaseNameFolder
            {
                get
                {
                    return objSettings.DatabaseNameFolder;
                }

                set
                {
                    objSettings.DatabaseNameFolder = value;

                    RaisePropertyChanged("DatabaseNameFolder");

                    config.AppSettings.Settings.Remove("DatabaseNameFolder");
                    config.AppSettings.Settings.Add("DatabaseNameFolder", DatabaseNameFolder);
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");

                }
            }


            public string DatabasePathFolder
            {
                get
                {
                    return objSettings.DatabasePathFolder;
                }

                set
                {
                    objSettings.DatabasePathFolder = value;

                    RaisePropertyChanged("DatabasePathFolder");

                    UpdateDatabasePathInConfig();

                }
            }

            public string OutputFormat
            {
                get
                {
                    return objSettings.OutputFormat;
                }

                set
                {

                    objSettings.OutputFormat = value;

                    RaisePropertyChanged("OutputFormat");
                    config.AppSettings.Settings.Remove("OutputFormat");
                    config.AppSettings.Settings.Add("OutputFormat", OutputFormat);
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");


                }
            }
        public bool ChkIgnoreSunday
            {
                get { return objSettings.ChkIgnoreSunday; }

                set
                {
                    objSettings.ChkIgnoreSunday = value;

                    RaisePropertyChanged("ChkIgnoreSunday");

                    config.AppSettings.Settings.Remove("ChkIgnoreSunday");
                    config.AppSettings.Settings.Add("ChkIgnoreSunday", objSettings.ChkIgnoreSunday.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }
      
            public bool ChkIgnoreSaturday
            {
                get { return objSettings.ChkIgnoreSaturday; }

                set
                {
                    objSettings.ChkIgnoreSaturday = value;

                    RaisePropertyChanged("ChkIgnoreSaturday");

                    config.AppSettings.Settings.Remove("ChkIgnoreSaturday");
                    config.AppSettings.Settings.Add("ChkIgnoreSaturday", objSettings.ChkIgnoreSaturday.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }


            public bool ChkNseEquity
            {
                get { return objSettings.ChkNseEquity; }

                set
                {
                    objSettings.ChkNseEquity = value;

                    RaisePropertyChanged("ChkNseEquity");

                    config.AppSettings.Settings.Remove("ChkNseEquity");
                    config.AppSettings.Settings.Add("ChkNseEquity", objSettings.ChkNseEquity.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }

            public bool ChkNseNcdex
            {
                get { return objSettings.ChkNseNcdex; }

                set
                {
                    objSettings.ChkNseNcdex = value;

                    RaisePropertyChanged("ChkNseNcdex");

                    config.AppSettings.Settings.Remove("ChkNseNcdex");
                    config.AppSettings.Settings.Add("ChkNseNcdex", objSettings.ChkNseNcdex.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }


            public bool ChkNseBulkdeal
            {
                get { return objSettings.ChkNseBulkdeal; }

                set
                {
                    objSettings.ChkNseBulkdeal = value;

                    RaisePropertyChanged("ChkNseBulkdeal");

                    config.AppSettings.Settings.Remove("ChkNseBulkdeal");
                    config.AppSettings.Settings.Add("ChkNseBulkdeal", objSettings.ChkNseBulkdeal.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");

                }
            }

            public bool ChkNseBlockdeal
            {
                get { return objSettings.ChkNseBlockdeal; }

                set
                {
                    objSettings.ChkNseBlockdeal = value;

                    RaisePropertyChanged("ChkNseBlockdeal");

                    config.AppSettings.Settings.Remove("ChkNseBlockdeal");
                    config.AppSettings.Settings.Add("ChkNseBlockdeal", objSettings.ChkNseBlockdeal.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }

            public bool ChkNseFIIFutures
            {
                get { return objSettings.ChkNseFIIFutures; }

                set
                {
                    objSettings.ChkNseFIIFutures = value;

                    RaisePropertyChanged("ChkNseFIIFutures");

                    config.AppSettings.Settings.Remove("ChkNseFIIFutures");
                    config.AppSettings.Settings.Add("ChkNseFIIFutures", objSettings.ChkNseFIIFutures.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }

            public bool ChkNseCombinedReport
            {
                get { return objSettings.ChkNseCombinedReport; }

                set
                {
                    objSettings.ChkNseCombinedReport = value;

                    RaisePropertyChanged("ChkNseCombinedReport");

                    config.AppSettings.Settings.Remove("ChkNseCombinedReport");
                    config.AppSettings.Settings.Add("ChkNseCombinedReport", objSettings.ChkNseCombinedReport.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }

            public bool ChkNseFO
            {
                get { return objSettings.ChkNseFO; }

                set
                {
                    objSettings.ChkNseFO = value;

                    RaisePropertyChanged("ChkNseFO");

                    config.AppSettings.Settings.Remove("ChkNseFO");
                    config.AppSettings.Settings.Add("ChkNseFO", objSettings.ChkNseFO.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }

            public bool chkNseForex
            {
                get { return objSettings.chkNseForex; }

                set
                {
                    objSettings.chkNseForex = value;

                    RaisePropertyChanged("ChkNseForex");

                    config.AppSettings.Settings.Remove("ChkNseForex");
                    config.AppSettings.Settings.Add("ChkNseForex", objSettings.chkNseForex.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }

            public bool ChkBseEquity
            {
                get { return objSettings.ChkBseEquity; }

                set
                {
                    objSettings.ChkBseEquity = value;

                    RaisePropertyChanged("ChkBseEquity");

                    config.AppSettings.Settings.Remove("ChkBseEquity");
                    config.AppSettings.Settings.Add("ChkBseEquity", objSettings.ChkBseEquity.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");

                }
            }

            public bool ChkBseFo
            {
                get { return objSettings.ChkBseFo; }

                set
                {
                    objSettings.ChkBseFo = value;

                    RaisePropertyChanged("ChkBseFo");

                    config.AppSettings.Settings.Remove("ChkBseFo");
                    config.AppSettings.Settings.Add("ChkBseFo", objSettings.ChkBseFo.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");

                }
            }

            public bool ChkYahooIEOD1
            {
                get { return objSettings.ChkYahooIEOD1; }

                set
                {
                    objSettings.ChkYahooIEOD1 = value;

                    RaisePropertyChanged("ChkYahooIEOD1");

                    config.AppSettings.Settings.Remove("ChkYahooIEOD1");
                    config.AppSettings.Settings.Add("ChkYahooIEOD1", objSettings.ChkYahooIEOD1.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }

            public bool ChkYahooIEOD5
            {
                get { return objSettings.ChkYahooIEOD5; }

                set
                {
                    objSettings.ChkYahooIEOD5 = value;

                    RaisePropertyChanged("ChkYahooIEOD5");

                    config.AppSettings.Settings.Remove("ChkYahooIEOD5");
                    config.AppSettings.Settings.Add("ChkYahooIEOD5", objSettings.ChkYahooIEOD5.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }

            public bool ChkYahooEOD
            {
                get { return objSettings.ChkYahooEOD; }

                set
                {
                    objSettings.ChkYahooEOD = value;

                    RaisePropertyChanged("ChkYahooEOD");

                    config.AppSettings.Settings.Remove("ChkYahooEOD");
                    config.AppSettings.Settings.Add("ChkYahooEOD", objSettings.ChkYahooEOD.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }


            public bool ChkYahooFundamental
            {
                get { return objSettings.ChkYahooFundamental; }

                set
                {
                    objSettings.ChkYahooFundamental = value;

                    RaisePropertyChanged("ChkYahooFundamental");

                    config.AppSettings.Settings.Remove("ChkYahooFundamental");
                    config.AppSettings.Settings.Add("ChkYahooFundamental", objSettings.ChkYahooFundamental.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }

            public bool ChkGoogleEOD
            {
                get { return objSettings.ChkGoogleEOD; }

                set
                {
                    objSettings.ChkGoogleEOD = value;

                    RaisePropertyChanged("ChkGoogleEOD");

                    config.AppSettings.Settings.Remove("ChkGoogleEOD");
                    config.AppSettings.Settings.Add("ChkGoogleEOD", objSettings.ChkGoogleEOD.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }

            public bool ChkGoogleIEOD
            {
                get { return objSettings.ChkGoogleIEOD; }

                set
                {
                    objSettings.ChkGoogleIEOD = value;

                    RaisePropertyChanged("ChkGoogleIEOD");

                    config.AppSettings.Settings.Remove("ChkGoogleIEOD");
                    config.AppSettings.Settings.Add("ChkGoogleIEOD", objSettings.ChkGoogleIEOD.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }

            public bool ChkMutualFund
            {
                get { return objSettings.ChkMutualFund; }

                set
                {
                    objSettings.ChkMutualFund = value;

                    RaisePropertyChanged("ChkMutualFund");

                    config.AppSettings.Settings.Remove("ChkMutualFund");
                    config.AppSettings.Settings.Add("ChkMutualFund", objSettings.ChkMutualFund.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }

            public bool ChkIndiaIndices
            {
                get { return objSettings.ChkIndiaIndices; }

                set
                {
                    objSettings.ChkIndiaIndices = value;

                    RaisePropertyChanged("ChkIndiaIndices");

                    config.AppSettings.Settings.Remove("ChkIndiaIndices");
                    config.AppSettings.Settings.Add("ChkIndiaIndices", objSettings.ChkIndiaIndices.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }

            public bool ChkFOP
            {
                get { return objSettings.ChkFOP; }

                set
                {
                    objSettings.ChkFOP = value;

                    RaisePropertyChanged("ChkFOP");

                    config.AppSettings.Settings.Remove("ChkFOP");
                    config.AppSettings.Settings.Add("ChkFOP", objSettings.ChkFOP.ToString());
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }

            public Nullable<DateTime> StartDate
            {
                get
                {
                    return objSettings.StartDate;

                }
                set
                {
                    if (objSettings.StartDate != value)
                    {
                        objSettings.StartDate = value;
                        RaisePropertyChanged("StartDate");
                    }
                }
            }

            public Nullable<DateTime> EndDate
            {
                get
                {
                    return objSettings.EndDate;
                }
                set
                {
                    if (objSettings.EndDate != value)
                    {
                        objSettings.EndDate = value;
                        RaisePropertyChanged("EndDate");
                    }
                }
            } 
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

            bool ValidateTargetFolder()
            {
                bool bRetVal;

                if (objSettings.TargetFolder == null)
                    bRetVal = false;
                else
                {
                    if (!Directory.Exists(objSettings.TargetFolder))
                        bRetVal = false;
                    else
                        bRetVal = true;
                }

                if(!bRetVal)
                    System.Windows.MessageBox.Show("Enter a Valid and Existing Directory Name!");

                return bRetVal;
            }

            bool ValidateDates()
            {
                bool bRetVal = false;

                if ((objSettings.StartDate.HasValue) || (objSettings.EndDate.HasValue))
                {

                    int i = objSettings.StartDate.Value.CompareTo(objSettings.EndDate.Value);

                    if (i > 0)
                    {
                        bRetVal = false;
                        System.Windows.MessageBox.Show("Start Date should be earlier than End Date!");
                    }
                    else
                        bRetVal = true;
                }

                return bRetVal;
            }

            bool ValidateInputs()
            {
                bool bRetVal=true;

                if (!ValidateDates())
                {
                    bRetVal = false;
                    return bRetVal;
                }

                if (!ValidateTargetFolder())
                {
                    bRetVal = false;
                    return bRetVal;
                }

                return bRetVal;

            }
            void RaisePropertyChanged(string propertyName)
            {
                // take a copy to prevent thread issues
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
       
     
            public void GetTargetDirectoryFolder()
            {
                    FolderBrowserDialog browse = new FolderBrowserDialog();

                    DialogResult result = browse.ShowDialog();

                    if (result.ToString() == "OK")
                    {
                        objSettings.TargetFolder = browse.SelectedPath;

                        RaisePropertyChanged("TargetFolder");

                        config.AppSettings.Settings.Remove("TargetFolder");
                        config.AppSettings.Settings.Add("TargetFolder", TargetFolder);
                        config.Save(ConfigurationSaveMode.Full);
                        ConfigurationManager.RefreshSection("appSettings");

                    }

             }

            public void GetOutputDirectoryFolder()
            {
                FolderBrowserDialog browse = new FolderBrowserDialog();

                DialogResult result = browse.ShowDialog();

                if (result.ToString() == "OK")
                {
                    objSettings.OutputFolder = browse.SelectedPath;

                    objSettings.CreateProcessingDirectoryStructure();


                    config.AppSettings.Settings.Remove("OutputFolder");
                    config.AppSettings.Settings.Add("OutputFolder", OutputFolder);
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");

                    RaisePropertyChanged("OutputFolder");

                }

            }

            public void UpdateAmibrokerExeFolderInConfig()
            {
                config.AppSettings.Settings.Remove("AmibrokerExeFolder");
                config.AppSettings.Settings.Add("AmibrokerExeFolder", AmibrokerExeFolder);
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
            }

            public void GetAmibrokerExeFolder()
            {
                FolderBrowserDialog browse = new FolderBrowserDialog();

                DialogResult result = browse.ShowDialog();

                if (result.ToString() == "OK")
                {
                    objSettings.AmibrokerExeFolder = browse.SelectedPath;

                    RaisePropertyChanged("AmibrokerExeFolder");

                    UpdateAmibrokerExeFolderInConfig();


                }

            }


            public void UpdateDatabasePathInConfig()
            {
                config.AppSettings.Settings.Remove("DatabasePathFolder");
                config.AppSettings.Settings.Add("DatabasePathFolder", DatabasePathFolder);
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
            }

            public void GetDatabasePathFolder()
            {
                FolderBrowserDialog browse = new FolderBrowserDialog();

                DialogResult result = browse.ShowDialog();

                if (result.ToString() == "OK")
                {
                    objSettings.DatabasePathFolder = browse.SelectedPath;

                    RaisePropertyChanged("DatabasePathFolder");

                    UpdateDatabasePathInConfig();

                }

            }

            public void AddMessageToLog(string strMessage)
            {
                AppendText = strMessage;
            }
    
         #endregion
     
        #region Commands


            void ExecuteGetTargetDirectoryFolder(object parameter)
            {
                if ((string)parameter == "OutputFolderButton")
                    GetOutputDirectoryFolder();
                else
                if ((string)parameter == "AmibrokerExeFolderButton")
                    GetAmibrokerExeFolder();
                else
                if ((string)parameter == "DatabasePathFolderButton")
                    GetDatabasePathFolder();
                else
                    GetTargetDirectoryFolder();
            }

            bool CanGetTargetDirectoryFolderExecute(object parameter)
            {
                return true;
            }

            public ICommand UpdateTargetDirectory { get { return new RelayCommand<object>(ExecuteGetTargetDirectoryFolder, CanGetTargetDirectoryFolderExecute); } }
            
        
            void ExecuteIsCheckBoxClicked(object parameter)
            {
                if (parameter.Equals("chkNseEquity"))
                {
                    //if (ChkNseEquity)
                    //    ChkNseEquity = false;
                    //else
                    //    ChkNseEquity = true;

                }
                else if (parameter.Equals("ChkNseNcdex"))
                {
                    //if (ChkNseNcdex)
                    //    ChkNseNcdex = false;
                    //else
                    //    ChkNseNcdex = true;

                }
                else if (parameter.Equals("ChkNseBulkdeal"))
                {
                    //if (ChkNseBulkdeal)
                    //    ChkNseBulkdeal = false;
                    //else
                    //    ChkNseBulkdeal = true;

                }

                else if (parameter.Equals("ChkNseBlockdeal"))
                {
                    //if (ChkNseBlockdeal)
                    //    ChkNseBlockdeal = false;
                    //else
                    //    ChkNseBlockdeal = true;

                }

                else if (parameter.Equals("ChkNseFIIFutures"))
                {
                    //if (ChkNseFIIFutures)
                    //    ChkNseFIIFutures = false;
                    //else
                    //    ChkNseFIIFutures = true;

                }
                else if (parameter.Equals("ChkNseCombinedReport"))
                {
                    //if (ChkNseCombinedReport)
                    //    ChkNseCombinedReport = false;
                    //else
                    //    ChkNseCombinedReport = true;

                }

                else if (parameter.Equals("ChkNseFO"))
                {
                    //if (ChkNseFO)
                    //    ChkNseFO = false;
                    //else
                    //    ChkNseFO = true;

                }
                else if (parameter.Equals("ChkNseForex"))
                {
                    //if (chkNseForex)
                    //    chkNseForex = false;
                    //else
                    //    chkNseForex = true;

                }

                else if (parameter.Equals("chkBseEquity"))
                {
                    //if (ChkBseEquity)
                    //    ChkBseEquity = false;
                    //else
                    //    ChkBseEquity = true;

                }

                else if (parameter.Equals("chkBseFo"))
                {
                    //if (ChkBseFo)
                    //    ChkBseFo = false;
                    //else
                    //    ChkBseFo = true;
                }

                else if (parameter.Equals("ChkYahooIEOD1"))
                {
                    //if (ChkYahooIEOD1)
                    //    ChkYahooIEOD1 = false;
                    //else
                    //    ChkYahooIEOD1 = true;
                }

                else if (parameter.Equals("ChkYahooIEOD5"))
                {
                    //if (ChkYahooIEOD5)
                    //    ChkYahooIEOD5 = false;
                    //else
                    //    ChkYahooIEOD5 = true;
                }

                else if (parameter.Equals("ChkYahooEOD"))
                {
                    //if (ChkYahooEOD)
                    //    ChkYahooEOD = false;
                    //else
                    //    ChkYahooEOD = true;
                }

                else if (parameter.Equals("ChkYahooFundamental"))
                {
                    //if (ChkYahooFundamental)
                    //    ChkYahooFundamental = false;
                    //else
                    //    ChkYahooFundamental = true;
                }

                else if (parameter.Equals("ChkGoogleEOD"))
                {
                    //if (ChkGoogleEOD)
                    //    ChkGoogleEOD = false;
                    //else
                    //    ChkGoogleEOD = true;
                }

                else if (parameter.Equals("ChkGoogleIEOD"))
                {
                    //if (ChkGoogleIEOD)
                    //    ChkGoogleIEOD = false;
                    //else
                    //    ChkGoogleIEOD = true;
                }

                else if (parameter.Equals("ChkMutualFund"))
                {
                    //if (ChkMutualFund)
                    //    ChkMutualFund = false;
                    //else
                    //    ChkMutualFund = true;
                }

                else if (parameter.Equals("ChkIndiaIndices"))
                {
                    //if (ChkIndiaIndices)
                    //    ChkIndiaIndices = false;
                    //else
                    //    ChkIndiaIndices = true;
                }

                else if (parameter.Equals("ChkFOP"))
                {
                    //if (ChkFOP)
                    //    ChkFOP = false;
                    //else
                    //    ChkFOP = true;
                }

                else if (parameter.Equals("chkIgnoreSaturday"))
                {
                    //if (ChkIgnoreSaturday)
                    //    ChkIgnoreSaturday = false;
                    //else
                    //    ChkIgnoreSaturday = true;
                }
                else if (parameter.Equals("chkIgnoreSunday"))
                {
                    //if (ChkIgnoreSunday)
                    //    ChkIgnoreSunday = false;
                    //else
                    //    ChkIgnoreSunday = true;
                }

            }

            bool CanIsCheckBoxClicked(object parameter)
            {
                return true;
            }

            public ICommand IsCheckBoxClicked {
                
                get 
                { 
                    
                    return new RelayCommand<object>(ExecuteIsCheckBoxClicked, CanIsCheckBoxClicked); 
                }
                          
            }


            void ExecuteEODControlClicked(object parameter)
            {
                Action<string> mydelegate = AddMessageToLog;

               
                if (parameter.Equals("btnStart"))
                {
                    bool bRetval = false;

                   // if (objSettings.ChkNseEquity)
                   // {

                        if(ValidateInputs())
                            bRetval = true;

                        if (bRetval)
                        {
                            objSettings.Load();


                            EODData.StartButtonClicked(objSettings, AddMessageToLog);
                            AddMessageToLog("Download Finished!\n");
                            //System.Windows.MessageBox.Show("Unzip CM*.* Files Manually in the same folder\n");

                            AddMessageToLog("Unzipping files ...\n");

                            string[] words = null;
                            string strOutputFormat = objSettings.OutputFormat;
                            words = strOutputFormat.Split(',');

                            Process pd = new Process();
                            foreach (string item in words)
                            {
                                if (item == "StdCSV")
                                {
                                    if (objSettings.ChkNseEquity)
                                        pd.ProcessNSEEQUITY(objSettings, AddMessageToLog);

                                    if(objSettings.ChkBseEquity)
                                        pd.ProcessBSEEQUITY(objSettings, AddMessageToLog);
                                }
                            }
                            
                            //pd.ProcessNSEEQUITY(objSettings, AddMessageToLog);
                            //pd.ProcessBSEEQUITY(objSettings, AddMessageToLog);
                            System.Windows.MessageBox.Show("Finished!\n");
                        }
                   // }
                }
                else if (parameter.Equals("btnExit"))
                {
                    App.Current.Shutdown();
                }
            }

            bool CanEODControlClicked(object parameter)
            {
                return true;
            }

            public ICommand EODControlClicked { get { return new RelayCommand<object>(ExecuteEODControlClicked, CanEODControlClicked); } }
        #endregion

   
    }

    public class Node
    {

        public Node(string n) { Title = n; }

        public string Title { get; set; }

        public bool IsSelected { get; set; }

    }

    public class ObservableNodeList : ObservableCollection<Node>
    {

        public ObservableNodeList()
        {

        }

        public override string ToString()
        {

            StringBuilder outString = new StringBuilder();

            foreach (Node s in this.Items)
            {

                if (s.IsSelected == true)
                {

                    outString.Append(s.Title);

                    outString.Append(',');

                }

            }

            return outString.ToString().TrimEnd(new char[] { ',' });

        }

    }
 
}
