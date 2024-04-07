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

    
    private void GetOptions(bool optional, int regionFilter=0)
    {

        // Clear old options
        dropdown.ClearOptions();
        optionsIds  = new BidirectionalDictionary<int, int>();
        localizeDropdown.ClearOptionsList();

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
            foreach (KeyValuePair<int, PolityType> pt in MapController.Instance.GetPolitiesType())
            {
                OptionsOrdered optionOrdered = new OptionsOrdered(pt.Key, pt.Value.Name, GetLocalizedOption("LOC_TABLE_HIST_POLITIES_TYPE", pt.Value.Name));
                polityTypeOptionsOrdered.Add(optionOrdered);
            }

            // Order the list
            List<OptionsOrdered> polityTypeSortedList = polityTypeOptionsOrdered.OrderBy(o => o.LocaleName).ToList();

            // Adding options and localized
            for(int i = 0; i < polityTypeSortedList.Count;i++)
            {
                optionsIds.Add(c, polityTypeSortedList[i].Key);
                AddLocalizedOption("LOC_TABLE_HIST_POLITIES_TYPE", polityTypeSortedList[i].KeyName);
                c++;
            }
        }
        else if (dataType == EditorDataType.IndividualPolity)
        {
            // List to order the options
            List<OptionsOrdered> individualOptionsOrdered = new List<OptionsOrdered>();
            foreach (KeyValuePair<int, Polity> p in MapController.Instance.GetPolities(CsvConnection.Instance.GetIndividualId()))
            {
                OptionsOrdered optionOrdered = new OptionsOrdered(p.Key, p.Value.Name, GetLocalizedOption("LOC_TABLE_HIST_POLITIES", p.Value.Name));
                individualOptionsOrdered.Add(optionOrdered);
            }
            
            // Order the list
            List<OptionsOrdered> individualSortedList = individualOptionsOrdered.OrderBy(o => o.LocaleName).ToList();

            // Adding options and localized
            for(int i = 0;i< individualSortedList.Count;i++)
            {
                optionsIds.Add(c, individualSortedList[i].Key);
                AddLocalizedOption("LOC_TABLE_HIST_POLITIES", individualSortedList[i].KeyName);
                c++;
            }

        }
        else if (dataType == EditorDataType.CollectivePolity)
        {
            // List to order the options
            List<OptionsOrdered> collectiveOptionsOrdered = new List<OptionsOrdered>();
            foreach (KeyValuePair<int, Polity> p in MapController.Instance.GetPolities(CsvConnection.Instance.GetCollectiveId()))
            {
                OptionsOrdered optionOrdered = new OptionsOrdered(p.Key, p.Value.Name, GetLocalizedOption("LOC_TABLE_HIST_POLITIES", p.Value.Name));
                collectiveOptionsOrdered.Add(optionOrdered);
            }

            // Order the list
            List<OptionsOrdered> individualSortedList = collectiveOptionsOrdered.OrderBy(o => o.LocaleName).ToList();

            // Adding options and localized
            for(int i = 0; i< individualSortedList.Count; i++)
            {
                optionsIds.Add(c, individualSortedList[i].Key);
                AddLocalizedOption("LOC_TABLE_HIST_POLITIES", individualSortedList[i].KeyName);
                c++;
            }
        }
        if (dataType == EditorDataType.Settlement)
        {

            // List to order the options
            List<OptionsOrdered> settlementOptionsOrdered = new List<OptionsOrdered>();
            foreach (KeyValuePair<int, Settlement> s in MapController.Instance.GetSettlements())
            {
                if(s.Value.RegionId == regionFilter || s.Value.RegionId == 0)
                {
                    OptionsOrdered optionOrdered = new OptionsOrdered(s.Key, s.Value.Name, GetLocalizedOption("LOC_TABLE_HIST_SETTLEMENTS", s.Value.Name));
                    settlementOptionsOrdered.Add(optionOrdered);
                }
            }

            // Order the list
            List<OptionsOrdered> settlementSortedList = settlementOptionsOrdered.OrderBy(o => o.LocaleName).ToList();

            // Adding options and localized
            for (int i = 0; i < settlementSortedList.Count; i++)
            {
                optionsIds.Add(c, settlementSortedList[i].Key);
                AddLocalizedOption("LOC_TABLE_HIST_SETTLEMENTS", settlementSortedList[i].KeyName);
                c++;
            }
        }

    }

    private void AddLocalizedOption(string table, string entry)
    {
        LocalizedString localizedString = new LocalizedString();
        localizedString.TableReference = LocalizeParams.DIC_LOCATION_TABLES[table];
        localizedString.TableEntryReference = entry;
        localizeDropdown.SetOptionsList(localizedString);
    }

    private string GetLocalizedOption(string table, string entry)
    {
        LocalizedString localizedString = new LocalizedString();
        localizedString.TableReference = LocalizeParams.DIC_LOCATION_TABLES[table];
        localizedString.TableEntryReference = entry;
        return localizedString.GetLocalizedString();
    }

    public void LoadOptions(bool optional, int regionFilter=0)
    {
        GetOptions(optional, regionFilter);
        localizeDropdown.LoadOptions();
    }

    public BidirectionalDictionary<int,int> GetOptionsIds(bool optional) 
    {
        LoadOptions(optional);
        return optionsIds; 
    }
        
}
