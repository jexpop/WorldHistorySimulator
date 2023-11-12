using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using System.Linq;


[RequireComponent(typeof(TMP_Dropdown))]
[RequireComponent(typeof(LocalizeDropdown))]
public class EditorDropdown : MonoBehaviour
{

    public EditorDataType dataType;

    public TMP_Dropdown dropdown;

    public LocalizeDropdown localizeDropdown;

    private BidirectionalDictionary<int, int> optionsIds;


    private class OptionsOrdered
    {
        private int _key;
        private string _keyName;
        private string _localeName;

        public int Key { get { return _key; } }
        public string KeyName { get { return _keyName; } }
        public string LocaleName { get { return _localeName; } }

        public OptionsOrdered(int key, string keyName, string localeName)
        {
            this._key= key;
            this._keyName= keyName;
            this._localeName= localeName;
        }
    }


    private void GetOptions(bool optional)
    {

        // Clear old options
        dropdown.ClearOptions();

        optionsIds  = new BidirectionalDictionary<int, int>();

        int c = 0;

        // Optional=true, the first option has not value
        if (optional)
        {
            optionsIds.Add(c, 0);
            c = 1;
            AddLocalizedOption("LOC_TABLE_EDITOR_FLOATING", ParamUI.GENERIC_NOT_SELECT);
        }

        // Get new options
        if (dataType == EditorDataType.PolityType)
        {
            // List to order the options
            List<OptionsOrdered> polityTypeOptionsOrdered = new List<OptionsOrdered>();
            foreach (KeyValuePair<int, PolityType> pt in MapManager.Instance.GetPolitiesType())
            {
                OptionsOrdered optionOrdered = new OptionsOrdered(pt.Key, pt.Value.Name, GetLocalizedOption("LOC_TABLE_HIST_POLITIES_TYPE", pt.Value.Name));
                polityTypeOptionsOrdered.Add(optionOrdered);
            }

            // Order the list
            List<OptionsOrdered> polityTypeSortedList = polityTypeOptionsOrdered.OrderBy(o => o.LocaleName).ToList();

            // Adding options and localized
            foreach (OptionsOrdered option in polityTypeSortedList)
            {
                optionsIds.Add(c, option.Key);
                AddLocalizedOption("LOC_TABLE_HIST_POLITIES_TYPE", option.KeyName);
                c++;
            }
        }
        else if (dataType == EditorDataType.IndividualPolity)
        {
            // List to order the options
            List<OptionsOrdered> individualOptionsOrdered = new List<OptionsOrdered>();
            foreach (KeyValuePair<int, Polity> p in MapManager.Instance.GetPolities(MapSqlConnection.Instance.GetIndividualId()))
            {
                OptionsOrdered optionOrdered = new OptionsOrdered(p.Key, p.Value.Name, GetLocalizedOption("LOC_TABLE_HIST_POLITIES", p.Value.Name));
                individualOptionsOrdered.Add(optionOrdered);
            }

            // Order the list
            List<OptionsOrdered> individualSortedList = individualOptionsOrdered.OrderBy(o => o.LocaleName).ToList();

            // Adding options and localized
            foreach (OptionsOrdered option in individualSortedList)
            {
                optionsIds.Add(c, option.Key);
                AddLocalizedOption("LOC_TABLE_HIST_POLITIES", option.KeyName);
                c++;
            }
        }
        else if (dataType == EditorDataType.CollectivePolity)
        {
            // List to order the options
            List<OptionsOrdered> collectiveOptionsOrdered = new List<OptionsOrdered>();
            foreach (KeyValuePair<int, Polity> p in MapManager.Instance.GetPolities(MapSqlConnection.Instance.GetCollectiveId()))
            {
                OptionsOrdered optionOrdered = new OptionsOrdered(p.Key, p.Value.Name, GetLocalizedOption("LOC_TABLE_HIST_POLITIES", p.Value.Name));
                collectiveOptionsOrdered.Add(optionOrdered);
            }

            // Order the list
            List<OptionsOrdered> individualSortedList = collectiveOptionsOrdered.OrderBy(o => o.LocaleName).ToList();

            // Adding options and localized
            foreach (OptionsOrdered option in individualSortedList)
            {
                optionsIds.Add(c, option.Key);
                AddLocalizedOption("LOC_TABLE_HIST_POLITIES", option.KeyName);
                c++;
            }
        }
    }

    private void AddLocalizedOption(string table, string entry)
    {
        LocalizedString localizedString = new LocalizedString();
        localizedString.TableReference = LocalizeDictionaries.DIC_LOCATION_TABLES[table];
        localizedString.TableEntryReference = entry;
        localizeDropdown.SetOptionsList(localizedString);
    }

    private string GetLocalizedOption(string table, string entry)
    {
        LocalizedString localizedString = new LocalizedString();
        localizedString.TableReference = LocalizeDictionaries.DIC_LOCATION_TABLES[table];
        localizedString.TableEntryReference = entry;
        return localizedString.GetLocalizedString();
    }

    public void LoadOptions(bool optional)
    {
        GetOptions(optional);
        localizeDropdown.LoadOptions();
    }

    public BidirectionalDictionary<int,int> GetOptionsIds(bool optional) 
    {
        LoadOptions(optional);
        return optionsIds; 
    }

}
