using UnityEngine;


public class ConfirmDialog : MonoBehaviour
{
    
    public ActionButtons actionButton;
    public EditorDataType dataType;


    public void SetActionButton()
    {
        switch (actionButton)
        {
            case ActionButtons.SaveButton: SaveButtonAction();
                break;
            case ActionButtons.ClearButton: ClearButtonAction();
                break;
            case ActionButtons.DeleteButton: DeleteButtonAction();
                break;
        }
    }

    private void SaveButtonAction()
    {
        switch (dataType)
        {
            case EditorDataType.PolityType:
                EditorUICanvasManager.Instance.PolityTypeSaveActionButtonEvent();
                break;
            case EditorDataType.Polity:
                EditorUICanvasManager.Instance.PolitySaveActionButtonEvent();
                break;
            case EditorDataType.Settlement:
                EditorUICanvasManager.Instance.SettlementSaveActionButtonEvent();
                break;
        }
        transform.parent.gameObject.SetActive(false);
    }

    private void ClearButtonAction()
    {
        switch (dataType)
        {
            case EditorDataType.PolityType:
                EditorUICanvasManager.Instance.PolityTypeClearActionButtonEvent();
                break;
            case EditorDataType.Polity:
                EditorUICanvasManager.Instance.PolityClearActionButtonEvent();
                break;
            case EditorDataType.Settlement:
                EditorUICanvasManager.Instance.SettlementClearActionButtonEvent();
                break;
        }        
        transform.parent.gameObject.SetActive(false);
    }

    private void DeleteButtonAction()
    {
        switch (dataType)
        {
            case EditorDataType.PolityType:
                EditorUICanvasManager.Instance.PolityTypeDeleteActionButtonEvent();
                break;
            case EditorDataType.Polity:
                EditorUICanvasManager.Instance.PolityDeleteActionButtonEvent();
                break;
            case EditorDataType.Settlement:
                EditorUICanvasManager.Instance.SettlementDeleteActionButtonEvent();
                break;
        }
        transform.parent.gameObject.SetActive(false);
    }

}
