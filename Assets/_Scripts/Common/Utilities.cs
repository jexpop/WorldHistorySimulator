using System.Collections.Generic;
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

}