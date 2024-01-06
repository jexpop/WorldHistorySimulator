using UnityEngine;
using UnityEngine.UI;

public class TimeTravelbutton : MonoBehaviour
{

    [Header("Custom map")]
    public Toggle layerCheckRivers;


    public void OnTimeTravelEvent()
    {
        bool globalLayer = EditorUICanvasController.Instance.layerCheckCollective.isOn;
        int optionLayer = EditorUICanvasController.Instance.layersDropdown.value;
        float rivers= layerCheckRivers.isOn ? 1f : 0f;
        MapController.Instance.CreateRegions(optionLayer, true, rivers);
        MapController.Instance.ShowCapitalSymbols();
    }

}
