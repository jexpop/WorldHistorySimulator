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
                EditorUICanvasController.Instance.PolityTypeSaveActionButtonEvent();
                break;
            case EditorDataType.Polity:
                EditorUICanvasController.Instance.PolitySaveActionButtonEvent();
                break;
            case EditorDataType.Settlement:
                EditorUICanvasController.Instance.SettlementSaveActionButtonEvent();
                break;
        }
        transform.parent.gameObject.SetActive(false);
    }

    private void ClearButtonAction()
    {
        switch (dataType)
        {
            case EditorDataType.PolityType:
                EditorUICanvasController.Instance.PolityTypeClearActionButtonEvent();
                break;
            case EditorDataType.Polity:
                EditorUICanvasController.Instance.PolityClearActionButtonEvent();
                break;
            case EditorDataType.Settlement:
                EditorUICanvasController.Instance.SettlementClearActionButtonEvent();
                break;
        }        
        transform.parent.gameObject.SetActive(false);
    }

    private void DeleteButtonAction()
    {
        switch (dataType)
        {
            case EditorDataType.PolityType:
                EditorUICanvasController.Instance.PolityTypeDeleteActionButtonEvent();
                break;
            case EditorDataType.Polity:
                EditorUICanvasController.Instance.PolityDeleteActionButtonEvent();
                break;
            case EditorDataType.Settlement:
                EditorUICanvasController.Instance.SettlementDeleteActionButtonEvent();
                break;
        }
        transform.parent.gameObject.SetActive(false);
    }

}
