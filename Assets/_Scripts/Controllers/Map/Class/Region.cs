using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HDROutputUtils;
using static UnityEngine.UI.GridLayoutGroup;

public class Region
{  

    private string _name;
    private string _type;
    private string _terrain;
    private Color32 _rgb32;
    private Settlement _settlement;
    private Polity _owner;
    private int _xMinCoordinates;
    private int _xMaxCoordinates;
    private int _yMinCoordinates;
    private int _yMaxCoordinates;
    private Vector2 _coordinatesCenter;
    private List<HistoryRegionRelation> _history;

    #region Get & Set
    public string Name { get{ return _name; }}
    public string Type { get { return _type; } }
    public string Terrain { get { return _terrain; } set { _terrain = value; } }
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

            float centerX = (_xMinCoordinates + ((_xMaxCoordinates - _xMinCoordinates) / 2)) - (ParamMap.MAP_SIZE_WIDTH / 2);
            float centerY = (_yMinCoordinates + (_yMaxCoordinates - _yMinCoordinates) / 2) - (ParamMap.MAP_SIZE_HIGHT / 2);
            this._coordinatesCenter = new Vector2(centerX, centerY);
            return _coordinatesCenter; 
        } 
    }
    public List<HistoryRegionRelation> History { get { return _history; } set { _history = value; } }
    #endregion

    public Region(string name, string type, string _terrain, int settlement, int owner, List<HistoryRegionRelation> history=null)
    {       

        this._name = name;
        this._type = type;
        this._terrain = _terrain;

        // If this region has settlement
        if (settlement != 0)
        {
            this._settlement = UpdateSettlement(settlement);
        }

        // If this region has owner
        if (owner != 0){
            this._owner = UpdateOwner(owner);
        }
        
        this._rgb32 = RegionColorSelect();

        // Initial coordinates
        this._xMinCoordinates = ParamMap.MAP_SIZE_WIDTH;
        this._yMinCoordinates = ParamMap.MAP_SIZE_HIGHT;
        this._xMaxCoordinates = 0;
        this._yMaxCoordinates = 0;

        this._history = history;

    }

    #region Auxiliar Methods
    public void ColorRecalculate()
    {
        this._rgb32 = RegionColorSelect(); // _owner == null ? ParamColor.COLOR_REGION_LAND : _owner.Rgb32;
    }

    private Settlement UpdateSettlement(int settlementId)
    {
        Dictionary<int, Settlement> settlements = MapController.Instance.GetSettlements();
        return settlements[settlementId];
    }

    private Polity UpdateOwner(int ownerId)
    {
        Dictionary<int, Polity> polities = MapController.Instance.GetPolities();
        return polities[ownerId];
    }

    /// <summary>
    /// Choose a region color
    /// </summary>
    /// <returns>new region color</returns>
    private Color32 RegionColorSelect()
    {
        // Default color
        Color32 color32 = ParamColor.COLOR_REGION_LAND;
        
        // Unknown region
        if (_owner == null)
        {
            // Is a water region?
            color32 = this._type == ParamUI.REGION_NAME_WATER ? ParamColor.COLOR_REGION_WATER : color32;

            // Water colors to the sea editor
            if (EditorUICanvasController.Instance.showSea.isOn)
            {
                switch (this._terrain){
                    case var value when value == ParamUI.REGION_NAME_COAST: color32 = ParamColor.COLOR_REGION_COAST; break;
                    case var value when value == ParamUI.REGION_NAME_LAKE: color32 = ParamColor.COLOR_REGION_LAKE; break;
                    case var value when value == ParamUI.REGION_NAME_OCEAN: color32 = ParamColor.COLOR_REGION_OCEAN; break;
                    case var value when value == ParamUI.REGION_NAME_SEA: color32 = ParamColor.COLOR_REGION_SEA; break;
                }
            }

        }
        else
        {// Region with owner
            color32.r = _owner.Rgb32.r;
            color32.g = _owner.Rgb32.g;
            color32.b = _owner.Rgb32.b;

            // Random color for political frontier ???
            /*if (color32.r.ToString() == "0") { color32.r= byte.Parse(Random.Range(0, 255).ToString("D3")); }
            if (color32.g.ToString() == "0") { color32.g = byte.Parse(Random.Range(0, 255).ToString("D3")); }
            if (color32.b.ToString() == "0") { color32.b = byte.Parse(Random.Range(0, 255).ToString("D3")); }*/
        }

        return color32;
    }
    #endregion

}
