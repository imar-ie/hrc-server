using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileWatchingService
{
    public static class Logger
    {
        public static void Log(String message)
        {
            try
            {
                string _message = String.Format("{0} {1}", message, Environment.NewLine);
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "logFile.txt", _message);

            }

            catch (Exception ex)
            { 
                // implement logging on next version 
            
            }
         }
    }
}
