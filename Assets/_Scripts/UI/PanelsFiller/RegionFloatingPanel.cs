using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class RegionFloatingPanel : MonoBehaviour
{

    public TextMeshProUGUI regionPanelValue;
    public TextMeshProUGUI regionSettlementValue;
    public GameObject regionHistoryButton;


    public void DeactivatePanel()
    {
        EditorUICanvasManager.Instance.DeactivateRegionPanel();
    }

    public void SetRegionValue(string value)
    {
        regionPanelValue.text = value;
    }
    public void SetSettlementValue(string table, string value)
    {
        LocalizationManager.Instance.AddLocalizeString(regionSettlementValue, table, value);
    }
    public void SetHistoryButtonText(string table, string value)
    {
        LocalizationManager.Instance.AddLocalizeString(regionHistoryButton, table, value);
    }

    public void SetButtonClick()
    {
        regionHistoryButton.GetComponent<Button>().onClick.AddListener(delegate { EditorUICanvasManager.Instance.ToggleChangeOwner(Int32.Parse(regionPanelValue.text)); });
    }

}
