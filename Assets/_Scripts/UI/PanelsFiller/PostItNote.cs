using UnityEngine;
using TMPro;


public class PostItNote : MonoBehaviour
{

    public TextMeshProUGUI polityTypeText,
                                             polityText,
                                             parentText, parentText2, parentText3,
                                             policyText;

    public GameObject parentInfoBox, parentInfoBox2, parentInfoBox3, 
                                    policyInfoBox;


    public void SetPolityType(string table, string value)
    {
        LocalizationManager.Instance.AddLocalizeString(polityTypeText, table, value);
    }

    public void SetPolity(string table, string value)
    {
        LocalizationManager.Instance.AddLocalizeString(polityText, table, value);
    }

    public void SetParent(string table, string value)
    {
        LocalizationManager.Instance.AddLocalizeString(parentText, table, value);
    }
    public void SetParent2(string table, string value)
    {
        LocalizationManager.Instance.AddLocalizeString(parentText2, table, value);
    }
    public void SetParent3(string table, string value)
    {
        LocalizationManager.Instance.AddLocalizeString(parentText3, table, value);
    }

    public void SetPolicy(string table, string value)
    {
        LocalizationManager.Instance.AddLocalizeString(policyText, table, value);
    }

    public void SetParentVisibility(bool show)
    {
        parentInfoBox.SetActive(show);
    }
    public void SetParentVisibility2(bool show)
    {
        parentInfoBox2.SetActive(show);
    }
    public void SetParentVisibility3(bool show)
    {
        parentInfoBox3.SetActive(show);
    }

    public void SetPolicyVisibility(bool show)
    {
        policyInfoBox.SetActive(show);
    }

}
