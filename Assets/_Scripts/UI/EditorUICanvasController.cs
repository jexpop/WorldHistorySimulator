using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;


public class EditorUICanvasController : Singleton<EditorUICanvasController>
{

    public UIStatus uiStatus;

    public GameObject editorMenuButton;
    public GameObject editorMenuSubbutton;
    public GameObject editorHistoryButton;
    public GameObject postItElement;

    [Header("Custom map")]
    public Toggle layerCheckCollective;
    public TMP_Dropdown layersDropdown;
    public TextMeshProUGUI coordinateX;
    public TextMeshProUGUI coordinateY;


    [Header("Terrain Tiles")]
    public List<Sprite> terrainTiles;

    [Header("Floating Panels")]
    public GameObject editorRegionPanel;
    public GameObject editorHistoryPanel;

    [Header("Timeline Information")]
    public TMP_Dropdown eraTimeline;
    public TMP_InputField dayTimeline;
    public TMP_InputField monthTimeline;
    public TMP_InputField yearTimeline;

    [Header("PolityType Data Messages")]
    public SimpleMessage polityTypeNewMessage;
    public IdentificatorMessage polityTypeMessage;
    public SimpleMessage polityTypeEmptyNameMessage;
    public SimpleMessage polityTypeOkNameMessage;
    public SimpleMessage polityTypeDuplicatedNameMessage;
    public SimpleMessage polityTypeNoRemoveMessage;
    public SimpleMessage polityTypeRelatedDataMessage;

    [Header("Polity Data Messages")]
    public SimpleMessage polityNewMessage;
    public IdentificatorMessage polityMessage;
    public SimpleMessage polityEmptyNameMessage;
    public SimpleMessage polityOkNameMessage;
    public SimpleMessage polityDuplicatedNameMessage;
    public SimpleMessage polityNoRemoveMessage;
    public SimpleMessage polityRelatedDataMessage;

    [Header("Settlement Data Messages")]
    public SimpleMessage settlementNewMessage;
    public IdentificatorMessage settlementMessage;
    public SimpleMessage settlementEmptyNameMessage;
    public SimpleMessage settlementOkNameMessage;
    public SimpleMessage settlementOkRegionMessage;
    public SimpleMessage settlementDuplicatedNameMessage;
    public SimpleMessage settlementNoRemoveMessage;
    public SimpleMessage settlementRelatedDataMessage;

    [Header("PolityType Data Values")]
    public Transform polityTypeContent;
    public TextMeshProUGUI polityTypeIdLabel;
    public TMP_InputField polityTypeNameInput;
    public GameObject polityTypeStatus;

    [Header("Polity Data Values")]
    public Transform polityContent;
    public TextMeshProUGUI polityIdLabel;
    public TMP_InputField polityNameInput;
    public Toggle polityPolicyCheck;
    public GameObject polityStatus;

    [Header("Settlement Data Values")]
    public Transform settlementContent;
    public TextMeshProUGUI settlementIdLabel;
    public TMP_InputField settlementNameInput;
    public TMP_InputField settlementRegionInput;
    public TMP_InputField settlementPixelXInput;
    public TMP_InputField settlementPixelYInput;
    public GameObject settlementStatus;

    [Header("Polity symbols Data Values")]
    public GameObject symbolItemPrefab;
    public GameObject contentPolitySymbols;

    // Panel of the regions - Floating panels
    private GameObject tmpPostItNote;
    private HistoryFloatingPanel tmpHistoryFloatingPanel;
    private RegionFloatingPanel tmpRegionFloatingPanel;
    private PostItNote postItNote;
    private RectTransform postItNoteRectTransform;
    private float regionPanelxPositionLast, regionPanelyPositionLast;


    private void Start()
    {
        // Initialise the post it note with polity information
        tmpPostItNote = Instantiate(postItElement);
        tmpPostItNote.transform.SetParent(this.transform.parent);
        postItNote = tmpPostItNote.GetComponent<PostItNote>();
        postItNoteRectTransform = tmpPostItNote.GetComponent<RectTransform>();
        tmpPostItNote.SetActive(false);

        // Floating panels components
        tmpRegionFloatingPanel = editorRegionPanel.GetComponent<RegionFloatingPanel>();
        tmpHistoryFloatingPanel = editorHistoryPanel.GetComponent<HistoryFloatingPanel>();

    }

    /// <summary>
    /// Set coordinates where is the mouse pointer
    /// </summary>
    /// <param name="x">x</param>
    /// <param name="y">y</param>
    public void SetCoordinates(string x, string y)
    {
        coordinateX.text = x; coordinateY.text = y;
    }

    /// <summary>
    /// Chnage status active/deactive of a UI gameobject
    /// </summary>
    /// <param name="gameobject">UI gameobject</param>
    public void ChangeActive(GameObject UIgameobject)
    {
        UIgameobject.SetActive(!UIgameobject.activeInHierarchy);
    }


    /*** Filling scrolls ***/
    /// <summary>
    /// Get information for a scrollview of the buttons
    /// </summary>
    /// <param name="scrollViewButton">Scroll view to fill</param>
    /// <param name="elementKey">Element to run click events</param>
    public void FillScrollButton(Transform scrollViewButton)
    {
        FillScrollButtonScript(scrollViewButton);
    }
    private void FillScrollButtonScript(Transform scrollViewButton, string elementKey = null)
    {

        string name = scrollViewButton.name;

        // Clear buttons
        foreach (Transform childButtons in scrollViewButton.transform)
        {
            Destroy(childButtons.gameObject);
        }

        switch (name)
        {

            // Polity Type Edit Menu
            case ParamUI.EDITMENU_SCROLLBUTTONS_POLITY_TYPE:
                FillScrollPolityType(scrollViewButton, elementKey);
                // End case Polity Type Edit Menu
                break;

            // Polity Edit Menu
            case ParamUI.EDITMENU_SCROLLBUTTONS_POLITY:
                FillScrollPolity(scrollViewButton, EditorDataType.Polity, elementKey);
                // End case Polity Edit Menu
                break;

            // Settlement Edit Menu
            case ParamUI.EDITMENU_SCROLLBUTTONS_SETTLEMENT:
                FillScrollSettlement(scrollViewButton, elementKey);
                // End case Settlement Edit Menu
                break;

            // Polity Edit Menu
            case ParamUI.EDITMENU_SCROLLBUTTONS_POLITY_SYMBOLS:
                FillScrollPolity(scrollViewButton, EditorDataType.PolitySymbols, elementKey);
                // End case Polity Edit Menu
                break;
        }

    }
    private void FillScrollPolityType(Transform scrollViewButton, string elementKey = null)
    {
        // Building list of polities type
        List<GameObject> polityTypeButtons = new List<GameObject>();
        foreach (KeyValuePair<int, PolityType> pt in GameManager.Instance.MAP_GetPolitiesType())
        {
            PolityType currentPolityType = pt.Value;

            // Instantiate buttons
            GameObject button = Instantiate(editorMenuButton);
            string polityTypeName = currentPolityType.Name;

            GameManager.Instance.LOC_AddLocalizeString(button, "LOC_TABLE_HIST_POLITIES_TYPE", polityTypeName);


            //*// OnClick() events //*//
            // Data filler
            button.GetComponent<Button>().onClick.AddListener(delegate { ButtonEventToFillInfo(EditorDataType.PolityType, pt.Key); });
            // Message status
            IdentificatorMessage message = Instantiate(polityTypeMessage);
            message.objectName = currentPolityType.Name;
            button.GetComponent<Button>().onClick.AddListener(delegate { UpdateModStatusMessage(message); });
            // Run click events for a element
            if (currentPolityType.Name == elementKey) { button.GetComponent<Button>().onClick.Invoke(); }
            //*//

            polityTypeButtons.Add(button);
        };

        // Reordering buttons
        List<GameObject> SortedPolityTypeButtons = polityTypeButtons.OrderBy(o => o.GetComponentInChildren<TextMeshProUGUI>().text).ToList();
        foreach (GameObject polityTypeButton in SortedPolityTypeButtons)
        {
            polityTypeButton.transform.SetParent(scrollViewButton.transform);
        }
    }
    private void FillScrollPolity(Transform scrollViewButton, EditorDataType editorType, string elementKey = null)
    {
        // Building list of polities
        List<GameObject> polityButtons = new List<GameObject>();
        foreach (KeyValuePair<int, Polity> p in GameManager.Instance.MAP_GetPolities())
        {
            Polity currentPolity = p.Value;

            // Instantiate buttons
            GameObject button = Instantiate(editorMenuButton);
            string polityName = currentPolity.Name;

            GameManager.Instance.LOC_AddLocalizeString(button, "LOC_TABLE_HIST_POLITIES", polityName);


            //*// OnClick() events //*//
            if (editorType == EditorDataType.Polity)
            {
                // Data filler
                button.GetComponent<Button>().onClick.AddListener(delegate { ButtonEventToFillInfo(EditorDataType.Polity, p.Key); });
                // Message status
                IdentificatorMessage message = Instantiate(polityMessage);
                message.objectName = currentPolity.Name;
                button.GetComponent<Button>().onClick.AddListener(delegate { UpdateModStatusMessage(message); });
                // Run click events for a element
                if (currentPolity.Name == elementKey) { button.GetComponent<Button>().onClick.Invoke(); }
            }
            if (editorType == EditorDataType.PolitySymbols)
            {
                // Data filler
                button.GetComponent<Button>().onClick.AddListener(delegate { ButtonEventToFillInfo(EditorDataType.PolitySymbols, p.Key); });
            }
            //*//

                polityButtons.Add(button);
        };

        // Reordering buttons
        List<GameObject> SortedPolityButtons = polityButtons.OrderBy(o => o.GetComponentInChildren<TextMeshProUGUI>().text).ToList();
        foreach (GameObject polityButton in SortedPolityButtons)
        {
            polityButton.transform.SetParent(scrollViewButton.transform);
        }
    }
    private void FillScrollSettlement(Transform scrollViewButton, string elementKey = null)
    {
        // Building list of settlements
        List<GameObject> settlementButtons = new List<GameObject>();
        foreach (KeyValuePair<int, Settlement> s in GameManager.Instance.MAP_GetSettlements())
        {
            Settlement currentSettlement = s.Value;

            // Instantiate buttons
            GameObject button = Instantiate(editorMenuButton);
            string settlementName = currentSettlement.Name;

            GameManager.Instance.LOC_AddLocalizeString(button, "LOC_TABLE_HIST_SETTLEMENTS", settlementName);


            //*// OnClick() events //*//
            // Data filler
            button.GetComponent<Button>().onClick.AddListener(delegate { ButtonEventToFillInfo(EditorDataType.Settlement, s.Key); });
            // Message status
            IdentificatorMessage message = Instantiate(settlementMessage);
            message.objectName = currentSettlement.Name;
            button.GetComponent<Button>().onClick.AddListener(delegate { UpdateModStatusMessage(message); });
            // Run click events for a element
            if (currentSettlement.Name == elementKey) { button.GetComponent<Button>().onClick.Invoke(); }
            //*//

            settlementButtons.Add(button);
        };

        // Reordering buttons
        List<GameObject> SortedSettlementButtons = settlementButtons.OrderBy(o => o.GetComponentInChildren<TextMeshProUGUI>().text).ToList();
        foreach (GameObject settlementButton in SortedSettlementButtons)
        {
            settlementButton.transform.SetParent(scrollViewButton.transform);
        }
    }
    /***                        ***/


    /*** Floating menus***/
    /// <summary>
    /// Activate the region panel
    /// </summary>
    /// <param name="x">new position X</param>
    /// <param name="y">new position Y</param>
    public void ActivateRegionPanel(float x, float y)
    {
        editorRegionPanel.SetActive(true);

        // Set status
        uiStatus = UIStatus.InfoRegion;

        // Move panel
        regionPanelxPositionLast = x;
        regionPanelyPositionLast = y;
        editorRegionPanel.transform.position = new Vector3(x, y, editorRegionPanel.transform.position.z);
    }
    /// <summary>
    /// Deactivate the region panel
    /// </summary>
    public void DeactivateRegionPanel()
    {
        editorRegionPanel.SetActive(false);

        // Set status
        uiStatus = UIStatus.Nothing;
    }
    /// <summary>
    /// Set the name and terrain's sprite of the region (ID)
    /// </summary>
    /// <param name="name">region name parameter</param>
    public void SetNameAndImageRegionPanel(string name, string terrain)
    {
        // Name
        tmpRegionFloatingPanel.SetRegionValue(name);

        // Sprite
        Sprite terrainTile = terrainTiles.FirstOrDefault(o => o.name.Equals(terrain));
        tmpRegionFloatingPanel.SetTerrainImage(terrainTile);
    }
    /// <summary>
    /// For debugging, show color of the owner
    /// </summary>
    /// <param name="rgb">color</param>
    public void ShowRgbOwner(string rgb)
    {
        tmpRegionFloatingPanel.ShowOwnerColor(rgb);
    }
    /// <summary>
    /// Show the setllements of the region and button 'Stages of History'
    /// </summary>
    /// <param name="button">is necessary to update the button?</param>
    /// <param name="setllement">setllement parameter</param>
    public void SetSettlementRegionPanel(bool button, string setllement = null)
    {
        if (setllement == null)
        {
            tmpRegionFloatingPanel.SetSettlementValue("LOC_TABLE_EDITOR_FLOATING", ParamUI.GENERIC_UNKNOWN);
        }
        else
        {
            tmpRegionFloatingPanel.SetSettlementValue("LOC_TABLE_HIST_SETTLEMENTS", setllement);
        }

        if (button)
        {
            tmpRegionFloatingPanel.SetHistoryButtonText("LOC_TABLE_EDITOR_FLOATING", ParamUI.REGION_HISTORY_BUTTON);
            tmpRegionFloatingPanel.SetButtonClick();
        }
    }
    /// <summary>
    /// On/Off history panel
    /// </summary>
    public void ToggleChangeOwner(int currentRegionId)
    {
        editorHistoryPanel.SetActive(!editorHistoryPanel.activeInHierarchy);

        if (editorHistoryPanel.activeInHierarchy)
        {
            // Set status
            uiStatus = UIStatus.OwnerSelection;
            tmpRegionFloatingPanel.SetHistoryButtonText("LOC_TABLE_EDITOR_FLOATING", ParamUI.GENERIC_CLOSE);

            // Get/Set size of the panels       
            float xPanel = editorRegionPanel.transform.position.x;
            float yPanel = editorRegionPanel.transform.position.y;
            float zPanel = editorRegionPanel.transform.position.z;
            float wPanel = tmpRegionFloatingPanel.GetRectTransform().rect.width;
            float hPanel = tmpRegionFloatingPanel.GetRectTransform().rect.height;
            float wHPanel = tmpHistoryFloatingPanel.GetRectTransform().rect.width;
            float x = xPanel < Screen.width / 2 ? xPanel : xPanel - wPanel - wHPanel;
            float y = yPanel < Screen.height / 2 ? yPanel + hPanel : yPanel - hPanel;
            editorHistoryPanel.transform.position = new Vector3(x, y, zPanel);

            bool isFisrtSettlement = true;

            // Building list of settlements
            foreach (KeyValuePair<int, Settlement> s in GameManager.Instance.MAP_GetSettlements())
            {
                if (s.Value.RegionId == currentRegionId || s.Value.RegionId == 0)
                {
                    Settlement currentSettlement = s.Value;

                    // First settlement for default
                    if (isFisrtSettlement)
                    {
                        tmpHistoryFloatingPanel.OnClickButtonEvent(currentRegionId, s);
                        isFisrtSettlement = false;
                    }                    

                    // Instantiate buttons
                    GameObject button = Instantiate(editorHistoryButton);

                    // Name of the button
                    string settlementName = currentSettlement.Name;

                    // Add the new button
                    tmpHistoryFloatingPanel.AddSettlementButton(button, "LOC_TABLE_HIST_SETTLEMENTS", settlementName);
        
                    //*// OnClick() event //*//                
                    button.GetComponent<Button>().onClick.AddListener(delegate { tmpHistoryFloatingPanel.OnClickButtonEvent(currentRegionId, s); });
                }
            }

            // Current stages
            LoadStages(currentRegionId);

        }
        else
        {
            // Set status
            uiStatus = UIStatus.InfoRegion;
            // The text of the button
            tmpRegionFloatingPanel.SetHistoryButtonText("LOC_TABLE_EDITOR_FLOATING", ParamUI.REGION_HISTORY_BUTTON);
            // Clean Buttons
            tmpHistoryFloatingPanel.CleanAllButtons(false);
        }

    }
    public void RefleshingHistory(int currentRegionId, bool delete, int settlementId = 0)
    {
        // Reload database
        GameManager.Instance.MAP_LoadHistoryRegionDictionaryFromDB(currentRegionId);

        // Load new stages
        LoadStages(currentRegionId);
        
        // Update region panel info
        if(settlementId != 0)
        {
            // Set settlement in the region panel
            if (delete)
            {
                SetSettlementRegionPanel(false, null);
            }
            else
            {
                string settlementKey = GameManager.Instance.MAP_GetSettlementsLocaleKeyById(settlementId);
                SetSettlementRegionPanel(false, settlementKey);
            }

            // Get current stage info
            Region currentRegion = GameManager.Instance.MAP_GetRegionById(currentRegionId);
            HistoryRegionRelation history = GetCurrentStageByRegion(currentRegion);
            int polityId = 0;
            if(history != null)
            {
                polityId = layersDropdown.value switch
                {
                    0 => history.Stage.PolityParentId_L1,
                    1 => Utilities.EitherInt(history.Stage.PolityParentId_L2, history.Stage.PolityParentId_L1),
                    2 => Utilities.EitherInt(Utilities.EitherInt(history.Stage.PolityParentId_L3, history.Stage.PolityParentId_L2), history.Stage.PolityParentId_L1),
                    3 => Utilities.EitherInt(Utilities.EitherInt(Utilities.EitherInt(history.Stage.PolityParentId_L4, history.Stage.PolityParentId_L3),history.Stage.PolityParentId_L2), history.Stage.PolityParentId_L1),
                    _ => 0
                };

                // Update polity of the region
                Polity polity = delete ? null : GameManager.Instance.MAP_GetPolityById(polityId);
                polity.Recolor();
                GameManager.Instance.MAP_ColorizeRegionsById(currentRegionId, polity);
            }
            else
            {
                // Update region in the map
                GameManager.Instance.MAP_ColorizeRegionsById(currentRegionId, null);
            }

        }
        
    }
    private void LoadStages(int regionId)
    {
        // Clean Buttons
        tmpHistoryFloatingPanel.CleanAllButtons(true);

        Region currentRegion = GameManager.Instance.MAP_GetRegionById(regionId);
        if(currentRegion.History != null)
        {
            for(int i = 0;i<currentRegion.History.Count;i++)
            {
                tmpHistoryFloatingPanel.toggleChangeOwnerEvent(regionId, currentRegion.History[i].StageId, currentRegion.History[i].SettlementId, currentRegion.History[i].Stage);
            }
        }
    }
    /// <summary>
    /// Get diagonal position to the floating panel
    /// </summary>
    /// <param name="mousePos">click position</param>
    /// <returns>new position (Vector2)</returns>
    public Vector2 CalculateNewPositionPanel(Vector3 mousePos)
    {
        float horizontalHalfScreen = Screen.width / 2;
        float verticalHalfScreen = Screen.height / 2;

        float horizontalQuarterScreen = Screen.width / 4;
        float verticalQuarterScreen = Screen.height / 4;

        float horizontalMargin = horizontalQuarterScreen / 3;

        float panelXPositionNew = mousePos.x < horizontalHalfScreen ? Screen.width - horizontalMargin : horizontalQuarterScreen - horizontalMargin;
        float panelYPositionNew = mousePos.y < verticalHalfScreen ? verticalQuarterScreen * 3 : verticalQuarterScreen;

        Vector2 panelPositionNew = new Vector2(panelXPositionNew, panelYPositionNew);
        return panelPositionNew;
    }
    /***                        ***/


    /*** PostIt Note - Polity information***/
    public void PostItPolityVisibility(Vector3 mousePos, bool showPostIt, Region region = null)
    {
        if(showPostIt != tmpPostItNote.activeInHierarchy) { ChangeActive(tmpPostItNote); }
        
        if (showPostIt)
        {
            
            // New position of the PostIt
            Vector2 newPosition = CalculateNewPositionPostItNote(mousePos);
            tmpPostItNote.transform.position = new Vector3(newPosition.x, newPosition.y, tmpPostItNote.transform.position.z);
            
            // Get current stage to fill the PostIt
            HistoryRegionRelation history = GetCurrentStageByRegion(region);
            string startEra = history.Stage.StartDate > 0 ? "A" : "B";
            string endEra = history.Stage.EndDate > 0 ? "A": "B";

            // Get polity names
            Polity polityL4 = history.Stage.PolityParentId_L4==0?null:GameManager.Instance.MAP_GetPolityById(history.Stage.PolityParentId_L4);
            Polity polityL3 = history.Stage.PolityParentId_L3 == 0 ? null : GameManager.Instance.MAP_GetPolityById(history.Stage.PolityParentId_L3);
            Polity polityL2 = history.Stage.PolityParentId_L2 == 0 ? null : GameManager.Instance.MAP_GetPolityById(history.Stage.PolityParentId_L2);
            Polity polityL1 = history.Stage.PolityParentId_L1 == 0 ? null : GameManager.Instance.MAP_GetPolityById(history.Stage.PolityParentId_L1);
            
            // Polity Owner L4
            if (polityL4 == region.Owner)
            {
                // Polity Type
                PolityType polityType = GameManager.Instance.MAP_GetPolityTypeById(history.Stage.PolityTypeIdParent_L4);
                postItNote.SetPolityType("LOC_TABLE_HIST_POLITIES_TYPE", polityType.Name);
                // Main polity
                postItNote.SetPolity("LOC_TABLE_HIST_POLITIES", polityL4.Name);
                // Image 
                string symbolFilenameL4 = history.Stage.IsSymbolForDate == 0 ? polityL4.Name + "_" + polityType.Name : polityL4.Name + "_" + polityType.Name + "_" + startEra + history.Stage.StartDate.ToString().PadLeft(8, '0') + "_" + endEra + history.Stage.EndDate.ToString().PadLeft(8, '0');
                postItNote.SetPolityImage(symbolFilenameL4);
                
                // Parent 1
                if (polityL3 != null)
                {
                    // Polity Type Parent 1
                    PolityType polityTypeL3 = GameManager.Instance.MAP_GetPolityTypeById(history.Stage.PolityTypeIdParent_L3);                    
                    // Parent 1
                    postItNote.SetParentVisibility(true);
                    postItNote.SetParent("LOC_TABLE_HIST_POLITIES", polityL3.Name);
                    // Image Parent 1
                    string symbolFilenameL41 = history.Stage.IsSymbolForDate == 0 ? polityL3.Name + "_" + polityTypeL3.Name : polityL3.Name + "_" + polityTypeL3.Name + "_" + startEra + history.Stage.StartDate.ToString().PadLeft(8, '0') + "_" + endEra + history.Stage.EndDate.ToString().PadLeft(8, '0');
                    postItNote.SetParentImage(symbolFilenameL41);
                }
                else
                {
                    postItNote.SetParentVisibility(false);
                }
                
                // Parent 2
                if (polityL2 != null)
                {
                    // Polity Type Parent 2
                    PolityType polityTypeL2 = GameManager.Instance.MAP_GetPolityTypeById(history.Stage.PolityTypeIdParent_L2);
                    // Parent 2
                    postItNote.SetParentVisibility2(true);
                    postItNote.SetParent2("LOC_TABLE_HIST_POLITIES", polityL2.Name);
                    // Image Parent 2
                    string symbolFilenameL42 = history.Stage.IsSymbolForDate == 0 ? polityL2.Name + "_" + polityTypeL2.Name : polityL2.Name + "_" + polityTypeL2.Name + "_" + startEra + history.Stage.StartDate.ToString().PadLeft(8, '0') + "_" + endEra + history.Stage.EndDate.ToString().PadLeft(8, '0');
                    postItNote.SetParentImage2(symbolFilenameL42);
                }
                else
                {
                    postItNote.SetParentVisibility2(false);
                }

                // Parent 3
                if (polityL3 != null)
                {
                    // Polity Type Parent 3
                    PolityType polityTypeL3 = GameManager.Instance.MAP_GetPolityTypeById(history.Stage.PolityTypeIdParent_L3);
                    // Parent 3
                    postItNote.SetParentVisibility3(true);
                    postItNote.SetParent3("LOC_TABLE_HIST_POLITIES", polityL3.Name);
                    // Image Parent 3
                    string symbolFilenameL43 = history.Stage.IsSymbolForDate == 0 ? polityL3.Name + "_" + polityTypeL3.Name : polityL3.Name + "_" + polityTypeL3.Name + "_" + startEra + history.Stage.StartDate.ToString().PadLeft(8, '0') + "_" + endEra + history.Stage.EndDate.ToString().PadLeft(8, '0');
                    postItNote.SetParentImage3(symbolFilenameL43);
                }
                else
                {
                    postItNote.SetParentVisibility3(false);
                }
            }else 

            // Polity Owner L3
            if (polityL3 == region.Owner)
            {
                // Polity Type
                PolityType polityType = GameManager.Instance.MAP_GetPolityTypeById(history.Stage.PolityTypeIdParent_L3);
                postItNote.SetPolityType("LOC_TABLE_HIST_POLITIES_TYPE", polityType.Name);
                // Main polity
                postItNote.SetPolity("LOC_TABLE_HIST_POLITIES", polityL3.Name);
                // Image
                string symbolFilenameL3 = history.Stage.IsSymbolForDate == 0 ? polityL3.Name + "_" + polityType.Name : polityL3.Name + "_" + polityType.Name + "_" + startEra + history.Stage.StartDate.ToString().PadLeft(8, '0') + "_" + endEra + history.Stage.EndDate.ToString().PadLeft(8, '0');
                postItNote.SetPolityImage(symbolFilenameL3);

                // Parent 1
                if (polityL2 != null)
                {
                    // Polity Type Parent 1
                    PolityType polityTypeL2 = GameManager.Instance.MAP_GetPolityTypeById(history.Stage.PolityTypeIdParent_L2);
                    // Parent 1
                    postItNote.SetParentVisibility(true);
                    postItNote.SetParent("LOC_TABLE_HIST_POLITIES", polityL2.Name);
                    // Image Parent 1
                    string symbolFilenameL31 = history.Stage.IsSymbolForDate == 0 ? polityL2.Name + "_" + polityTypeL2.Name : polityL2.Name + "_" + polityTypeL2.Name + "_" + startEra + history.Stage.StartDate.ToString().PadLeft(8, '0') + "_" + endEra + history.Stage.EndDate.ToString().PadLeft(8, '0');
                    postItNote.SetParentImage(symbolFilenameL31);
                }
                else
                {
                    postItNote.SetParentVisibility(false);
                }

                // Parent 2
                if (polityL1 != null)
                {
                    // Polity Type Parent 2
                    PolityType polityTypeL1 = GameManager.Instance.MAP_GetPolityTypeById(history.Stage.PolityTypeIdParent_L1);
                    // Parent 2
                    postItNote.SetParentVisibility2(true);
                    postItNote.SetParent2("LOC_TABLE_HIST_POLITIES", polityL1.Name);
                    // Image Parent 2
                    string symbolFilenameL32 = history.Stage.IsSymbolForDate == 0 ? polityL1.Name + "_" + polityTypeL1.Name : polityL1.Name + "_" + polityTypeL1.Name + "_" + startEra + history.Stage.StartDate.ToString().PadLeft(8, '0') + "_" + endEra + history.Stage.EndDate.ToString().PadLeft(8, '0');
                    postItNote.SetParentImage2(symbolFilenameL32);
                }
                else
                {
                    postItNote.SetParentVisibility2(false);
                }

                postItNote.SetParentVisibility3(false);
            } else 

            // Polity Owner L2
            if (polityL2 == region.Owner)
            {
                // Polity Type
                PolityType polityType = GameManager.Instance.MAP_GetPolityTypeById(history.Stage.PolityTypeIdParent_L2);
                postItNote.SetPolityType("LOC_TABLE_HIST_POLITIES_TYPE", polityType.Name);
                // Main polity
                postItNote.SetPolity("LOC_TABLE_HIST_POLITIES", polityL2.Name);
                // Image
                string symbolFilenameL2 = history.Stage.IsSymbolForDate == 0 ? polityL2.Name + "_" + polityType.Name : polityL2.Name + "_" + polityType.Name + "_" + startEra + history.Stage.StartDate.ToString().PadLeft(8, '0') + "_" + endEra + history.Stage.EndDate.ToString().PadLeft(8, '0');
                postItNote.SetPolityImage(symbolFilenameL2);

                // Parent 1
                if (polityL1 != null)
                {
                    // Polity Type Parent 1
                    PolityType polityTypeL1 = GameManager.Instance.MAP_GetPolityTypeById(history.Stage.PolityTypeIdParent_L1);
                    // Parent 1
                    postItNote.SetParentVisibility(true);
                    postItNote.SetParent("LOC_TABLE_HIST_POLITIES", polityL1.Name);
                    // Image Parent 1
                    string symbolFilenameL21 = history.Stage.IsSymbolForDate == 0 ? polityL1.Name + "_" + polityTypeL1.Name : polityL1.Name + "_" + polityTypeL1.Name + "_" + startEra + history.Stage.StartDate.ToString().PadLeft(8, '0') + "_" + endEra + history.Stage.EndDate.ToString().PadLeft(8, '0');
                    postItNote.SetParentImage(symbolFilenameL21);
                }
                else
                {
                    postItNote.SetParentVisibility(false);
                }

                postItNote.SetParentVisibility2(false);
                postItNote.SetParentVisibility3(false);
            } else 

            // Polity Owner L1
            if (polityL1.Name == region.Owner.Name)
            {
                // Polity Type
                PolityType polityType = GameManager.Instance.MAP_GetPolityTypeById(history.Stage.PolityTypeIdParent_L1);
                postItNote.SetPolityType("LOC_TABLE_HIST_POLITIES_TYPE", polityType.Name);

                // Main polity
                postItNote.SetPolity("LOC_TABLE_HIST_POLITIES", polityL1.Name);
                postItNote.SetParentVisibility(false);
                postItNote.SetParentVisibility2(false);
                postItNote.SetParentVisibility3(false);
                // Image
                string symbolFilenameL1 = history.Stage.IsSymbolForDate == 0 ? polityL1.Name + "_" + polityType.Name : polityL1.Name + "_" + polityType.Name + "_" + startEra + history.Stage.StartDate.ToString().PadLeft(8, '0') + "_" + endEra + history.Stage.EndDate.ToString().PadLeft(8, '0');
                postItNote.SetPolityImage(symbolFilenameL1);
            }
                
            // Policy
            if (history.Stage.PolicyId != 0)
            {
                postItNote.SetPolicyVisibility(true);
                Polity policy = GameManager.Instance.MAP_GetPolityById(history.Stage.PolicyId);
                postItNote.SetPolicy("LOC_TABLE_HIST_POLITIES", policy.Name);
            }
            else
            {
                postItNote.SetPolicyVisibility(false);
            }
            
        }
    }
    private Vector2 CalculateNewPositionPostItNote(Vector3 mousePos)
    {
        float horizontalHalfScreen = Screen.width / 2;
        float verticalHalfScreen = Screen.height / 2;

        float w = postItNoteRectTransform.rect.width;
        float h = postItNoteRectTransform.rect.height;

        float w_margin = 100;

        float panelXPositionNew = mousePos.x < horizontalHalfScreen ? mousePos.x + w : mousePos.x - w + w_margin;
        float panelYPositionNew = mousePos.y < verticalHalfScreen ? mousePos.y + h : mousePos.y - h;

        Vector2 panelPositionNew = new Vector2(panelXPositionNew, panelYPositionNew);
        return panelPositionNew;
    }
    private HistoryRegionRelation GetCurrentStageByRegion(Region region)
    {
        HistoryRegionRelation currentHistory = null;
        List<HistoryRegionRelation> history = region.History;

        foreach(HistoryRegionRelation stage in history)
        {
            int startDate = stage.Stage.StartDate;
            int endDate = stage.Stage.EndDate;
            if(IsDateCurrent(startDate, endDate)) {
                currentHistory = stage; 
            }
        }

        return currentHistory;
    }
    /***                        ***/


    /*** Message status functions ***/
    public void UpdateNewStatusMessage(SimpleMessage simpleMessage)
    {
        GameManager.Instance.LOC_AddLocalizeString(simpleMessage);
    }

    public void UpdateModStatusMessage(IdentificatorMessage identificatorMessage)
    {
        GameManager.Instance.LOC_AddLocalizeString(identificatorMessage);
    }
    /***                        ***/


    /*** List Button events ***/
    private void ButtonEventToFillInfo(EditorDataType dataType, int labelId)
    {
        string currentName = "";
        string table = "";

        // Current element from all elements from the DataBase
        if (dataType == EditorDataType.PolityType)
        {
            // Clearing old text
            polityTypeNameInput.text = "";

            Dictionary<int, PolityType> politiesType = GameManager.Instance.MAP_GetPolitiesType();
            PolityType polityType = politiesType.Where(x => x.Key.Equals(labelId)).Select(x => x.Value).FirstOrDefault();
            polityTypeIdLabel.text = labelId.ToString();
            currentName = polityType.Name;
            table = "LOC_TABLE_HIST_POLITIES_TYPE";
            GameManager.Instance.LOC_AddLocalizeString(polityTypeNameInput, table, currentName);

            // Focus on this field
            if (polityTypeNameInput.placeholder.GetComponent<TextMeshProUGUI>().text != polityTypeNameInput.name)
            {
                polityTypeNameInput.text = polityTypeNameInput.placeholder.GetComponent<TextMeshProUGUI>().text;
            }
        }
        else if(dataType == EditorDataType.Polity)
        {
            // Clearing old text
            polityNameInput.text = "";

            GameManager.Instance.MAP_LoadPolitiesDictionaryFromDB(GetCurrentTimeline(false), 1);
            Dictionary<int, Polity> polities = GameManager.Instance.MAP_GetPolities();
            Polity polity = polities.Where(x => x.Key.Equals(labelId)).Select(x => x.Value).FirstOrDefault();
            polityIdLabel.text = labelId.ToString();            
            currentName = polity.Name;
            table = "LOC_TABLE_HIST_POLITIES";
            GameManager.Instance.LOC_AddLocalizeString(polityNameInput, table, currentName);

            // Focus on this field
            if (polityNameInput.placeholder.GetComponent<TextMeshProUGUI>().text != polityNameInput.name)
            {
                polityNameInput.text = polityNameInput.placeholder.GetComponent<TextMeshProUGUI>().text;
            }

            // Check IsCollective
            polityPolicyCheck.isOn = polity.IsCollective;

        }
        else if (dataType == EditorDataType.Settlement)
        {
            // Clearing old text
            settlementNameInput.text = "";
            settlementRegionInput.text = "0";
            settlementPixelXInput.text = "0";
            settlementPixelYInput.text = "0";

            GameManager.Instance.MAP_LoadSettlementsDictionaryFromDB();
            Dictionary<int, Settlement> settlements = GameManager.Instance.MAP_GetSettlements();
            Settlement settlement = settlements.Where(x => x.Key.Equals(labelId)).Select(x => x.Value).FirstOrDefault();
            settlementIdLabel.text = labelId.ToString();
            currentName = settlement.Name;
            table = "LOC_TABLE_HIST_SETTLEMENTS";
            GameManager.Instance.LOC_AddLocalizeString(settlementNameInput, table, currentName);

            // Focus on this field
            if (settlementNameInput.placeholder.GetComponent<TextMeshProUGUI>().text != settlementNameInput.name)
            {
                settlementNameInput.text = settlementNameInput.placeholder.GetComponent<TextMeshProUGUI>().text;
            }

            settlementRegionInput.text = settlement.RegionId.ToString();
            settlementPixelXInput.text = settlement.PixelCoordinates.x.ToString();
            settlementPixelYInput.text = settlement.PixelCoordinates.y.ToString();

        }
        else if (dataType == EditorDataType.PolitySymbols)
        {
            // Remove old panels
            GameObject[] oldSymbolPanels = GameObject.FindGameObjectsWithTag(ParamUI.TAG_EDITOR_POLITY_SYMBOLS);
            foreach (GameObject oldSymbolPanel in oldSymbolPanels)
            {
                Destroy(oldSymbolPanel);
            }

            // Clone the panel of the symbols
            List<int> polityTypesId = GameManager.Instance.MAP_GetPolityTypesByPolity(labelId);
            List<int> polityTypesId_Policy = GameManager.Instance.MAP_GetPolityTypesByPolicy(labelId);
            polityTypesId.AddRange(polityTypesId_Policy);
            foreach(int polityTypeId in polityTypesId)
            {
                GameObject symbolItem = Instantiate(symbolItemPrefab);
                symbolItem.transform.SetParent(contentPolitySymbols.transform);
                symbolItem.GetComponent<PolitySymbolPanel>().SetSymbolInfo(labelId, polityTypeId);
            }
        }

    }
    /***                        ***/


    /*** Action Button events ***/
    // SAVE EVENT
    private void SaveActionButtonEvent(EditorDataType dataType, IdentificatorMessage identificatorMessage, List<SimpleMessage> okMessages, SimpleMessage emptyNameMessage, SimpleMessage duplicatedNameMessage, string loc_table, List<TMP_InputField> inputs, string idLabel, Toggle check = null)
    {
        // Remove old fields messages
        foreach(SimpleMessage message in okMessages)
        {
            GameManager.Instance.LOC_AddLocalizeString(message);
        }

        // Check flags
        bool fChkGlobalIsOk = true;
        bool fChkLocalIsOk = true;

        //  Global Checks for every input (Region input is a exception. For this case, the value 0 or empty is all regions) ...
        foreach (TMP_InputField input in inputs)
        {
            if (input.tag != ParamUI.TAG_NOTLOCALIZATION && MessageHelper.IsFieldEmpty(input.text) == true)
            {
                GameManager.Instance.LOC_AddLocalizeString(emptyNameMessage);
                fChkGlobalIsOk = false;
            }
        }

        // If global check is correct
        if (fChkGlobalIsOk)
        {
            {// If all ok  then insert/modify data            
                if (idLabel == "0")
                {
                    foreach (TMP_InputField input in inputs)
                    {
                        if (input.tag != ParamUI.TAG_NOTLOCALIZATION && GameManager.Instance.LOC_KeyExist(loc_table, input.text))
                        {// Check duplicate key
                            GameManager.Instance.LOC_AddLocalizeString(duplicatedNameMessage);
                            fChkLocalIsOk = false;
                        }
                        if (input.tag != ParamUI.TAG_NOTLOCALIZATION && GameManager.Instance.LOC_ValueExist(loc_table, input.text, input.text))
                        {// Check duplicate locate value (current language)
                            GameManager.Instance.LOC_AddLocalizeString(duplicatedNameMessage);
                            fChkLocalIsOk = false;
                        }
                    }

                    // If all correct, the data is inserted
                    if (fChkLocalIsOk)
                    {

                        // Insert new locale data (All languanges)
                        foreach (TMP_InputField input in inputs)
                        {
                            if (input.tag != ParamUI.TAG_NOTLOCALIZATION)
                            {
                                GameManager.Instance.LOC_InsertNewEntry(loc_table, input.text, input.text);
                            }
                        }

                        string messageStatusName = "";
                        if (dataType == EditorDataType.PolityType)
                        {
                            // Insert new data
                            string polityTypeName = inputs.First(x => x.name.Equals(ParamUI.EDITMENU_POLITYTYPE_NAME_INPUT)).text;                            
                            CsvConnection.Instance.AddPolityType(polityTypeName);

                            // Status name
                            messageStatusName = polityTypeName;

                            // Reload dictionary
                            GameManager.Instance.MAP_LoadPolitiesTypeDictionaryFromDB();
                            FillScrollButton(polityTypeContent);
                        }
                        else if (dataType == EditorDataType.Polity)
                        {
                            // Insert new data
                            string polityName = inputs.First(x => x.name.Equals(ParamUI.EDITMENU_POLITY_NAME_INPUT)).text;
                            CsvConnection.Instance.AddPolity(polityName, check.isOn);

                            // Status name
                            messageStatusName = polityName;

                            // Reload dictionary
                            GameManager.Instance.MAP_LoadPolitiesDictionaryFromDB(GetCurrentTimeline(false), 1);
                            FillScrollButton(polityContent);
                        }
                        else if (dataType == EditorDataType.Settlement)
                        {
                            // Insert new data
                            string settlementName = inputs.First(x => x.name.Equals(ParamUI.EDITMENU_SETTLEMENT_NAME_INPUT)).text;
                            string settlementRegion = inputs.First(x => x.name.Equals(ParamUI.EDITMENU_SETTLEMENT_REGION_INPUT)).text;
                            string settlementX = inputs.First(x => x.name.Equals(ParamUI.EDITMENU_SETTLEMENT_PIXEL_X)).text;
                            string settlementY = inputs.First(x => x.name.Equals(ParamUI.EDITMENU_SETTLEMENT_PIXEL_Y)).text;
                            settlementRegion = int.TryParse(settlementRegion, out int sr) == true ? settlementRegion : "0";
                            settlementX = int.TryParse(settlementX, out int sx) == true ? settlementX : "0";
                            settlementY = int.TryParse(settlementY, out int sy) == true ? settlementY : "0";
                            CsvConnection.Instance.AddSettlement(settlementName, Int32.Parse(settlementRegion), Int32.Parse(settlementX), Int32.Parse(settlementY));

                            // Status name
                            messageStatusName = settlementName;

                            // Reload dictionary
                            GameManager.Instance.MAP_LoadSettlementsDictionaryFromDB();
                            FillScrollButton(settlementContent);
                        }

                        // Change status information
                        IdentificatorMessage message = Instantiate(identificatorMessage);
                        message.objectName = messageStatusName;
                        UpdateModStatusMessage(message);
                        // Reload form
                        int lastId = CsvConnection.Instance.GetLastIdAdded(dataType);
                        ButtonEventToFillInfo(dataType, lastId);
                    }                    
 
                }
                else
                { // Update new data
                    if (dataType == EditorDataType.PolityType)
                    {
                        string polityTypeLocaleId = GameManager.Instance.MAP_GetPolitiesTypeLocaleKeyById(Int32.Parse(idLabel));
                        string polityTypeName = inputs.First(x => x.name.Equals(ParamUI.EDITMENU_POLITYTYPE_NAME_INPUT)).text;
                        if (GameManager.Instance.LOC_ValueExist(loc_table, polityTypeLocaleId, polityTypeName))
                        {// Check duplicate locate value (current language)
                            GameManager.Instance.LOC_AddLocalizeString(duplicatedNameMessage);
                        }
                        else
                        {
                            // Update new current locale data
                            GameManager.Instance.LOC_UpdateEntry(loc_table, polityTypeLocaleId, polityTypeName);
                            // Update displayed data
                            FillScrollButtonScript(polityTypeContent, polityTypeLocaleId);
                        }
                    }
                    else if (dataType == EditorDataType.Polity)
                    {
                        string polityLocaleId = GameManager.Instance.MAP_GetPolitiesLocaleKeyById(Int32.Parse(idLabel));
                        string polityName = inputs.First(x => x.name.Equals(ParamUI.EDITMENU_POLITY_NAME_INPUT)).text;
                        if (GameManager.Instance.LOC_ValueExist(loc_table, polityLocaleId, polityName))
                        {// Check duplicate locate value (current language)
                            GameManager.Instance.LOC_AddLocalizeString(duplicatedNameMessage);
                        }
                        else
                        {
                            // Update new current locale data                    
                            GameManager.Instance.LOC_UpdateEntry(loc_table, polityLocaleId, polityName);
                            // Update csv file
                            CsvConnection.Instance.UpdatePolity(Int32.Parse(idLabel), polityLocaleId, check.isOn);
                            // Update displayed data
                            FillScrollButtonScript(polityContent, polityLocaleId);
                            ButtonEventToFillInfo(dataType, Int32.Parse(idLabel));
                        }
                    }
                    else if (dataType == EditorDataType.Settlement)
                    {
                        string settlementLocaleId = GameManager.Instance.MAP_GetSettlementsLocaleKeyById(Int32.Parse(idLabel));
                        string settlementName = inputs.First(x => x.name.Equals(ParamUI.EDITMENU_SETTLEMENT_NAME_INPUT)).text;
                        if (GameManager.Instance.LOC_ValueExist(loc_table, settlementLocaleId, settlementName))
                        {// Check duplicate locate value (current language)
                            GameManager.Instance.LOC_AddLocalizeString(duplicatedNameMessage);
                        }
                        else
                        {
                            // Update new current locale data
                            string settlementRegion = inputs.First(x => x.name.Equals(ParamUI.EDITMENU_SETTLEMENT_REGION_INPUT)).text;
                            string settlementX = inputs.First(x => x.name.Equals(ParamUI.EDITMENU_SETTLEMENT_PIXEL_X)).text;
                            string settlementY = inputs.First(x => x.name.Equals(ParamUI.EDITMENU_SETTLEMENT_PIXEL_Y)).text;
                            GameManager.Instance.LOC_UpdateEntry(loc_table, settlementLocaleId, settlementName);
                            // Update CSV file
                            settlementRegion = int.TryParse(settlementRegion, out int sr) == true ? settlementRegion : "0";
                            settlementX = int.TryParse(settlementX, out int sx) == true ? settlementX : "0";
                            settlementY = int.TryParse(settlementY, out int sy) == true ? settlementY : "0";
                            CsvConnection.Instance.UpdateSettlement(Int32.Parse(idLabel), settlementLocaleId, Int32.Parse(settlementRegion), Int32.Parse(settlementX), Int32.Parse(settlementY));
                            // Update displayed data
                            FillScrollButtonScript(settlementContent, settlementLocaleId);
                            ButtonEventToFillInfo(dataType, Int32.Parse(idLabel));
                        }
                    }

                }
            }
        }
 
    }
    public void PolityTypeSaveActionButtonEvent()
    {
        List<SimpleMessage> okMessages = new List<SimpleMessage>
        {
            polityTypeOkNameMessage
        };
        List<TMP_InputField> inputs = new List<TMP_InputField>
        {
            polityTypeNameInput
        };
        SaveActionButtonEvent(EditorDataType.PolityType, polityTypeMessage, okMessages, polityTypeEmptyNameMessage, polityTypeDuplicatedNameMessage, "LOC_TABLE_HIST_POLITIES_TYPE", inputs, polityTypeIdLabel.text);
    }
    public void PolitySaveActionButtonEvent()
    {
        List<SimpleMessage> okMessages = new List<SimpleMessage>
        {
            polityOkNameMessage
        };
        List<TMP_InputField> inputs = new List<TMP_InputField>
        {
            polityNameInput
        };
        SaveActionButtonEvent(EditorDataType.Polity, polityMessage, okMessages, polityEmptyNameMessage, polityDuplicatedNameMessage, "LOC_TABLE_HIST_POLITIES", inputs, polityIdLabel.text, polityPolicyCheck);
    }
    public void SettlementSaveActionButtonEvent()
    {
        List<SimpleMessage> okMessages = new List<SimpleMessage>
        {
            settlementOkNameMessage,
            settlementOkRegionMessage
        };
        List<TMP_InputField> inputs = new List<TMP_InputField>
        {
            settlementNameInput,
            settlementRegionInput,
            settlementPixelXInput,
            settlementPixelYInput
        };
        SaveActionButtonEvent(EditorDataType.Settlement, settlementMessage, okMessages, settlementEmptyNameMessage, settlementDuplicatedNameMessage, "LOC_TABLE_HIST_SETTLEMENTS", inputs, settlementIdLabel.text);
    }

    // CLEAR EVENT
    private void ClearMessages(GameObject status, SimpleMessage okNameMessage)
    {

        // Main message texts
        status.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";

        // Fields message status
        GameManager.Instance.LOC_AddLocalizeString(okNameMessage);
    }
    public void PolityTypeClearActionButtonEvent()
    {
        // Values and Id
        polityTypeIdLabel.text = "0";
        polityTypeNameInput.text = "";

        ClearMessages(polityTypeStatus, polityTypeOkNameMessage);
    }
    public void PolityClearActionButtonEvent()
    {
        // Values and Id
        polityIdLabel.text = "0";
        polityNameInput.text = "";
        polityPolicyCheck.isOn = false;

        ClearMessages(polityStatus, polityOkNameMessage);
    }
    public void SettlementClearActionButtonEvent()
    {
        // Values and Id
        settlementIdLabel.text = "0";
        settlementNameInput.text = "";
        settlementRegionInput.text = "";
        settlementPixelXInput.text = "";
        settlementPixelYInput.text = "";

        ClearMessages(settlementStatus, settlementOkNameMessage);
        ClearMessages(settlementStatus, settlementOkRegionMessage);
    }

    // DELETE EVENT
    public void PolityTypeDeleteActionButtonEvent()
    {
        if (polityTypeIdLabel.text == "0")
        {
            GameManager.Instance.LOC_AddLocalizeString(polityTypeNoRemoveMessage);
        }
        else if (GameManager.Instance.MAP_PolityTypeIsRelated(Int32.Parse(polityTypeIdLabel.text)))
        {
            GameManager.Instance.LOC_AddLocalizeString(polityTypeRelatedDataMessage);
        }
        else
        {
            // Delete data
            CsvConnection.Instance.RemovePolityType(Int32.Parse(polityTypeIdLabel.text));

            // Delete locale data (All languanges)
            string polityTypeLocaleId = GameManager.Instance.MAP_GetPolitiesTypeLocaleKeyById(Int32.Parse(polityTypeIdLabel.text));
            GameManager.Instance.LOC_DeleteEntry("LOC_TABLE_HIST_POLITIES_TYPE", polityTypeLocaleId);

            // Update displayed data
            // Reload dictionary
            GameManager.Instance.MAP_LoadPolitiesTypeDictionaryFromDB();
            FillScrollButton(polityTypeContent);
            // New status information
            SimpleMessage message = Instantiate(polityTypeNewMessage);
            UpdateNewStatusMessage(message);
            PolityTypeClearActionButtonEvent();
        }
    }
    public void PolityDeleteActionButtonEvent()
    {
        if (polityIdLabel.text == "0")
        {
            GameManager.Instance.LOC_AddLocalizeString(polityNoRemoveMessage);
        }
        else if (GameManager.Instance.MAP_PolityIsRelated(Int32.Parse(polityIdLabel.text)))
        {
            GameManager.Instance.LOC_AddLocalizeString(polityRelatedDataMessage);
        }
        else
        {
            // Delete data
            CsvConnection.Instance.RemovePolity(Int32.Parse(polityIdLabel.text));

            // Delete locale data (All languanges)
            string polityLocaleId = GameManager.Instance.MAP_GetPolitiesLocaleKeyById(Int32.Parse(polityIdLabel.text));
            GameManager.Instance.LOC_DeleteEntry("LOC_TABLE_HIST_POLITIES", polityLocaleId);

            // Update displayed data
            // Reload dictionary
            GameManager.Instance.MAP_LoadPolitiesDictionaryFromDB(GetCurrentTimeline(false), 1);
            FillScrollButton(polityContent);
            // New status information
            SimpleMessage message = Instantiate(polityNewMessage);
            UpdateNewStatusMessage(message);
            PolityClearActionButtonEvent();
        }
    }
    public void SettlementDeleteActionButtonEvent()
    {
        if (settlementIdLabel.text == "0")
        {
            GameManager.Instance.LOC_AddLocalizeString(settlementNoRemoveMessage);
        }
        /*else if (GameManager.Instance.MAP_PolityIsRelated(Int32.Parse(polityIdLabel.text)))  ------ TO CHECK
        {
            GameManager.Instance.LOC_AddLocalizeString(polityRelatedDataMessage);
        }*/
        else
        {
            // Delete data
            CsvConnection.Instance.RemoveSettlement(Int32.Parse(settlementIdLabel.text));

            // Delete locale data (All languanges)
            string settlementLocaleId = GameManager.Instance.MAP_GetSettlementsLocaleKeyById(Int32.Parse(settlementIdLabel.text));
            GameManager.Instance.LOC_DeleteEntry("LOC_TABLE_HIST_SETTLEMENTS", settlementLocaleId);

            // Update displayed data
            // Reload dictionary
            GameManager.Instance.MAP_LoadSettlementsDictionaryFromDB();
            FillScrollButton(settlementContent);
            // New status information
            SimpleMessage message = Instantiate(settlementNewMessage);
            UpdateNewStatusMessage(message);
            SettlementClearActionButtonEvent();
        }
    }
    /***                        ***/


    /*** Timeline ***/
    /// <summary>
    /// Get current timeline
    /// </summary>
    /// <param name="button">Event button</param>
    /// <returns>Timeline in numeric format</returns>  
    public int GetCurrentTimeline(bool button)
    {
        int era;
        string day, month, year;

        era = eraTimeline.value;

        // To check the source of the calling
        if (button)
        {      
            day = dayTimeline.text == "" ? dayTimeline.placeholder.GetComponent<TextMeshProUGUI>().text : dayTimeline.text;
            month = monthTimeline.text == "" ? monthTimeline.placeholder.GetComponent<TextMeshProUGUI>().text : monthTimeline.text;
            year = yearTimeline.text == "" ? yearTimeline.placeholder.GetComponent<TextMeshProUGUI>().text : yearTimeline.text;
        }
        else
        {
            day = dayTimeline.placeholder.GetComponent<TextMeshProUGUI>().text;
            month = monthTimeline.placeholder.GetComponent<TextMeshProUGUI>().text;
            year = yearTimeline.placeholder.GetComponent<TextMeshProUGUI>().text;
        }

        int date = Int32.Parse(year + month.PadLeft(2, '0') + day.PadLeft(2, '0'));
        int timeline = era > 0 ? date * -1 : date;

        return timeline;
    }
    public bool IsDateCurrent(int start, int end)
    {        
        int currentDate = GetCurrentTimeline(true);

        return currentDate >= start && currentDate <= end ? true : false;
    }
    /***                        ***/


}