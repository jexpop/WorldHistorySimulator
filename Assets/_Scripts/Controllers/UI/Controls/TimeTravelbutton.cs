using UnityEngine;
using UnityEngine.UI;

public class TimeTravelbutton : MonoBehaviour
{
    public void OnTimeTravelEvent()
    {
        bool globalLayer = EditorUICanvasController.Instance.layerCheckCollective.isOn;
        int optionLayer = EditorUICanvasController.Instance.layersDropdown.value;
        float rivers= EditorUICanvasController.Instance.layerCheckRivers.isOn ? 1f : 0f;
        float sea = EditorUICanvasController.Instance.showSea.isOn ? 1f : 0f;
        MapController.Instance.CreateRegions(optionLayer, true, rivers, sea);
        MapController.Instance.ShowCapitalSymbols();
        MapController.Instance.ShowSettlementMarkers();

        //Save editor player preferences
        string currentTimeLine = GameManager.Instance.UI_GetCurrentTimeline(true).ToString();
        GameManager.Instance.playerEditorData.timeLineEra = currentTimeLine[..1] == "-" ? 1 : 0;
        string currentTimeLineAbs = currentTimeLine[..1] == "-" ? currentTimeLine.Substring(1, currentTimeLine.Length - 1).PadLeft(8, '0') : currentTimeLine.Substring(0, currentTimeLine.Length).PadLeft(8, '0');
        GameManager.Instance.playerEditorData.timeLineYear = currentTimeLineAbs.Substring(0, 4).TrimStart('0');
        GameManager.Instance.playerEditorData.timeLineMonth = currentTimeLineAbs.Substring(4, 2).TrimStart('0');
        GameManager.Instance.playerEditorData.timeLineDay = currentTimeLineAbs.Substring(6, 2).TrimStart('0');
        GameManager.Instance.SavePlayerEditorConfiguration();
    }

}
