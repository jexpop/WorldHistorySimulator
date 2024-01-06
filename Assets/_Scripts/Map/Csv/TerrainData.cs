public class TerrainData
{

    private string _terrainId, _terrainName, _terrainTypeId;

    public string TerrainId { get { return _terrainId; } set { _terrainId = value; } }
    public string TerrainName { get { return _terrainName; } set { _terrainName = value; } }
    public string TerrainTypeId { get { return _terrainTypeId; } set { _terrainTypeId = value; } }

    public TerrainData(string terrainId, string terrainName, string terrainTypeId)
    {
        this._terrainId = terrainId;
        this._terrainName = terrainName;
        this._terrainTypeId = terrainTypeId;
    }

}
