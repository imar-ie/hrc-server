using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System.Configuration;
//using Microsoft.WindowsAzure.Storage;


namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {      
        public override void Run()
        {
            
            Trace.WriteLine("Listening to text file uploads...");
                        
            while (true)
            {
                //call listAllBlobs method from ReadBlobs class
                ReadBlobs.ListAllBlobs();
                //sleep for 10 seconds
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

           // RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }
        
    

    }
}
