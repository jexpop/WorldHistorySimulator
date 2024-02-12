using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UI;


public enum GameScene{
    MainMenu, // Main menu of the game
    MapEditor, // History editor
    MapSelect, // Select your polity
    MapPlay, // In the game, world map
    CityMap, // In the game, city view
    BattleMap // In the game, battle view
}

public class GameManager : Singleton<GameManager>
{

    public GameScene currentGameScene;

    public string STREAMING_FOLDER;

    void Start()
    {
        STREAMING_FOLDER = Application.streamingAssetsPath;
        currentGameScene = GameScene.MapEditor;
        LoadScene(currentGameScene);
    }

    public void LoadScene(GameScene gameScene)
    {
        // Dynamic load of the scene with the GameScene parameter
        SceneManager.LoadScene(gameScene.ToString() + "Scene");
    }

    // UI Controller
    public UIStatus UI_GetUIStatus() { return EditorUICanvasController.Instance.uiStatus; }
    public void UI_SetUIStatus(UIStatus status) { EditorUICanvasController.Instance.uiStatus = status; }
    public void UI_PostItPolityVisibility(Vector3 mousePos, bool showPostIt, Region region = null) { EditorUICanvasController.Instance.PostItPolityVisibility(mousePos, showPostIt, region); }
    public Vector2 UI_CalculateNewPositionPanel(Vector3 mousePos) { return EditorUICanvasController.Instance.CalculateNewPositionPanel(mousePos); }
    public void UI_SetNameAndImageRegionPanel(string name, string terrain) { EditorUICanvasController.Instance.SetNameAndImageRegionPanel(name, terrain); }
    public void UI_ShowRgbOwner(string rgb) { EditorUICanvasController.Instance.ShowRgbOwner(rgb); }
    public void UI_SetSettlementRegionPanel(bool button, string setllement = null) { EditorUICanvasController.Instance.SetSettlementRegionPanel(button, setllement); }
    public void UI_ActivateRegionPanel(float x, float y) { EditorUICanvasController.Instance.ActivateRegionPanel(x, y); }
    public void UI_DeactivateRegionPanel() { EditorUICanvasController.Instance.DeactivateRegionPanel(); }
    public int UI_GetCurrentTimeline(bool button) { return EditorUICanvasController.Instance.GetCurrentTimeline(button); }
    public int UI_GetLayerValue() { return EditorUICanvasController.Instance.layersDropdown.value; }
    public bool UI_IsDateCurrent(int start, int end) { return EditorUICanvasController.Instance.IsDateCurrent(start, end); }
    public void UI_SetCoordinates(string x, string y) { EditorUICanvasController.Instance.SetCoordinates(x, y); }

    // MAP Controller
    public Dictionary<int, PolityType> MAP_GetPolitiesType() { return MapController.Instance.GetPolitiesType(); }
    public Dictionary<int, Polity> MAP_GetPolities(int policy = 0) { return MapController.Instance.GetPolities(policy); }
    public Dictionary<int, Settlement> MAP_GetSettlements() { return MapController.Instance.GetSettlements(); }
    public string MAP_GetPolitiesTypeLocaleKeyById(int id) { return MapController.Instance.GetPolitiesTypeLocaleKeyById(id); }
    public string MAP_GetPolitiesLocaleKeyById(int id) { return MapController.Instance.GetPolitiesLocaleKeyById(id); }
    public string MAP_GetSettlementsLocaleKeyById(int id) { return MapController.Instance.GetSettlementsLocaleKeyById(id); }
    public Region MAP_GetRegionById(int id) { return MapController.Instance.GetRegionById(id); }
    public Polity MAP_GetPolityById(int id) { return MapController.Instance.GetPolityById(id); }
    public PolityType MAP_GetPolityTypeById(int id) { return MapController.Instance.GetPolityTypeById(id); }
    public void MAP_ColorizeRegionsById(int regionId, Polity owner) { MapController.Instance.ColorizeRegionsById(regionId, owner); }
    public void MAP_LoadPolitiesTypeDictionaryFromDB() { MapController.Instance.LoadPolitiesTypeDictionaryFromDB(); }
    public void MAP_LoadPolitiesDictionaryFromDB(int currentTime, int polityLayer) { MapController.Instance.LoadPolitiesDictionaryFromDB(currentTime, polityLayer); }
    public void MAP_LoadSettlementsDictionaryFromDB() { MapController.Instance.LoadSettlementsDictionaryFromDB(); }
    public void MAP_LoadHistoryRegionDictionaryFromDB(int regionId) { MapController.Instance.LoadHistoryRegionDictionaryFromDB(regionId); }
    public List<int> MAP_GetPolityTypesByPolity(int polityId) { return MapController.Instance.GetPolityTypesByPolity(polityId); }
    public List<int> MAP_GetPolityTypesByPolicy(int policyId) { return MapController.Instance.GetPolityTypesByPolicy(policyId); }
    public bool MAP_PolityTypeIsRelated(int polityTypeId) { return MapController.Instance.PolityTypeIsRelated(polityTypeId); }
    public bool MAP_PolityIsRelated(int polityId) { return MapController.Instance.PolityIsRelated(polityId); }

    // LOCALIZATION Controller
    public void LOC_AddLocalizeString(TextMeshProUGUI text, string table, string key) { LocalizationController.Instance.AddLocalizeString(text, table, key); }
    public void LOC_AddLocalizeString(GameObject button, string table, string key) { LocalizationController.Instance.AddLocalizeString(button, table, key); }
    public void LOC_AddLocalizeString(TMP_InputField input, string table, string key) { LocalizationController.Instance.AddLocalizeString(input, table, key); }
    public void LOC_AddLocalizeString(SimpleMessage message) { LocalizationController.Instance.AddLocalizeString(message); }
    public void LOC_AddLocalizeString(IdentificatorMessage message) { LocalizationController.Instance.AddLocalizeString(message); }
    public void LOC_AddLocalizeString(LocalizeDropdown localizeDropdown, string table, string value) { LocalizationController.Instance.AddLocalizeString(localizeDropdown, table, value); }
    public void LOC_InsertNewEntry(string table, string key, string value, int locale = -1) { LocalizationController.Instance.InsertNewEntry(table, key, value, locale); }
    public void LOC_UpdateEntry(string table, string key, string newValue) { LocalizationController.Instance.UpdateEntry(table, key, newValue); }
    public void LOC_DeleteEntry(string table, string key) { LocalizationController.Instance.DeleteEntry(table, key); }
    public bool LOC_KeyExist(string table, string key) { return LocalizationController.Instance.KeyExist(table, key); }
    public bool LOC_ValueExist(string table, string key, string value) { return LocalizationController.Instance.ValueExist(table, key, value); }

}
