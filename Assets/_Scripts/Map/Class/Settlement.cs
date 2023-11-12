public class Settlement
{

    private string _name;
    private int _regionId;

    public string Name { get { return _name; } }
    public int RegionId { get { return _regionId; } }


    public Settlement(string name, int regionId)
    {
        this._name = name;
        this._regionId = regionId;
    }

}
