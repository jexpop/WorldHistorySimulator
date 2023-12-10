using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEditor;

public class LocalizationManager : Singleton<LocalizationManager>
{

    /*** Data validations ***/
    public bool KeyExist(string table, string key)
    {
        var tableCollection = LocalizationSettings.StringDatabase.GetTable(LocalizeDictionaries.DIC_LOCATION_TABLES[table], LocalizationSettings.SelectedLocale);
        var data = tableCollection.GetEntry(key);
        return data is not null;
    }
    public bool ValueExist(string table, string key, string value)
    {
        var tableCollection = LocalizationSettings.StringDatabase.GetTable(LocalizeDictionaries.DIC_LOCATION_TABLES[table], LocalizationSettings.SelectedLocale);
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
        stringComponent.StringReference.SetReference(LocalizeDictionaries.DIC_LOCATION_TABLES[table], key);
    }
    // Function overloading: Button information
    public void AddLocalizeString(GameObject button, string table, string key)
    {
        LocalizeStringEvent stringComponent = button.transform.GetChild(0).GetComponent<LocalizeStringEvent>();
        stringComponent.RefreshString();
        stringComponent.StringReference.SetReference(LocalizeDictionaries.DIC_LOCATION_TABLES[table], key);
    }
    // Function overloading: Input text
    public void AddLocalizeString(TMP_InputField input, string table, string key)
    {
        LocalizeStringEvent InputLocalizeComponent = input.placeholder.GetComponent<LocalizeStringEvent>();
        InputLocalizeComponent.RefreshString();
        InputLocalizeComponent.StringReference.SetReference(LocalizeDictionaries.DIC_LOCATION_TABLES[table], key);
    }
    // Function overloading: Simple status message
    public void AddLocalizeString(SimpleMessage message)
    {
        LocalizeStringEvent stringComponent = MessageHelper.FindSimpleMessageLocalize(message);
        stringComponent.StringReference.SetReference(LocalizeDictionaries.DIC_LOCATION_TABLES[message.locationTable.ToString()], LocalizeDictionaries.DIC_LOCATION_KEYS[message.locationKey.ToString()]);

    }
    // Function overloading: Identificator status message
    public void AddLocalizeString(IdentificatorMessage message)
    {
        LocalizeStringEvent[] stringComponent = MessageHelper.FindIdentificatorMessageLocalize(message);
        stringComponent[0].RefreshString();
        stringComponent[0].StringReference.SetReference(LocalizeDictionaries.DIC_LOCATION_TABLES[message.locationTable.ToString()], LocalizeDictionaries.DIC_LOCATION_KEYS[message.locationKey.ToString()]);
        stringComponent[1].RefreshString();
        stringComponent[1].StringReference.SetReference(LocalizeDictionaries.DIC_LOCATION_TABLES[message.idLocationTable.ToString()], message.objectName);
    }
    // Function overloading: Dropdown
    public void AddLocalizeString(LocalizeDropdown localizeDropdown, string table, string value)
    {
        LocalizedString localized = new LocalizedString();
        localized.TableReference = LocalizeDictionaries.DIC_LOCATION_TABLES[table];
        localized.TableEntryReference = value;
        localizeDropdown.SetOptionsList(localized);
    }
    /***                        ***/


    /*** Modify the Table Collection ***/
    public void InsertNewEntry(string table, string key, string value)
    {
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var tableCollection = LocalizationSettings.StringDatabase.GetTable(LocalizeDictionaries.DIC_LOCATION_TABLES[table], LocalizationSettings.AvailableLocales.Locales[i]);
            tableCollection.AddEntry(key, value);

            #if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(tableCollection);
            EditorUtility.SetDirty(tableCollection.SharedData);
            #endif
        }

    }    
    public void UpdateEntry(string table, string key, string newValue)
    {
        var tableCollection = LocalizationSettings.StringDatabase.GetTable(LocalizeDictionaries.DIC_LOCATION_TABLES[table], LocalizationSettings.SelectedLocale);
        tableCollection.AddEntry(key, newValue);

        #if UNITY_EDITOR
                AssetDatabase.SaveAssets();
                EditorUtility.SetDirty(tableCollection);
                EditorUtility.SetDirty(tableCollection.SharedData);
        #endif
    }
    public void DeleteEntry(string table, string key)
    {
        var tableCollection = LocalizationSettings.StringDatabase.GetTable(LocalizeDictionaries.DIC_LOCATION_TABLES[table], LocalizationSettings.SelectedLocale);
        tableCollection.SharedData.RemoveKey(key);

        #if UNITY_EDITOR
                AssetDatabase.SaveAssets();
                EditorUtility.SetDirty(tableCollection);
                EditorUtility.SetDirty(tableCollection.SharedData);
        #endif
    }
    /***                        ***/

}
