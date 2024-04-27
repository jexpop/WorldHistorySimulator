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

    public BidirectionalDictionary<int,int> GetOptionsIds(bool optional, int filter=0) 
    {
        LoadOptions(optional, filter);
        return optionsIds; 
    }
    public BidirectionalDictionary<int, int> GetOptionsIds(bool optional, string filter = " ")
    {
        LoadOptions(optional, filter);
        return optionsIds;
    }

    // Load Options
    public void LoadOptions(bool optional, int regionFilter)
    {
        GetOptions(optional, regionFilter);
        localizeDropdown.LoadOptions();
    }
    public void LoadOptions(bool optional, string stringFilter)
    {
        GetOptions(optional, stringFilter);
        localizeDropdown.LoadOptions();
    }

    // Get options
    private void ClearOldOptions()
    {
        dropdown.ClearOptions();
        optionsIds = new BidirectionalDictionary<int, int>();
        localizeDropdown.ClearOptionsList();
    }
    private int FirstOptionCheck(bool optional)
    {
        // Optional=true, the first option has not value
        int c = 0;
        if (optional)
        {
            optionsIds.Add(c, 0);
            c = 1;
            AddLocalizedOption("LOC_TABLE_EDITOR_FLOATING", ParamUI.GENERIC_NOT_SELECT);
        }
        return c;
    }
    private void GetOptions(bool optional, int regionFilter)
    {
        ClearOldOptions();
        int c = 0;
        c = FirstOptionCheck(optional);

        // Get new options
        if (dataType == EditorDataType.Settlement)
        {

            // List to order the options
            List<OptionsOrdered> settlementOptionsOrdered = new List<OptionsOrdered>();
            foreach (KeyValuePair<int, Settlement> s in MapController.Instance.GetSettlements())
            {
                if (s.Value.RegionId == regionFilter | s.Value.RegionId == 0)
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
    private void GetOptions(bool optional, string stringFilter)
    {
        ClearOldOptions();
        int c = 0;
        if(stringFilter == "") { c = FirstOptionCheck(optional); }        
        
        // Get new options
        if (dataType == EditorDataType.PolityType)
        {
            // List to order the options
            List<OptionsOrdered> polityTypeOptionsOrdered = new List<OptionsOrdered>();
            foreach (KeyValuePair<int, PolityType> pt in MapController.Instance.GetPolitiesType())
            {
                string localizedName = GetLocalizedOption("LOC_TABLE_HIST_POLITIES_TYPE", pt.Value.Name);
                string compareName=Utilities.RemoveDiacritics(localizedName).ToLower();
                if (compareName.IndexOf(stringFilter.ToLower()) > -1 | stringFilter==" ")
                {
                    OptionsOrdered optionOrdered = new OptionsOrdered(pt.Key, pt.Value.Name, localizedName);
                    polityTypeOptionsOrdered.Add(optionOrdered);
                }
            }

            // Order the list
            List<OptionsOrdered> polityTypeSortedList = polityTypeOptionsOrdered.OrderBy(o => o.LocaleName).ToList();

            // Adding options and localized
            for (int i = 0; i < polityTypeSortedList.Count; i++)
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
                string localizedName = GetLocalizedOption("LOC_TABLE_HIST_POLITIES", p.Value.Name);
                string compareName = Utilities.RemoveDiacritics(localizedName).ToLower();
                if (compareName.IndexOf(stringFilter.ToLower()) > -1 | stringFilter == " ")
                {
                    OptionsOrdered optionOrdered = new OptionsOrdered(p.Key, p.Value.Name, localizedName);
                    individualOptionsOrdered.Add(optionOrdered);
                }
            }

            // Order the list
            List<OptionsOrdered> individualSortedList = individualOptionsOrdered.OrderBy(o => o.LocaleName).ToList();

            // Adding options and localized
            for (int i = 0; i < individualSortedList.Count; i++)
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
                string localizedName = GetLocalizedOption("LOC_TABLE_HIST_POLITIES", p.Value.Name);
                string compareName = Utilities.RemoveDiacritics(localizedName).ToLower();
                if (compareName.IndexOf(stringFilter.ToLower()) > -1 | stringFilter == " ")
                {
                    OptionsOrdered optionOrdered = new OptionsOrdered(p.Key, p.Value.Name, localizedName);
                    collectiveOptionsOrdered.Add(optionOrdered);
                }   
            }

            // Order the list
            List<OptionsOrdered> individualSortedList = collectiveOptionsOrdered.OrderBy(o => o.LocaleName).ToList();

            // Adding options and localized
            for (int i = 0; i < individualSortedList.Count; i++)
            {
                optionsIds.Add(c, individualSortedList[i].Key);
                AddLocalizedOption("LOC_TABLE_HIST_POLITIES", individualSortedList[i].KeyName);
                c++;
            }
        }
    }

}
