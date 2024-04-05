using UnityEngine;


[CreateAssetMenu(fileName = "IdentificatorMessage", menuName = "Custom Scriptable Objects/Messages/IdentificatorMessage")]
public class IdentificatorMessage : Message
{

    [Header("Status data")]
    public LocationTables locationTable;
    public LocationKeys locationKey;
    public string statusText;

    [Header("ID data")]
    public LocationTables idLocationTable;
    public string objectName;
    public string idText;

}
