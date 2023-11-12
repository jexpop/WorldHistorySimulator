using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using TMPro;

public class MessageHelper : MonoBehaviour
{

    /// <summary>
    /// Check if the text is empty
    /// </summary>
    /// <param name="text">text to check</param>
    /// <returns>result checked</returns>
    public static bool IsFieldEmpty(string text)
    {
        bool empty = false;

        if (text == "") { empty = true; }
        return empty;
    }

    /// <summary>
    /// Find localize component from message passed by parameter
    /// </summary>
    /// <param name="simpleMessage">message</param>
    /// <returns>localize</returns>
    public static LocalizeStringEvent FindSimpleMessageLocalize(SimpleMessage simpleMessage)
    {

        LocalizeStringEvent stringComponent = null;

        GameObject[] messages = GameObject.FindGameObjectsWithTag(GameConst.TAG_UI_MESSAGES);

        List<string> messageTexts = new List<string>();
        messageTexts.Add(simpleMessage.messageText);

        foreach (GameObject message in messages)
        {
            if (Utilities.CompareStringsMember(message.name, messageTexts))
            {
                // Info string
                if (message.name == simpleMessage.messageText)
                {
                    TextMeshProUGUI messageTMPro = message.GetComponent<TextMeshProUGUI>();
                    stringComponent = messageTMPro.transform.GetComponent<LocalizeStringEvent>();
                }
            }
        }

        return stringComponent;

    }

    /// <summary>
    /// Find localize component from message passed by parameter
    /// </summary>
    /// <param name="identificatorMessage">message</param>
    /// <returns>localize array</returns>
    public static LocalizeStringEvent[] FindIdentificatorMessageLocalize(IdentificatorMessage identificatorMessage)
    {

        LocalizeStringEvent stringInfoComponent = null;
        LocalizeStringEvent stringButtonComponent = null;
        LocalizeStringEvent[] strings = new LocalizeStringEvent[2];

        GameObject[] messages = GameObject.FindGameObjectsWithTag(GameConst.TAG_UI_MESSAGES);

        List<string> messageTexts = new List<string>();
        messageTexts.Add(identificatorMessage.statusText);
        messageTexts.Add(identificatorMessage.idText);

        foreach (GameObject message in messages)
        {
            if (Utilities.CompareStringsMember(message.name, messageTexts))
            {
                // Info string
                if (message.name == identificatorMessage.statusText)
                {                    
                    TextMeshProUGUI messageTMPro = message.GetComponent<TextMeshProUGUI>();
                    stringInfoComponent = messageTMPro.transform.GetComponent<LocalizeStringEvent>();
                }
                // Button name
                if (message.name == identificatorMessage.idText)
                {                    
                    TextMeshProUGUI messageTMPro = message.GetComponent<TextMeshProUGUI>();
                    stringButtonComponent = messageTMPro.transform.GetComponent<LocalizeStringEvent>();
                }
            }
        }
        strings[0] = stringInfoComponent;
        strings[1] = stringButtonComponent;

        return strings;

    }

}
