public class PolityData
{

    private string _polityId, _polityName, _rgb, _policyId;

    public string PolityId { get { return _polityId; } set { _polityId = value; } }
    public string PolityName { get { return _polityName; } set { _polityName = value; } }
    public string Rgb { get { return _rgb; } set { _rgb = value; } }
    public string PolicyId { get { return _policyId; } set { _policyId = value; } }

    public PolityData(string polityId, string polityName, string rgb, string policyId)
    {
        this._polityId = polityId;
        this._polityName = polityName;
        this._rgb = rgb;
        this._policyId = policyId;
    }

}
