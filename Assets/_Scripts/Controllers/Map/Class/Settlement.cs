using UnityEngine;

public class Settlement
{

    private string _name;
    private int _regionId;
    private Vector2Int _pixelCoordinates;

    public string Name { get { return _name; } }
    public int RegionId { get { return _regionId; } }
    public Vector2Int PixelCoordinates { get { return _pixelCoordinates; } }


    public Settlement(string name, int regionId, Vector2Int pixelCoordinates)
    {
        this._name = name;
        this._regionId = regionId;
        this._pixelCoordinates = pixelCoordinates;
    }

}