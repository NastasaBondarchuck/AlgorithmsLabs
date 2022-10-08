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
        // public static void Main(string[] args)
        // {
        //     //string text = "1\n5\n6\n8\n9\n1\n8\n5\n7\n8\n0\n2\n5\n14\n19\n12\n14\n2\n3\n7\n0\n5\n4\n6";
        //     List<int> Text = new List<int>();
        //     Random rand = new Random();
        //     for (int i = 0; i < 6; i++)
        //     {
        //         Text.Add(rand.Next(0, 10));
        //     }
        //     using (BinaryWriter A = new BinaryWriter(new FileStream("A1.bin", FileMode.OpenOrCreate))) {
        //         foreach (var num in Text)
        //         {
        //             A.Write(num);
        //         }
        //     }
        //     // StreamWriter A = new StreamWriter("A1.txt", false);
        //     //  A.Write(text);
        //     //  A.Write(" ");
        //     //  A.Close();
        //     Console.WriteLine(Text);
        //     List<string> FileBList = CreateBFiles(4);
        //     SplitArray("A1.txt", FileBList);
        //     // List<string> FileCList = CreateCFiles(FileBList.Count);
        //
        // }
        
        static void Main(string[] args) {
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
            SplitArray(file_name, ListBFiles);
            Console.WriteLine();
            for (int i = 0; i < length; i++) {
                Console.WriteLine(ListBFiles[i]);
                using (BinaryReader reader = new BinaryReader(new FileStream(ListBFiles[i], FileMode.Open, FileAccess.Read))) {
                    while (reader.PeekChar() != -1) {
                        Console.WriteLine(reader.ReadInt32());
                    }
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }

        static void CreateNumFile(string file_name) {
            int[] numbers = new int[10];
            Random random = new Random(((int)DateTime.Now.Ticks));
            for (int i = 0; i < numbers.Length; i++) {
                numbers[i] = random.Next(0, i);
            }
            using (BinaryWriter writer = new BinaryWriter(new FileStream(file_name, FileMode.OpenOrCreate))) {
                foreach (int number in numbers) {
                    writer.Write(number);
                }
            }
        }

        static List<string> CreateBFiles(int length) {
            List<string> ListBFiles = new List<string>();
            for (int i = 1; i <= length; i++) {
                StreamWriter newFile = new StreamWriter($"B{i}.bin", false);
                ListBFiles.Add($"B{i}.bin");
                newFile.Close();
            }
            return ListBFiles;
        }

        public static void SplitArray(string path, List<string> Bfiles) {
            using (BinaryReader file = new BinaryReader(new FileStream(path, FileMode.OpenOrCreate))) {
                int i = 0;
                List<int> ToFile = new List<int>();
                int current;
                int next;
                current = file.ReadInt32();
                while (file.PeekChar() != -1) {
                    
                    if (file.PeekChar() == -1) {
                        file.BaseStream.Position = file.BaseStream.Position - 1;
                    }
                    next = file.ReadInt32();
                    
                    if (next >= current) {
                        ToFile.Add(current);
                        current = next;
                    }
                    else {
                        ToFile.Add(current);
                        current = next;
                        if (i < Bfiles.Count) {
                            using (BinaryWriter B = new BinaryWriter(new FileStream(Bfiles[i], FileMode.Append))) {
                                foreach (var to in ToFile) {
                                    B.Write(to);
                                }
                            }
                            i++;
                        }
                        else {
                            i = 0;
                            using (BinaryWriter B = new BinaryWriter(new FileStream(Bfiles[i], FileMode.Append))) {
                                foreach (var to in ToFile) {
                                    B.Write(to);
                                }
                            }
                            i++;
                        }
                        ToFile.Clear();
                    }
                    if (file.PeekChar() == -1) {
                        ToFile.Add(next);
                        if (i < Bfiles.Count) {
                            using (BinaryWriter B = new BinaryWriter(new FileStream(Bfiles[i], FileMode.Append))) {
                                foreach (var to in ToFile) {
                                    B.Write(to);
                                }
                            }
                            i++;
                        }
                        else {
                            i = 0;
                            using (BinaryWriter B = new BinaryWriter(new FileStream(Bfiles[i], FileMode.Append))) {
                                foreach (var to in ToFile) {
                                    B.Write(to);
                                }
                            }
                            i++;
                        }
                        ToFile.Clear();
                    }
                }
            }
        }
        
        
    }
}