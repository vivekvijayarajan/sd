using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;


using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;


namespace StockD
{
    public static class Utility
    {
        public static string strLog = String.Empty;

        public static void Save(string strPath, string strContent)
        {
            using (Stream stream = File.Create(strPath))
            using (StreamWriter writer = new StreamWriter(stream, Encoding.ASCII))
            {
                writer.Write(strContent);
    
            }
        }

        public static void UnzipFiles(string strDirectoryName)
        {

            //Get all ZIP files present under this month folder
            string[] zip_files = null;

            zip_files = Directory.GetFiles(Path.Combine(strDirectoryName), "*.zip");
         
            foreach (string strzipfile in zip_files)
            {


                using (ZipInputStream s = new ZipInputStream(File.OpenRead(strzipfile)))
                {

                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {

                        //Console.WriteLine(theEntry.Name);

                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);

                        // create directory
                        //if (directoryName.Length > 0)
                        //{
                        //    Directory.CreateDirectory(directoryName);
                        //}

                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(strDirectoryName + "\\" + theEntry.Name))
                            {

                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }


}
