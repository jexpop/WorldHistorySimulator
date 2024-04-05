using TMPro;
using UnityEngine;


public class OnMouseName : MouseController
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
        LocalizationSetTextName(nameText, name);
        backgroundRectTransform.sizeDelta = new Vector2(nameText.text.Length * 3.5f, backgroundRectTransform.sizeDelta.y);
    }

}
