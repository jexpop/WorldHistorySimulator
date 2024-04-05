using UnityEngine;


[CreateAssetMenu(fileName = "SimpleMessage", menuName = "Custom Scriptable Objects/Messages/SimpleMessage")]
public class SimpleMessage : Message
{

    public LocationTables locationTable;
    public LocationKeys locationKey;

    public string messageText;

}
