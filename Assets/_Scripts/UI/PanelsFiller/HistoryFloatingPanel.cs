using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryFloatingPanel : MonoBehaviour
{

    public Transform settlementContent, stageContent;

    public GameObject stageItem, editorStageButton;
    

    /// <summary>
    /// Remove all buttons when you close the panel
    /// </summary>
    public void CleanAllButtons(bool onlyStages)
    {
        if(!onlyStages)
        {
            for (int c = 0; c < settlementContent.childCount; c++)
            {
                Destroy(settlementContent.GetChild(c).gameObject);
            }
        }
        for (int c = 0; c < stageContent.childCount; c++)
        {
            Destroy(stageContent.GetChild(c).gameObject);
        }
    }

    /// <summary>
    /// Add the list of the settlements
    /// </summary>
    /// <param name="button">Control template</param>
    /// <param name="table">Localisation table</param>
    /// <param name="value">Localization key</param>
    public void AddSettlementButton(GameObject button, string table, string value)
    {       
        LocalizationController.Instance.AddLocalizeString(button, table, value);
        button.transform.SetParent(settlementContent);
    }

    /// <summary>
    /// Add a new stage from a button event
    /// </summary>
    /// <param name="region">Current region</param>
    /// <param name="settlement">Owner</param>
    public void OnClickButtonEvent(int regionId, KeyValuePair<int, Settlement> settlement)
    {
        AddNewStage(regionId, 0, settlement.Value, settlement.Key);
    }

    /// <summary>
    /// Add a new stage from the ToggleChangeOwner function
    /// </summary>
    /// <param name="regionId">Current region id</param>
    /// <param name="stageId">Current stage id</param>
    /// <param name="settlementId">Id settlement</param>
    /// <param name="historyStage">Parameters of the stage</param>
    public void toggleChangeOwnerEvent(int regionId, int stageId, int settlementId, HistoryStage historyStage)
    {
        // Instantiate stage's buttons
        GameObject button = Instantiate(editorStageButton);

        // Add  text to the button
        button.GetComponentInChildren<TextMeshProUGUI>().text = Utilities.FormatedDate(historyStage.StartDate) + " - " + Utilities.FormatedDate(historyStage.EndDate);

        // Add the button into the parent
        button.transform.SetParent(stageContent);

        //*// OnClick() event //*//
        Settlement settlement = MapController.Instance.GetSettlementById(settlementId);
        button.GetComponent<Button>().onClick.AddListener(delegate { AddNewStage(regionId, stageId, settlement, settlementId, historyStage); });

    }

    /// <summary>
    /// Add a new stage
    /// </summary>
    /// <param name="regionId">Current region id</param>
    /// <param name="idStage">Current stage id</param>
    /// <param name="settlement">Settlement</param>
    ///  <param name="settlementId">Current settlement id</param>
    /// <param name="historyStage">Parameters of the stage</param>
    private void AddNewStage(int regionId, int idStage, Settlement settlement, int settlementId, HistoryStage historyStage = null)
    {        
        StageFloatingPanel stageScript = stageItem.GetComponent<StageFloatingPanel>();
        stageScript.SetStageId(idStage);
        stageScript.SetRegionId(regionId);
        stageScript.SetSettlementId(settlementId);
        stageScript.SetSettlementName("LOC_TABLE_HIST_SETTLEMENTS", settlement.Name);
        stageScript.SetHistory(historyStage);
    }

}
