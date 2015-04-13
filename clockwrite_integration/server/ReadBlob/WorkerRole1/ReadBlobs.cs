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
using System.Data.SqlClient;
using System.Collections.Generic;

//creating workerrole
namespace WorkerRole1
{
    class ReadBlobs
    {
        //creating storage account
        private static CloudStorageAccount _storageAccount = null;

        //method that lists all blobs
        public static void ListAllBlobs()
        {
            //declaring credentials of azure account
            StorageCredentials credentials = new StorageCredentials("hrclocker", "s9gO6plPWeanVpPwOkwHzktQyVyZnHNtKWyG02pJ52jtOW/FEinD8YDYGzjWOfLrYbb9KslNrfOQfytnk+ga2g==");
            _storageAccount = new CloudStorageAccount(credentials, true);

            //targets clockins container
            CloudBlobContainer blobContainer = _storageAccount.CreateCloudBlobClient().GetContainerReference("clockins");
           

            string blobPrefix = null;

            bool useFlatBlobListing = true;

            var blobs = blobContainer.ListBlobs(blobPrefix, useFlatBlobListing, BlobListingDetails.None);

           
            // lists all blobs and retrieves attributes
            foreach (IListBlobItem item in blobs)
            {
                CloudBlockBlob blob = (CloudBlockBlob)item;
                blob.FetchAttributes();
                
              
                string line;

                //open each blob
                using (var stream = blob.OpenRead())
                {
                   //read the file
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            //read each line
                            line = reader.ReadLine();

                            if (line != null)
                            {

                                
                                // remove tabs and store those lines
                                string sepTab = "\t";
                                string[] lines = line.Split(sepTab.ToCharArray());

                                // credentials for an sql connection with azure portal
                                SqlConnectionStringBuilder connStringBuilder = new SqlConnectionStringBuilder();
                                connStringBuilder.DataSource = "cqaa8fugxp.database.windows.net"; //datasource
                                connStringBuilder.InitialCatalog = "hrlockerapi_db"; // sql database
                                connStringBuilder.Encrypt = true;
                                connStringBuilder.TrustServerCertificate = false;
                                connStringBuilder.UserID = "hrlocker"; // username
                                connStringBuilder.Password = "9cG1bE3EwC96rPv1"; //password for sql database
                                
                                //making sql connection
                                using(SqlConnection con = new SqlConnection(connStringBuilder.ToString()))
                                {
                                    //query to insert all the lines in cloud database from text file
                                    string query = "INSERT INTO dbo.clockin_data(EmployeeID, Date, Time, InOutMode, VerifyMode, WorkCode, ClockRef) VALUES (@EmployeeID,@Date,@Time,@InOutMode,@VerifyMode,@WorkCode,@ClockRef)";
                                    SqlCommand cmd = new SqlCommand(query, con);
                                    

                                    cmd.Parameters.AddWithValue("@EmployeeID", lines[0]);
                                    cmd.Parameters.AddWithValue("@Date", lines[1].ToString());
                                    cmd.Parameters.AddWithValue("@Time", lines[2].ToString());
                                    cmd.Parameters.AddWithValue("@InOutMode", lines[3]);
                                    cmd.Parameters.AddWithValue("@VerifyMode", lines[4]);
                                    cmd.Parameters.AddWithValue("@WorkCode", lines[5]);
                                    cmd.Parameters.AddWithValue("@ClockRef", lines[6].ToString());

                              

                                    con.Open();
                                  
                                    cmd.ExecuteNonQuery();
                                   
                                    con.Close();
                                }

                              
                            }
                        }
                    }
                }
                
              
                //delete the blob after successful data entry
                blob.DeleteIfExists();
            }

         


        }
    }
}


















