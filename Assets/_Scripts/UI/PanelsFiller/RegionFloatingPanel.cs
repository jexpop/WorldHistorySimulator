using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class RegionFloatingPanel : MonoBehaviour
{

    public TextMeshProUGUI regionPanelValue;
    public TextMeshProUGUI regionSettlementValue;
    public GameObject regionHistoryButton;
    public Image terrainImage;
    public TextMeshProUGUI RGB;

    private Button tmpRegionHistoryButton;


    void Awake()
    {
        tmpRegionHistoryButton = regionHistoryButton.GetComponent<Button>();
    }

    public void SetRegionValue(string value)
    {
        regionPanelValue.text = value;
    }
    public void SetSettlementValue(string table, string value)
    {
        LocalizationController.Instance.AddLocalizeString(regionSettlementValue, table, value);
    }
    public void SetHistoryButtonText(string table, string value)
    {
        LocalizationController.Instance.AddLocalizeString(regionHistoryButton, table, value);
    }

    public void SetButtonClick()
    {
        // Add new listener but removing old listeners
        tmpRegionHistoryButton.onClick.RemoveAllListeners();
        tmpRegionHistoryButton.onClick.AddListener(delegate { EditorUICanvasController.Instance.ToggleChangeOwner(Int32.Parse(regionPanelValue.text)); });
    }

    public void SetTerrainImage(Sprite sprite)
    {
        terrainImage.sprite = sprite;
    }

    public void ShowOwnerColor(string rgb)
    {
        RGB.text = rgb;
    }

}
