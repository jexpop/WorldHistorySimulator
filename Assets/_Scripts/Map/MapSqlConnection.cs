using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using Aron.Weiler;


public class MapSqlConnection : Singleton<MapSqlConnection>
{

    string conn;
    string sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    IDataReader cursor;
    readonly string DATABASE_NAME = ParamResources.DB_PATH;


    /// <summary>
    /// Sqlite database path
    /// </summary>
    /// 
    private void ConnectionConfig()
    {
        string filepath = Application.streamingAssetsPath + DATABASE_NAME;
        conn = "URI=file:" + filepath;
        dbconn = new SqliteConnection(conn);
    }

    /// <summary>
    /// Open connection to the database
    /// </summary>
    private void ConnectionOpen()
    {
        ConnectionConfig();
        dbconn.Open();
    }

    /// <summary>
    /// Close connection with the database
    /// </summary>
    private void ConnectionClose()
    {
        dbconn.Close();
        dbconn.Dispose();

        dbcmd.Dispose();

        cursor.Close();
        cursor.Dispose();
    }

    /// <summary>
    /// Gets information from the map regions to build a dictionary of regions using RGB colors/Id as the key and the Region class as the value
    /// </summary>
    /// <param name="timeline">Filter of the dates</param>
    /// <returns>Regions Dictionary</returns>
    public MultiKeyDictionary<Vector3Int, int, Region> GetInfoRegions(int timeline, int option)
    {

        // Selection of the layer
        string layerOption = ParamResources.DB_LAYERS_FIELD[option]; 

        /*** Main information of the regions ***/
        ConnectionOpen();
        dbcmd = dbconn.CreateCommand();
        sqlQuery = "SELECT r.RegionId, th.SettlementId, r.Name, r.Red, r.Green, r.Blue, tt.TerrainTypeName, t.TerrainName, ifnull(p.PolityId ,0) FROM Region r INNER JOIN Terrain t ON t.TerrainId=r.TerrainId INNER JOIN TerrainType tt ON tt.TerrainTypeId=t.TerrainTypeId LEFT OUTER JOIN TerritoryHistory th ON th.RegionId=r.RegionId AND th.StartDate <= " + timeline + " AND th.EndDate >= " + timeline + " LEFT OUTER JOIN Polity p ON p.PolityId = " + layerOption;
        dbcmd.CommandText = sqlQuery;

        MultiKeyDictionary<Vector3Int, int, Region> regions = new MultiKeyDictionary<Vector3Int, int, Region>();
        List<int> regionIdList = new List<int>();
        using (cursor = dbcmd.ExecuteReader())
        {
            while (cursor.Read())
            {            
                // Keys
                int regionId = cursor.GetInt32(0);
                regionIdList.Add(regionId);
                Vector3Int regionKey = new Vector3Int(cursor.GetInt32(3), cursor.GetInt32(4), cursor.GetInt32(5));
                                
                int settlementId = cursor.IsDBNull(1) ? 0 : cursor.GetInt32(1);
                string regionName = cursor.GetString(2);
                string regionTerrainType = cursor.GetString(6); // Land, sea, ...
                string regionTerrain = cursor.GetString(7);
                int ownerId = cursor.GetInt32(8);

                regions.Add(
                                    regionKey,
                                    regionId,
                                    new Region(
                                                        regionName, 
                                                        regionTerrainType,
                                                        regionTerrain,
                                                        settlementId,
                                                        ownerId, 
                                                        null                                                
                                     )
                 ); ;

            }                
            ConnectionClose();
        }
        /***                        ***/

        /*** History information ***/
        ConnectionOpen();
        dbcmd = dbconn.CreateCommand();
        sqlQuery = "SELECT RegionId, " +
                                        "StageId, SettlementId, StartDate, EndDate, " +
                                        "L1_PolityParentId, L2_PolityParentId, L3_PolityParentId,  L4_PolityParentId, " +
                                        "L1_PolityTypeIdParent, L2_PolityTypeIdParent, L3_PolityTypeIdParent, L4_PolityTypeIdParent, " +
                                        "L1_PolityCapital, L2_PolityCapital, L3_PolityCapital, L4_PolityCapital, " +
                                        "PolicyPolityId, PolicyPolityTypeId, PolicyCapital FROM TerritoryHistory ORDER BY RegionId, StartDate";
        dbcmd.CommandText = sqlQuery;

        using (cursor = dbcmd.ExecuteReader())
        {
            int tempRegion = 0;
            List<HistoryRegionRelation> history = new List<HistoryRegionRelation>();
            while (cursor.Read())
            {

                // Key
                int regionId = cursor.GetInt32(0);
                if(tempRegion == 0) { tempRegion = regionId; }

                int stageId = cursor.GetInt32(1);
                int settlementId = cursor.GetInt32(2);

                // Stage of History                
                int startDate = cursor.GetInt32(3);
                int endDate = cursor.GetInt32(4);
                int polityParentId_L1 = cursor.IsDBNull(5) ? 0 : cursor.GetInt32(5);
                int polityParentId_L2 = cursor.IsDBNull(6) ? 0 : cursor.GetInt32(6);
                int polityParentId_L3 = cursor.IsDBNull(7) ? 0 : cursor.GetInt32(7);
                int polityParentId_L4 = cursor.IsDBNull(8) ? 0 : cursor.GetInt32(8);
                int polityTypeIdParent_L1 = cursor.IsDBNull(9) ? 0 : cursor.GetInt32(9);
                int polityTypeIdParent_L2 = cursor.IsDBNull(10) ? 0 : cursor.GetInt32(10);
                int polityTypeIdParent_L3 = cursor.IsDBNull(11) ? 0 : cursor.GetInt32(11);
                int polityTypeIdParent_L4 = cursor.IsDBNull(12) ? 0 : cursor.GetInt32(12);
                int capital_L1 = cursor.IsDBNull(13) ? 0 : cursor.GetInt32(13);
                int capital_L2 = cursor.IsDBNull(14) ? 0 : cursor.GetInt32(14);
                int capital_L3 = cursor.IsDBNull(15) ? 0 : cursor.GetInt32(15);
                int capital_L4 = cursor.IsDBNull(16) ? 0 : cursor.GetInt32(16); ;
                int policyPolityId = cursor.IsDBNull(17) ? 0 : cursor.GetInt32(17);
                int policyPolityTypeId = cursor.IsDBNull(18) ? 0 : cursor.GetInt32(18);
                int policyCapital = cursor.IsDBNull(19) ? 0 : cursor.GetInt32(19);
                HistoryStage stage = new HistoryStage(
                                    startDate, endDate,
                                    polityParentId_L1, polityParentId_L2, polityParentId_L3, polityParentId_L4,
                                    polityTypeIdParent_L1, polityTypeIdParent_L2, polityTypeIdParent_L3, polityTypeIdParent_L4,
                                    capital_L1, capital_L2, capital_L3, capital_L4,
                                    policyPolityId, policyPolityTypeId, policyCapital
                );

                // History of the region
                if (tempRegion != regionId)
                {
                    regions[tempRegion].History = history;
                    history = new List<HistoryRegionRelation>();                    
                    tempRegion = regionId;
                }
                HistoryRegionRelation historyRegion = new HistoryRegionRelation(stageId, settlementId, stage);
                history.Add(historyRegion);
            }
            regions[tempRegion].History = history;
            ConnectionClose();
        }
        /***                        ***/

        return regions;
    }
    public List<HistoryRegionRelation> GetHistoryByRegionId(int regionId)
    {
        ConnectionOpen();
        dbcmd = dbconn.CreateCommand();
        sqlQuery = "SELECT StageId, SettlementId, StartDate, EndDate, " +
                                                            "L1_PolityParentId, L2_PolityParentId, L3_PolityParentId,  L4_PolityParentId, " +
                                                            "L1_PolityTypeIdParent, L2_PolityTypeIdParent, L3_PolityTypeIdParent, L4_PolityTypeIdParent, " +
                                                            "L1_PolityCapital, L2_PolityCapital, L3_PolityCapital, L4_PolityCapital, " +
                                                            "PolicyPolityId, PolicyPolityTypeId, PolicyCapital FROM TerritoryHistory WHERE RegionId=" + regionId + " ORDER BY StartDate";
        dbcmd.CommandText = sqlQuery;
        List<HistoryRegionRelation> history = new List<HistoryRegionRelation>();
        using (cursor = dbcmd.ExecuteReader())
        {
            while (cursor.Read())
            {

                int stageId = cursor.GetInt32(0);
                int settlementId = cursor.GetInt32(1);

                // Stage of History                
                int startDate = cursor.GetInt32(2);
                int endDate = cursor.GetInt32(3);
                int polityParentId_L1 = cursor.IsDBNull(4) ? 0 : cursor.GetInt32(4);
                int polityParentId_L2 = cursor.IsDBNull(5) ? 0 : cursor.GetInt32(5);
                int polityParentId_L3 = cursor.IsDBNull(6) ? 0 : cursor.GetInt32(6);
                int polityParentId_L4 = cursor.IsDBNull(7) ? 0 : cursor.GetInt32(7);
                int polityTypeIdParent_L1 = cursor.IsDBNull(8) ? 0 : cursor.GetInt32(8);
                int polityTypeIdParent_L2 = cursor.IsDBNull(9) ? 0 : cursor.GetInt32(9);
                int polityTypeIdParent_L3 = cursor.IsDBNull(10) ? 0 : cursor.GetInt32(10);
                int polityTypeIdParent_L4 = cursor.IsDBNull(11) ? 0 : cursor.GetInt32(11);
                int capital_L1 = cursor.IsDBNull(12) ? 0 : cursor.GetInt32(12);
                int capital_L2 = cursor.IsDBNull(13) ? 0 : cursor.GetInt32(13);
                int capital_L3 = cursor.IsDBNull(14) ? 0 : cursor.GetInt32(14);
                int capital_L4 = cursor.IsDBNull(15) ? 0 : cursor.GetInt32(15);
                int policyPolityId = cursor.IsDBNull(16) ? 0 : cursor.GetInt32(16);
                int policyPolityTypeId = cursor.IsDBNull(17) ? 0 : cursor.GetInt32(17);
                int policyCapital = cursor.IsDBNull(18) ? 0 : cursor.GetInt32(18);
                HistoryStage stage = new HistoryStage(
                                    startDate, endDate, 
                                    polityParentId_L1, polityParentId_L2, polityParentId_L3, polityParentId_L4,  
                                    polityTypeIdParent_L1, polityTypeIdParent_L2, polityTypeIdParent_L3, polityTypeIdParent_L4,
                                    capital_L1, capital_L2, capital_L3, capital_L4,
                                    policyPolityId, policyPolityTypeId, policyCapital
                );

                // History of the region
                HistoryRegionRelation historyRegion = new HistoryRegionRelation(stageId, settlementId, stage);
                history.Add(historyRegion);

            }
            ConnectionClose();
        }
            return history;
     }

    /// <summary>
    /// Gets information from the polities type to build a dictionary
    /// </summary>
    /// <returns>Polities Type Dictionary</returns>
    public Dictionary<int, PolityType> GetInfoPolitiesType()
    {
        ConnectionOpen();
        dbcmd = dbconn.CreateCommand();
        sqlQuery = "SELECT PolityTypeId, PolityTypeName FROM PolityType ORDER BY PolityTypeName";
        dbcmd.CommandText = sqlQuery;

        Dictionary<int, PolityType> politiesType = new Dictionary<int, PolityType>();
        using (cursor = dbcmd.ExecuteReader())
        {
            while (cursor.Read())
            {

                int polityTypeKey = cursor.GetInt32(0);
                string polityTypeName = cursor.GetString(1);

                politiesType.Add(
                                    polityTypeKey,
                                    new PolityType(polityTypeName)
                 );

            }
            ConnectionClose();
        }

        return politiesType;
    }

    /// <summary>
    /// Gets information from the polity to build a dictionary of political frontiers
    /// </summary>
    /// <param name="time">Current time to filter the color shown</param>
    /// <param name="polityParentHierarchy">Hierarchy to filter the color shown</param>
    /// <returns>Polities Dictionary</returns>
    public Dictionary<int, Polity> GetInfoPolities(string time, int polityParentHierarchy)
    {
        string polityParentHierarchyExtra = polityParentHierarchy switch
        {
            2 => "or th.L2_PolityParentId=p.PolityId ",
            3 => "or th.L2_PolityParentId=p.PolityId or th.L3_PolityParentId=p.PolityId ",
            4 => "or th.L2_PolityParentId=p.PolityId or th.L3_PolityParentId=p.PolityId or th.L4_PolityParentId=p.PolityId ",
            _ => ""
        };

        ConnectionOpen();
        dbcmd = dbconn.CreateCommand();
        sqlQuery = "SELECT p.PolityId, p.PolityName, ifnull(p.RGB,'999.999.999'), " +
                                "CASE WHEN py.PolicyName = '"+ ParamResources.DB_IS_COLLECTIVE+ "' THEN TRUE ELSE FALSE END IsCollective, " +
                                "(select th.stageId from TerritoryHistory th where (th.L1_PolityParentId=p.PolityId " + polityParentHierarchyExtra + ") and th.StartDate<=" + time + " and th.EndDate>=" + time + " limit 1) VisibilityCheck " +
                                "FROM Polity p INNER JOIN Policy py ON py.PolicyId = p.PolicyId ORDER BY p.PolityName";
        dbcmd.CommandText = sqlQuery;
        
        Dictionary<int, Polity> polities = new Dictionary<int, Polity>();
        using (cursor = dbcmd.ExecuteReader())
        {
            while (cursor.Read())
            {

                int polityKey = cursor.GetInt32(0);
                string polityName = cursor.GetString(1);
                string polityColor = cursor.GetString(2);
                bool policy = cursor.GetBoolean(3);
                int colorVisibility = cursor.IsDBNull(4) ? 0 : cursor.GetInt32(4);

                polityColor = colorVisibility == 0 ? "255.255.255" : polityColor;

                polities.Add(
                                    polityKey,
                                    new Polity(polityName, polityColor, policy)
                 );

            }
            ConnectionClose();
        }

        return polities;
    }    

    /// <summary>
    /// Get the last ID inserted
    /// </summary>
    /// <param name="dataType">Data Collection Reference</param>
    /// <returns>Lastet ID</returns>
    public int GetLastIdAdded(EditorDataType dataType)
    {
        ConnectionOpen();
        dbcmd = dbconn.CreateCommand();
        if (dataType == EditorDataType.PolityType)
        {
            sqlQuery = "SELECT PolityTypeId FROM PolityType ORDER BY PolityTypeId DESC LIMIT 1";
        }
        else if (dataType == EditorDataType.Polity)
        {
            sqlQuery = "SELECT PolityId FROM Polity ORDER BY PolityId DESC LIMIT 1";
        }
        dbcmd.CommandText = sqlQuery;

        int id = 0;
        using (cursor = dbcmd.ExecuteReader())
        {
            while (cursor.Read())
            {
                id = cursor.GetInt32(0);
            }
            ConnectionClose();
        }

        return id;
    }

    /// <summary>
    /// Get ID for collective text
    /// </summary>
    /// <returns>Collective ID</returns>
    public int GetCollectiveId()
    {
        ConnectionOpen();
        dbcmd = dbconn.CreateCommand();
        sqlQuery = "SELECT PolicyId FROM Policy WHERE PolicyName='" + ParamResources.DB_IS_COLLECTIVE + "' ";
        dbcmd.CommandText = sqlQuery;

        int id = 0;
        using (cursor = dbcmd.ExecuteReader())
        {
            while (cursor.Read())
            {
                id = cursor.GetInt32(0);
            }
            ConnectionClose();
        }

        return id;
    }

    /// <summary>
    /// Get ID for individual text
    /// </summary>
    /// <returns>Individual ID</returns>
    public int GetIndividualId()
    {
        ConnectionOpen();
        dbcmd = dbconn.CreateCommand();
        sqlQuery = "SELECT PolicyId FROM Policy WHERE PolicyName='" + ParamResources.DB_IS_INDIVIDUAL + "' ";
        dbcmd.CommandText = sqlQuery;

        int id = 0;
        using (cursor = dbcmd.ExecuteReader())
        {
            while (cursor.Read())
            {
                id = cursor.GetInt32(0);
            }
            ConnectionClose();
        }

        return id;
    }

    /// <summary>
    /// Gets information from the settlement to build a dictionary of settlements
    /// </summary>
    /// <returns>Polities Dictionary</returns>
    public Dictionary<int, Settlement> GetInfoSettlements()
    {
        ConnectionOpen();
        dbcmd = dbconn.CreateCommand();
        sqlQuery = "SELECT SettlementId, SettlementName, RegionId FROM Settlement ORDER BY SettlementName";
        dbcmd.CommandText = sqlQuery;

        Dictionary<int, Settlement> settlements = new Dictionary<int, Settlement>();
        using (cursor = dbcmd.ExecuteReader())
        {
            while (cursor.Read())
            {

                int settlementKey = cursor.GetInt32(0);
                string settlementName = cursor.GetString(1);
                int regionId = cursor.GetInt32(2);

                settlements.Add(
                                    settlementKey,
                                    new Settlement(settlementName, regionId)
                 );

            }
            ConnectionClose();
        }

        return settlements;
    }

    /// <summary>
    /// Generate a queue of colors for the polity's colors
    /// </summary>
    /// <returns>Colors</returns>
    public Queue<Color32> GetColors()
    {
        ConnectionOpen();
        dbcmd = dbconn.CreateCommand();
        sqlQuery = "SELECT R, G, B FROM PolityColorsBaked";
        dbcmd.CommandText = sqlQuery;

        List<Color32> colors = new List<Color32>();
        using (cursor = dbcmd.ExecuteReader())
        {
            while (cursor.Read())
            {

                int r = cursor.GetInt32(0);
                int g = cursor.GetInt32(1);
                int b = cursor.GetInt32(2);

                Color32 color = new Color32((byte)r, (byte)g, (byte)b, 255);
                colors.Add(color);

            }
            ConnectionClose();
        }

        // Random items in the list
        Utilities.Shuffle(colors);

        return new Queue<Color32>(colors); ;
    }



    /************************************************
     * INSERT / UPDATE / DELETE OPERATIONS 
    /************************************************/
    private void SentSQLOperation(string sqlQuery)
    {
        ConnectionOpen();
        dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteScalar();
        ConnectionClose();
    }

    // Polity type
    public void AddPolityType(string polityTypeName)
    {
        string sInsert = "INSERT INTO PolityType VALUES(NULL, '" + polityTypeName + "')";
        SentSQLOperation(sInsert);
    }
    public void RemovePolityType(int polityTypeId)
    {
        string sDelete = "DELETE FROM PolityType WHERE PolityTypeId=" + polityTypeId;
        SentSQLOperation(sDelete);
    }

    // Polity
    public void AddPolity(string polityName, bool isCollective)
    {
        int policy = isCollective ? GetCollectiveId() : 1;
        string sInsert = "INSERT INTO Polity VALUES(NULL, '" + polityName + "', '000.000.000',  " + policy + ")";
        SentSQLOperation(sInsert);
    }
    public void UpdatePolity(int polityId, bool isCollective)
    {
        int policy = isCollective ? GetCollectiveId() : 1;
        string sUpdate = "UPDATE Polity SET PolicyId=" + policy + " WHERE PolityId=" + polityId;
        SentSQLOperation(sUpdate);
    }
    public void RemovePolity(int polityId)
    {
        string sDelete = "DELETE FROM Polity WHERE PolityId=" + polityId;
        SentSQLOperation(sDelete);
    }

    // Settlement
    public void AddSettlement(string settlementName, int regionId)
    {
        string sInsert = "INSERT INTO Settlement VALUES(NULL, '" + settlementName + "', " + regionId + ")";
        SentSQLOperation(sInsert);
    }
    public void UpdateSettlement(int settlementId, int regionId)
    {
        string sUpdate = "UPDATE Settlement SET RegionId=" + regionId + " WHERE SettlementId=" + settlementId;
        SentSQLOperation(sUpdate);
    }
    public void RemoveSettlement(int settlementId)
    {
        string sDelete = "DELETE FROM Settlement WHERE SettlementId=" + settlementId;
        SentSQLOperation(sDelete);
    }

    // The stage of the region
    public void AddStage(
                        int regionId, int settlementId, 
                        int startDate, int toDate, 
                        int polityParentId_L1, int polityParentId_L2, int polityParentId_L3, int polityParentId_L4,  
                        int polityTypeIdParent_L1, int polityTypeIdParent_L2, int polityTypeIdParent_L3, int polityTypeIdParent_L4, 
                        int polityCapital_L1, int polityCapital_L2, int polityCapital_L3, int polityCapital_L4,
                        int policyPolityId, int policyPolityTypeId, int policyCapital)
    {
        string qPolityParentId_L1 = polityParentId_L1 == 0 ? "NULL" : polityParentId_L1.ToString();
        string qPolityParentId_L2 = polityParentId_L2 == 0 ? "NULL" : polityParentId_L2.ToString();
        string qPolityParentId_L3 = polityParentId_L3 == 0 ? "NULL" : polityParentId_L3.ToString();
        string qPolityParentId_L4 = polityParentId_L4 == 0 ? "NULL" : polityParentId_L4.ToString();
        string qPolityTypeIdParent_L1 = polityTypeIdParent_L1 == 0 ? "NULL" : polityTypeIdParent_L1.ToString();
        string qPolityTypeIdParent_L2 = polityTypeIdParent_L2 == 0 ? "NULL" : polityTypeIdParent_L2.ToString();
        string qPolityTypeIdParent_L3 = polityTypeIdParent_L3 == 0 ? "NULL" : polityTypeIdParent_L3.ToString();
        string qPolityTypeIdParent_L4 = polityTypeIdParent_L4 == 0 ? "NULL" : polityTypeIdParent_L4.ToString();
        string qPolityCapital_L1 = polityCapital_L1 == 0 ? "NULL" : polityCapital_L1.ToString();
        string qPolityCapital_L2 = polityCapital_L2 == 0 ? "NULL" : polityCapital_L2.ToString();
        string qPolityCapital_L3 = polityCapital_L3 == 0 ? "NULL" : polityCapital_L3.ToString();
        string qPolityCapital_L4 = polityCapital_L4 == 0 ? "NULL" : polityCapital_L4.ToString();
        string qPolicyPolityId = policyPolityId == 0 ? "NULL" : policyPolityId.ToString();
        string qPolicyPolityTypeId = policyPolityTypeId == 0 ? "NULL" : policyPolityTypeId.ToString();
        string qPolicyCapital = policyCapital == 0 ? "NULL" : policyCapital.ToString();

        string sInsert = "INSERT INTO TerritoryHistory VALUES(NULL, " +
                                                                                regionId + ", " + settlementId + ", " +
                                                                                startDate + ", " + toDate + ", " +
                                                                                qPolityParentId_L1 + "," + qPolityParentId_L2 + "," + qPolityParentId_L3 + "," + qPolityParentId_L4 + "," +
                                                                                qPolityTypeIdParent_L1 + ", " + qPolityTypeIdParent_L2 + ", " + qPolityTypeIdParent_L3 + ", " + qPolityTypeIdParent_L4 + ", " +
                                                                                qPolityCapital_L1 + ", " + qPolityCapital_L2 + ", " + qPolityCapital_L3 + ", " + qPolityCapital_L4 + ", " +
                                                                                qPolicyPolityId + ", " + qPolicyPolityTypeId + "," + qPolicyCapital + ")";
        SentSQLOperation(sInsert);
    }
    public void UpdateStage(
                        int stageId, 
                        int startDate, int toDate, 
                        int polityParentId_L1, int polityParentId_L2, int polityParentId_L3, int polityParentId_L4, 
                        int polityTypeIdParent_L1, int polityTypeIdParent_L2, int polityTypeIdParent_L3, int polityTypeIdParent_L4,
                        int polityCapital_L1, int polityCapital_L2, int polityCapital_L3, int polityCapital_L4,
                        int policyPolityId, int policyPolityTypeId, int policyCapital
        )
    {
        string qPolityParentId_L1 = polityParentId_L1 == 0 ? "NULL" : polityParentId_L1.ToString();
        string qPolityParentId_L2 = polityParentId_L2 == 0 ? "NULL" : polityParentId_L2.ToString();
        string qPolityParentId_L3 = polityParentId_L3 == 0 ? "NULL" : polityParentId_L3.ToString();
        string qPolityParentId_L4 = polityParentId_L4 == 0 ? "NULL" : polityParentId_L4.ToString();
        string qPolityTypeIdParent_L1 = polityTypeIdParent_L1 == 0 ? "NULL" : polityTypeIdParent_L1.ToString();
        string qPolityTypeIdParent_L2 = polityTypeIdParent_L2 == 0 ? "NULL" : polityTypeIdParent_L2.ToString();
        string qPolityTypeIdParent_L3 = polityTypeIdParent_L3 == 0 ? "NULL" : polityTypeIdParent_L3.ToString();
        string qPolityTypeIdParent_L4 = polityTypeIdParent_L4 == 0 ? "NULL" : polityTypeIdParent_L4.ToString();
        string qPolityCapital_L1 = polityCapital_L1 == 0 ? "NULL" : polityCapital_L1.ToString();
        string qPolityCapital_L2 = polityCapital_L2 == 0 ? "NULL" : polityCapital_L2.ToString();
        string qPolityCapital_L3 = polityCapital_L3 == 0 ? "NULL" : polityCapital_L3.ToString();
        string qPolityCapital_L4 = polityCapital_L4 == 0 ? "NULL" : polityCapital_L4.ToString();
        string qPolicyPolityId = policyPolityId == 0 ? "NULL" : policyPolityId.ToString();
        string qPolicyPolityTypeId = policyPolityTypeId == 0 ? "NULL" : policyPolityTypeId.ToString();
        string qPolicyCapital = policyCapital == 0 ? "NULL" : policyCapital.ToString();

        string sUpdate = "UPDATE TerritoryHistory " +
                                    "SET StartDate=" + startDate + ", EndDate=" + toDate +
                                    ", L1_PolityParentId=" + qPolityParentId_L1 + ", L2_PolityParentId=" + qPolityParentId_L2 + ", L3_PolityParentId=" + qPolityParentId_L3 + ", L4_PolityParentId=" + qPolityParentId_L4 + 
                                    ", L1_PolityTypeIdParent=" + qPolityTypeIdParent_L1 + ", L2_PolityTypeIdParent=" + qPolityTypeIdParent_L2 + ", L3_PolityTypeIdParent=" + qPolityTypeIdParent_L3 + ", L4_PolityTypeIdParent=" + qPolityTypeIdParent_L4 +
                                    ", L1_PolityCapital=" + qPolityCapital_L1 + ", L2_PolityCapital=" + qPolityCapital_L2 + ", L3_PolityCapital=" + qPolityCapital_L3 + ", L4_PolityCapital=" + qPolityCapital_L4 +
                                    ", PolicyPolityId=" + qPolicyPolityId + ", PolicyPolityTypeId=" + qPolicyPolityTypeId + ", PolicyCapital=" + qPolicyCapital +  " " +
                                    "WHERE StageId=" + stageId;
        SentSQLOperation(sUpdate);
    }
    public void RemoveStage(int stageId)
    {
        string sDelete = "DELETE FROM TerritoryHistory WHERE StageId=" + stageId;
        SentSQLOperation(sDelete);
    }

}
