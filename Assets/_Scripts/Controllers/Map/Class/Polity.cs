using UnityEngine;

public class Polity
{

    private string _name;
    private Color32 _rgb32;
    private bool _isCollective;

    #region Get & Set
    public string Name { get { return _name; } }
    public Color32 Rgb32 { get { return _rgb32; } }
    public bool IsCollective { get { return _isCollective; } }
    #endregion

    public Polity(string name, string rgb, bool isCollective)
    {
        this._name = name;
        this._rgb32 = rgb=="255.255.255"? ParamColor.COLOR_WHITE : PolityColorSelect(rgb);
        this._isCollective = isCollective;
    }

    #region Auxiliar Methods
    /// <summary>
    /// Mix color (random & predefined) for the political frontier
    /// </summary>
    /// <param name="color">csv color</param>
    /// <returns>new color</returns>
    private Color32 PolityColorSelect(string color)
    {
        Color32 color32 = new Color32();
        
        // Colors predefined
        color32.r = byte.Parse(color.Substring(0, 3));
        color32.g = byte.Parse(color.Substring(4, 3));
        color32.b = byte.Parse(color.Substring(8, 3));
        
        // Random color for the political frontier - this color is unique
        if(color32.r.ToString() == "0" & color32.g.ToString() == "0" & color32.b.ToString() == "0")
        {            
            if (MapController.Instance.polityColorList.Count > 0)
            {
                color32 = MapController.Instance.polityColorList.Dequeue();
            }
        }

        return color32;
    }

    /// <summary>
    /// Recolor when the polity is white
    /// </summary>
    public void Recolor()
    {
        if (this._rgb32.Equals(ParamColor.COLOR_WHITE))
        {
            this._rgb32 = PolityColorSelect("000.000.000");
        }
    }
    #endregion

}
