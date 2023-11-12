using UnityEngine;

public class TimeTravelbutton : MonoBehaviour
{

    public void OnTimeTravelEvent()
    {
        bool globalLayer = EditorUICanvasManager.Instance.layerCheckCollective.isOn;
        int optionLayer = EditorUICanvasManager.Instance.layersDropdown.value;
        MapManager.Instance.CreateRegions(optionLayer, true);
    }

}
