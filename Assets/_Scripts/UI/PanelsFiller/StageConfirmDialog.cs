using UnityEngine;


public class StageConfirmDialog : MonoBehaviour
{

    public ActionButtons actionButton;
    public EditorDataType dataType;

    public StageFloatingPanel stageFloatingPanel;


    public void SetActionButton()
    {
        switch (actionButton)
        {
            case ActionButtons.SaveButton:
                SaveButtonAction();
                break;
            case ActionButtons.DeleteButton:
                DeleteButtonAction();
                break;
        }
    }

    private void SaveButtonAction()
    {
        switch (dataType)
        {
            case EditorDataType.StagePanel:
                stageFloatingPanel.SaveActionButtonEvent();
                break;
        }
        transform.parent.gameObject.SetActive(false);
    }

    private void DeleteButtonAction()
    {
        switch (dataType)
        {
            case EditorDataType.StagePanel:
                stageFloatingPanel.DeleteActionButtonEvent();
                break;
        }
        transform.parent.gameObject.SetActive(false);
    }

}
