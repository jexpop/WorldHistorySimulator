using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class RegionFloatingPanel : MonoBehaviour
{

    public TextMeshProUGUI regionPanelValue;
    public TextMeshProUGUI regionSettlementValue;
    public GameObject regionHistoryButton;
    public Button terrainImageButton;
    public TextMeshProUGUI RGB;
    public GameObject terrainPanel;
    public Transform terrainScrollView;
    public GameObject terrainButtonPrefab;

    private Button tmpRegionHistoryButton;
    private RectTransform rectTransformComponent;
    private Image terrainImage;


    void Awake()
    {
        rectTransformComponent = GetComponent<RectTransform>();
        tmpRegionHistoryButton = regionHistoryButton.GetComponent<Button>();
        terrainImage = terrainImageButton.GetComponentInChildren<Image>();
    }

    public RectTransform GetRectTransform()
    {
        return rectTransformComponent;
    }

    #region Set Values
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
    #endregion

    #region Terrain Panel
    public void EnableTerrainPanel()
    {
        // TODO Check for all screens
        terrainPanel.transform.position = new Vector2(1000, 500);

        terrainPanel.SetActive(true);

        // List of tiles
        int regionId = int.Parse(regionPanelValue.text);
        Region region = EditorUICanvasController.Instance.GetRegionById(regionId);
        List<TerrainData> terrains = EditorUICanvasController.Instance.GetTerrainsByType(region.Type);
        
        if (terrains.Count > 0)
        {

            // Clear buttons
            foreach (Transform childButtons in terrainScrollView.transform)
            {
                Destroy(childButtons.gameObject);
            }

            foreach (TerrainData terrain in terrains)
            {

                // New terrain info
                GameObject terrainPanel = Instantiate(terrainButtonPrefab);

                // Button
                Button terrainButton = terrainPanel.GetComponentInChildren<Button>();
                Sprite terrainTile = EditorUICanvasController.Instance.terrainTiles.FirstOrDefault(o => o.name.Equals(terrain.TerrainName));

                terrainButton.GetComponentInChildren<Image>().sprite = terrainTile;
                terrainButton.onClick.AddListener(delegate { ChangeTerrain(regionId, region.Type, terrain, terrainTile); });

                // Text
                TextMeshProUGUI terrainText = terrainPanel.GetComponentInChildren<TextMeshProUGUI>();
                EditorUICanvasController.Instance.AddLocalizeString(terrainText, "LOC_TABLE_EDITOR_FLOATING", terrain.TerrainName);

                // Parent
                terrainPanel.transform.SetParent(terrainScrollView.transform);
            }
        }

    }
    private void ChangeTerrain(int regionId, string terrainType, TerrainData terrain, Sprite terrainTile)
    {
        // Update the new terrain in Csv file
        EditorUICanvasController.Instance.UpdateTerrain(regionId, terrain.TerrainId);

        // Update the new terrain in the map (dictionary and color)
        EditorUICanvasController.Instance.UpdateTerrainRegion(regionId, terrain.TerrainName, terrainType);

        // Update the new terrain in the panel
        SetTerrainImage(terrainTile);

        // Close list of the terrains
        terrainPanel.SetActive(false);
    }
    #endregion

}
