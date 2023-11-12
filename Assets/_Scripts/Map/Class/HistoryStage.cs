public class HistoryStage
{

    private int _startDate;
    private int _endDate;
    private int _polityParentId_L1;
    private int _polityParentId_L2;
    private int _polityParentId_L3;
    private int _polityParentId_L4;
    private int _polityTypeIdParent_L1;
    private int _polityTypeIdParent_L2;
    private int _polityTypeIdParent_L3;
    private int _polityTypeIdParent_L4;
    private int _policyId;
    private int _policyTypeId;

    public int StartDate { get { return _startDate; } }
    public int EndDate { get { return _endDate; } }
    public int PolityParentId_L1 { get { return _polityParentId_L1; } }
    public int PolityParentId_L2 { get { return _polityParentId_L2; } }
    public int PolityParentId_L3 { get { return _polityParentId_L3; } }
    public int PolityParentId_L4 { get { return _polityParentId_L4; } }
    public int PolityTypeIdParent_L1 { get { return _polityTypeIdParent_L1; } }
    public int PolityTypeIdParent_L2 { get { return _polityTypeIdParent_L2; } }
    public int PolityTypeIdParent_L3 { get { return _polityTypeIdParent_L3; } }
    public int PolityTypeIdParent_L4 { get { return _polityTypeIdParent_L4; } }
    public int PolicyId { get { return _policyId; } }
    public int PolicyTypeId { get { return _policyTypeId; } }


    public HistoryStage(
                                        int startDate, int endDate, 
                                        int polityParentId_L1, int polityParentId_L2, int polityParentId_L3, int polityParentId_L4, 
                                        int polityTypeIdParent_L1, int polityTypeIdParent_L2, int polityTypeIdParent_L3, int polityTypeIdParent_L4, 
                                        int policyPolityId, int policyPolityTypeId)
    {
        this._startDate = startDate;
        this._endDate = endDate;
        this._polityParentId_L1 = polityParentId_L1;
        this._polityParentId_L2 = polityParentId_L2;
        this._polityParentId_L3 = polityParentId_L3;
        this._polityParentId_L4 = polityParentId_L4;
        this._polityTypeIdParent_L1 = polityTypeIdParent_L1;
        this._polityTypeIdParent_L2 = polityTypeIdParent_L2;
        this._polityTypeIdParent_L3 = polityTypeIdParent_L3;
        this._polityTypeIdParent_L4 = polityTypeIdParent_L4;
        this._policyId = policyPolityId;
        this._policyTypeId = policyPolityTypeId;
    }

}
