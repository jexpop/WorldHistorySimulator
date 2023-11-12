using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;


public class EditorUICanvasManager : Singleton<EditorUICanvasManager>
{

    public UIStatus uiStatus;

    public GameObject editorMenuButton;
    public GameObject editorMenuSubbutton;
    public GameObject editorHistoryButton;
    public GameObject postItElement;
    public Toggle layerCheckCollective;
    public TMP_Dropdown layersDropdown;


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
    public GameObject settlementStatus;

    // Panel of the regions - Floating panels
    private GameObject tmpEditorRegionPanel, tmpEditorHistoryPanel, tmpPostItNote;
    private float regionPanelxPositionLast, regionPanelyPositionLast;


    private void Start()
    {
        // Initialise the post it note with polity information
        tmpPostItNote= Instantiate(postItElement);
        tmpPostItNote.transform.SetParent(this.transform.parent);
        tmpPostItNote.SetActive(false);
    }

    /// <summary>
    /// Chnage status active/deactive of a UI gameobject
    /// </summary>
    /// <param name="gameobject">UI gameobject</param>
    public void ChangeActive(GameObject UIgameobject)
    {
        UIgameobject.SetActive(!UIgameobject.activeInHierarchy);
    }

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
            case GameConst.UI_EDITMENU_SCROLLBUTTONS_POLITY_TYPE:

                // Building list of polities type
                List<GameObject> polityTypeButtons = new List<GameObject>();
                foreach (KeyValuePair<int, PolityType> pt in MapManager.Instance.GetPolitiesType())
                {
                    PolityType currentPolityType = pt.Value;

                    // Instantiate buttons
                    GameObject button = Instantiate(editorMenuButton);
                    string polityTypeName = currentPolityType.Name;

                    LocalizationManager.Instance.AddLocalizeString(button, "LOC_TABLE_HIST_POLITIES_TYPE", polityTypeName);


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

                // End case Polity Type Edit Menu
                break;

            // Polity Edit Menu
            case GameConst.UI_EDITMENU_SCROLLBUTTONS_POLITY:

                // Building list of polities
                List<GameObject> polityButtons= new List<GameObject>();
                foreach (KeyValuePair<int, Polity> p in MapManager.Instance.GetPolities())
                {
                    Polity currentPolity = p.Value;

                    // Instantiate buttons
                    GameObject button = Instantiate(editorMenuButton);
                    string polityName = currentPolity.Name;

                    LocalizationManager.Instance.AddLocalizeString(button, "LOC_TABLE_HIST_POLITIES", polityName);


                    //*// OnClick() events //*//
                    // Data filler
                    button.GetComponent<Button>().onClick.AddListener(delegate { ButtonEventToFillInfo(EditorDataType.Polity, p.Key); });
                    // Message status
                    IdentificatorMessage message = Instantiate(polityMessage);
                    message.objectName = currentPolity.Name;
                    button.GetComponent<Button>().onClick.AddListener(delegate { UpdateModStatusMessage(message); });
                    // Run click events for a element
                    if (currentPolity.Name == elementKey) { button.GetComponent<Button>().onClick.Invoke(); }
                    //*//

                    polityButtons.Add(button);
                };

                // Reordering buttons
                List<GameObject> SortedPolityButtons = polityButtons.OrderBy(o => o.GetComponentInChildren<TextMeshProUGUI>().text).ToList();
                foreach (GameObject polityButton in SortedPolityButtons)
                {
                    polityButton.transform.SetParent(scrollViewButton.transform);
                }

                // End case Polity Edit Menu
                break;

            // Settlement Edit Menu
            case GameConst.UI_EDITMENU_SCROLLBUTTONS_SETTLEMENT:

                // Building list of settlements
                List<GameObject> settlementButtons = new List<GameObject>();
                foreach (KeyValuePair<int, Settlement> s in MapManager.Instance.GetSettlements())
                {
                    Settlement currentSettlement = s.Value;

                    // Instantiate buttons
                    GameObject button = Instantiate(editorMenuButton);
                    string settlementName = currentSettlement.Name;

                    LocalizationManager.Instance.AddLocalizeString(button, "LOC_TABLE_HIST_SETTLEMENTS", settlementName);


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

                // End case Settlement Edit Menu
                break;
        }

    }


    /*** Floating menus***/
    /// <summary>
    /// Activate the region panel
    /// </summary>
    /// <param name="x">new position X</param>
    /// <param name="y">new position Y</param>
    public void ActivateRegionPanel(float x, float y)
    {
        // Destroy the latest region panel
        Destroy(tmpEditorRegionPanel);

        // New panel
        tmpEditorRegionPanel = Instantiate(editorRegionPanel);

        // Set parent
        tmpEditorRegionPanel.transform.SetParent(this.transform.parent);

        // Set status
        uiStatus = UIStatus.InfoRegion;

        // Move panel
        regionPanelxPositionLast = x;
        regionPanelyPositionLast = y;
        tmpEditorRegionPanel.transform.position = new Vector3(x, y, tmpEditorRegionPanel.transform.position.z);
    }
    /// <summary>
    /// Deactivate the region panel
    /// </summary>
    public void DeactivateRegionPanel()
    {
        // Set status
        uiStatus = UIStatus.Nothing;

        // Destroy the region panel
        Destroy(tmpEditorRegionPanel);
    }
    /// <summary>
    /// Set the name of the region (ID)
    /// </summary>
    /// <param name="name">region name parameter</param>
    public void SetNameRegionPanel(string name)
    {
        tmpEditorRegionPanel.GetComponent<RegionFloatingPanel>().SetRegionValue(name);
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
            tmpEditorRegionPanel.GetComponent<RegionFloatingPanel>().SetSettlementValue("LOC_TABLE_EDITOR_FLOATING", GameConst.UI_GENERIC_UNKNOWN);
        }
        else
        {
            tmpEditorRegionPanel.GetComponent<RegionFloatingPanel>().SetSettlementValue("LOC_TABLE_HIST_SETTLEMENTS", setllement);
        }

        if (button)
        {
            tmpEditorRegionPanel.GetComponent<RegionFloatingPanel>().SetHistoryButtonText("LOC_TABLE_EDITOR_FLOATING", GameConst.UI_REGION_HISTORY_BUTTON);
            tmpEditorRegionPanel.GetComponent<RegionFloatingPanel>().SetButtonClick();
        }
    }
    /// <summary>
    /// On/Off history panel
    /// </summary>
    public void ToggleChangeOwner(int currentRegionId)
    {
        if (tmpEditorHistoryPanel == null)
        {

            // New panel
            tmpEditorHistoryPanel = Instantiate(editorHistoryPanel);

            // Set parent
            tmpEditorHistoryPanel.transform.SetParent(this.transform.parent);

            // Set status
            uiStatus = UIStatus.OwnerSelection;
            tmpEditorRegionPanel.GetComponent<RegionFloatingPanel>().SetHistoryButtonText("LOC_TABLE_EDITOR_FLOATING", GameConst.UI_GENERIC_CLOSE);
                        
            // Get/Set size of the panels       
            float xPanel = tmpEditorRegionPanel.transform.position.x;
            float yPanel = tmpEditorRegionPanel.transform.position.y;
            float zPanel = tmpEditorRegionPanel.transform.position.z;
            float wPanel = tmpEditorRegionPanel.GetComponent<RectTransform>().rect.width;
            float hPanel = tmpEditorRegionPanel.GetComponent<RectTransform>().rect.height;
            float wHPanel = tmpEditorHistoryPanel.GetComponent<RectTransform>().rect.width;
            float x = xPanel < Screen.width / 2 ? xPanel : xPanel - wPanel - wHPanel;
            float y = yPanel < Screen.height / 2 ? yPanel + hPanel : yPanel - hPanel;
            tmpEditorHistoryPanel.transform.position = new Vector3(x, y, zPanel);

            // Building list of settlements
            foreach (KeyValuePair<int, Settlement> s in MapManager.Instance.GetSettlements())
            {
                if(s.Value.RegionId==currentRegionId || s.Value.RegionId == 0)
                {
                    Settlement currentSettlement = s.Value;

                    // Instantiate buttons
                    GameObject button = Instantiate(editorHistoryButton);

                    // Name of the button
                    string settlementName = currentSettlement.Name;

                    // Add the new button
                    tmpEditorHistoryPanel.GetComponent<HistoryFloatingPanel>().AddSettlementButton(button, "LOC_TABLE_HIST_SETTLEMENTS", settlementName);

                    //*// OnClick() event //*//                
                    button.GetComponent<Button>().onClick.AddListener(delegate { tmpEditorHistoryPanel.GetComponent<HistoryFloatingPanel>().OnClickButtonEvent(currentRegionId, s); });
                }
            }

            // Current stages
            LoadStages(currentRegionId);

        }
        else
        {
            // Destroy panel
            Destroy(tmpEditorHistoryPanel);
            // Set status
            uiStatus = UIStatus.InfoRegion;
            // The text of the button
            tmpEditorRegionPanel.GetComponent<RegionFloatingPanel>().SetHistoryButtonText("LOC_TABLE_EDITOR_FLOATING", GameConst.UI_REGION_HISTORY_BUTTON);
        }
    }
    public void RefleshingHistory(int currentRegionId, bool delete, int settlementId = 0)
    {
        // Reload database
        MapManager.Instance.LoadHistoryRegionDictionaryFromDB(currentRegionId);

        // Clean old stages
        GameObject[] editorStages = GameObject.FindGameObjectsWithTag(GameConst.TAG_UI_EDITOR_STAGES);
        foreach(GameObject stage in editorStages)
        {
            Destroy(stage);
        }

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
                string settlementKey = MapManager.Instance.GetSettlementsLocaleKeyById(settlementId);
                SetSettlementRegionPanel(false, settlementKey);
            }

            // Get current stage info
            Region currentRegion = MapManager.Instance.GetRegionById(currentRegionId);
            HistoryRegionRelation history = GetCurrentStageByRegion(currentRegion);
            int polityId = 0;
            if(history != null)
            {
                polityId = layersDropdown.value switch
                {
                    0 => history.Stage.PolityParentId_L1,
                    1 => history.Stage.PolityParentId_L2,
                    2 => history.Stage.PolityParentId_L3,
                    3 => history.Stage.PolityParentId_L4,
                    _ => 0
                };
            }

            // Update region in the map
            Polity polity = delete ? null : MapManager.Instance.GetPolityById(polityId);
            MapManager.Instance.ColorizeRegionsById(currentRegionId, polity);
        }
        else
        {
            // Update region in the map
            MapManager.Instance.ColorizeRegionsById(currentRegionId, null);
        }

    }
    private void LoadStages(int regionId)
    {
        Region currentRegion = MapManager.Instance.GetRegionById(regionId);
        if(currentRegion.History != null)
        {
            foreach (HistoryRegionRelation history in currentRegion.History)
            {
                tmpEditorHistoryPanel.GetComponent<HistoryFloatingPanel>().toggleChangeOwnerEvent(regionId, history.StageId, history.SettlementId, history.Stage);
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

            // Get polity names
            Polity polityL4 = history.Stage.PolityParentId_L4==0?null:MapManager.Instance.GetPolityById(history.Stage.PolityParentId_L4);
            Polity polityL3 = history.Stage.PolityParentId_L3 == 0 ? null : MapManager.Instance.GetPolityById(history.Stage.PolityParentId_L3);
            Polity polityL2 = history.Stage.PolityParentId_L2 == 0 ? null : MapManager.Instance.GetPolityById(history.Stage.PolityParentId_L2);
            Polity polityL1 = history.Stage.PolityParentId_L1 == 0 ? null : MapManager.Instance.GetPolityById(history.Stage.PolityParentId_L1);
            
            // Polity Owner L4
            if (polityL4 == region.Owner)
            {
                // Polity Type
                PolityType polityType = MapManager.Instance.GetPolityTypeById(history.Stage.PolityTypeIdParent_L4);
                tmpPostItNote.GetComponent<PostItNote>().SetPolityType("LOC_TABLE_HIST_POLITIES_TYPE", polityType.Name);
                // Main polity
                tmpPostItNote.GetComponent<PostItNote>().SetPolity("LOC_TABLE_HIST_POLITIES", polityL4.Name);
                
                // Parent 1
                if(polityL3 != null)
                {
                    tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility(true);
                    tmpPostItNote.GetComponent<PostItNote>().SetParent("LOC_TABLE_HIST_POLITIES", polityL3.Name);
                }
                else
                {
                    tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility(false);
                }

                // Parent 2
                if (polityL2 != null)
                {
                    tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility2(true);
                    tmpPostItNote.GetComponent<PostItNote>().SetParent2("LOC_TABLE_HIST_POLITIES", polityL2.Name);
                }
                else
                {
                    tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility2(false);
                }

                // Parent 3
                if (polityL3 != null)
                {
                    tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility3(true);
                    tmpPostItNote.GetComponent<PostItNote>().SetParent3("LOC_TABLE_HIST_POLITIES", polityL3.Name);
                }
                else
                {
                    tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility3(false);
                }
            }else 

            // Polity Owner L3
            if (polityL3 == region.Owner)
            {
                // Polity Type
                PolityType polityType = MapManager.Instance.GetPolityTypeById(history.Stage.PolityTypeIdParent_L3);
                tmpPostItNote.GetComponent<PostItNote>().SetPolityType("LOC_TABLE_HIST_POLITIES_TYPE", polityType.Name);
                // Main polity
                tmpPostItNote.GetComponent<PostItNote>().SetPolity("LOC_TABLE_HIST_POLITIES", polityL3.Name);

                // Parent 1
                if (polityL2 != null)
                {
                    tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility(true);
                    tmpPostItNote.GetComponent<PostItNote>().SetParent("LOC_TABLE_HIST_POLITIES", polityL2.Name);
                }
                else
                {
                    tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility(false);
                }

                // Parent 2
                if (polityL1 != null)
                {
                    tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility2(true);
                    tmpPostItNote.GetComponent<PostItNote>().SetParent2("LOC_TABLE_HIST_POLITIES", polityL1.Name);
                }
                else
                {
                    tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility2(false);
                }

                tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility3(false);
            } else 

            // Polity Owner L2
            if (polityL2 == region.Owner)
            {
                // Polity Type
                PolityType polityType = MapManager.Instance.GetPolityTypeById(history.Stage.PolityTypeIdParent_L2);
                tmpPostItNote.GetComponent<PostItNote>().SetPolityType("LOC_TABLE_HIST_POLITIES_TYPE", polityType.Name);
                Debug.Log("entro 2");
                // Main polity
                tmpPostItNote.GetComponent<PostItNote>().SetPolity("LOC_TABLE_HIST_POLITIES", polityL2.Name);

                // Parent 1
                if (polityL1 != null)
                {
                    tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility(true);
                    tmpPostItNote.GetComponent<PostItNote>().SetParent("LOC_TABLE_HIST_POLITIES", polityL1.Name);
                }
                else
                {
                    tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility(false);
                }

                tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility2(false);
                tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility3(false);
            } else 

            // Polity Owner L1
            if (polityL1.Name == region.Owner.Name)
            {
                // Polity Type
                PolityType polityType = MapManager.Instance.GetPolityTypeById(history.Stage.PolityTypeIdParent_L1);
                tmpPostItNote.GetComponent<PostItNote>().SetPolityType("LOC_TABLE_HIST_POLITIES_TYPE", polityType.Name);
                
                // Main polity
                tmpPostItNote.GetComponent<PostItNote>().SetPolity("LOC_TABLE_HIST_POLITIES", polityL1.Name);
                tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility(false);
                tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility2(false);
                tmpPostItNote.GetComponent<PostItNote>().SetParentVisibility3(false);
            }

            // Policy
            if (history.Stage.PolicyId != 0)
            {
                tmpPostItNote.GetComponent<PostItNote>().SetPolicyVisibility(true);
                Polity policy = MapManager.Instance.GetPolityById(history.Stage.PolicyId);
                tmpPostItNote.GetComponent<PostItNote>().SetPolicy("LOC_TABLE_HIST_POLITIES", policy.Name);
            }
            else
            {
                tmpPostItNote.GetComponent<PostItNote>().SetPolicyVisibility(false);
            }
            
        }
    }
    private Vector2 CalculateNewPositionPostItNote(Vector3 mousePos)
    {
        float horizontalHalfScreen = Screen.width / 2;
        float verticalHalfScreen = Screen.height / 2;

        float w = tmpPostItNote.GetComponent<RectTransform>().rect.width;
        float h = tmpPostItNote.GetComponent<RectTransform>().rect.height;

        float panelXPositionNew = mousePos.x < horizontalHalfScreen ? mousePos.x + w : mousePos.x - w;
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
        LocalizationManager.Instance.AddLocalizeString(simpleMessage);
    }

    public void UpdateModStatusMessage(IdentificatorMessage identificatorMessage)
    {
        LocalizationManager.Instance.AddLocalizeString(identificatorMessage);
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
            
            Dictionary<int, PolityType> politiesType = MapManager.Instance.GetPolitiesType();
            PolityType polityType = politiesType.Where(x => x.Key.Equals(labelId)).Select(x => x.Value).FirstOrDefault();
            polityTypeIdLabel.text = labelId.ToString();
            currentName = polityType.Name;
            table = "LOC_TABLE_HIST_POLITIES_TYPE";
            LocalizationManager.Instance.AddLocalizeString(polityTypeNameInput, table, currentName);

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

            MapManager.Instance.LoadPolitiesDictionaryFromDB();
            Dictionary<int, Polity> polities = MapManager.Instance.GetPolities();
            Polity polity = polities.Where(x => x.Key.Equals(labelId)).Select(x => x.Value).FirstOrDefault();
            polityIdLabel.text = labelId.ToString();            
            currentName = polity.Name;
            table = "LOC_TABLE_HIST_POLITIES";
            LocalizationManager.Instance.AddLocalizeString(polityNameInput, table, currentName);

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

            MapManager.Instance.LoadSettlementsDictionaryFromDB();
            Dictionary<int, Settlement> settlements = MapManager.Instance.GetSettlements();
            Settlement settlement = settlements.Where(x => x.Key.Equals(labelId)).Select(x => x.Value).FirstOrDefault();
            settlementIdLabel.text = labelId.ToString();
            currentName = settlement.Name;
            table = "LOC_TABLE_HIST_SETTLEMENTS";
            LocalizationManager.Instance.AddLocalizeString(settlementNameInput, table, currentName);

            // Focus on this field
            if (settlementNameInput.placeholder.GetComponent<TextMeshProUGUI>().text != settlementNameInput.name)
            {
                settlementNameInput.text = settlementNameInput.placeholder.GetComponent<TextMeshProUGUI>().text;
            }

            settlementRegionInput.text = settlement.RegionId.ToString();
            
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
            LocalizationManager.Instance.AddLocalizeString(message);
        }

        // Check flags
        bool fChkGlobalIsOk = true;
        bool fChkLocalIsOk = true;

        //  Global Checks for every input (Region input is a exception. For this case, the value 0 or empty is all regions) ...
        foreach (TMP_InputField input in inputs)
        {
            if (input.name!= GameConst.UI_EDITMENU_SETTLEMENT_REGION_INPUT && MessageHelper.IsFieldEmpty(input.text) == true)
            {
                LocalizationManager.Instance.AddLocalizeString(emptyNameMessage);
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
                        if (input.name != GameConst.UI_EDITMENU_SETTLEMENT_REGION_INPUT && LocalizationManager.Instance.KeyExist(loc_table, input.text))
                        {// Check duplicate key
                            LocalizationManager.Instance.AddLocalizeString(duplicatedNameMessage);
                            fChkLocalIsOk = false;
                        }
                        if (input.name != GameConst.UI_EDITMENU_SETTLEMENT_REGION_INPUT && LocalizationManager.Instance.ValueExist(loc_table, input.text, input.text))
                        {// Check duplicate locate value (current language)
                            LocalizationManager.Instance.AddLocalizeString(duplicatedNameMessage);
                            fChkLocalIsOk = false;
                        }
                    }

                    // If all correct, the data is inserted
                    if (fChkLocalIsOk)
                    {

                        // Insert new locale data (All languanges)
                        foreach (TMP_InputField input in inputs)
                        {
                            if (input.name != GameConst.UI_EDITMENU_SETTLEMENT_REGION_INPUT)
                            {
                                LocalizationManager.Instance.InsertNewEntry(loc_table, input.text, input.text);
                            }
                        }

                        string messageStatusName = "";
                        if (dataType == EditorDataType.PolityType)
                        {
                            // Insert new data
                            string polityTypeName = inputs.First(x => x.name.Equals(GameConst.UI_EDITMENU_POLITYTYPE_NAME_INPUT)).text;                            
                            MapSqlConnection.Instance.AddPolityType(polityTypeName);

                            // Status name
                            messageStatusName = polityTypeName;

                            // Reload dictionary
                            MapManager.Instance.LoadPolitiesTypeDictionaryFromDB();
                            FillScrollButton(polityTypeContent);
                        }
                        else if (dataType == EditorDataType.Polity)
                        {
                            // Insert new data
                            string polityName = inputs.First(x => x.name.Equals(GameConst.UI_EDITMENU_POLITY_NAME_INPUT)).text;
                            MapSqlConnection.Instance.AddPolity(polityName, check.isOn);

                            // Status name
                            messageStatusName = polityName;

                            // Reload dictionary
                            MapManager.Instance.LoadPolitiesDictionaryFromDB();
                            FillScrollButton(polityContent);
                        }
                        else if (dataType == EditorDataType.Settlement)
                        {
                            // Insert new data
                            string settlementName = inputs.First(x => x.name.Equals(GameConst.UI_EDITMENU_SETTLEMENT_NAME_INPUT)).text;
                            string settlementRegion = inputs.First(x => x.name.Equals(GameConst.UI_EDITMENU_SETTLEMENT_REGION_INPUT)).text;
                            settlementRegion = int.TryParse(settlementRegion, out int n) == true ? settlementRegion : "0";
                            MapSqlConnection.Instance.AddSettlement(settlementName, Int32.Parse(settlementRegion));

                            // Status name
                            messageStatusName = settlementName;

                            // Reload dictionary
                            MapManager.Instance.LoadSettlementsDictionaryFromDB();
                            FillScrollButton(settlementContent);
                        }

                        // Change status information
                        IdentificatorMessage message = Instantiate(identificatorMessage);
                        message.objectName = messageStatusName;
                        UpdateModStatusMessage(message);
                        // Reload form
                        int lastId = MapSqlConnection.Instance.GetLastIdAdded(dataType);
                        ButtonEventToFillInfo(dataType, lastId);
                    }                    
 
                }
                else
                { // Update new data
                    if (dataType == EditorDataType.PolityType)
                    {
                        string polityTypeLocaleId = MapManager.Instance.GetPolitiesTypeLocaleKeyById(Int32.Parse(idLabel));
                        string polityTypeName = inputs.First(x => x.name.Equals(GameConst.UI_EDITMENU_POLITYTYPE_NAME_INPUT)).text;
                        if (LocalizationManager.Instance.ValueExist(loc_table, polityTypeLocaleId, polityTypeName))
                        {// Check duplicate locate value (current language)
                            LocalizationManager.Instance.AddLocalizeString(duplicatedNameMessage);
                        }
                        else
                        {
                            // Update new current locale data
                            LocalizationManager.Instance.UpdateEntry(loc_table, polityTypeLocaleId, polityTypeName);
                            // Update displayed data
                            FillScrollButtonScript(polityTypeContent, polityTypeLocaleId);
                        }
                    }
                    else if (dataType == EditorDataType.Polity)
                    {
                        string polityLocaleId = MapManager.Instance.GetPolitiesLocaleKeyById(Int32.Parse(idLabel));
                        string polityName = inputs.First(x => x.name.Equals(GameConst.UI_EDITMENU_POLITY_NAME_INPUT)).text;
                        if (LocalizationManager.Instance.ValueExist(loc_table, polityLocaleId, polityName))
                        {// Check duplicate locate value (current language)
                            LocalizationManager.Instance.AddLocalizeString(duplicatedNameMessage);
                        }
                        else
                        {
                            // Update new current locale data                    
                            LocalizationManager.Instance.UpdateEntry(loc_table, polityLocaleId, polityName);
                            // Update not string fields
                            MapSqlConnection.Instance.UpdatePolity(Int32.Parse(idLabel), check.isOn);
                            // Update displayed data
                            FillScrollButtonScript(polityContent, polityLocaleId);
                            ButtonEventToFillInfo(dataType, Int32.Parse(idLabel));
                        }
                    }
                    else if (dataType == EditorDataType.Settlement)
                    {
                        string settlementLocaleId = MapManager.Instance.GetSettlementsLocaleKeyById(Int32.Parse(idLabel));
                        string settlementName = inputs.First(x => x.name.Equals(GameConst.UI_EDITMENU_SETTLEMENT_NAME_INPUT)).text;
                        if (LocalizationManager.Instance.ValueExist(loc_table, settlementLocaleId, settlementName))
                        {// Check duplicate locate value (current language)
                            LocalizationManager.Instance.AddLocalizeString(duplicatedNameMessage);
                        }
                        else
                        {
                            // Update new current locale data
                            string settlementRegion = inputs.First(x => x.name.Equals(GameConst.UI_EDITMENU_SETTLEMENT_REGION_INPUT)).text;
                            LocalizationManager.Instance.UpdateEntry(loc_table, settlementLocaleId, settlementName);
                            // Update not string fields
                            settlementRegion = int.TryParse(settlementRegion, out int n) == true ? settlementRegion : "0";
                            MapSqlConnection.Instance.UpdateSettlement(Int32.Parse(idLabel), Int32.Parse(settlementRegion));
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
            settlementRegionInput
        };
        SaveActionButtonEvent(EditorDataType.Settlement, settlementMessage, okMessages, settlementEmptyNameMessage, settlementDuplicatedNameMessage, "LOC_TABLE_HIST_SETTLEMENTS", inputs, settlementIdLabel.text);
    }

    // CLEAR EVENT
    private void ClearMessages(GameObject status, SimpleMessage okNameMessage)
    {
        // Main message texts
        status.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";

        // Fields message status
        LocalizationManager.Instance.AddLocalizeString(okNameMessage);
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

        ClearMessages(settlementStatus, settlementOkNameMessage);
        ClearMessages(settlementStatus, settlementOkRegionMessage);
    }

    // DELETE EVENT
    public void PolityTypeDeleteActionButtonEvent()
    {
        if (polityTypeIdLabel.text == "0")
        {
            LocalizationManager.Instance.AddLocalizeString(polityTypeNoRemoveMessage);
        }
        else if (MapManager.Instance.PolityTypeIsRelated(Int32.Parse(polityTypeIdLabel.text)))
        {
            LocalizationManager.Instance.AddLocalizeString(polityTypeRelatedDataMessage);
        }
        else
        {
            // Delete data
            MapSqlConnection.Instance.RemovePolityType(Int32.Parse(polityTypeIdLabel.text));

            // Delete locale data (All languanges)
            string polityTypeLocaleId = MapManager.Instance.GetPolitiesTypeLocaleKeyById(Int32.Parse(polityTypeIdLabel.text));
            LocalizationManager.Instance.DeleteEntry("LOC_TABLE_HIST_POLITIES_TYPE", polityTypeLocaleId);

            // Update displayed data
            // Reload dictionary
            MapManager.Instance.LoadPolitiesTypeDictionaryFromDB();
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
            LocalizationManager.Instance.AddLocalizeString(polityNoRemoveMessage);
        }
        else if (MapManager.Instance.PolityIsRelated(Int32.Parse(polityIdLabel.text)))
        {
            LocalizationManager.Instance.AddLocalizeString(polityRelatedDataMessage);
        }
        else
        {
            // Delete data
            MapSqlConnection.Instance.RemovePolity(Int32.Parse(polityIdLabel.text));

            // Delete locale data (All languanges)
            string polityLocaleId = MapManager.Instance.GetPolitiesLocaleKeyById(Int32.Parse(polityIdLabel.text));
            LocalizationManager.Instance.DeleteEntry("LOC_TABLE_HIST_POLITIES", polityLocaleId);

            // Update displayed data
            // Reload dictionary
            MapManager.Instance.LoadPolitiesDictionaryFromDB();
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
            LocalizationManager.Instance.AddLocalizeString(settlementNoRemoveMessage);
        }
        /*else if (MapManager.Instance.PolityIsRelated(Int32.Parse(polityIdLabel.text)))  ------ TO CHECK
        {
            LocalizationManager.Instance.AddLocalizeString(polityRelatedDataMessage);
        }*/
        else
        {
            // Delete data
            MapSqlConnection.Instance.RemoveSettlement(Int32.Parse(settlementIdLabel.text));

            // Delete locale data (All languanges)
            string settlementLocaleId = MapManager.Instance.GetSettlementsLocaleKeyById(Int32.Parse(settlementIdLabel.text));
            LocalizationManager.Instance.DeleteEntry("LOC_TABLE_HIST_SETTLEMENTS", settlementLocaleId);

            // Update displayed data
            // Reload dictionary
            MapManager.Instance.LoadSettlementsDictionaryFromDB();
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