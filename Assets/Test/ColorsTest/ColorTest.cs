using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;

public class ColorTest : MonoBehaviour
{

    public List<Color32> colorList = new List<Color32>();

    string fileName = "Assets\\Test\\ColorsTest\\Colors.txt";

    void Start()
    {

        // Reserved colors
        colorList.Add(ParamColor.COLOR_BLACK);
        colorList.Add(ParamColor.COLOR_REGION_HIGHLIGHT);
        colorList.Add(ParamColor.COLOR_REGION_LAKE);
        colorList.Add(ParamColor.COLOR_REGION_LAND);
        colorList.Add(ParamColor.COLOR_REGION_SEA);
        colorList.Add(ParamColor.COLOR_WHITE);

        // Create the file
        if (File.Exists(fileName))
        {
            Debug.Log(fileName + " already exists.");
            return;
        }
        var sr = File.CreateText(fileName);

        // List of colors
        for(int r = 0; r < 256; r++)
        {
            for(int g = 0; g < 256; g++)
            {
                for(int b = 0; b < 256; b++)
                {
                    Color32 color = new Color32((byte)r, (byte)g, (byte)b, 255);

                    if (!DeltaEColor.SimilarColorFromList(color, colorList))
                    {
                        colorList.Add(color);
                        sr.WriteLine("Insert into PolityColorsBaked values (" + r + "," + g + "," + b +");");
                    }
                }
            }
        }

        sr.Close();
        Debug.Log("Done");

    }

}
