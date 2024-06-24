using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PostItNote : MonoBehaviour
{

    public TextMeshProUGUI polityTypeText,
                                             polityText,
                                             parentText, parentText2, parentText3,
                                             policyText;

    public GameObject parentInfoBox, parentInfoBox2, parentInfoBox3, 
                                    policyInfoBox;

    public RawImage polityImage,
                         parentImage, parentImage2, parentImage3,
                         policyImage;

    #region Names
    public void SetPolityType(string table, string value)
    {        
        LocalizationController.Instance.AddLocalizeString(polityTypeText, table, value);
    }
    public void SetPolity(string table, string value)
    {
        LocalizationController.Instance.AddLocalizeString(polityText, table, value);
    }
    public void SetParent(string table, string value)
    {
        LocalizationController.Instance.AddLocalizeString(parentText, table, value);
    }
    public void SetParent2(string table, string value)
    {
        LocalizationController.Instance.AddLocalizeString(parentText2, table, value);
    }
    public void SetParent3(string table, string value)
    {
        LocalizationController.Instance.AddLocalizeString(parentText3, table, value);
    }
    public void SetPolicy(string table, string value)
    {
        LocalizationController.Instance.AddLocalizeString(policyText, table, value);
    }
    #endregion

    #region Visibility
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
    #endregion

    #region Set images with the symbols
    public void SetPolityImage(string symbolFilename)
    {
        // Get the symbol
        Texture2D tex = MapController.Instance.GetSymbolTexture(symbolFilename);

        // Transparence
        polityImage.color = tex != null ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

        //Assigns the texture
        polityImage.texture = tex;
    }
    public void SetParentImage(string symbolFilename)
    {
        // Get the symbol
        Texture2D tex = MapController.Instance.GetSymbolTexture(symbolFilename);

        // Transparence
        parentImage.color = tex != null ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

        //Assigns the texture
        parentImage.texture = tex;
    }
    public void SetParentImage2(string symbolFilename)
    {
        // Get the symbol
        Texture2D tex = MapController.Instance.GetSymbolTexture(symbolFilename);

        // Transparence
        parentImage2.color = tex != null ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

        //Assigns the texture
        parentImage2.texture = tex;
    }
    public void SetParentImage3(string symbolFilename)
    {
        // Get the symbol
        Texture2D tex = MapController.Instance.GetSymbolTexture(symbolFilename);

        // Transparence
        parentImage3.color = tex != null ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

        //Assigns the texture
        parentImage3.texture = tex;
    }
    public void SetPolicyImage(string symbolFilename)
    {
        // Get the symbol
        Texture2D tex = MapController.Instance.GetSymbolTexture(symbolFilename);

        // Transparence
        policyImage.color = tex != null ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

        //Assigns the texture
        policyImage.texture = tex;
    }
    #endregion

}
