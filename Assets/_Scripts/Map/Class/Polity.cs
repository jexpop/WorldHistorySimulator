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
        this._rgb32 = PolityColorSelect(rgb);
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
        color32.r = byte.Parse(color.Substring(0, 3));
        color32.g = byte.Parse(color.Substring(4, 3));
        color32.b = byte.Parse(color.Substring(8, 3));

        // Random color for the political frontier - this color is unique
        bool check = false;
        while (!check)
        {
            if (color32.r.ToString() == "0") { color32.r = byte.Parse(Random.Range(0, 255).ToString("D3")); }
            if (color32.g.ToString() == "0") { color32.g = byte.Parse(Random.Range(0, 255).ToString("D3")); }
            if (color32.b.ToString() == "0") { color32.b = byte.Parse(Random.Range(0, 255).ToString("D3")); }
            if(!DeltaEColor.SimilarColorFromList(color32, MapManager.Instance.polityColorList))
            {
                MapManager.Instance.polityColorList.Add(color32);
                check = true;
            }
        }

        return color32;
    }

}
