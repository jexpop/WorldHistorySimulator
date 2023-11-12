using UnityEngine;

public class MapBox : MonoBehaviour
{

    public GameObject mapPrefab;


    void Start()
    {
        GameObject currentMap=GameObject.Instantiate(mapPrefab);
        currentMap.transform.SetParent(transform,false);
    }

}
