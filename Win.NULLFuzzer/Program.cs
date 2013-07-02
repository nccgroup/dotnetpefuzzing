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

namespace Win.NULLFuzzer
{
    class Program
    {
        static void Main(string[] args)
        {
            Stream streamIn = null;
            Stream streamOut = null;
            BinaryWriter binW = null;
            BinaryReader binR = null;
            byte[] rawIn = null;
            byte[] rawOut = null;
            int intLength = 0;

            // Argument error checking
            if (args.Length != 2)
            {
                // Display the proper way to call the program.
                Console.WriteLine("[i] Usage: " + System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + " [in.file] [out.path]");
                return;
            }
            else if (File.Exists(args[0]) == false)
            {
                Console.WriteLine("[!] The file '" + args[0] + "' does not exist");
                return;
            }
            else if (Directory.Exists(args[1]) == false)
            {
                Console.WriteLine("[!] The directory '" + args[1] + "' does not exist");
                return;
            }

            streamIn = new FileStream(args[0],FileMode.Open,FileAccess.Read);
            binR = new BinaryReader(streamIn);
            // Yes yes this in theory truncates but we know out module wont be > 2GB in size eh!
            intLength = Convert.ToInt32(streamIn.Length);
            rawIn = binR.ReadBytes(Convert.ToInt32(streamIn.Length));
            binR.Close();
            streamIn.Close();

            int intCount = 0;
            while (intCount < intLength-3)
            {
                streamOut = new FileStream(args[1] + "\\" + intCount.ToString() + "-" + Path.GetFileName(args[0]),FileMode.Create,FileAccess.Write);
                binW = new BinaryWriter(streamOut);
                rawOut = rawIn;
                rawOut[intCount] = 0x00;
                binW.Write(rawOut);
                binW.Close();
                streamOut.Close();
                intCount++;
            }
        }
        
    }
}
