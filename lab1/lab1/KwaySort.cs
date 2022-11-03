using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lab1
{
    public class KwaySort
    {
        public static void MergeSort(string fileName, out string sortedFileName,  bool Modified, int numberOfFIles = 5)
        {
            if (numberOfFIles < 2)
                throw new ArgumentException(null, nameof(numberOfFIles));
            if (Modified)
            {
                string[] BFilesArray = Enumerable.Range(1, numberOfFIles).Select(i => $"B{i}mod.bin").ToArray();
                string[] CFilesArray = Enumerable.Range(1, numberOfFIles).Select(i => $"C{i}mod.bin").ToArray();
                SplitFile(fileName, BFilesArray);
                SortHelper(BFilesArray, CFilesArray, out sortedFileName);
            }
            else
            {
                string[] BFilesArray = Enumerable.Range(1, numberOfFIles).Select(i => $"B{i}.bin").ToArray();
                string[] CFilesArray = Enumerable.Range(1, numberOfFIles).Select(i => $"C{i}.bin").ToArray();
                SplitFile(fileName, BFilesArray);
                SortHelper(BFilesArray, CFilesArray, out sortedFileName);
            }
        }

        public static void SortParts(string fileName, string resultFileName, int size, int shareSize)
        {
            if (File.Exists(resultFileName))
            {
                File.Delete(resultFileName);
            }

            int[] array = new int[shareSize];
            var reader = new BinaryReader(File.Open(fileName, FileMode.Open));
            var writer = new BinaryWriter(File.Open(resultFileName, FileMode.OpenOrCreate));
            for (int i = 0; i < size / shareSize; i++)
            {
                for (int j = 0; j < shareSize; j++)
                {
                    array[j] = reader.ReadInt32();
                }

                Array.Sort(array);
                for (int j = 0; j < shareSize; j++)
                {
                    writer.Write(array[j]);
                }
            }

            reader.Close();
            writer.Close();
        }

        private static void SortHelper(string[] BFilesArray, string[] CFIlesArray, out string fileName)
        {
            var readers = BFilesArray.Select(f => new BinaryReader(File.OpenRead(f))).ToList();
            readers.Where(r => (r.BaseStream.Position == r.BaseStream.Length)).ToList().ForEach(r =>
            {
                r.Dispose();
                readers.Remove(r);
            });

            if (readers.Count == 1)
            {
                fileName = ((FileStream) readers.First().BaseStream).Name;
                return;
            }

            var writers = CFIlesArray.Select(f => new BinaryWriter(File.Open(f, FileMode.Create))).ToList();
            var currentWriter = writers.First();
            var currentReader = readers.First();
            var nums = new List<int>();
            var nextNums = new List<int>();
            var readerAndPrevNum = readers.ToDictionary(r => r, _ => int.MinValue);

            while (readers.Count != 0)
            {
                while (currentReader.BaseStream.Position == currentReader.BaseStream.Length)
                {
                    var readerToRemove = currentReader;
                    currentReader = readers.Next(currentReader);
                    readers.Remove(readerToRemove);
                    readerAndPrevNum.Remove(readerToRemove);
                    readerToRemove.Dispose();
                    if (readers.Count == 0)
                    {
                        break;
                    }
                }

                if (readers.Count == 0)
                {
                    nums.Sort();
                    foreach (int n in nums)
                    {
                        currentWriter.Write(n);
                    }

                    currentWriter = writers.Next(currentWriter);
                    nextNums.Sort();
                    foreach (int n in nextNums)
                    {
                        currentWriter.Write(n);
                    }

                    break;
                }

                int num = currentReader.ReadInt32();
                if (num >= readerAndPrevNum[currentReader])
                {
                    nums.Add(num);
                    readerAndPrevNum[currentReader] = num;
                }
                else
                {
                    nextNums.Add(num);
                    readerAndPrevNum[currentReader] = num;
                    currentReader = readers.Next(currentReader);
                }

                if (nextNums.Count >= readers.Count)
                {
                    nums.Sort();
                    foreach (int n in nums)
                    {
                        currentWriter.Write(n);
                    }

                    currentWriter = writers.Next(currentWriter);
                    nums.Clear();
                    nums.AddRange(nextNums);
                    nextNums.Clear();
                }
            }

            readers.ForEach(r => r.Dispose());
            writers.ForEach(w => w.Dispose());
            SortHelper(CFIlesArray, BFilesArray, out fileName);
            
        }


        private static void SplitFile(string fileName, string[] BFilesArray)
        {
            var writers = BFilesArray.Select(f => new BinaryWriter(File.Open(f, FileMode.Create))).ToList();
            var currentWriter = writers.First();
            var reader = new BinaryReader(File.OpenRead(fileName));
            int previous = int.MinValue;

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                int current = reader.ReadInt32();
                if (current >= previous)
                {
                    currentWriter.Write(current);
                }
                else
                {
                    currentWriter = writers.Next(currentWriter);
                    currentWriter.Write(current);
                }

                previous = current;
            }

            writers.ForEach(w => w.Dispose());
            reader.Close();
        }
    }
}