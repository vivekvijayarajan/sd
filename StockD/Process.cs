using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;
using System.IO;
using System.Globalization;
using FileHelpers.RunTime;
using System.Data;

namespace StockD
{
    class Process
    {
        public void Init()
        {
        }

        string GetFileNameWithPath(string[] strMTOArr, string strMTOFileNAme)
        {
            for (int i = 0; i < strMTOArr.Length; i++)
                if (((strMTOArr[i]).ToUpper()).Contains(strMTOFileNAme.ToUpper()))
                    return strMTOArr[i];

            return null;
        }

        public DelimitedClassBuilder BuildNSECMPFile()
        {
            DelimitedClassBuilder cb = new DelimitedClassBuilder("CMPFILE", ",");

            cb.IgnoreFirstLines = 0;

            
            cb.AddField("Symbol", typeof(string));
            cb.AddField("Series", typeof(string));
            cb.AddField("Open", typeof(double));
            cb.AddField("High", typeof(double));
            cb.AddField("Low", typeof(double));
            cb.AddField("Close", typeof(double));
            cb.AddField("Last", typeof(double));
            cb.AddField("PrevClose", typeof(double));
            cb.AddField("Tottrdqty", typeof(int));
            cb.AddField("Tottrdval", typeof(double));
            cb.AddField("Timestamp", typeof(string));
            cb.AddField("Totaltrades", typeof(int));
            cb.AddField("Isin", typeof(string));
            cb.AddField("OI", typeof(int));
            cb.LastField.FieldNullValue = 0;

            return cb;
        }

        public void ExecuteNSEEQUITYProcessing(string[] strMTOArr, string[] strNSEArr, string strNSESEC, string strOutputFormat, string strOutputFolder, Action<string> AddMessageToLog)
        {
            FileHelperEngine engineMTO = new FileHelperEngine(typeof(NSEMTO));

            DelimitedClassBuilder cb = BuildNSECMPFile();
            FileHelperEngine engineCMP = new FileHelperEngine(typeof(NSECMP));

            FileHelperEngine engineSEC = new FileHelperEngine(typeof(NSESEC));

            foreach (string obj in strNSEArr)
            {
                
                //Get NSE Equity Filename day, month, year
                int index = obj.IndexOf("cm");

                string day = obj.Substring(index+2, 2);
                string monthname = obj.Substring(index+4, 3);
                string year = obj.Substring(index+7,4);
                int month = Convert.ToDateTime("01-" + monthname + "-2011").Month;

                if (month < 10)
                    monthname = "0";
                else
                    monthname = "";
                monthname += month.ToString();

                string MTOfilename = "MTO_" + day + monthname + year + ".DAT";

                string MTOfilenamewithpath = GetFileNameWithPath(strMTOArr, MTOfilename);

                if (!File.Exists(MTOfilenamewithpath))
                {
                    AddMessageToLog("File " + MTOfilenamewithpath + " does not exist!");
                    continue;
                }

                NSEMTO[] resmto = engineMTO.ReadFile(MTOfilenamewithpath) as NSEMTO[];


                if (!File.Exists(obj))
                {
                    AddMessageToLog("File " + obj + " does not exist!");
                    continue;
                }

                NSECMP[] rescmp = engineCMP.ReadFile(obj) as NSECMP[];

                if (!File.Exists(strNSESEC))
                {
                    AddMessageToLog("File " + strNSESEC + " does not exist!");
                    continue;
                }
                
                NSESEC[] ressec = engineSEC.ReadFile(strNSESEC) as NSESEC[];

                int iTotalRows = rescmp.Length;


                for (int i = 0; i < iTotalRows; i++)
                {
                    if (rescmp[i].Series == "EQ" || rescmp[i].Series == "BE")
                    {

                        //Copy OI from MTO
                        for (int j = 0; j < resmto.Length; j++)
                        {
                            if ((resmto[j].NameOfSecurity == (string)rescmp[i].Symbol) && (resmto[j].series == (string)rescmp[i].Series))
                            {

                                rescmp[i].OI = resmto[j].DeliverableQty;
                                break;
                            }
                        }

                        //Copy Security Name from SEC
                        for (int j = 0; j < ressec.Length; j++)
                        {
                            if ((ressec[j].Symbol == (string)rescmp[i].Symbol))
                            {
                                rescmp[i].SecurityName = ressec[j].SecurityName;
                                break;
                            }
                        }

                    }
              
                }

               
                //engineCMP.HeaderText = "Symbol,Series,Open,High,Low,Close,Last,PrevClose,Tottrdqty,Tottrdval,Timestamp,Totaltrades,Isin,OI,SecurityName";
                
                //Dump File data
                engineCMP.HeaderText = "Ticker,Series,Open,High,Low,Close,Last,PrevClose,Volume,Tottrdval,Date,Totaltrades,Isin,OPENINT,NAME";
                engineCMP.WriteFile(obj, rescmp);

                int totrows = 0;

                int itmp = 0;
                int cnt = 0;
                //Calculate number of rows which have series as EQ or BE and are not NULL
                while(cnt < rescmp.Length)
                {
                    if (rescmp[cnt].Series == "EQ" || rescmp[cnt].Series == "BE")
                        totrows++;

                    cnt++;
                }

                NSECMPFINAL[] finalarr = new NSECMPFINAL[totrows];
                DateTime myDate;
                itmp = 0;
                int icntr = 0;
                while (icntr < rescmp.Length)
                {
                    if (rescmp[icntr].Series == "EQ" || rescmp[icntr].Series == "BE")
                    {
                        finalarr[itmp] = new NSECMPFINAL();
                        finalarr[itmp].Ticker = rescmp[icntr].Symbol;
                        finalarr[itmp].Name = rescmp[icntr].SecurityName;

                        myDate = DateTime.Parse(rescmp[icntr].Timestamp);
                        finalarr[itmp].Date = String.Format("{0:yyyyMMdd}", myDate);
                        finalarr[itmp].Open = rescmp[icntr].Open;
                        finalarr[itmp].High = rescmp[icntr].High;
                        finalarr[itmp].Low = rescmp[icntr].Low;
                        finalarr[itmp].Close = rescmp[icntr].Close;
                        finalarr[itmp].Volume = rescmp[icntr].Tottrdqty;
                        finalarr[itmp].OpenInt = rescmp[icntr].OI;

                        itmp++;
                    }
                    icntr++;
                }

                FileHelperEngine engineCMPFINAL = new FileHelperEngine(typeof(NSECMPFINAL));
                engineCMPFINAL.HeaderText = "Ticker,Name,Date,Open,High,Low,Close,Volume,Openint";
                engineCMPFINAL.WriteFile(obj, finalarr);

                //FileHelpers.CsvOptions options = new FileHelpers.CsvOptions("ImportRecord", ',', obj);
                //options.HeaderLines = 1;
                //FileHelperEngine test = new FileHelpers.CsvEngine(options);
                ////DataTable header = test.ReadStringAsDT(FileHelpers.CommonEngine.RawReadFirstLines(obj, 1));
                ////test.Options.IgnoreFirstLines = 0;
                //DataTable dttest = test.ReadFileAsDT(obj);

                string[] words = null;
                words = strOutputFormat.Split(',');

                //Get Filename
                index = obj.IndexOf("cm");

                string fname = obj.Substring(index, 19);

                
                string folder;
                foreach (string item in words)
                {
                    string outputfoldername = Path.Combine(strOutputFolder, item, "NSE", "Equity");

                    if (item == "StdCSV" || item == "Metastock" || item == "Ninja" || item == "FCharts")
                    {
                        folder = Path.Combine(strOutputFolder, item, "NSE", "Equity", fname);

                        if (!Directory.Exists(outputfoldername))
                            AddMessageToLog("Directory " + outputfoldername + " does not exist!");
                        else
                            File.Copy(obj, folder, true);
                    }
                    else
                        if (item == "Amibroker")
                        {
                            engineCMPFINAL.Options.IgnoreFirstLines = 1;
                            engineCMPFINAL.WriteFile(obj, finalarr);

                            folder = Path.Combine(strOutputFolder, item, "NSE", "Equity", fname);


                            if (!Directory.Exists(outputfoldername))
                                AddMessageToLog("Directory " + outputfoldername + " does not exist!");
                            else                            
                                File.Copy(obj, folder, true);
                        }
                }
                File.Delete(obj);

            }


        }

        public void ProcessNSEEQUITY(Settings objSettings, Action<string> AddMessageToLog)
        {

            AddMessageToLog("Starting Processing...\n");

            string NSEMTOFolder = Path.Combine(objSettings.TargetFolder, "EODData", "NSE", "MTO");
            string NSEEQUITYFolder = Path.Combine(objSettings.TargetFolder, "EODData", "NSE", "NSE Equity");
            string NSEESECFILE = Path.Combine(objSettings.TargetFolder, "EODData", "NSE", "SEC", "sec_list.csv");

            var allmtocsv_files_list = new List<string>();
            var allnsecsv_files_list = new List<string>();

            int count = 0;
            string[] yearfolders;

            while (count < 2)
            {
                //Get all year folders present under this folder

                if (count == 0)
                {
                    yearfolders = Directory.GetDirectories(NSEMTOFolder);
                }
                else
                {
                    yearfolders = Directory.GetDirectories(NSEEQUITYFolder);
                }


                foreach (string yearfolder in yearfolders)
                {
                    //Get all month folders present under this year folder

                    string[] monthfolders;
                    if (count == 0)
                    {
                        monthfolders = Directory.GetDirectories(Path.Combine(NSEMTOFolder, yearfolder));
                    }
                    else
                    {
                        monthfolders = Directory.GetDirectories(Path.Combine(NSEEQUITYFolder, yearfolder));
                    }

                    foreach (string monthfolder in monthfolders)
                    {

                        Utility.UnzipFiles(monthfolder);
                        //Get all MTO files present under this month folder
                        string[] mtocsv_files = null, nsecsv_files = null;

                        if (count == 0)
                            mtocsv_files = Directory.GetFiles(Path.Combine(monthfolder), "*.DAT");
                        else
                            nsecsv_files = Directory.GetFiles(Path.Combine(monthfolder), "*.csv");

                        if (count == 0)
                            allmtocsv_files_list.AddRange(mtocsv_files);
                        else
                            allnsecsv_files_list.AddRange(nsecsv_files);

                    }
                }
                count++;
            }

            string[] allmtocsv_files_array = allmtocsv_files_list.ToArray();
            string[] allnsecsv_files_array = allnsecsv_files_list.ToArray();



            ExecuteNSEEQUITYProcessing(allmtocsv_files_array, allnsecsv_files_array, NSEESECFILE, objSettings.OutputFormat, objSettings.OutputFolder, AddMessageToLog);

            AddMessageToLog("Processing Finished\n");   
        }

        public void ProcessBSEEQUITY(Settings objSettings, Action<string> AddMessageToLog)
        {

            AddMessageToLog("Starting BEEQUITY Processing...\n");

            string BSESCBFolder = Path.Combine(objSettings.TargetFolder, "EODData", "BSE", "BSE_MTO");
            string BSEEQUITYFolder = Path.Combine(objSettings.TargetFolder, "EODData", "BSE", "BSE_EQUITY");
            //string NSEESECFILE = Path.Combine(objSettings.TargetFolder, "EODData", "NSE", "SEC", "sec_list.csv");

            var allscbtxt_files_list = new List<string>();
            var allbsecsv_files_list = new List<string>();

            int count = 0;
            string[] yearfolders;

            while (count < 2)
            {
                //Get all year folders present under this folder

                if (count == 0)
                {
                    yearfolders = Directory.GetDirectories(BSESCBFolder);
                }
                else
                {
                    yearfolders = Directory.GetDirectories(BSEEQUITYFolder);
                }


                foreach (string yearfolder in yearfolders)
                {
                    //Get all month folders present under this year folder

                    string[] monthfolders;
                    if (count == 0)
                    {
                        monthfolders = Directory.GetDirectories(Path.Combine(BSESCBFolder, yearfolder));
                    }
                    else
                    {
                        monthfolders = Directory.GetDirectories(Path.Combine(BSEEQUITYFolder, yearfolder));
                    }

                    foreach (string monthfolder in monthfolders)
                    {

                        Utility.UnzipFiles(monthfolder);

                        //Get all MTO files present under this month folder
                        string[] scbtxt_files = null, bsecsv_files = null;

                        if (count == 0)
                            scbtxt_files = Directory.GetFiles(Path.Combine(monthfolder), "*.txt");
                        else
                            bsecsv_files = Directory.GetFiles(Path.Combine(monthfolder), "*.csv");

                        if (count == 0)
                            allscbtxt_files_list.AddRange(scbtxt_files);
                        else
                            allbsecsv_files_list.AddRange(bsecsv_files);

                    }
                }
                count++;
            }

            string[] allscbtxt_files_array = allscbtxt_files_list.ToArray();
            string[] allbsecsv_files_array = allbsecsv_files_list.ToArray();



            ExecuteBSEEQUITYProcessing(allbsecsv_files_array, allscbtxt_files_array, objSettings.OutputFormat, objSettings.OutputFolder, AddMessageToLog);

            AddMessageToLog("Processing BSEEQUITY Finished\n");
        }

        public void ExecuteBSEEQUITYProcessing(string[] strBSECSVArr, string[] strSCBTXTArr, string strOutputFormat, string strOutputFolder, Action<string> AddMessageToLog)
        {
            FileHelperEngine engineBSECSV = new FileHelperEngine(typeof(BSECSV));

            DelimitedClassBuilder cb = BuildNSECMPFile();
            FileHelperEngine engineSCBTXT = new FileHelperEngine(typeof(SCBTXT));


            foreach (string obj in strBSECSVArr)
            {

                //Get BSE Equity Filename day, month, year
                string [] words = obj.Split('\\');

                string strbseequityfilename = words[words.Length - 1];
                string strday = strbseequityfilename.Substring(2, 2);
                string strmon = strbseequityfilename.Substring(4, 2);
                string stryear = strbseequityfilename.Substring(6, 2);

                int index = obj.IndexOf("EQ");
                string dt = strbseequityfilename.Substring(2,6);

                string scbtxtfilename = "SCBSEALL" + strbseequityfilename.Substring(2, 4) + ".TXT";

                 if (!File.Exists(obj))
                 {
                     AddMessageToLog("File " + strbseequityfilename + " does not exist!");
                    continue;
                 }



                string SCBSETXTfilenamewithpath = GetFileNameWithPath(strSCBTXTArr, scbtxtfilename);

                if (!File.Exists(SCBSETXTfilenamewithpath))
                {
                    AddMessageToLog("File " + scbtxtfilename + " does not exist!");
                    continue;
                }

                BSECSV[] resbsecsv = engineBSECSV.ReadFile(obj) as BSECSV[];
        
    
                
       
                SCBTXT[] resscbtxt = engineSCBTXT.ReadFile(SCBSETXTfilenamewithpath) as SCBTXT[];
      



                int iTotalRows = resbsecsv.Length;


                for (int i = 0; i < iTotalRows; i++)
                {
   
                        //Copy OI from MTO
                        for (int j = 0; j < resscbtxt.Length; j++)
                        {
                            if (resbsecsv[i].sc_code == resscbtxt[j].scripcode)
                            {

                                resbsecsv[i].openint = resscbtxt[j].deliveryqty;
                                break;
                            }
                        }

                }

                int totrows = 0;

                int itmp = 0;
                int cnt = 0;

                BSECSVFINAL[] finalarr = new BSECSVFINAL[resbsecsv.Length];
                DateTime myDate;
                itmp = 0;
                int icntr = 0;
                while (icntr < resbsecsv.Length)
                {
                        finalarr[icntr] = new BSECSVFINAL();
                        finalarr[icntr].ticker = resbsecsv[icntr].sc_code;
                        finalarr[icntr].name = resbsecsv[icntr].sc_name;

                        //myDate = Convert.ToDateTime(dt);
                        //myDate = DateTime.ParseExact(dt, "ddMMyyyy", CultureInfo.InvariantCulture);

                        //myDate=Convert.ToDateTime(strday + "-"+ strmon + "-20" + stryear);
                        //finalarr[itmp].date = myDate.ToString("yyyyMMdd"); //String.Format("{0:yyyyMMdd}", dt);
                        finalarr[icntr].date = "20" + stryear + strmon + strday; // String.Format("{0:yyyyMMdd}", myDate);
                        finalarr[icntr].open = resbsecsv[icntr].open;
                        finalarr[icntr].high = resbsecsv[icntr].high;
                        finalarr[icntr].low = resbsecsv[icntr].low;
                        finalarr[icntr].close = resbsecsv[icntr].close;
                        finalarr[icntr].volume = resbsecsv[icntr].no_of_shrs;
                        if ((resbsecsv[icntr].openint) == null)
                            resbsecsv[icntr].openint = 0;
                        finalarr[icntr].openint = resbsecsv[icntr].openint;  //enint;


                        icntr++;
                }

                FileHelperEngine engineBSECSVFINAL = new FileHelperEngine(typeof(BSECSVFINAL));
                engineBSECSVFINAL.HeaderText = "Ticker,Name,Date,Open,High,Low,Close,Volume,OPENINT";
                engineBSECSVFINAL.WriteFile(obj, finalarr);



                string folder;
                words = null;
                words = strOutputFormat.Split(',');
                foreach (string item in words)
                {
                    if (item == "StdCSV" || item == "Metastock" || item == "Ninja" || item == "FCharts")
                    {
                        folder = Path.Combine(strOutputFolder, item, "BSE", "Equity", strbseequityfilename);
                        File.Copy(obj, folder, true);
                    }
                    else
                        if (item == "Amibroker")
                        {
                            engineBSECSVFINAL.Options.IgnoreFirstLines = 1;
                            engineBSECSVFINAL.WriteFile(obj, finalarr);

                            folder = Path.Combine(strOutputFolder, item, "BSE", "Equity", strbseequityfilename);
                            File.Copy(obj, folder, true);
                        }
                }
                File.Delete(obj);

            }


        }
        
    }
}
