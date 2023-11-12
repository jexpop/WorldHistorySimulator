using System.Collections.Generic;
using UnityEngine;

public class Region
{  

    private string _name;
    private string _type;
    private Color32 _rgb32;
    private Settlement _settlement;
    private Polity _owner;
    private int _xMinCoordinates;
    private int _xMaxCoordinates;
    private int _yMinCoordinates;
    private int _yMaxCoordinates;
    private Vector2 _coordinatesCenter;
    private List<HistoryRegionRelation> _history;

    public string Name { get{ return _name; }}
    public string Type { get { return _type; } }
    public Color32 Rgb32 { get { return _rgb32; } }
    public Settlement Settlement { get { return _settlement; } set { _settlement = value; } }
    public Polity Owner { get { return _owner; } set { _owner = value; } }
    public int XminCoordinates { get { return _xMinCoordinates; } set{ _xMinCoordinates = value; } }
    public int XmaxCoordinates { get { return _xMaxCoordinates; } set { _xMaxCoordinates = value; } }
    public int YminCoordinates { get { return _yMinCoordinates; } set { _yMinCoordinates = value; } }
    public int YmaxCoordinates { get { return _yMaxCoordinates; } set { _yMaxCoordinates = value; } }
    public Vector2 CoordinatesCenter {  
        get 
        {

            float centerX = (_xMinCoordinates + ((_xMaxCoordinates - _xMinCoordinates) / 2)) - (GameConst.MAP_SIZE_WIDTH / 2);
            float centerY = (_yMinCoordinates + (_yMaxCoordinates - _yMinCoordinates) / 2) - (GameConst.MAP_SIZE_HIGHT / 2);
            this._coordinatesCenter = new Vector2(centerX, centerY);
            return _coordinatesCenter; 
        } 
    }
    public List<HistoryRegionRelation> History { get { return _history; } set { _history = value; } }


    public Region(string name, string type, int settlement, int owner, List<HistoryRegionRelation> history=null)
    {       

        this._name = name;
        this._type = type;

        // If this region has settlement
        if (settlement != 0)
        {
            Dictionary<int, Settlement> settlements = MapManager.Instance.GetSettlements();
            this._settlement = settlements[settlement];
        }

        // If this region has owner
        if (owner != 0){
            Dictionary<int, Polity> polities = MapManager.Instance.GetPolities();
            this._owner = polities[owner];
        }
        
        this._rgb32 = RegionColorSelect();

        // Initial coordinates
        this._xMinCoordinates = GameConst.MAP_SIZE_WIDTH;
        this._yMinCoordinates = GameConst.MAP_SIZE_HIGHT;
        this._xMaxCoordinates = 0;
        this._yMaxCoordinates = 0;

        this._history = history;

    }

    public void ColorRecalculate()
    {
        this._rgb32 = _owner == null ? GameConst.MAP_REGION_COLOR_LAND : _owner.Rgb32;
    }

    /// <summary>
    /// Choose a region color
    /// </summary>
    /// <returns>new region color</returns>
    private Color32 RegionColorSelect()
    {
        Color32 color32 = GameConst.MAP_REGION_COLOR_LAND;
        if (_owner == null)
        {
            switch (this._type)
            {
                case "sea": color32 = GameConst.MAP_REGION_COLOR_SEA; break;
                case "lake": color32 = GameConst.MAP_REGION_COLOR_LAKE; break;
            }
        }
        else
        {
            color32.r = _owner.Rgb32.r;
            color32.g = _owner.Rgb32.g;
            color32.b = _owner.Rgb32.b;

            // Random color for political frontier
            if (color32.r.ToString() == "0") { color32.r= byte.Parse(Random.Range(0, 255).ToString("D3")); }
            if (color32.g.ToString() == "0") { color32.g = byte.Parse(Random.Range(0, 255).ToString("D3")); }
            if (color32.b.ToString() == "0") { color32.b = byte.Parse(Random.Range(0, 255).ToString("D3")); }
        }

        return color32;
    }

}
