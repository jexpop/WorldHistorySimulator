using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class InputField : UIBehaviour
{
    private TMP_InputField _field;

    public GameObject label;

    public GameObject relatedDropdown;
    private EditorDropdown relatedEditorDropdown;


    protected override void Awake()
    {
        base.Awake();
        _field = GetComponent<TMP_InputField>();
        _field.onFocusSelectAll = true;

        if(relatedDropdown != null)
        {
            relatedEditorDropdown = relatedDropdown.GetComponent<EditorDropdown>();
        }
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

        if(relatedDropdown != null)
        {
            int fieldNumeric = 0;
            if(int.TryParse(_field.text, out fieldNumeric))
            {
                relatedEditorDropdown.LoadOptions(true, fieldNumeric);
            }
            else
            {
                relatedEditorDropdown.LoadOptions(true, _field.text);
            }
             
        }

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _field.Select();
    }

}