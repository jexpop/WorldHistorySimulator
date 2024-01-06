public class TerritoryHistoryData
{

    private string _stageId, _regionId, _settlementId;
    private int _startDate, _endDate;
    private string _l1_PolityParentId, _l2_PolityParentId, _l3_PolityParentId, _l4_PolityParentId,
        _l1_PolityTypeIdParent, _l2_PolityTypeIdParent, _l3_PolityTypeIdParent, _l4_PolityTypeIdParent,
        _l1_PolityCapital, _l2_PolityCapital, _l3_PolityCapital, _l4_PolityCapital,
        _policyPolityId, _policyPolityTypeId, _policyCapital;

    public string StageId { get { return _stageId; } set { _stageId = value; } }
    public string RegionId { get { return _regionId; } set { _regionId = value; } }
    public string SettlementId { get { return _settlementId; } set { _settlementId = value; } }
    public int StartDate { get { return _startDate; } set { _startDate = value; } }
    public int EndDate { get { return _endDate; } set { _endDate = value; } }
    public string L1_PolityParentId { get { return _l1_PolityParentId; } set { _l1_PolityParentId = value; } }
    public string L2_PolityParentId { get { return _l2_PolityParentId; } set { _l2_PolityParentId = value; } }
    public string L3_PolityParentId { get { return _l3_PolityParentId; } set { _l3_PolityParentId = value; } }
    public string L4_PolityParentId { get { return _l4_PolityParentId; } set { _l4_PolityParentId = value; } }
    public string L1_PolityTypeIdParent { get { return _l1_PolityTypeIdParent; } set { _l1_PolityTypeIdParent = value; } }
    public string L2_PolityTypeIdParent { get { return _l2_PolityTypeIdParent; } set { _l2_PolityTypeIdParent = value; } }
    public string L3_PolityTypeIdParent { get { return _l3_PolityTypeIdParent; } set { _l3_PolityTypeIdParent = value; } }
    public string L4_PolityTypeIdParent { get { return _l4_PolityTypeIdParent; } set { _l4_PolityTypeIdParent = value; } }
    public string L1_PolityCapital { get { return _l1_PolityCapital; } set { _l1_PolityCapital = value; } }
    public string L2_PolityCapital { get { return _l2_PolityCapital; } set { _l2_PolityCapital = value; } }
    public string L3_PolityCapital { get { return _l3_PolityCapital; } set { _l3_PolityCapital = value; } }
    public string L4_PolityCapital { get { return _l4_PolityCapital; } set { _l4_PolityCapital = value; } }
    public string PolicyPolityId { get { return _policyPolityId; } set { _policyPolityId = value; } }
    public string PolicyPolityTypeId { get { return _policyPolityTypeId; } set { _policyPolityTypeId = value; } }
    public string PolicyCapital { get { return _policyCapital; } set { _policyCapital = value; } }

    public TerritoryHistoryData(string stageId, string regionId, string settlementId, int startDate, int endDate,
        string l1_PolityParentId, string l2_PolityParentId, string l3_PolityParentId, string l4_PolityParentId,
        string l1_PolityTypeIdParent, string l2_PolityTypeIdParent, string l3_PolityTypeIdParent, string l4_PolityTypeIdParent,
        string l1_PolityCapital, string l2_PolityCapital, string l3_PolityCapital, string l4_PolityCapital,
        string policyPolityId, string policyPolityTypeId, string policyCapital) 
    {
        this._stageId = stageId;
        this._regionId = regionId;
        this._settlementId = settlementId;
        this._startDate = startDate;
        this._endDate = endDate;
        this._l1_PolityParentId = l1_PolityParentId;
        this._l2_PolityParentId = l2_PolityParentId;
        this._l3_PolityParentId = l3_PolityParentId;
        this._l4_PolityParentId = l4_PolityParentId;
        this._l1_PolityTypeIdParent = l1_PolityTypeIdParent;
        this._l2_PolityTypeIdParent = l2_PolityTypeIdParent;
        this._l3_PolityTypeIdParent = l3_PolityTypeIdParent;
        this._l4_PolityTypeIdParent = l4_PolityTypeIdParent;
        this._l1_PolityCapital = l1_PolityCapital;
        this._l2_PolityCapital = l2_PolityCapital;
        this._l3_PolityCapital = l3_PolityCapital;
        this._l4_PolityCapital = l4_PolityCapital;
        this._policyPolityId = policyPolityId;
        this._policyPolityTypeId = policyPolityTypeId;
        this._policyCapital = policyCapital;
    }

}
