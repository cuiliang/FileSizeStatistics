using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSizeStatistics
{
    internal enum FileSizeRange
    {
        F1K, //小于等于1k的
        F2K,
        F4K,
        F8K,
        F16K,
        F32K,
        F64K,
        MORE
    };

    class Program
    {
        static void Main(string[] args)
        {
            string path = @"d:\";

            if (args.Length == 0)
            {
                Console.WriteLine("Path not provided, will caculate d:\\...");
            }
            else
            {
                path = args[0];
            }

             

            if (!Directory.Exists(path))
            {
                Console.WriteLine($"Path {path} does not exists.");
                return;
            }

            Console.WriteLine("Begin to caculate ...");

            int[] counters = new int[10]; //最后一个保存总数
           
            DoStatistics(path, counters);

            Console.WriteLine("\nResult==================================");
            Console.WriteLine("FileSize\tCount\t%");
            foreach (var range in  Enum.GetValues(typeof (FileSizeRange)))
            {
                Console.WriteLine($"{range}\t{counters[(int)range]}\t{counters[(int)range] * 100.0 / counters[counters.Length-1]}");
            }

            Console.WriteLine("\nDone. Press any key...\n");

            Console.ReadKey();
        }

        

        private static void DoStatistics(string path, int[] counters)
        {
            StatisticsFolder(path, counters);
        }

        private static void StatisticsFolder(string path, int[] counters)
        {
            var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                try
                {
                    StatisticsFile(file, counters);
                }
                catch(Exception ex)
                {
                    //Console.WriteLine($"Error process file:{file} ex={ex.Message}");
                    Console.Write("x");
                }

            }

            var subFolders = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var folder in subFolders)
            {
                try
                {
                    StatisticsFolder(folder, counters);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Error process folder:{folder} ex={ex.Message}");
                    Console.Write("X");
                }
                
            }
        }

        private static void StatisticsFile(string file, int[] counters)
        {
            FileInfo  info = new FileInfo(file);
            counters[(int) GetFileSizeRange(info.Length)] ++;

            counters[counters.Length - 1] ++; //最后一个保存总数

            if (counters[counters.Length - 1] % 100 == 0)
            {
                Console.Write(".");
            }

        }

        //private static int GetTotal(int[] counters)
        //{
        //    int total = 0;
        //    for (int i = 0; i < counters.Length; i++)
        //    {
        //        total += counters[i];
        //    }

        //    return total;
        //}

        private static FileSizeRange GetFileSizeRange(long fileLength)
        {
            if (fileLength <= 1024)
            {
                return FileSizeRange.F1K;
            }
            if (fileLength <= 1024*2)
            {
                return FileSizeRange.F2K;
            }
            if (fileLength <= 1024*4)
            {
                return FileSizeRange.F4K;
            }
            if (fileLength <= 1024 * 8)
            {
                return FileSizeRange.F8K;
            }
            if (fileLength <= 1024 * 16)
            {
                return FileSizeRange.F16K;
            }
            if (fileLength <= 1024 * 32)
            {
                return FileSizeRange.F32K;
            }
            if (fileLength <= 1024 * 64)
            {
                return FileSizeRange.F64K;
            }
            return FileSizeRange.MORE;
        }
    }
}
