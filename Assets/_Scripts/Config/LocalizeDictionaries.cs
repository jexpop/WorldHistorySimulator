using System.Collections.Generic;

public class LocalizeDictionaries
{
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
}
