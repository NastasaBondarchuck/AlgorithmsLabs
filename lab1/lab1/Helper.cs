using System;
using System.Collections.Generic;
using System.IO;

namespace lab1
{
    public static class Helper
    {
        public static long MegabytesToBytes(int megabytes) => (long) Math.Pow(2, 20) * megabytes;
        
        public static T Next<T>(this List<T> source, T item)
        {
            int indexOfItem = source.IndexOf(item);
            return indexOfItem < source.Count - 1 ? source[indexOfItem + 1] : source[0];
        }
        
        public static int[] GetArrayOfParts(int start, int size, string fileName)
        {
            var array = new int[size];
            var reader = new BinaryReader(File.OpenRead(fileName));
            for (int i = 0; i < start; i++)
            {
                reader.ReadInt32();
            }

            for (int i = 0; i < size; i++)
            {
                array[i] = reader.ReadInt32();
            }
            reader.Close();
            return array;
        }
        
        public static void ShowFile(string fileName, int size)
        {
            var array = GetArrayOfParts(0, size, fileName);
            Console.Write("[ ");
            foreach (var i in array)
            {
                Console.Write(i + ", ");
            }
            Console.WriteLine("]");
        }
    }
}