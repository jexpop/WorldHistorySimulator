using System.Globalization;
using TMPro;
using UnityEngine;


public class PolitySymbolPanel : MonoBehaviour
{

    private int polityTypeId;

    public TextMeshProUGUI polityName;
    public TextMeshProUGUI polityTypeName;
    public TMP_InputField symbolId;


    public void SetSymbolInfo(int polityId, int polityTypeId)
    {
        this.polityTypeId = polityTypeId;
        SetPolity(polityId);
        SetPolityType(polityTypeId);
        SetId(polityId, polityTypeId);
    }

    private void SetPolity(int polityId)
    {
        polityName.text = MapController.Instance.GetPolityById(polityId).Name;
        LocalizationController.Instance.AddLocalizeString(polityName, "LOC_TABLE_HIST_POLITIES", polityName.text);
    }

    private void SetPolityType(int polityTypeId)
    {
        polityTypeName.text = MapController.Instance.GetPolityTypeById(polityTypeId).Name;
        LocalizationController.Instance.AddLocalizeString(polityTypeName, "LOC_TABLE_HIST_POLITIES_TYPE", polityTypeName.text);
    }

    private void SetId(int polityId, int polityTypeId)
    {
        string polity = MapController.Instance.GetPolityById(polityId).Name;
        string polityType = MapController.Instance.GetPolityTypeById(polityTypeId).Name;
        symbolId.text = Utilities.PascalStrings(polity + "_" + polityType);
    }

}