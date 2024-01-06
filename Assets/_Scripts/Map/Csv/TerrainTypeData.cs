public class TerrainTypeData
{

    private string _terrainTypeId, _terrainTypeName;

    public string TerrainTypeId { get { return _terrainTypeId; } set { _terrainTypeId = value; } }
    public string TerrainTypeName { get { return _terrainTypeName; } set { _terrainTypeName = value; } }

    public TerrainTypeData(string terrainTypeId, string terrainTypeName)
    {
        this._terrainTypeId = terrainTypeId;
        this._terrainTypeName = terrainTypeName;
    }

}
