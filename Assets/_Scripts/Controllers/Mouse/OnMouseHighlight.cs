using UnityEngine;

public class OnMouseHighlight : MonoBehaviour
{

    private void OnMouseOver()
    {
        this.transform.localScale = new Vector3(2, 2, 1);
    }

    private void OnMouseExit()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
    }

}
