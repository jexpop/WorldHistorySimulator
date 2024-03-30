using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;
 
public class LocalizeDropdown : MonoBehaviour
{
    [SerializeField] private List<LocalizedString> dropdownOptions;
    private TMP_Dropdown tmpDropdown;
    List<TMP_Dropdown.OptionData> tmpDropdownOptions;

    private void Awake()
    {
        tmpDropdownOptions = new List<TMP_Dropdown.OptionData>();
        LoadOptions();
    }

    public void LoadOptions()
    {
        for (int i = 0; i < dropdownOptions.Count; i++)
        {
            tmpDropdownOptions.Add(new TMP_Dropdown.OptionData(dropdownOptions[i].GetLocalizedString()));
        }
        if (!tmpDropdown) tmpDropdown = GetComponent<TMP_Dropdown>();
        tmpDropdown.options = tmpDropdownOptions;
    }

    public void SetOptionsList(LocalizedString localized)
    {
        dropdownOptions.Add(localized);
    }

    public void ClearOptionsList()
    {
        dropdownOptions.Clear();
    }


    Locale currentLocale;
    private void ChangedLocale(Locale newLocale)
    {
        if (currentLocale == newLocale) return;
        currentLocale = newLocale;
        List<TMP_Dropdown.OptionData> tmpDropdownOptions = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < dropdownOptions.Count; i++)
        {
            tmpDropdownOptions.Add(new TMP_Dropdown.OptionData(dropdownOptions[i].GetLocalizedString()));
        }
        tmpDropdown.options = tmpDropdownOptions;
    }

    private void Update()
    {
        LocalizationSettings.SelectedLocaleChanged += ChangedLocale;
    }
}