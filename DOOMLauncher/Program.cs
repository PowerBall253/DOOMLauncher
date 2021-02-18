using System.Diagnostics;
using System;
using Memory;

namespace DOOMLauncher
{
    class Program
    {
        public static Mem m = new Mem();

        static void Main()
        {
            var doom = Process.Start("DOOMx64vk.exe");
            doom.WaitForExit();

            while (true)
            {
                try
                {
                    if (m.GetProcIdFromName("DOOMx64vk") == 0)
                        throw new Exception();
                    break;
                }
                catch (Exception) { }
            }

            int pID = m.GetProcIdFromName("DOOMx64vk");
            bool openProc = false;
            if (pID > 0)
                openProc = m.OpenProcess(pID);

            if (openProc)
            {
                while (true)
                {
                    try
                    {
                        if (BitConverter.ToString(m.ReadBytes("DOOMx64vk.exe+18a31d0", 7)) != "0F-B6-81-89-4C-03-00")
                            throw new Exception();
                        break;
                    }
                    catch (Exception) { }
                }

                m.WriteMemory("DOOMx64vk.exe+18a31d0", "bytes", "31 C0 90 90 90 90 90");
                Console.WriteLine("Process has been patched succesfully.");
                m.CloseProcess();
                System.Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Failed to open process!");
                System.Environment.Exit(1);
            }            
        }
    }
}
