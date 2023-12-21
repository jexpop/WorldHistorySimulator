using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageFloatingPanel : MonoBehaviour
{

    public TextMeshProUGUI settlementNameLabel;

    [Header("Dates of the stage")]
    public TMP_InputField dFromDate, mFromDate, yFromDate;
    public TMP_InputField dToDate, mToDate, yToDate;

    [Header("Components of the owner's parent Level 1")]
    public TMP_Dropdown parentTypeDropdown_L1;
    public TMP_Dropdown parentDropdown_L1;
    public EditorDropdown parentTypeEditorDropdown_L1;
    public EditorDropdown parentEditorDropdown_L1;
    public Toggle isCapital_L1;

    [Header("Components of the owner's parent Level 2")]
    public TMP_Dropdown parentTypeDropdown_L2;
    public TMP_Dropdown parentDropdown_L2;
    public EditorDropdown parentTypeEditorDropdown_L2;
    public EditorDropdown parentEditorDropdown_L2;
    public Toggle isCapital_L2;

    [Header("Components of the owner's parent Level 3")]
    public TMP_Dropdown parentTypeDropdown_L3;
    public TMP_Dropdown parentDropdown_L3;
    public EditorDropdown parentTypeEditorDropdown_L3;
    public EditorDropdown parentEditorDropdown_L3;
    public Toggle isCapital_L3;

    [Header("Components of the owner's parent Level 4")]
    public TMP_Dropdown parentTypeDropdown_L4;
    public TMP_Dropdown parentDropdown_L4;
    public EditorDropdown parentTypeEditorDropdown_L4;
    public EditorDropdown parentEditorDropdown_L4;
    public Toggle isCapital_L4;

    [Header("Components of the policy")]
    public TMP_Dropdown policyTypeDropdown;
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
        EditorUICanvasManager.Instance.ChangeActive(UIgameobject);
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
        LocalizationManager.Instance.AddLocalizeString(settlementNameLabel, table, key);
    }


    private int GetDropdownValue(EditorDropdown dropdown, int id, bool isForward, bool option)
    {
        if (isForward)
        {
            return dropdown.GetOptionsIds(option).Forward[id];
        }
        else
        {
            return dropdown.GetOptionsIds(option).Reverse[id];
        }
        
    }


    public void SetHistory(HistoryStage historyStage = null)
    {
        string fromDate, toDate;

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

            // Parent 
            parentTypeDropdown_L1.value = GetDropdownValue(parentTypeEditorDropdown_L1, historyStage.PolityTypeIdParent_L1, false, false);

            parentTypeDropdown_L2.value = GetDropdownValue(parentTypeEditorDropdown_L2, historyStage.PolityTypeIdParent_L2, false, true);
            parentTypeDropdown_L3.value = GetDropdownValue(parentTypeEditorDropdown_L3, historyStage.PolityTypeIdParent_L3, false, true);
            parentTypeDropdown_L4.value = GetDropdownValue(parentTypeEditorDropdown_L4, historyStage.PolityTypeIdParent_L4, false, true);

            parentDropdown_L1.value = GetDropdownValue(parentEditorDropdown_L1, historyStage.PolityParentId_L1, false, false);

            parentDropdown_L2.value = GetDropdownValue(parentEditorDropdown_L2, historyStage.PolityParentId_L2, false, true);
            parentDropdown_L3.value = GetDropdownValue(parentEditorDropdown_L3, historyStage.PolityParentId_L3, false, true);
            parentDropdown_L4.value = GetDropdownValue(parentEditorDropdown_L4, historyStage.PolityParentId_L4, false, true);

            isCapital_L1.isOn=historyStage.Capital_L1==1?true:false;
            isCapital_L2.isOn = historyStage.Capital_L2 == 1 ? true : false;
            isCapital_L3.isOn = historyStage.Capital_L3 == 1 ? true : false;
            isCapital_L4.isOn = historyStage.Capital_L4 == 1 ? true : false;

            // Policy
            policyTypeDropdown.value = GetDropdownValue(policyTypeEditorDropdown, historyStage.PolicyTypeId, false, true);
            policyDropdown.value = GetDropdownValue(policyEditorDropdown, historyStage.PolicyId, false, true);
            isCapital_Policy.isOn = historyStage.PolicyCapital == 1 ? true : false;

        }
        else
        {
            // Current date
            string currentYear = EditorUICanvasManager.Instance.GetCurrentTimeline(false).ToString();

            int yearLength = currentYear.Substring(0,1)=="-" ? 5 : 4;

            // Dates format
            dFromDate.text = "01";
            mFromDate.text = "01";
            yFromDate.text = currentYear.Substring(0, yearLength).TrimStart(new Char[] { '0' });
            dToDate.text = "31";
            mToDate.text = "12";
            yToDate.text = currentYear.Substring(0, yearLength).TrimStart(new Char[] { '0' });

            // Parent
            parentTypeEditorDropdown_L1.LoadOptions(false);
            parentTypeEditorDropdown_L2.LoadOptions(true);
            parentTypeEditorDropdown_L3.LoadOptions(true);
            parentTypeEditorDropdown_L4.LoadOptions(true);
            parentEditorDropdown_L1.LoadOptions(false);
            parentEditorDropdown_L2.LoadOptions(true);
            parentEditorDropdown_L3.LoadOptions(true);
            parentEditorDropdown_L4.LoadOptions(true);
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
            policyTypeEditorDropdown.LoadOptions(true);
            policyEditorDropdown.LoadOptions(true);
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
    private bool DatesExist(int idStage = 0)
    {// For negative dates it doesn't check ok - TODO Check it
        int newStartDate = Int32.Parse(yFromDate.text + mFromDate.text.PadLeft(2, '0') + dFromDate.text.PadLeft(2, '0'));
        int newEndDate = Int32.Parse(yToDate.text + mToDate.text.PadLeft(2, '0') + dToDate.text.PadLeft(2, '0'));

        bool exist = false;

        Region region = MapManager.Instance.GetRegionById(regionId);
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
    // SAVE BUTTON
    public void SaveActionButtonEvent()
    {
        SaveActionEvent(stageOkMessage);
    }
    private void SaveActionEvent(SimpleMessage okNameMessage)
    {
        // Remove old fields messages
        LocalizationManager.Instance.AddLocalizeString(okNameMessage);

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
            LocalizationManager.Instance.AddLocalizeString(emptyDatesMessage);
        }
        else if (!DatesRangeOk())
        {
            LocalizationManager.Instance.AddLocalizeString(koDatesMessage);
        }
        else
        {            
            // If all ok  then insert/modify data            
            if (stageId == 0)
            {
                if (DatesExist())
                {// Check duplicate dates
                    LocalizationManager.Instance.AddLocalizeString(duplicatedDatesMessage);
                }
                else
                {
                    MapSqlConnection.Instance.AddStage( 
                                                                                    regionId,
                                                                                    settlementId,
                                                                                    Int32.Parse(yFromDate.text + mFromDate.text.PadLeft(2, '0') + dFromDate.text.PadLeft(2, '0')),
                                                                                    Int32.Parse(yToDate.text + mToDate.text.PadLeft(2, '0') + dToDate.text.PadLeft(2, '0')),
                                                                                    GetDropdownValue(parentEditorDropdown_L1, parentDropdown_L1.value, true, false),
                                                                                    GetDropdownValue(parentEditorDropdown_L2, parentDropdown_L2.value, true, true),
                                                                                    GetDropdownValue(parentEditorDropdown_L3, parentDropdown_L3.value, true, true),
                                                                                    GetDropdownValue(parentEditorDropdown_L4, parentDropdown_L4.value, true, true),
                                                                                    GetDropdownValue(parentTypeEditorDropdown_L1, parentTypeDropdown_L1.value, true, false),
                                                                                    GetDropdownValue(parentTypeEditorDropdown_L2, parentTypeDropdown_L2.value, true, true),
                                                                                    GetDropdownValue(parentTypeEditorDropdown_L3, parentTypeDropdown_L3.value, true, true),
                                                                                    GetDropdownValue(parentTypeEditorDropdown_L4, parentTypeDropdown_L4.value, true, true),
                                                                                    isCapital_L1.isOn == true ? 1 : 0,
                                                                                    isCapital_L2.isOn == true ? 1 : 0,
                                                                                    isCapital_L3.isOn == true ? 1 : 0,
                                                                                    isCapital_L4.isOn == true ? 1 : 0,
                                                                                    GetDropdownValue(policyEditorDropdown, policyDropdown.value, true, true),
                                                                                    GetDropdownValue(policyTypeEditorDropdown, policyTypeDropdown.value, true, true),
                                                                                    isCapital_Policy.isOn == true ? 1 : 0
                                                                                );

                    // Add data in the dictionary of the stages
                    HistoryStage stage = new HistoryStage(
                                                                                    Int32.Parse(yFromDate.text + mFromDate.text.PadLeft(2, '0') + dFromDate.text.PadLeft(2, '0')),
                                                                                    Int32.Parse(yToDate.text + mToDate.text.PadLeft(2, '0') + dToDate.text.PadLeft(2, '0')),
                                                                                    GetDropdownValue(parentEditorDropdown_L1, parentDropdown_L1.value, true, false),
                                                                                    GetDropdownValue(parentEditorDropdown_L2, parentDropdown_L2.value, true, true),
                                                                                    GetDropdownValue(parentEditorDropdown_L3, parentDropdown_L3.value, true, true),
                                                                                    GetDropdownValue(parentEditorDropdown_L4, parentDropdown_L4.value, true, true),
                                                                                    GetDropdownValue(parentTypeEditorDropdown_L1, parentTypeDropdown_L1.value, true, false),
                                                                                    GetDropdownValue(parentTypeEditorDropdown_L2, parentTypeDropdown_L2.value, true, true),
                                                                                    GetDropdownValue(parentTypeEditorDropdown_L3, parentTypeDropdown_L3.value, true, true),
                                                                                    GetDropdownValue(parentTypeEditorDropdown_L4, parentTypeDropdown_L4.value, true, true),
                                                                                    isCapital_L1.isOn == true ? 1 : 0,
                                                                                    isCapital_L2.isOn == true ? 1 : 0,
                                                                                    isCapital_L3.isOn == true ? 1 : 0,
                                                                                    isCapital_L4.isOn == true ? 1 : 0,
                                                                                    GetDropdownValue(policyEditorDropdown, policyDropdown.value, true, true),
                                                                                    GetDropdownValue(policyTypeEditorDropdown, policyTypeDropdown.value, true, true),
                                                                                    isCapital_Policy.isOn == true ? 1 : 0
                        );
                    HistoryRegionRelation history = new HistoryRegionRelation(
                                                                                    MapSqlConnection.Instance.GetLastIdAdded(EditorDataType.StagePanel),
                                                                                    settlementId,
                                                                                    stage
                        );
                    MapManager.Instance.GetRegionById(regionId).History.Add(history);

                    // Refreshing data                   
                    if (EditorUICanvasManager.Instance.IsDateCurrent(Int32.Parse(yFromDate.text + mFromDate.text.PadLeft(2, '0') + dFromDate.text.PadLeft(2, '0')), Int32.Parse(yToDate.text + mToDate.text.PadLeft(2, '0') + dToDate.text.PadLeft(2, '0'))))
                    {
                        EditorUICanvasManager.Instance.RefleshingHistory(regionId, false, settlementId);
                    }
                    else
                    {
                        EditorUICanvasManager.Instance.RefleshingHistory(regionId, false);
                    }                   
                }
            }
            else
            { // Update new data
                if (DatesExist(stageId))
                {// Check duplicate dates
                    LocalizationManager.Instance.AddLocalizeString(duplicatedDatesMessage);
                }
                else
                {
                    MapSqlConnection.Instance.UpdateStage( 
                                                                stageId,
                                                                Int32.Parse(yFromDate.text + mFromDate.text.PadLeft(2, '0') + dFromDate.text.PadLeft(2, '0')),
                                                                Int32.Parse(yToDate.text + mToDate.text.PadLeft(2, '0') + dToDate.text.PadLeft(2, '0')),
                                                                GetDropdownValue(parentEditorDropdown_L1, parentDropdown_L1.value, true, false),
                                                                GetDropdownValue(parentEditorDropdown_L2, parentDropdown_L2.value, true, true),
                                                                GetDropdownValue(parentEditorDropdown_L3, parentDropdown_L3.value, true, true),
                                                                GetDropdownValue(parentEditorDropdown_L4, parentDropdown_L4.value, true, true),
                                                                GetDropdownValue(parentTypeEditorDropdown_L1, parentTypeDropdown_L1.value, true, false),
                                                                GetDropdownValue(parentTypeEditorDropdown_L2, parentTypeDropdown_L2.value, true, true),
                                                                GetDropdownValue(parentTypeEditorDropdown_L3, parentTypeDropdown_L3.value, true, true),
                                                                GetDropdownValue(parentTypeEditorDropdown_L4, parentTypeDropdown_L4.value, true, true),
                                                                isCapital_L1.isOn == true ? 1 : 0,
                                                                isCapital_L2.isOn == true ? 1 : 0,
                                                                isCapital_L3.isOn == true ? 1 : 0,
                                                                isCapital_L4.isOn == true ? 1 : 0,
                                                                GetDropdownValue(policyEditorDropdown, policyDropdown.value, true, true),
                                                                GetDropdownValue(policyTypeEditorDropdown, policyTypeDropdown.value, true, true),
                                                                isCapital_Policy.isOn == true ? 1 : 0
                                                            );

                    // Update data in the dictionary of the stages
                    HistoryStage stage = MapManager.Instance.GetRegionById(regionId).History.Where(x => x.StageId.Equals(stageId)).Select(x => x.Stage).FirstOrDefault();
                    stage.StartDate = Int32.Parse(yFromDate.text + mFromDate.text.PadLeft(2, '0') + dFromDate.text.PadLeft(2, '0'));
                    stage.EndDate = Int32.Parse(yToDate.text + mToDate.text.PadLeft(2, '0') + dToDate.text.PadLeft(2, '0'));
                    stage.PolityParentId_L1 = GetDropdownValue(parentEditorDropdown_L1, parentDropdown_L1.value, true, false);
                    stage.PolityParentId_L2 = GetDropdownValue(parentEditorDropdown_L2, parentDropdown_L2.value, true, true);
                    stage.PolityParentId_L3 = GetDropdownValue(parentEditorDropdown_L3, parentDropdown_L3.value, true, true);
                    stage.PolityParentId_L4 = GetDropdownValue(parentEditorDropdown_L4, parentDropdown_L4.value, true, true);
                    stage.PolityTypeIdParent_L1 = GetDropdownValue(parentTypeEditorDropdown_L1, parentTypeDropdown_L1.value, true, false);
                    stage.PolityTypeIdParent_L2 = GetDropdownValue(parentTypeEditorDropdown_L2, parentTypeDropdown_L2.value, true, true);
                    stage.PolityTypeIdParent_L3 = GetDropdownValue(parentTypeEditorDropdown_L3, parentTypeDropdown_L3.value, true, true);
                    stage.PolityTypeIdParent_L4 = GetDropdownValue(parentTypeEditorDropdown_L4, parentTypeDropdown_L4.value, true, true);
                    stage.Capital_L1 = isCapital_L1.isOn == true ? 1 : 0;
                    stage.Capital_L2 = isCapital_L2.isOn == true ? 1 : 0;
                    stage.Capital_L3 = isCapital_L3.isOn == true ? 1 : 0;
                    stage.Capital_L4 = isCapital_L4.isOn == true ? 1 : 0;
                    stage.PolicyId = GetDropdownValue(policyEditorDropdown, policyDropdown.value, true, true);
                    stage.PolicyTypeId = GetDropdownValue(policyTypeEditorDropdown, policyTypeDropdown.value, true, true);
                    stage.PolicyCapital = isCapital_Policy.isOn == true ? 1 : 0;

                    // Refreshing data                   
                    if (EditorUICanvasManager.Instance.IsDateCurrent(Int32.Parse(yFromDate.text + mFromDate.text.PadLeft(2, '0') + dFromDate.text.PadLeft(2, '0')), Int32.Parse(yToDate.text + mToDate.text.PadLeft(2, '0') + dToDate.text.PadLeft(2, '0'))))
                    {
                        EditorUICanvasManager.Instance.RefleshingHistory(regionId, false, settlementId);
                    }
                    else
                    {
                        EditorUICanvasManager.Instance.RefleshingHistory(regionId, false);
                    }
                }
            }
        }

    } 


    // DELETE BUTTON
    public void DeleteActionButtonEvent()
    {
        // Delete
        if (stageId != 0) { MapSqlConnection.Instance.RemoveStage(stageId); }

        // Reload data in the dictionary
        HistoryRegionRelation history = MapManager.Instance.GetRegionById(regionId).History.Where(x => x.StageId.Equals(stageId)).Select(x => x).FirstOrDefault();
        MapManager.Instance.GetRegionById(regionId).History.Remove(history);

        // Refreshing data                   
        if (EditorUICanvasManager.Instance.IsDateCurrent(Int32.Parse(yFromDate.text + mFromDate.text.PadLeft(2, '0') + dFromDate.text.PadLeft(2, '0')), Int32.Parse(yToDate.text + mToDate.text.PadLeft(2, '0') + dToDate.text.PadLeft(2, '0'))))
        {
            EditorUICanvasManager.Instance.RefleshingHistory(regionId, true, settlementId);
        }
        else
        {
            EditorUICanvasManager.Instance.RefleshingHistory(regionId, true);
        }
    }
    /***                        ***/

}
