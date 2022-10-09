using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Text;

namespace lab1
{
    internal class Program
    {

        static void Main(string[] args) 
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;


            string file_name = "numbers.bin";
            CreateNumFile(file_name);
            Console.WriteLine("numbers:");
            using (BinaryReader reader = new BinaryReader(new FileStream(file_name, FileMode.Open, FileAccess.Read))) {
                while (reader.PeekChar() != -1) {
                    Console.WriteLine(reader.ReadInt32());
                }
            }
            int length = 4;
            List<string> ListBFiles = CreateBFiles(length);
            List<string> ListCFiles = CreateCFiles(length);
            SplitArray(file_name, ListBFiles);
            Console.WriteLine();
            KwaySort.MergeSort(ListBFiles, ListCFiles);
            string path = "";
            FileInfo Bfile = new FileInfo(ListBFiles[0]);
            FileInfo CFile = new FileInfo(ListCFiles[0]);
            if (Bfile.Exists)
            {
                path = ListBFiles[0];
            }
            else if (CFile.Exists)
            {
                path = ListCFiles[0];
            }
            Console.WriteLine("Sorted: ");
            using (BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read))) {
                while (reader.PeekChar() != -1) {
                    Console.WriteLine(reader.ReadInt32());
                }
            }

            Console.ReadLine();
        }

        static void CreateNumFile(string file_name) {
            int[] numbers = new int[200];
            Random random = new Random();
            for (int i = 0; i < 200; i++) {
                numbers[i] = random.Next(0, 50);
            }
            using (BinaryWriter writer = new BinaryWriter(new FileStream(file_name, FileMode.Create))) {
                foreach (int number in numbers) {
                    writer.Write(number);
                }
            }
        }

        public static List<string> CreateBFiles(int length) {
            List<string> ListBFiles = new List<string>();
            for (int i = 1; i <= length; i++) {
                using (StreamWriter newFile = new StreamWriter($"B{i}.bin", false))
                {
                    ListBFiles.Add($"B{i}.bin");
                }
            }
            return ListBFiles;
        }

        public static List<string> CreateCFiles(int length)
        {
            List<string> ListCFiles = new List<string>();
            for (int i = 1; i <= length; i++)
            {
                using (StreamWriter newFile = new StreamWriter($"C{i}.bin", false))
                {
                    ListCFiles.Add($"C{i}.bin");
                }
            }

            return ListCFiles;
        }

        public static void WriteToFile(string path, List<int> ToFile, ref int counter)
        {
            using (BinaryWriter B = new BinaryWriter(new FileStream(path, FileMode.Append))) {
                foreach (var to in ToFile) {
                    B.Write(to);
                    if (path == "B1.bin")
                    {
                        counter++;
                    }
                }
            }
        }
        public static void SplitArray(string path, List<string> Bfiles) {
            using (BinaryReader file = new BinaryReader(new FileStream(path, FileMode.OpenOrCreate))) {
                int i = 0;
                int seriesCounter = 0;
                List<int> ToFile = new List<int>();
                int current;
                int next;
                while (file.BaseStream.Position < file.BaseStream.Length)
                {
                    current = file.ReadInt32();
                    next = file.PeekChar();
                    if (next >= current) {
                        ToFile.Add(current);
                    }
                    else {
                        ToFile.Add(current);
                        if (i < Bfiles.Count) {
                            WriteToFile(Bfiles[i], ToFile, ref seriesCounter);
                            i++;
                        }
                        else {
                            i = 0;
                            WriteToFile(Bfiles[i], ToFile, ref seriesCounter);
                            i++;
                        }
                        ToFile.Clear();
                    }
                    
                }
            }
        }
        
    }
}