using System.Collections.Generic;
using UnityEngine;

public class GameConst
{

    /* Tag constants */
    public static string TAG_FLAG_MARKER = "FlagMarker";
    public static string TAG_UI_MESSAGES = "UIMessages";
    public static string TAG_UI_EDITOR_STAGES = "EditorStages";

    /* Map constants */
    public static string MAP_MOUSE_SCROLL = "Mouse ScrollWheel";
    public static int MAP_SIZE_WIDTH = 5632;
    public static int MAP_SIZE_HIGHT = 2048;

    // Raycast limit to avoid out-of-range (off-map) errors on MapShower
    public static float MAP_MARGIN_BOX_MIN = 2885.0f;
    public static float MAP_MARGIN_BOX_MAX = 11534192.0f;

    // Positions, minimum and maximum possible movement
    public static float MAP_PAN_SPEED = 10.0f;
    public static float MAP_X_MIN = -2615.0f, MAP_Y_MIN = -940.0f;
    public static float MAP_X_MAX = 2615.0f, MAP_Y_MAX = 940.0f;

    // Camera zoom speed, minimum and maximum
    public static float MAP_ZOOM_SPEED = 100.0f;
    public static float MAP_ZOOM_MIN = 20.0f;
    public static float MAP_ZOOM_MAX = 100.0f;

    // Default colors for regions
    public static Color32 MAP_REGION_COLOR_LAND = new Color32(170, 170, 170, 255);
    public static Color32 MAP_REGION_COLOR_SEA = new Color32(0, 151, 251, 255);
    public static Color32 MAP_REGION_COLOR_LAKE = new Color32(0, 151, 251, 255);  //new Color32(112, 204, 254, 255);


    /* UI constants */
    public static string UI_GENERIC_UNKNOWN = "Unknown";
    public static string UI_GENERIC_CLOSE = "Close";
    public static string UI_GENERIC_NOT_SELECT = "NoSelectOption";
    public static string UI_REGION_PANEL = "RegionPanel";
    public static string UI_REGION_NAME = "RegionNameValue";
    public static string UI_REGION_OWNER_LABEL = "RegionOwnerValue";
    public static string UI_REGION_OWNER_BUTTON = "RegionOwnerButton";
    public static string UI_REGION_HISTORY_BUTTON = "Stages of History";
    public static string UI_REGION_POLITIES_LIST = "PolityScrollView";    
    public static string UI_REGION_NAME_LAND = "land";
    public static string UI_REGION_NAME_LAKE = "lake";
    public static string UI_REGION_NAME_SEA = "sea";
    public static string UI_POLITY_NAME = "PolityNameLabel";
    public const string UI_EDITMENU_SCROLLBUTTONS_POLITY_TYPE = "PolityTypeContent";
    public const string UI_EDITMENU_SCROLLBUTTONS_POLITY = "PolityContent";
    public const string UI_EDITMENU_SCROLLBUTTONS_SETTLEMENT = "SettlementContent";
    public const string UI_EDITMENU_POLITYTYPE_NAME_INPUT = "PolityTypeNameInput";
    public const string UI_EDITMENU_POLITY_NAME_INPUT = "PolityNameInput";
    public const string UI_EDITMENU_SETTLEMENT_NAME_INPUT = "SettlementNameInput";
    public const string UI_EDITMENU_SETTLEMENT_REGION_INPUT = "SettlementRegionInput";    


    /* Localize dictionaries */
    public static Dictionary<string, string> DIC_LOCATION_TABLES = new()
    {
        {"LOC_TABLE_EDITOR_EDIT_PANEL", "Editor Edit Panel"},
        {"LOC_TABLE_EDITOR_FLOATING", "Editor Floating Panels"},
        {"LOC_TABLE_HIST_POLITIES_TYPE", "Historical Polities Type"},
        {"LOC_TABLE_HIST_POLITIES", "Historical Polities"},
        {"LOC_TABLE_HIST_SETTLEMENTS", "Historical Settlements"}        
    };
    public static Dictionary<string, string> DIC_LOCATION_KEYS = new()
    {        
        { "LOC_KEY_NEW_STATUS", "NewDataStatusPanel" },
        { "LOC_KEY_EDITING_STATUS", "EditDataStatusPanel"},
        { "LOC_KEY_OK_FIELD", "FieldIsOk" },
        { "LOC_KEY_EMPTY_FIELD", "FieldIsEmpty" },
        { "LOC_KEY_EMPTY_DATES", "DatesEmpty" },
        { "LOC_KEY_DUPLICATE_FIELD", "DuplicatedField" },
        { "LOC_KEY_DUPLICATE_DATES", "DuplicatedDates" },
        { "LOC_KEY_KO_RANGE_DATES", "ErrorRangeDates" },
        { "LOC_KEY_NO_DATA_REMOVE", "NoDataDelete" },
        { "LOC_KEY_DATA_RELATED", "DataRelated" }
    };
    

    /* Database constants */
    public static string DB_PATH = "/Sqlite/HistoricalData.db";
    public static string DB_IS_COLLECTIVE = "Collective Policy";
    public static string DB_IS_INDIVIDUAL = "Individual Policy";
    public static string DB_COLLECTIVE_FIELD = "ifnull(PolicyPolityId, L1_PolityParentId)";
    public static string[] DB_LAYERS_FIELD = new string[] { "L1_PolityParentId", "ifnull(L2_PolityParentId, L1_PolityParentId)", "ifnull(L3_PolityParentId, ifnull(L2_PolityParentId, L1_PolityParentId))", "ifnull(L4_PolityParentId, ifnull(L3_PolityParentId, ifnull(L2_PolityParentId, L1_PolityParentId)))" };
    public static int DB_REGIONS_COUNT = 1900;


    /* Assets */
    public static string ASSET_MSG_ED_STATUS_MOD_PT = "Assets/Messages/EditorStatusModPolityType.asset";


}
