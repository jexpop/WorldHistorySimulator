using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Polity
{

    private string _name;
    private Color32 _rgb32;
    private bool _isCollective;


    public string Name { get { return _name; } }
    public Color32 Rgb32 { get { return _rgb32; } }
    public bool IsCollective { get { return _isCollective; } }


    public Polity(string name, string rgb, bool isCollective)
    {
        this._name = name;
        this._rgb32 = rgb=="255.255.255"? ParamColor.COLOR_WHITE : PolityColorSelect(rgb);
        this._isCollective = isCollective;
    }

    /// <summary>
    /// Mix color (random & database) for the political frontier
    /// </summary>
    /// <param name="color">database color</param>
    /// <returns>new color</returns>
    private Color32 PolityColorSelect(string color)
    {
        Color32 color32 = new Color32();
        
        // Colors from database
        color32.r = byte.Parse(color.Substring(0, 3));
        color32.g = byte.Parse(color.Substring(4, 3));
        color32.b = byte.Parse(color.Substring(8, 3));

        // Random color for the political frontier - this color is unique
        if(color32.r.ToString() == "0" & color32.g.ToString() == "0" & color32.b.ToString() == "0")
        {
            if(MapManager.Instance.polityColorList.Count > 0)
            {
                color32 = MapManager.Instance.polityColorList.Dequeue();
            }
        }

        return color32;
    }

}
