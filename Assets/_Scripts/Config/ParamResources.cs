public class ParamResources
{  

    /* Database constants */
    public static string DB_PATH = "/Sqlite/HistoricalData.db";
    public static string DB_IS_COLLECTIVE = "Collective Policy";
    public static string DB_IS_INDIVIDUAL = "Individual Policy";
    public static string DB_COLLECTIVE_FIELD = "ifnull(PolicyPolityId, L1_PolityParentId)";
    public static string[] DB_LAYERS_FIELD = new string[] { "L1_PolityParentId", "ifnull(L2_PolityParentId, L1_PolityParentId)", "ifnull(L3_PolityParentId, ifnull(L2_PolityParentId, L1_PolityParentId))", "ifnull(L4_PolityParentId, ifnull(L3_PolityParentId, ifnull(L2_PolityParentId, L1_PolityParentId)))" };
    public static int DB_REGIONS_COUNT = 1900;

    /* Assets */
    public static string ASSET_MSG_ED_STATUS_MOD_PT = "Assets/Messages/EditorStatusModPolityType.asset";

    /* Polity's symbols */
    public static string SYMBOLS_FOLDER = "/PolitySymbols/";

    /* Localization */
    public static string LOCALIZATION_PATH = "/Localization/";

}