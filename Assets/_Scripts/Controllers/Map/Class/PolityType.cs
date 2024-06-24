public class PolityType
{

    private string _name;

    #region Get & Set
    public string Name { get { return _name; } }
    #endregion

    public PolityType(string name)
    {
        this._name = name;
    }

}
