using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;


public static class Utilities
{

    /// <summary>
    /// This function compares multiple values with other
    /// </summary>
    /// <param name="compared">Value compared</param>
    /// <param name="elements">List of values to compare</param>
    /// <returns></returns>
    public static bool CompareStringsMember(string compared, List<string> elements)
    {
        bool isMember = false;
        foreach(string element in elements)
        {
            if (element == compared)
            {
                isMember = true;                
                break;
            }
        }
        return isMember;
    }

    /// <summary>
    /// Check if a number exists in a range
    /// </summary>
    /// <param name="start">Firts value</param>
    /// <param name="stop">Last value</param>
    /// <param name="check">Value checked</param>
    /// <returns>result checked</returns>
    public static bool Range(int start, int stop, int check)
    {
        bool result = false;
        for (int i = start; i <= stop; i++) { if(i==check) { result = true; } }
        return result;
    }

    /// <summary>
    /// Random items in a list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    private static System.Random rng = new System.Random();  
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Get a Texture 2D from a defined path
    /// </summary>
    /// <param name="path">path of the image</param>
    /// <returns>Texture 2D</returns>
    public static Texture2D GetTexture2D(string path)
    {
        if (File.Exists(path))
        {
            //Converts desired path into byte array
            byte[] pngBytes = File.ReadAllBytes(path);

            //Creates texture and loads byte array data to create image
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false); 
            tex.LoadImage(pngBytes); // revisar loadimage

            return tex;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Capitalize on the first letter of each word in a string
    /// </summary>
    /// <param name="line">any words</param>
    /// <returns>words in pascal case</returns>
    public static string PascalStrings(string line)
    {       
        string pascalCase = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(line.ToLower());
        return pascalCase.Replace(" ", "");
    }

    /// <summary>
    /// Compare if first value is zero then return second value
    /// </summary>
    /// <param name="first">first int</param>
    /// <param name="second">second int</param>
    /// <returns>second or first values</returns>
    public static int EitherInt(int first, int second)
    {
        if(first == 0)
        {
            return second;
        }
        else
        {
            return first;
        }
    }

}