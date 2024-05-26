using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageFloatingPanel : MonoBehaviour
{

    public TextMeshProUGUI settlementNameLabel;

    [Header("Components of the copy to other region")]
    public TMP_InputField copyRegionId;
    public TMP_Dropdown settlementDropdown;
    public EditorDropdown settlementEditorDropdown;

    [Header("Dates of the stage")]
    public TMP_InputField dFromDate, mFromDate, yFromDate;
    public TMP_InputField dToDate, mToDate, yToDate;
    public Toggle isSymbolForDate;

    [Header("Components of the owner's parent Level 1")]
    public TMP_InputField parentTypeFilter_L1;
    public TMP_Dropdown parentTypeDropdown_L1;
    public TMP_InputField parentFilter_L1;
    public TMP_Dropdown parentDropdown_L1;
    public EditorDropdown parentTypeEditorDropdown_L1;
    public EditorDropdown parentEditorDropdown_L1;
    public Toggle isCapital_L1;

    [Header("Components of the owner's parent Level 2")]
    public TMP_InputField parentTypeFilter_L2;
    public TMP_Dropdown parentTypeDropdown_L2;
    public TMP_InputField parentFilter_L2;
    public TMP_Dropdown parentDropdown_L2;
    public EditorDropdown parentTypeEditorDropdown_L2;
    public EditorDropdown parentEditorDropdown_L2;
    public Toggle isCapital_L2;

    [Header("Components of the owner's parent Level 3")]
    public TMP_InputField parentTypeFilter_L3;
    public TMP_Dropdown parentTypeDropdown_L3;
    public TMP_InputField parentFilter_L3;
    public TMP_Dropdown parentDropdown_L3;
    public EditorDropdown parentTypeEditorDropdown_L3;
    public EditorDropdown parentEditorDropdown_L3;
    public Toggle isCapital_L3;

    [Header("Components of the owner's parent Level 4")]
    public TMP_InputField parentTypeFilter_L4;
    public TMP_Dropdown parentTypeDropdown_L4;
    public TMP_InputField parentFilter_L4;
    public TMP_Dropdown parentDropdown_L4;
    public EditorDropdown parentTypeEditorDropdown_L4;
    public EditorDropdown parentEditorDropdown_L4;
    public Toggle isCapital_L4;

    [Header("Components of the policy")]
    public TMP_InputField policyTypeFilter;
    public TMP_Dropdown policyTypeDropdown;
    public TMP_InputField policyFilter;
    public TMP_Dropdown policyDropdown;
    public EditorDropdown policyTypeEditorDropdown;
    public EditorDropdown policyEditorDropdown;
    public Toggle isCapital_Policy;

    [Header("Status messages")]
    public SimpleMessage stageOkMessage;
    public SimpleMessage emptyDatesMessage;
    public SimpleMessage duplicatedDatesMessage;
    public SimpleMessage koDatesMessage;


    private int stageId = 0;
    private int regionId = 0;
    private int settlementId = 0;


    public void ChangeActive(GameObject UIgameobject)
    {
        EditorUICanvasController.Instance.ChangeActive(UIgameobject);
    }

    public void SetStageId(int stageId)
    {
        this.stageId = stageId;
    }
    public void SetRegionId(int regionId)
    {
        this.regionId = regionId;
    }
    public void SetSettlementId(int settlementId)
    {
        this.settlementId = settlementId;
    }
    public void SetSettlementName(string table, string key)
    {
        LocalizationController.Instance.AddLocalizeString(settlementNameLabel, table, key);
    }


    private int GetDropdownValue(EditorDropdown dropdown, int id, bool isForward, bool option, int filter=0)
    {

        if (isForward)
        {
            return dropdown.GetOptionsIds(option, filter).Forward[id];
        }
        else
        {
            return dropdown.GetOptionsIds(option, filter).Reverse[id];
        }
        
    }
    private int GetDropdownValue(EditorDropdown dropdown, int id, bool isForward, bool option, string filter)
    {       
        if (isForward)
        {
            return dropdown.GetOptionsIds(option, filter).Forward[id];
        }
        else
        {
            return dropdown.GetOptionsIds(option, filter).Reverse[id];
        }

    }


    public void SetHistory(HistoryStage historyStage = null)
    {
        string fromDate, toDate;

        // Clear filters
        parentTypeFilter_L1.text = "";
        parentTypeFilter_L2.text = "";
        parentTypeFilter_L3.text = "";
        parentTypeFilter_L4.text = "";
        parentFilter_L1.text = "";
        parentFilter_L2.text = "";
        parentFilter_L3.text = "";
        parentFilter_L4.text = "";
        policyTypeFilter.text = "";
        policyFilter.text = "";

        if (historyStage != null)
        {
            // Dates
            fromDate = historyStage.StartDate.ToString("D8");
            toDate = historyStage.EndDate.ToString("D8");

            int fromDateLength = fromDate.Substring(0, 1) == "-" ? 5 : 4;
            int toDateLength = toDate.Substring(0, 1) == "-" ? 5 : 4;

            // Dates format
            dFromDate.text = fromDate.Substring(fromDateLength + 2, 2).TrimStart(new Char[] { '0' });
            mFromDate.text = fromDate.Substring(fromDateLength, 2).TrimStart(new Char[] { '0' });
            yFromDate.text = fromDate.Substring(0, fromDateLength).TrimStart(new Char[] { '0' });
            dToDate.text = toDate.Substring(toDateLength + 2, 2).TrimStart(new Char[] { '0' });
            mToDate.text = toDate.Substring(toDateLength, 2).TrimStart(new Char[] { '0' });
            yToDate.text = toDate.Substring(0, toDateLength).TrimStart(new Char[] { '0' });

            // Symbol date
            isSymbolForDate.isOn = historyStage.IsSymbolForDate == 1 ? true : false;

            // Parent 
            parentTypeDropdown_L1.value = GetDropdownValue(parentTypeEditorDropdown_L1, historyStage.PolityTypeIdParent_L1, false, false, "");
            parentTypeDropdown_L2.value = GetDropdownValue(parentTypeEditorDropdown_L2, historyStage.PolityTypeIdParent_L2, false, true, "");
            parentTypeDropdown_L3.value = GetDropdownValue(parentTypeEditorDropdown_L3, historyStage.PolityTypeIdParent_L3, false, true, "");
            parentTypeDropdown_L4.value = GetDropdownValue(parentTypeEditorDropdown_L4, historyStage.PolityTypeIdParent_L4, false, true, "");

            parentDropdown_L1.value = GetDropdownValue(parentEditorDropdown_L1, historyStage.PolityParentId_L1, false, false, "");
            parentDropdown_L2.value = GetDropdownValue(parentEditorDropdown_L2, historyStage.PolityParentId_L2, false, true, "");
            parentDropdown_L3.value = GetDropdownValue(parentEditorDropdown_L3, historyStage.PolityParentId_L3, false, true, "");
            parentDropdown_L4.value = GetDropdownValue(parentEditorDropdown_L4, historyStage.PolityParentId_L4, false, true, "");

            isCapital_L1.isOn=historyStage.Capital_L1==1?true:false;
            isCapital_L2.isOn = historyStage.Capital_L2 == 1 ? true : false;
            isCapital_L3.isOn = historyStage.Capital_L3 == 1 ? true : false;
            isCapital_L4.isOn = historyStage.Capital_L4 == 1 ? true : false;

            // Policy
            policyTypeDropdown.value = GetDropdownValue(policyTypeEditorDropdown, historyStage.PolicyTypeId, false, true, "");
            policyDropdown.value = GetDropdownValue(policyEditorDropdown, historyStage.PolicyId, false, true, "");
            isCapital_Policy.isOn = historyStage.PolicyCapital == 1 ? true : false;

        }
        else
        {
            // Current date
            string currentYear = EditorUICanvasController.Instance.GetCurrentTimeline(false).ToString();

            int yearLength = currentYear.Substring(0,1)=="-" ? 5 : 4;

            // Dates format
            dFromDate.text = "01";
            mFromDate.text = "01";
            yFromDate.text = currentYear.Substring(0, yearLength).TrimStart(new Char[] { '0' });
            dToDate.text = "31";
            mToDate.text = "12";
            yToDate.text = currentYear.Substring(0, yearLength).TrimStart(new Char[] { '0' });

            // Symbol date
            isSymbolForDate.isOn = false;

            // Parent
            parentTypeEditorDropdown_L1.LoadOptions(false, "");
            parentTypeEditorDropdown_L2.LoadOptions(true, "");
            parentTypeEditorDropdown_L3.LoadOptions(true, "");
            parentTypeEditorDropdown_L4.LoadOptions(true, "");
            parentEditorDropdown_L1.LoadOptions(false, "");
            parentEditorDropdown_L2.LoadOptions(true, "");
            parentEditorDropdown_L3.LoadOptions(true, "");
            parentEditorDropdown_L4.LoadOptions(true, "");
            parentTypeDropdown_L1.value = 0;
            parentTypeDropdown_L2.value = 0;
            parentTypeDropdown_L3.value = 0;
            parentTypeDropdown_L4.value = 0;
            parentDropdown_L1.value = 0;
            parentDropdown_L2.value = 0;
            parentDropdown_L3.value = 0;
            parentDropdown_L4.value = 0;
            isCapital_L1.isOn = false;
            isCapital_L2.isOn = false;
            isCapital_L3.isOn = false;
            isCapital_L4.isOn = false;

            // Policy
            policyTypeEditorDropdown.LoadOptions(true, "");
            policyEditorDropdown.LoadOptions(true, "");
            policyTypeDropdown.value = 0;
            policyDropdown.value = 0;
            isCapital_Policy.isOn = false;

        }

    }


    /*** CHECKS ***/
    private bool DatesRangeOk()
    {
        int startDate = Int32.Parse(yFromDate.text + mFromDate.text.PadLeft(2, '0') + dFromDate.text.PadLeft(2, '0'));
        int endDate = Int32.Parse(yToDate.text + mToDate.text.PadLeft(2, '0') + dToDate.text.PadLeft(2, '0'));
        bool result = startDate <= endDate ? true : false;
        return result;
    }
    private bool DatesExist(int checkedRegion, int idStage = 0)
    {
        int newStartDate = Int32.Parse(yFromDate.text + mFromDate.text.PadLeft(2, '0') + dFromDate.text.PadLeft(2, '0'));
        int newEndDate = Int32.Parse(yToDate.text + mToDate.text.PadLeft(2, '0') + dToDate.text.PadLeft(2, '0'));

        bool exist = false;

        Region region = MapController.Instance.GetRegionById(checkedRegion);
        List<HistoryRegionRelation> regionHistory = region.History;
        if (regionHistory != null)
        {
            foreach (HistoryRegionRelation stage in regionHistory)
            {
                if (
                        (Utilities.Range(stage.Stage.StartDate, stage.Stage.EndDate, newStartDate) ||
                        Utilities.Range(stage.Stage.StartDate, stage.Stage.EndDate, newEndDate))
                        && stage.StageId != idStage
                     )
                {
                    exist = true;
                }
            }
        }

        return exist;
    }
    /***                        ***/


    /*** Action Button events ***/
    // COPY BUTTON
    public void CopyActionButtonEvent()
    {
        // Clear other filters
        parentTypeFilter_L1.text = "";
        parentTypeFilter_L2.text = "";
        parentTypeFilter_L3.text = "";
        parentTypeFilter_L4.text = "";
        parentFilter_L1.text = "";
        parentFilter_L2.text = "";
        parentFilter_L3.text = "";
        parentFilter_L4.text = "";
        policyTypeFilter.text = "";
        policyFilter.text = "";

        int regionId = Int32.Parse(copyRegionId.text);
        int settlementId = GetDropdownValue(settlementEditorDropdown, settlementDropdown.value-1, true, false, regionId);
        SaveActionEvent(stageOkMessage, true, regionId, settlementId);
    }
    // SAVE BUTTON
    public void SaveActionButtonEvent()
    {
        SaveActionEvent(stageOkMessage);
    }
    private void SaveActionEvent(SimpleMessage okNameMessage, bool isCopy=false, int region=0, int settlement=0)
    {
        // Remove old fields messages
        LocalizationController.Instance.AddLocalizeString(okNameMessage);
        
        // Global Checks
        if (
                MessageHelper.IsFieldEmpty(dFromDate.text) == true ||
                MessageHelper.IsFieldEmpty(mFromDate.text) == true ||
                MessageHelper.IsFieldEmpty(yFromDate.text) == true ||
                MessageHelper.IsFieldEmpty(dToDate.text) == true ||
                MessageHelper.IsFieldEmpty(mToDate.text) == true ||
                MessageHelper.IsFieldEmpty(yToDate.text) == true 
            )
        {
            LocalizationController.Instance.AddLocalizeString(emptyDatesMessage);
        }
        else if (!DatesRangeOk())
        {
            LocalizationController.Instance.AddLocalizeString(koDatesMessage);
        }
        else
        {
            int regionNew = region == 0 ? regionId : region;
            int settlementNew = settlement == 0 ? settlementId : settlement;

            // If all ok  then insert/modify data            
            if (stageId == 0 || isCopy == true)
            {
                if (DatesExist(regionNew))
                {// Check duplicate dates
                    LocalizationController.Instance.AddLocalizeString(duplicatedDatesMessage);
                }
                else
                {

                    // Data
                    int tmpFromDate = Int32.Parse(yFromDate.text + mFromDate.text.PadLeft(2, '0') + dFromDate.text.PadLeft(2, '0'));
                    int tmpToDate = Int32.Parse(yToDate.text + mToDate.text.PadLeft(2, '0') + dToDate.text.PadLeft(2, '0'));                    
                    int tmpParentEditorDropdown_L1 = GetDropdownValue(parentEditorDropdown_L1, parentDropdown_L1.value, true, false, parentFilter_L1.text);
                    int tmpParentEditorDropdown_L2 = GetDropdownValue(parentEditorDropdown_L2, parentDropdown_L2.value, true, true, parentFilter_L2.text);
                    int tmpParentEditorDropdown_L3 = GetDropdownValue(parentEditorDropdown_L3, parentDropdown_L3.value, true, true, parentFilter_L3.text);
                    int tmpParentEditorDropdown_L4 = GetDropdownValue(parentEditorDropdown_L4, parentDropdown_L4.value, true, true, parentFilter_L4.text);
                    int tmpParentTypeEditorDropdown_L1 = GetDropdownValue(parentTypeEditorDropdown_L1, parentTypeDropdown_L1.value, true, false, parentTypeFilter_L1.text);
                    int tmpParentTypeEditorDropdown_L2 = GetDropdownValue(parentTypeEditorDropdown_L2, parentTypeDropdown_L2.value, true, true, parentTypeFilter_L2.text);
                    int tmpParentTypeEditorDropdown_L3 = GetDropdownValue(parentTypeEditorDropdown_L3, parentTypeDropdown_L3.value, true, true, parentTypeFilter_L3.text);
                    int tmpParentTypeEditorDropdown_L4 = GetDropdownValue(parentTypeEditorDropdown_L4, parentTypeDropdown_L4.value, true, true, parentTypeFilter_L4.text);
                    int tmpIsCapital_L1 = isCapital_L1.isOn == true ? 1 : 0;
                    int tmpIsCapital_L2 = isCapital_L2.isOn == true ? 1 : 0;
                    int tmpIsCapital_L3 = isCapital_L3.isOn == true ? 1 : 0;
                    int tmpIsCapital_L4 = isCapital_L4.isOn == true ? 1 : 0;
                    int tmpPolicyEditorDropdown = GetDropdownValue(policyEditorDropdown, policyDropdown.value, true, true, policyFilter.text);
                    int tmpPolicyTypeEditorDropdown = GetDropdownValue(policyTypeEditorDropdown, policyTypeDropdown.value, true, true, policyTypeFilter.text);
                    int tmpIsCapital_Policy = isCapital_Policy.isOn == true ? 1 : 0;
                    int tmpIsSymbolForDate = isSymbolForDate.isOn == true ? 1 : 0;

                    CsvConnection.Instance.AddStage(
                                                                                    regionNew,
                                                                                    settlementNew,
                                                                                    tmpFromDate, tmpToDate,
                                                                                    tmpParentEditorDropdown_L1, tmpParentEditorDropdown_L2, tmpParentEditorDropdown_L3, tmpParentEditorDropdown_L4,
                                                                                    tmpParentTypeEditorDropdown_L1, tmpParentTypeEditorDropdown_L2, tmpParentTypeEditorDropdown_L3, tmpParentTypeEditorDropdown_L4,
                                                                                    tmpIsCapital_L1, tmpIsCapital_L2, tmpIsCapital_L3, tmpIsCapital_L4,
                                                                                    tmpPolicyEditorDropdown, tmpPolicyTypeEditorDropdown,
                                                                                    tmpIsCapital_Policy, tmpIsSymbolForDate
                                                                                );

                    // Add data in the dictionary of the stages
                    HistoryStage stage = new HistoryStage(
                                                                                    tmpFromDate, tmpToDate,
                                                                                    tmpParentEditorDropdown_L1, tmpParentEditorDropdown_L2, tmpParentEditorDropdown_L3, tmpParentEditorDropdown_L4,
                                                                                    tmpParentTypeEditorDropdown_L1, tmpParentTypeEditorDropdown_L2, tmpParentTypeEditorDropdown_L3, tmpParentTypeEditorDropdown_L4,
                                                                                    tmpIsCapital_L1, tmpIsCapital_L2, tmpIsCapital_L3, tmpIsCapital_L4,
                                                                                    tmpPolicyEditorDropdown, tmpPolicyTypeEditorDropdown,
                                                                                    tmpIsCapital_Policy, tmpIsSymbolForDate
                        );

                    HistoryRegionRelation history = new HistoryRegionRelation(
                                                                                    CsvConnection.Instance.GetLastIdAdded(EditorDataType.StagePanel),
                                                                                    settlementNew,
                                                                                    stage
                        );

                    MapController.Instance.GetRegionById(regionNew).History.Add(history);

                    // Refreshing UI stage
                    SetHistory(stage);

                    //if (isCopy == false)
                    //{
                        // Refreshing data                   
                        if (EditorUICanvasController.Instance.IsDateCurrent(Int32.Parse(yFromDate.text + mFromDate.text.PadLeft(2, '0') + dFromDate.text.PadLeft(2, '0')), Int32.Parse(yToDate.text + mToDate.text.PadLeft(2, '0') + dToDate.text.PadLeft(2, '0'))))
                        {
                            EditorUICanvasController.Instance.RefleshingHistory(regionNew, false, settlementNew, isCopy);
                        }
                        else
                        {
                            EditorUICanvasController.Instance.RefleshingHistory(regionNew, false, 0, isCopy);
                        }
                    //}
               
                }           
            }
            else
            { // Update new data
                if (DatesExist(regionNew, stageId))
                {// Check duplicate dates
                    LocalizationController.Instance.AddLocalizeString(duplicatedDatesMessage);
                }
                else
                {

                    // Data
                    int tmpFromDate = Int32.Parse(yFromDate.text + mFromDate.text.PadLeft(2, '0') + dFromDate.text.PadLeft(2, '0'));
                    int tmpToDate = Int32.Parse(yToDate.text + mToDate.text.PadLeft(2, '0') + dToDate.text.PadLeft(2, '0'));
                    int tmpParentEditorDropdown_L1 = GetDropdownValue(parentEditorDropdown_L1, parentDropdown_L1.value, true, false, parentFilter_L1.text);
                    int tmpParentEditorDropdown_L2 = GetDropdownValue(parentEditorDropdown_L2, parentDropdown_L2.value, true, true, parentFilter_L2.text);
                    int tmpParentEditorDropdown_L3 = GetDropdownValue(parentEditorDropdown_L3, parentDropdown_L3.value, true, true, parentFilter_L3.text);
                    int tmpParentEditorDropdown_L4 = GetDropdownValue(parentEditorDropdown_L4, parentDropdown_L4.value, true, true, parentFilter_L4.text);
                    int tmpParentTypeEditorDropdown_L1 = GetDropdownValue(parentTypeEditorDropdown_L1, parentTypeDropdown_L1.value, true, false, parentTypeFilter_L1.text);
                    int tmpParentTypeEditorDropdown_L2 = GetDropdownValue(parentTypeEditorDropdown_L2, parentTypeDropdown_L2.value, true, true, parentTypeFilter_L2.text);
                    int tmpParentTypeEditorDropdown_L3 = GetDropdownValue(parentTypeEditorDropdown_L3, parentTypeDropdown_L3.value, true, true, parentTypeFilter_L3.text);
                    int tmpParentTypeEditorDropdown_L4 = GetDropdownValue(parentTypeEditorDropdown_L4, parentTypeDropdown_L4.value, true, true, parentTypeFilter_L4.text);
                    int tmpIsCapital_L1 = isCapital_L1.isOn == true ? 1 : 0;
                    int tmpIsCapital_L2 = isCapital_L2.isOn == true ? 1 : 0;
                    int tmpIsCapital_L3 = isCapital_L3.isOn == true ? 1 : 0;
                    int tmpIsCapital_L4 = isCapital_L4.isOn == true ? 1 : 0;
                    int tmpPolicyEditorDropdown = GetDropdownValue(policyEditorDropdown, policyDropdown.value, true, true, policyFilter.text);
                    int tmpPolicyTypeEditorDropdown = GetDropdownValue(policyTypeEditorDropdown, policyTypeDropdown.value, true, true, policyTypeFilter.text);
                    int tmpIsCapital_Policy = isCapital_Policy.isOn == true ? 1 : 0;
                    int tmpIsSymbolForDate = isSymbolForDate.isOn == true ? 1 : 0;

                    CsvConnection.Instance.UpdateStage( 
                                                                stageId,
                                                                tmpFromDate, tmpToDate,
                                                                tmpParentEditorDropdown_L1, tmpParentEditorDropdown_L2, tmpParentEditorDropdown_L3, tmpParentEditorDropdown_L4,
                                                                tmpParentTypeEditorDropdown_L1, tmpParentTypeEditorDropdown_L2, tmpParentTypeEditorDropdown_L3, tmpParentTypeEditorDropdown_L4,
                                                                tmpIsCapital_L1, tmpIsCapital_L2, tmpIsCapital_L3, tmpIsCapital_L4,
                                                                tmpPolicyEditorDropdown, tmpPolicyTypeEditorDropdown,
                                                                tmpIsCapital_Policy,
                                                                regionId, settlementId,
                                                                tmpIsSymbolForDate
                                                            );
                   
                    // Update data in the dictionary of the stages
                    HistoryStage stage = MapController.Instance.GetRegionById(regionId).History.Where(x => x.StageId.Equals(stageId)).Select(x => x.Stage).FirstOrDefault();
                    stage.StartDate = tmpFromDate;
                    stage.EndDate = tmpToDate;
                    stage.PolityParentId_L1 = tmpParentEditorDropdown_L1;
                    stage.PolityParentId_L2 = tmpParentEditorDropdown_L2;
                    stage.PolityParentId_L3 = tmpParentEditorDropdown_L3;
                    stage.PolityParentId_L4 = tmpParentEditorDropdown_L4;
                    stage.PolityTypeIdParent_L1 = tmpParentTypeEditorDropdown_L1;
                    stage.PolityTypeIdParent_L2 = tmpParentTypeEditorDropdown_L2;
                    stage.PolityTypeIdParent_L3 = tmpParentTypeEditorDropdown_L3;
                    stage.PolityTypeIdParent_L4 = tmpParentTypeEditorDropdown_L4;
                    stage.Capital_L1 = tmpIsCapital_L1;
                    stage.Capital_L2 = tmpIsCapital_L2;
                    stage.Capital_L3 = tmpIsCapital_L3;
                    stage.Capital_L4 = tmpIsCapital_L4;
                    stage.PolicyId = tmpPolicyEditorDropdown;
                    stage.PolicyTypeId = tmpPolicyTypeEditorDropdown;
                    stage.PolicyCapital = tmpIsCapital_Policy;
                    stage.IsSymbolForDate = tmpIsSymbolForDate;

                    // Refreshing UI stage
                    SetHistory(stage);

                    // Refreshing data                   
                    if (EditorUICanvasController.Instance.IsDateCurrent(Int32.Parse(yFromDate.text + mFromDate.text.PadLeft(2, '0') + dFromDate.text.PadLeft(2, '0')), Int32.Parse(yToDate.text + mToDate.text.PadLeft(2, '0') + dToDate.text.PadLeft(2, '0'))))
                    {
                        EditorUICanvasController.Instance.RefleshingHistory(regionId, false, settlementId);
                    }
                    else
                    {
                        EditorUICanvasController.Instance.RefleshingHistory(regionId, false, 0);
                    }
                }
            }
        }

    } 


    // DELETE BUTTON
    public void DeleteActionButtonEvent()
    {
        // Delete
        if (stageId != 0) { CsvConnection.Instance.RemoveStage(stageId); }

        // Reload data in the dictionary
        HistoryRegionRelation history = MapController.Instance.GetRegionById(regionId).History.Where(x => x.StageId.Equals(stageId)).Select(x => x).FirstOrDefault();
        MapController.Instance.GetRegionById(regionId).History.Remove(history);

        // Refreshing data                   
        if (EditorUICanvasController.Instance.IsDateCurrent(Int32.Parse(yFromDate.text + mFromDate.text.PadLeft(2, '0') + dFromDate.text.PadLeft(2, '0')), Int32.Parse(yToDate.text + mToDate.text.PadLeft(2, '0') + dToDate.text.PadLeft(2, '0'))))
        {
            EditorUICanvasController.Instance.RefleshingHistory(regionId, true, settlementId);
        }
        else
        {
            EditorUICanvasController.Instance.RefleshingHistory(regionId, true, 0);
        }
    }
    /***                        ***/

}
