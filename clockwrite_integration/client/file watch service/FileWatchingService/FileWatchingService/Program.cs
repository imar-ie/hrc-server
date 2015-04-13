using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration.Install;
using System.Reflection;



namespace FileWatchingService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

#if debug
            Service1 s=new Service1();
            s.OnDebug();

            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
              

#else
                      
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new Service1() 
			};
                
            ServiceBase.Run(ServicesToRun);
                
         

            

#endif
        }

    }

   
}
