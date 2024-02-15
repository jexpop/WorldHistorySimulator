using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class OnMouseName : MonoBehaviour
{

    public GameObject _canvas;
    public TextMeshProUGUI nameText;
    public RectTransform backgroundRectTransform;

    private GameObject textGroup;


    void Start()
    {
        _canvas.SetActive(false);

        textGroup=_canvas.transform.GetChild(0).gameObject;
        textGroup.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 5, this.transform.position.z);

    }

    private void OnMouseOver()
    {
        _canvas.SetActive(true);
    }

    private void OnMouseExit()
    {
        _canvas.SetActive(false);
    }

    public void SetTextName(string name)
    {        
        LocalizationController.Instance.AddLocalizeString(nameText, "LOC_TABLE_HIST_SETTLEMENTS", name);
        backgroundRectTransform.sizeDelta = new Vector2(nameText.text.Length * 3.5f, backgroundRectTransform.sizeDelta.y);
    }

}
