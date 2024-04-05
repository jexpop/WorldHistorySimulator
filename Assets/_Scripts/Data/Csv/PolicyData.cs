public class PolicyData
{

    private string _policyId, _policyName;

    public string PolicyId { get { return _policyId; } set { _policyId = value; } }
    public string PolicyName { get { return _policyName; } set { _policyName = value; } }

    public PolicyData(string policyId, string policyName)
    {
        this._policyId = policyId;
        this._policyName = policyName;
    }

}
