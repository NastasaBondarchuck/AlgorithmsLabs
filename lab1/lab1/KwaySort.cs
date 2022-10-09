using System;
using System.Collections.Generic;
using System.IO;

namespace lab1
{
    public class KwaySort
    {
        public static void MergeSort(List<string> FileBList, List<string> FileCList)
        {
            List<long> SeekList = CreateSeekList(FileBList);
            int index = 0;
            ResursiveMerge(FileBList, FileCList, SeekList, ref index);
            DeleteFiles(FileBList);
            if (GetNumberOfFiles(FileCList) > 1)
            {
                MergeSort(FileCList, FileBList);
            }
        }

        public static int GetNumberOfFiles(List<string> FileList)
        {
            int number = 0;
            foreach (var path in FileList)
            {
                FileInfo file = new FileInfo(path);
                if (file.Exists && !IsFileEmpty(path))
                {
                    number++;
                }
            }

            return number;
        }
        public static void DeleteFiles(List<string> FileList)
        {
            foreach (var path in FileList)
            {
                FileInfo file = new FileInfo(path);
                file.Delete();
            }
        }
        
        public static void ResursiveMerge(List<string> FileBList, List<string> FileCList, List<long> SeekList, ref int index)
        {
            // int seek = 0;
            List<List<int>> SeriesList = new List<List<int>>();
            GetSeriesList(FileBList, SeekList, SeriesList);
            List<int> ToFile = new List<int>();
            GetSeriesToFile(SeriesList, ToFile);
            WriteToCFile(ToFile, ref index, FileCList);
            if (!IsFilesEnded(FileBList, SeekList))
            {
                ResursiveMerge(FileBList, FileCList, SeekList, ref index);
            }
        }
        
        public static void WriteToCFile(List<int> ToFIle, ref int index, List<string> FileCList)
        {
            if (index >= FileCList.Count)
            {
                int temp = index % FileCList.Count;
                index = temp;
            }

            // string path = "";
            // if (FileBList[0] == "B1.bin")
            // {
            //     path = $"C{index + 1}.bin";
            // }
            // else if (FileBList[0] == "C1.bin")
            // {
            //     path = $"B{index + 1}.bin";
            // }
            //
            using (BinaryWriter file = new BinaryWriter(new FileStream(FileCList[index], FileMode.Append)))
            {
                foreach (var number in ToFIle)
                {
                    file.Write(number);
                }
            }
            ToFIle.Clear();
            index++;
        }
        public static bool IsFilesEnded(List<string> FileList, List<long> SeekList)
        {
            foreach (var path in FileList)
            {
                using (BinaryReader file = new BinaryReader(new FileStream(path, FileMode.Open)))
                {
                    file.BaseStream.Position = SeekList[FileList.IndexOf(path)];
                    if (file.PeekChar() != -1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool IsFileEmpty(string path)
        {
           using (BinaryReader file = new BinaryReader(new FileStream(path, FileMode.Open)))
           {
                file.BaseStream.Position = 0;
                if (file.PeekChar() != -1)
                {
                    return false;
                }
           }
           return true;
        }
        public static void GetSeriesToFile(List<List<int>> SeriesList, List<int> ToFile)
        {
            int i = 0;
            while (i < SeriesList.Count)
            {
                if (SeriesList[i].Count == 0)
                {
                    SeriesList.RemoveAt(i);
                }
                i++;
            }
            List<int> ElementsList = new List<int>();
            foreach (var series in SeriesList)
            {
                if (series.Count != 0)
                {
                    ElementsList.Add(series[0]); 
                }
            }
            int minIndex = FindMin(ElementsList);
            while (SeriesList[minIndex].Count == 0)
            {
                SeriesList.RemoveAt(minIndex);
            }
            SeriesList[minIndex].Remove(ElementsList[minIndex]);

            ToFile.Add(ElementsList[minIndex]);
            if (ElementsList.Count > 1)
            {
                ElementsList.Clear();
                GetSeriesToFile(SeriesList, ToFile);
            }
            else if (ElementsList.Count == 1)
            {
                foreach (var element in SeriesList[minIndex])
                {
                    ToFile.Add(element);
                }
                ElementsList.Clear();
                SeriesList.Clear();
            }
            
        }
        public static void GetSeriesList(List<string> ListFiles, List<long> SeekList, List<List<int>> SeriesList)
        {
            // List<List<int>> SeriesList = new List<List<int>>();
            foreach (var file in ListFiles)
            {
                long seek = SeekList[ListFiles.IndexOf(file)];
                List<int> series = ReadSeries(file, ref seek);
                SeriesList.Add(series);
                SeekList[ListFiles.IndexOf(file)] = seek;
            }
            //GetSeriesList(ListFiles, SeekList, SeriesList);
        }

        public static List<long> CreateSeekList(List<string> ListFiles)
        {
            List<long> SeekList = new List<long>();
            foreach (var file in ListFiles)
            {
                SeekList.Add(0);
            }

            return SeekList;
        }
        

        public static List<int> ReadSeries(string path, ref long seek)
        {
            List<int> series = new List<int>();
            using (BinaryReader file = new BinaryReader(new FileStream(path, FileMode.OpenOrCreate)))
            {
                int current, next;
                file.BaseStream.Position = seek;
                while (file.BaseStream.Position < file.BaseStream.Length - sizeof(Int32))
                {
                    current = file.ReadInt32();
                    next = file.ReadInt32();
                    file.BaseStream.Position -= sizeof(Int32);
                    if (next >= current)
                    {
                        series.Add(current);
                    }
                    else
                    {
                        series.Add(current);
                        seek = file.BaseStream.Position;
                        break;
                    }
                }
            }
            return series;
        }

        public static int FindMin(List<int> sequence)
        {
            int minindex = 0;
            for (int index = 0; index < sequence.Count; index++)
            {
                if (sequence[index] <= sequence[minindex])
                {
                    minindex = index;
                }
            }

            return minindex;
        }
    }
}