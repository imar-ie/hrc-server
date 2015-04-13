using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;



namespace FileWatchingService
{
    class FileWatcher
    {

        private FileSystemWatcher _fileWatcher;

        public FileWatcher()
        { 
            _fileWatcher = new FileSystemWatcher(PathLocation());


            _fileWatcher.Filter = "*.txt"; // only .txt files

           

                _fileWatcher.Created += new FileSystemEventHandler(_fileWatcher_Created);
                //_fileWatcher.Deleted +=new FileSystemEventHandler(_fileWatcher_Deleted);
                //_fileWatcher.Changed +=new FileSystemEventHandler(_fileWatcher_Changed);

                _fileWatcher.EnableRaisingEvents = true;

            

        }

        //checks for internet connection
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://azure.microsoft.com/en-us/"))
                {
                    
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private string PathLocation()
        {
            string value = String.Empty;

            try
            {
                value = System.Configuration.ConfigurationManager.AppSettings["location"];     
            }
            catch (Exception ex)
            { 
                //implement logging on future version

            }

            return value;
        }

        void _fileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (CheckForInternetConnection())
            {
                Logger.Log(String.Format("File Created: Path:{0}, Name:{1}", e.FullPath, e.Name));
                BlobUploader.Upload(e.FullPath);
                BlobUploader.checkFileExists();
            }
         }

        //void _fileWatcher_Deleted(object sender, FileSystemEventArgs e)
        //{
        //    Logger.Log(String.Format("File Deleted: Path:{0}, Name:{1}", e.FullPath, e.Name));
        //    BlobUploader.Upload(e.FullPath);
        //}

        //void _fileWatcher_Changed(object sender, FileSystemEventArgs e)
        //{
        //    Logger.Log(String.Format("File Changed: Path:{0}, Name:{1}", e.FullPath, e.Name));
        //    BlobUploader.Upload(e.FullPath);
        //}


      }


    

}
