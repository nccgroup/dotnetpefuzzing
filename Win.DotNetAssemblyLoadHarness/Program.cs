/*
Released as open source by NCC Group Plc - http://www.nccgroup.com/

Developed by Ollie Whitehouse, ollie dot whitehouse at nccgroup dot com

http://www.github.com/nccgroup/dotnetpefuzzing

Released under AGPL see LICENSE for more information
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Reflection;

//
// Based in part on samples from:
//   http://msdn.microsoft.com/en-us/library/system.io.filesystemwatcher.aspx
//  
//

namespace Win.DotNetAssemblyLoadHarness
{
    class Program
    {

        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="args">program arguments</param>
        static void Main(string[] args)
        {
            
            // Argument error checking
            if (args.Length != 1)
            {
                // Display the proper way to call the program.
                Console.WriteLine("[i] Usage: " + System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + " [directory]");
                return;
            } else if (Directory.Exists(args[0]) == false)
            {
                Console.WriteLine("[!] The directory '" + args[0] + "' does not exist");
                return;
            }

            // Only return from here when someone presses x
            WatchFS(args);
        }


        /// <summary>
        /// File system watcher
        /// </summary>
        /// <param name="args">program arguments</param>
        static void WatchFS(string[] args)
        {

            // What the file system for changes
            FileSystemWatcher fsWatcher = new FileSystemWatcher();
            fsWatcher.Path = args[0];
            fsWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            fsWatcher.Filter = "*.dll";

            // Add event handlers for only the one we care about
            fsWatcher.Created += new FileSystemEventHandler(OnChanged);
            
            // Begin watching.
            fsWatcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            Console.WriteLine("[i] Watching directory: " + args[0]);
            Console.WriteLine("[i] Press \'x\' to quit");

            // This is the loop that blocks
            while (Console.Read() != 'x');
        }

        /// <summary>
        /// Event handler
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
           
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("[i] File: " + e.FullPath + " " + e.ChangeType);
            Thread threadDoIt = new Thread(() => ProcessFile(e.FullPath));
            threadDoIt.Start();
        }

        /// <summary>
        /// Function run inside the thread
        /// </summary>
        /// <param name="strFilename">Full path to the file to process</param>
        private static void ProcessFile(string strFilename)
        {
            Console.WriteLine("[i] " + Thread.CurrentThread.ManagedThreadId + " processing " + strFilename);
            try
            {
                Assembly.Load(strFilename);
                Console.WriteLine("[!] " + Thread.CurrentThread.ManagedThreadId + " loaded " + strFilename + " OK");
            }
            catch (Exception exExcp)
            {
                Console.WriteLine("[!] " + Thread.CurrentThread.ManagedThreadId + " errored " + exExcp.Message);
            }
        }
    }
}
