using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;

public class InputField : UIBehaviour
{
    private TMP_InputField _field;

    public GameObject label;


    protected override void Awake()
    {
        base.Awake();
        _field = GetComponent<TMP_InputField>();
        _field.onFocusSelectAll = true;
    }

    protected override void Start()
    {
        // onValueChanged expects a `void(string)` so instead of adding an anonymous 
        // delegate you can never remove again rather make sure your method implements 
        // the correct signature and add it directly
        _field.onValueChanged.AddListener(textChangedEvent);
    }

    // We need this signature in order to directly add and remove this method as 
    // a listener for the onValueChanged event
    public void textChangedEvent(string userText)
    {
        if (userText == "") 
        {
            _field.placeholder.GetComponent<TextMeshProUGUI>().text = label.GetComponent<TextMeshProUGUI>().text; 
        }        
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _field.Select();
    }

}