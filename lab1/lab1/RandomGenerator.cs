using System;
using System.IO;

namespace lab1
{
    public class RandomGenerator
    {
        public static void GenerateBySize(string fileName, int megabytes, int minNum = 0, int maxNum = 1_000_000_000)
        {
            Random random = new Random();
            var writer = new BinaryWriter(File.Open(fileName, FileMode.Create));

            for (int i = 0; i % 10_000 != 0 || !(new FileInfo(fileName).Length >= Helper.MegabytesToBytes(megabytes)); i++)
            {
                writer.Write(random.Next(minNum, maxNum));
            }
            writer.Close();
        }
        public static void GenerateByLinesCount(string fileName, long linesCount, int minNum = 0, int maxNum = 1_000_000_000)
        {
            Random random = new Random();
            var writer = new BinaryWriter(File.Open(fileName, FileMode.Create));
            int currentCount = 0;

            while (currentCount++ != linesCount)
            {
                writer.Write(random.Next(minNum, maxNum));
            }
            writer.Close();
        }
    }
}