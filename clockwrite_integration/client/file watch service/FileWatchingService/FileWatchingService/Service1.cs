using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace FileWatchingService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        public void onDebug()
        {
            OnStart(null);
        
        }

        protected override void OnStart(string[] args)
        {

            //System.Diagnostics.Debugger.Launch();
            FileWatcher f = new FileWatcher();
            //BlobUploader b = new BlobUploader();
        }

        protected override void OnStop()
        {

        }
    }
}
