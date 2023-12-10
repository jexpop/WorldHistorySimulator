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

    /// **** SET IMAGES WITH THE SYMBOLS ****///
    private Texture2D GetImage(string name, string typeName)
    {
        // Join words
        string filename = name + "_" + typeName;

        // Get image's path
        string imagePath = Application.streamingAssetsPath + ParamResources.SYMBOLS_FOLDER + filename + ".png";

        // Texture
        Texture2D tex = Utilities.GetTexture2D(Utilities.PascalStrings(imagePath));

        //Return the texture
        return tex;
    }
    public void SetPolityImage(string polityName, string polityTypeName)
    {
        // Get the symbol
        Texture2D tex = GetImage(polityName, polityTypeName);

        // Transparence
        polityImage.color = tex != null ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

        //Assigns the texture
        polityImage.texture = tex;
    }
    public void SetParentImage(string parentName, string parentTypeName)
    {
        // Get the symbol
        Texture2D tex = GetImage(parentName, parentTypeName);

        // Transparence
        parentImage.color = tex != null ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

        //Assigns the texture
        parentImage.texture = tex;
    }
    public void SetParentImage2(string parentName2, string parentTypeName2)
    {
        // Get the symbol
        Texture2D tex = GetImage(parentName2, parentTypeName2);

        // Transparence
        parentImage2.color = tex != null ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

        //Assigns the texture
        parentImage2.texture = tex;
    }
    public void SetParentImage3(string parentName3, string parentTypeName3)
    {
        // Get the symbol
        Texture2D tex = GetImage(parentName3, parentTypeName3);

        // Transparence
        parentImage3.color = tex != null ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

        //Assigns the texture
        parentImage3.texture = tex;
    }
    /// ****                             ****///

}
