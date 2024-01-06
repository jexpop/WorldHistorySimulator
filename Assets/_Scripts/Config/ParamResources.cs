using UnityEngine;

public class ParamResources
{  

    /* Database constants */
    public static string DB_PATH = "/Sqlite/HistoricalData.db";
    public static string DB_IS_COLLECTIVE = "Collective Policy";
    public static string DB_IS_INDIVIDUAL = "Individual Policy";
    public static string DB_COLLECTIVE_FIELD = "ifnull(PolicyPolityId, L1_PolityParentId)";
    public static string[] DB_LAYERS_FIELD = new string[] { "L1_PolityParentId", "ifnull(L2_PolityParentId, L1_PolityParentId)", "ifnull(L3_PolityParentId, ifnull(L2_PolityParentId, L1_PolityParentId))", "ifnull(L4_PolityParentId, ifnull(L3_PolityParentId, ifnull(L2_PolityParentId, L1_PolityParentId)))" };
    public static int DB_REGIONS_COUNT = 1900;

    /* Csv */
    public static string CSV_HISTORY_PATH = "/HistoricalData/";
    public static string CSV_HISTORY_TABLE_CHRONOLOGY = "TerritoryHistory.csv";
    public static string CSV_HISTORY_TABLE_COLORS = "PolityColorsBaked.csv";
    public static string CSV_HISTORY_TABLE_POLICIES = "Policy.csv";
    public static string CSV_HISTORY_TABLE_POLITIES = "Polity.csv";
    public static string CSV_HISTORY_TABLE_POLITIES_TYPE = "PolityType.csv";
    public static string CSV_HISTORY_TABLE_REGIONS = "Region.csv";
    public static string CSV_HISTORY_TABLE_SETTLEMENTS = "Settlement.csv";
    public static string CSV_HISTORY_TABLE_TERRAIN_TYPES = "TerrainType.csv";
    public static string CSV_HISTORY_TABLE_TERRAINS = "Terrain.csv";

    /* Assets */
    public static string ASSET_MSG_ED_STATUS_MOD_PT = "Assets/Messages/EditorStatusModPolityType.asset";

    /* Polity's symbols */
    public static string STREAMING_FOLDER = Application.streamingAssetsPath;
    public static string SYMBOLS_FOLDER = "/PolitySymbols/";

    /* Localization */
    public static string LOCALIZATION_PATH = "/Localization/";

}