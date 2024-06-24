using System.Collections.Generic;
using UnityEngine;


public class PlacementObjects : MonoBehaviour
{

    public List<GameObject> objectsPrefabs = new List<GameObject>();

    private Dictionary<string, GameObject> objectsDictionary = new Dictionary<string,GameObject>();


    private void Awake()
    {
        /// Dictionary of the tags
        foreach(GameObject obj in objectsPrefabs)
        {
            objectsDictionary[obj.tag] = obj;
        }
    }

    #region Remove Methods
    /// <summary>
    /// Remove the object with the same tag
    /// </summary>
    /// <param name="tag">tag of the object</param>
    private void RemoveObjects(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
    }
    public void RemoveMapObjects(string tag)
    {
        RemoveObjects(tag);
    }
    #endregion

    #region Put Methods
    /// <summary>
    /// Put a new object in the map
    /// </summary>
    /// <param name="tag">tag of the object</param>
    /// <param name="region">the object will be put in this region</param>
    /// <param name="texture">optional param to custom images</param>
    private void PutObjects(string tag, Region region)
    {
        GameObject instantiatedObject = Instantiate(objectsDictionary[tag]);
        instantiatedObject.transform.position = new Vector3(region.CoordinatesCenter.x, region.CoordinatesCenter.y, -1);
    }
    private void PutObjects(string tag, Vector2Int coordinates, string name, Texture2D texture = null)
    {
        GameObject instantiatedObject = Instantiate(objectsDictionary[tag]);
        instantiatedObject.transform.position = new Vector3(coordinates.x, coordinates.y, -1);
        instantiatedObject.GetComponentInChildren<OnMouseName>().SetTextName(name);

        if (texture != null)
        {
            Sprite symbolSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 200.0f);
            instantiatedObject.GetComponentInChildren<SpriteRenderer>().sprite = symbolSprite;
        }
    }
    public void PutMapObjects(string tag, Region region)
    {
        PutObjects(tag, region);
    }
    public void PutMapObjects(string tag, Vector2Int coordinates, string name)
    {        
        PutObjects(tag, coordinates, name);
    }
    public void PutMapObjectsCustomSprites(string tag, Vector2Int coordinates, Texture2D texture, string name)
    {
        PutObjects(tag, coordinates, name, texture);
    }
    #endregion

}
