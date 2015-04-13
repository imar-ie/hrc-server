using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Data.Services.Client;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text.RegularExpressions;
using System.Security.Permissions;

namespace FileWatchingService
{
    public class BlobUploader
    {

        private static CloudStorageAccount _storageAccount = null;


        public static void Upload(string file) 
        {
            Console.WriteLine("Uploading - " + file + "...");

            //declare credentials
            StorageCredentials credentials = new StorageCredentials("hrclocker", "s9gO6plPWeanVpPwOkwHzktQyVyZnHNtKWyG02pJ52jtOW/FEinD8YDYGzjWOfLrYbb9KslNrfOQfytnk+ga2g==");
            
            //create storage account
            _storageAccount = new CloudStorageAccount(credentials, true);

            // Create the blob client. 
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container that we wantto use 
            CloudBlobContainer container = blobClient.GetContainerReference("clockins");

            // Create the container if it doesn't already exist. 
            container.CreateIfNotExists();

            //Retrieve reference to a blob
            // TODO need to check if clockwrite logs have unique file names, for now use datetime

            string machineName = System.Environment.MachineName;
            DateTime now = DateTime.Now;

            Logger.Log("Machine Name: " + machineName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(String.Format("{0:d/M/yyyy_HH:mm:ss.fff}" +"_" +machineName, now) + ".txt"); //FIXME bad ?

            //upload the file
            using (var fileStream = System.IO.File.OpenRead(@file))
            {
                blockBlob.UploadFromStream(fileStream);
                Logger.Log("Uploaded: " + blockBlob.Name);
            }

            // TODO is there a success/failure callback ? 
            if (blockBlob.Exists())
            {
                Logger.Log("blockBlob Exists: " + file);
                //Console.WriteLine("Uploaded...");

                // delete the file 
                File.Delete(@file);
                Logger.Log("Deleted: " + file);
            }
            
           
        }


        //check for other existing files in the path folder and upload them too
        public static void checkFileExists()
        {
            String filepath= System.Configuration.ConfigurationManager.AppSettings["location"]; // file path

            String[] files = System.IO.Directory.GetFiles(filepath, "*.txt");//(@"C:\Clockrite\Database", "*.txt");
            Logger.Log("Filepath: "+filepath);
            Logger.Log("Total Files:" + files.Length);

            if (files.Length > 0)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    
                if (FileWatcher.CheckForInternetConnection()) { //check for internet connection and upload file if exists
                    Upload(files[i]);
                }
                    
             }

            }
        }
    }
}
