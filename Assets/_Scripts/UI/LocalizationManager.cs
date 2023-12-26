using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.AddressableAssets;
using System.Collections;
using UnityEngine.Localization.Tables;
using System.IO;


public class LocalizationManager : Singleton<LocalizationManager>
{

    UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<StringTable> politiesTypeTable_ca;
    UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<StringTable> politiesTypeTable_en;
    UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<StringTable> politiesTypeTable_es;
    UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<StringTable> politiesTable_ca;
    UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<StringTable> politiesTable_en;
    UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<StringTable> politiesTable_es;
    UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<StringTable> settlementsTable_ca;
    UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<StringTable> settlementsTable_en;
    UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<StringTable> settlementsTable_es;


    private IEnumerator Start()
    {

        // Load the table polities type
        politiesTypeTable_ca = LocalizationSettings.StringDatabase.GetTableAsync(LocalizeParams.DIC_LOCATION_TABLES["LOC_TABLE_HIST_POLITIES_TYPE"], LocalizationSettings.AvailableLocales.Locales[LocalizeParams.LocaleCatalan]);
        yield return politiesTypeTable_ca;
        politiesTypeTable_en = LocalizationSettings.StringDatabase.GetTableAsync(LocalizeParams.DIC_LOCATION_TABLES["LOC_TABLE_HIST_POLITIES_TYPE"], LocalizationSettings.AvailableLocales.Locales[LocalizeParams.LocaleEnglish]);
        yield return politiesTypeTable_en;
        politiesTypeTable_es = LocalizationSettings.StringDatabase.GetTableAsync(LocalizeParams.DIC_LOCATION_TABLES["LOC_TABLE_HIST_POLITIES_TYPE"], LocalizationSettings.AvailableLocales.Locales[LocalizeParams.LocaleSpanish]);
        yield return politiesTypeTable_es;

        // Load the table polities
        politiesTable_ca = LocalizationSettings.StringDatabase.GetTableAsync(LocalizeParams.DIC_LOCATION_TABLES["LOC_TABLE_HIST_POLITIES"], LocalizationSettings.AvailableLocales.Locales[LocalizeParams.LocaleCatalan]);
        yield return politiesTable_ca;
        politiesTable_en = LocalizationSettings.StringDatabase.GetTableAsync(LocalizeParams.DIC_LOCATION_TABLES["LOC_TABLE_HIST_POLITIES"], LocalizationSettings.AvailableLocales.Locales[LocalizeParams.LocaleEnglish]);
        yield return politiesTable_en;
        politiesTable_es = LocalizationSettings.StringDatabase.GetTableAsync(LocalizeParams.DIC_LOCATION_TABLES["LOC_TABLE_HIST_POLITIES"], LocalizationSettings.AvailableLocales.Locales[LocalizeParams.LocaleSpanish]);
        yield return politiesTable_es;

        // Load the table settlements
        settlementsTable_ca = LocalizationSettings.StringDatabase.GetTableAsync(LocalizeParams.DIC_LOCATION_TABLES["LOC_TABLE_HIST_SETTLEMENTS"], LocalizationSettings.AvailableLocales.Locales[LocalizeParams.LocaleCatalan]);
        yield return settlementsTable_ca;
        settlementsTable_en = LocalizationSettings.StringDatabase.GetTableAsync(LocalizeParams.DIC_LOCATION_TABLES["LOC_TABLE_HIST_SETTLEMENTS"], LocalizationSettings.AvailableLocales.Locales[LocalizeParams.LocaleEnglish]);
        yield return settlementsTable_en;
        settlementsTable_es = LocalizationSettings.StringDatabase.GetTableAsync(LocalizeParams.DIC_LOCATION_TABLES["LOC_TABLE_HIST_SETTLEMENTS"], LocalizationSettings.AvailableLocales.Locales[LocalizeParams.LocaleSpanish]);
        yield return settlementsTable_es;

        // Prevent the tables from being unloaded and the changes lost
        Addressables.ResourceManager.Acquire(politiesTypeTable_ca);
        Addressables.ResourceManager.Acquire(politiesTypeTable_en);
        Addressables.ResourceManager.Acquire(politiesTypeTable_es);
        Addressables.ResourceManager.Acquire(politiesTable_ca);
        Addressables.ResourceManager.Acquire(politiesTable_en);
        Addressables.ResourceManager.Acquire(politiesTable_es);
        Addressables.ResourceManager.Acquire(settlementsTable_ca);
        Addressables.ResourceManager.Acquire(settlementsTable_en);
        Addressables.ResourceManager.Acquire(settlementsTable_es);

#if UNITY_EDITOR
        Debug.Log("Localization deactivated in Editor");
#else
        ImportFromCSV();
#endif
    }

    /*** Data validations ***/
    public bool KeyExist(string table, string key)
    {
        var tableCollection = LocalizationSettings.StringDatabase.GetTable(LocalizeParams.DIC_LOCATION_TABLES[table], LocalizationSettings.SelectedLocale);
        var data = tableCollection.GetEntry(key);
        return data is not null;
    }
    public bool ValueExist(string table, string key, string value)
    {
        var tableCollection = LocalizationSettings.StringDatabase.GetTable(LocalizeParams.DIC_LOCATION_TABLES[table], LocalizationSettings.SelectedLocale);
        var keyChecked = tableCollection.GetEntry(key);
        bool valueExist = false;

        if(keyChecked != null) // When it is new data, it always saves the key/value
        {
            if (keyChecked.Value != value) // When it is modification, only check other keys
            {
                valueExist = tableCollection.ContainsValue(value);
            }
        }

        return valueExist;
    }
    /***                        ***/


    /*** Add Localize String to UI elements ***/
    // Function overloading: Button information
    public void AddLocalizeString(TextMeshProUGUI text, string table, string key)
    {
        LocalizeStringEvent stringComponent = text.transform.GetComponent<LocalizeStringEvent>();
        stringComponent.RefreshString();
        stringComponent.StringReference.SetReference(LocalizeParams.DIC_LOCATION_TABLES[table], key);
    }
    // Function overloading: Button information
    public void AddLocalizeString(GameObject button, string table, string key)
    {
        LocalizeStringEvent stringComponent = button.transform.GetChild(0).GetComponent<LocalizeStringEvent>();
        stringComponent.RefreshString();
        stringComponent.StringReference.SetReference(LocalizeParams.DIC_LOCATION_TABLES[table], key);
    }
    // Function overloading: Input text
    public void AddLocalizeString(TMP_InputField input, string table, string key)
    {
        LocalizeStringEvent InputLocalizeComponent = input.placeholder.GetComponent<LocalizeStringEvent>();
        InputLocalizeComponent.RefreshString();
        InputLocalizeComponent.StringReference.SetReference(LocalizeParams.DIC_LOCATION_TABLES[table], key);
    }
    // Function overloading: Simple status message
    public void AddLocalizeString(SimpleMessage message)
    {
        LocalizeStringEvent stringComponent = MessageHelper.FindSimpleMessageLocalize(message);
        stringComponent.StringReference.SetReference(LocalizeParams.DIC_LOCATION_TABLES[message.locationTable.ToString()], LocalizeParams.DIC_LOCATION_KEYS[message.locationKey.ToString()]);

    }
    // Function overloading: Identificator status message
    public void AddLocalizeString(IdentificatorMessage message)
    {
        LocalizeStringEvent[] stringComponent = MessageHelper.FindIdentificatorMessageLocalize(message);
        stringComponent[0].RefreshString();
        stringComponent[0].StringReference.SetReference(LocalizeParams.DIC_LOCATION_TABLES[message.locationTable.ToString()], LocalizeParams.DIC_LOCATION_KEYS[message.locationKey.ToString()]);
        stringComponent[1].RefreshString();
        stringComponent[1].StringReference.SetReference(LocalizeParams.DIC_LOCATION_TABLES[message.idLocationTable.ToString()], message.objectName);
    }
    // Function overloading: Dropdown
    public void AddLocalizeString(LocalizeDropdown localizeDropdown, string table, string value)
    {
        LocalizedString localized = new LocalizedString();
        localized.TableReference = LocalizeParams.DIC_LOCATION_TABLES[table];
        localized.TableEntryReference = value;
        localizeDropdown.SetOptionsList(localized);
    }
    /***                        ***/


    /*** Modify the Table Collection ***/
    public void InsertNewEntry(string table, string key, string value, int locale=-1)
    {
            int start = 0;
            int end = LocalizationSettings.AvailableLocales.Locales.Count;

            if (locale > -1)
            {
                start = locale;
                end = locale + 1;
            }

            for (int i = start; i < end; ++i)
            {

            if (table == "LOC_TABLE_HIST_POLITIES_TYPE")
            {
                if (i == 0) { politiesTypeTable_ca.Result.AddEntry(key, value); }
                else if (i == 1) { politiesTypeTable_en.Result.AddEntry(key, value); }
                else if (i == 2) { politiesTypeTable_es.Result.AddEntry(key, value); }
            }
            else if (table == "LOC_TABLE_HIST_POLITIES")
            {
                if (i == LocalizeParams.LocaleCatalan) { politiesTable_ca.Result.AddEntry(key, value); }
                else if (i == LocalizeParams.LocaleEnglish) { politiesTable_en.Result.AddEntry(key, value); }
                else if (i == LocalizeParams.LocaleSpanish) { politiesTable_es.Result.AddEntry(key, value); }
            }
            else if (table == "LOC_TABLE_HIST_SETTLEMENTS")
            {
                if (i == LocalizeParams.LocaleCatalan) { settlementsTable_ca.Result.AddEntry(key, value); }
                else if (i == LocalizeParams.LocaleEnglish) { settlementsTable_en.Result.AddEntry(key, value); }
                else if (i == LocalizeParams.LocaleSpanish) { settlementsTable_es.Result.AddEntry(key, value); }
            }

#if UNITY_EDITOR
            Debug.Log("Export CSV deactivated in Editor");
#else
            if (locale == -1)
            {
                ExpostToCSV(table, LocalizationSettings.AvailableLocales.Locales[i]);
            }
#endif
        }
    }    
    public void UpdateEntry(string table, string key, string newValue)
    {
           string currentLocale = LocalizationSettings.SelectedLocale.Identifier.ToString();

            if (table == "LOC_TABLE_HIST_POLITIES_TYPE")
            {
                if (currentLocale == LocalizeParams.IdentifierCatalan) { politiesTypeTable_ca.Result.AddEntry(key, newValue); }
                else if (currentLocale == LocalizeParams.IdentifierEnglish) { politiesTypeTable_en.Result.AddEntry(key, newValue); }
                else if (currentLocale == LocalizeParams.IdentifierSpanish) { politiesTypeTable_es.Result.AddEntry(key, newValue); }
            }
            else if (table == "LOC_TABLE_HIST_POLITIES")
            {
                if (currentLocale == LocalizeParams.IdentifierCatalan) { politiesTable_ca.Result.AddEntry(key, newValue); }
                else if (currentLocale == LocalizeParams.IdentifierEnglish) { politiesTable_en.Result.AddEntry(key, newValue); }
                else if (currentLocale == LocalizeParams.IdentifierSpanish) { politiesTable_es.Result.AddEntry(key, newValue); }
            }
            else if (table == "LOC_TABLE_HIST_SETTLEMENTS")
            {
                if (currentLocale == LocalizeParams.IdentifierCatalan) { settlementsTable_ca.Result.AddEntry(key, newValue); }
                else if (currentLocale == LocalizeParams.IdentifierEnglish) { settlementsTable_en.Result.AddEntry(key, newValue); }
                else if (currentLocale == LocalizeParams.IdentifierSpanish) { settlementsTable_es.Result.AddEntry(key, newValue); }
            }

#if UNITY_EDITOR
        Debug.Log("Export CSV deactivated in Editor");
#else
        ExpostToCSV(table, LocalizationSettings.SelectedLocale);
#endif
    }
    public void DeleteEntry(string table, string key)
    {
        if (table == "LOC_TABLE_HIST_POLITIES_TYPE") { politiesTypeTable_ca.Result.RemoveEntry(key); politiesTypeTable_en.Result.RemoveEntry(key); politiesTypeTable_es.Result.RemoveEntry(key); }
        else if (table == "LOC_TABLE_HIST_POLITIES") { politiesTable_ca.Result.RemoveEntry(key); politiesTable_en.Result.RemoveEntry(key); politiesTable_es.Result.RemoveEntry(key); }
        else if (table == "LOC_TABLE_HIST_SETTLEMENTS") { settlementsTable_ca.Result.RemoveEntry(key); settlementsTable_en.Result.RemoveEntry(key); settlementsTable_es.Result.RemoveEntry(key); }

#if UNITY_EDITOR
        Debug.Log("Export CSV deactivated in Editor");
#else
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            ExpostToCSV(table, LocalizationSettings.AvailableLocales.Locales[i]);
        }
#endif
    }
    /***                        ***/


    /*** CSV convertion methods ***/
    private void ExpostToCSV(string table, Locale locale)
    {
        string path = Application.streamingAssetsPath + ParamResources.LOCALIZATION_PATH + "/" + table + "_" + locale.Identifier + ".csv";
        var tableCollection = LocalizationSettings.StringDatabase.GetTable(LocalizeParams.DIC_LOCATION_TABLES[table], locale);

        using (StreamWriter writer = new StreamWriter(path, false)) 
        {
            foreach (var valuePair in tableCollection)
            {
                string line = valuePair.Value.Key.ToString() + ";" + valuePair.Value.Value.ToString();
                writer.WriteLine(line);
            }
        }      

    }
    private void ImportFromCSV()
    {
        // Clear tables
        politiesTypeTable_ca.Result.Clear();
        politiesTypeTable_en.Result.Clear();
        politiesTypeTable_es.Result.Clear();
        politiesTable_ca.Result.Clear();
        politiesTable_en.Result.Clear();
        politiesTable_es.Result.Clear();
        settlementsTable_ca.Result.Clear();
        settlementsTable_en.Result.Clear();
        settlementsTable_es.Result.Clear();

        // Tables
        string[] tables = {
                         "LOC_TABLE_HIST_POLITIES_TYPE",
                         "LOC_TABLE_HIST_POLITIES",
                         "LOC_TABLE_HIST_SETTLEMENTS"
                     };

        // Import localizations
        foreach(string table in tables)
        {
            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
            {
                string path = Application.streamingAssetsPath + ParamResources.LOCALIZATION_PATH + "/" + table + "_" + LocalizationSettings.AvailableLocales.Locales[i].Identifier + ".csv";
                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] fields = line.Split(';');
                        InsertNewEntry(table, fields[0], fields[1], i);
                    }
                }
            }
        }

    }
    /***                        ***/

}
