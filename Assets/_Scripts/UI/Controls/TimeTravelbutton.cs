using UnityEngine;
using UnityEngine.UI;

public class TimeTravelbutton : MonoBehaviour
{

    [Header("Custom map")]
    public Toggle layerCheckRivers;


    public void OnTimeTravelEvent()
    {
        bool globalLayer = EditorUICanvasManager.Instance.layerCheckCollective.isOn;
        int optionLayer = EditorUICanvasManager.Instance.layersDropdown.value;
        float rivers= layerCheckRivers.isOn ? 1f : 0f;
        MapManager.Instance.CreateRegions(optionLayer, true, rivers);
        MapManager.Instance.ShowCapitalSymbols();
    }

}
