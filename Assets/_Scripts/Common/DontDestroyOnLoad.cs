using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

}
