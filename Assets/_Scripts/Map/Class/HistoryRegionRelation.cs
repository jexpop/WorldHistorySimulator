using System.Collections.Generic;


public class HistoryRegionRelation
{

    private int _stageId;
    private int _settlementId;
    private HistoryStage _stage;


    public int StageId { get { return _stageId; } }
    public int SettlementId { get { return _settlementId; } }
    public HistoryStage Stage { get { return _stage; } }


    public HistoryRegionRelation(int stgaeId, int settlementId, HistoryStage stage)
    {
        this._stageId = stgaeId;
        this._settlementId = settlementId;
        this._stage = stage;
    }

}