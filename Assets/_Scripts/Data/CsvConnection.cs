using System.Collections.Generic;
using UnityEngine;
using Aron.Weiler;
using System.IO;
using System.Linq;
using System.Globalization;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;


public class CsvConnection : Singleton<CsvConnection>
{

    /// <summary>
    /// Read a csv file by name to convert it in a table
    /// </summary>
    /// <param name="filename">name del csv file</param>
    /// <param name="subpath">subdirectory of the file</param>
    /// <param name="filename">is the first line header?</param>
    /// <returns></returns>
    private List<string[]> GetCsvTable(string filename, string subpath, bool isTitle)
    {
        string path = GameManager.Instance.STREAMING_FOLDER + subpath + "/" + filename;

        List<string[]> table = new List<string[]>();
        using (StreamReader reader = new StreamReader(path,System.Text.Encoding.UTF8))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (!isTitle)
                {
                    string[] fields = line.Split(';');
                    table.Add(fields);
                }
                else
                {
                    isTitle = false;
                }                
            }
        }

        return table;
    }

    private List<PolicyData> GetPolicyModel()
    {
        List<string[]> policyCsv = new List<string[]>();
        policyCsv = GetCsvTable(ParamResources.CSV_HISTORY_TABLE_POLICIES, ParamResources.CSV_HISTORY_PATH, true);

        List<PolicyData> policyTable = new List<PolicyData>();
        foreach (string[] row in policyCsv)
        {
            PolicyData policyData =
                new PolicyData(row[0], row[1]);
            policyTable.Add(policyData);
        }

        return policyTable;
    }

    private List<PolityData> GetPolityModel() // Revisar, puede que no se necesite
    {
        List<string[]> polityCsv = new List<string[]>();
        polityCsv = GetCsvTable(ParamResources.CSV_HISTORY_TABLE_POLITIES, ParamResources.CSV_HISTORY_PATH, true);

        List<PolityData> polityTable = new List<PolityData>();
        foreach (string[] row in polityCsv)
        {
            PolityData polityData =
                new PolityData(row[0], row[1], row[2], row[3]);
            polityTable.Add(polityData);
        }

        return polityTable;
    }

    private List<TerrainData> GetTerrainModel()
    {
        List<string[]> terrainCsv = new List<string[]>();
        terrainCsv = GetCsvTable(ParamResources.CSV_HISTORY_TABLE_TERRAINS, ParamResources.CSV_HISTORY_PATH, true);

        List<TerrainData> terrainTable = new List<TerrainData>();
        foreach (string[] row in terrainCsv)
        {
            TerrainData terrainData =
                new TerrainData(row[0], row[1], row[2]);
            terrainTable.Add(terrainData);
        }

        return terrainTable;
    }

    private List<TerrainTypeData> GetTerrainTypeModel()
    {
        List<string[]> terrainTypeCsv = new List<string[]>();
        terrainTypeCsv = GetCsvTable(ParamResources.CSV_HISTORY_TABLE_TERRAIN_TYPES, ParamResources.CSV_HISTORY_PATH, true);

        List<TerrainTypeData> terrainTypeTable = new List<TerrainTypeData>();
        foreach (string[] row in terrainTypeCsv)
        {
            TerrainTypeData terrainTypeData =
                new TerrainTypeData(row[0], row[1]);
            terrainTypeTable.Add(terrainTypeData);
        }

        return terrainTypeTable;
    }

    private List<TerritoryHistoryData> GetTerritoryHistoryModel()
    {
        List<string[]> chronologyCsv = new List<string[]>();
        chronologyCsv = GetCsvTable(ParamResources.CSV_HISTORY_TABLE_CHRONOLOGY, ParamResources.CSV_HISTORY_PATH, true);

        List<TerritoryHistoryData> chronology = new List<TerritoryHistoryData>();
        foreach (string[] row in chronologyCsv)
        {
            int startDate = int.Parse(row[3], NumberStyles.Integer, CultureInfo.InvariantCulture);
            int endDate = int.Parse(row[4], NumberStyles.Integer, CultureInfo.InvariantCulture);

            TerritoryHistoryData chronologyData =
                new TerritoryHistoryData(row[0], row[1], row[2], startDate, endDate, row[5], row[6], row[7], row[8], row[9], row[10], row[11], row[12], row[13], row[14], row[15], row[16], row[17], row[18], row[19], row[20]);

            chronology.Add(chronologyData);
        }

        return chronology;
    }

    /**************************************************************************************************************************************************************************/
    /******************************************************************************** METHODS PUBLICS ***********************************************************************/
    /**************************************************************************************************************************************************************************/

    public List<string[]> GetLanguageTable(string filename, string subpath, bool isTitle)
    {
        return GetCsvTable(filename, subpath, isTitle);
    }

    /// <summary>
    /// Gets information from the polities type to build a dictionary
    /// </summary>
    /// <returns>Polities Type Dictionary</returns>
    public Dictionary<int, PolityType> GetInfoPolitiesType()
    {

        List<string[]> politiesTypeTable = new List<string[]>();
        politiesTypeTable=GetCsvTable(ParamResources.CSV_HISTORY_TABLE_POLITIES_TYPE, ParamResources.CSV_HISTORY_PATH, true);

        Dictionary<int, PolityType> politiesType = new Dictionary<int, PolityType>();
        foreach (string[] row in politiesTypeTable)
        {
            int polityTypeKey = int.Parse(row[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
            string polityTypeName = row[1];

            politiesType.Add(
                                polityTypeKey,
                                new PolityType(polityTypeName)
             );
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

        // Convert time to numeric
        int intTime = int.Parse(time, NumberStyles.Integer, CultureInfo.InvariantCulture);

        // The hierarchy of the administrative layers
        string polityParentHierarchyExtra = polityParentHierarchy switch
        {
            2 => "| th.L2_PolityParentId=row[0] ",
            3 => "| th.L2_PolityParentId=row[0] | th.L3_PolityParentId=row[0] ",
            4 => "| th.L2_PolityParentId=row[0] | th.L3_PolityParentId=row[0] | th.L4_PolityParentId=row[0] ",
            _ => ""
        };

        // Polity table
        List<string[]> politiesTable = new List<string[]>();
        politiesTable = GetCsvTable(ParamResources.CSV_HISTORY_TABLE_POLITIES, ParamResources.CSV_HISTORY_PATH, true);

        // Territory History Table
        List<TerritoryHistoryData> chronologyTable = GetTerritoryHistoryModel();

        /// Policy table
        List<PolicyData> policyTable = GetPolicyModel();

        Dictionary<int, Polity> polities = new Dictionary<int, Polity>();
        foreach (string[] row in politiesTable)
        {
            int polityKey = int.Parse(row[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
            string polityName = row[1];

            string polityColor = string.IsNullOrWhiteSpace(row[2]) ? "999.999.999" : row[2];            
            string stageId = chronologyTable.Where(c => (c.L1_PolityParentId==row[0] + polityParentHierarchyExtra) & c.StartDate<= intTime & c.EndDate>=intTime ).Select(c => c.StageId).FirstOrDefault(); // Visibility Check
            polityColor = stageId == "" ? "255.255.255" : polityColor;

            string policy= policyTable.Where(p => p.PolicyId == row[3]).Select(p => p.PolicyName).FirstOrDefault();
            bool isCollective = policy == ParamResources.DB_IS_COLLECTIVE ? true : false;
     
                polities.Add(
                                    polityKey,
                                    new Polity(polityName, polityColor, isCollective)
                 );
             
        }

        return polities;
    }

    /// <summary>
    /// Gets information from the settlement to build a dictionary of settlements
    /// </summary>
    /// <returns>Polities Dictionary</returns>
    public Dictionary<int, Settlement> GetInfoSettlements()
    {
        List<string[]> settlementsTable = new List<string[]>();
        settlementsTable = GetCsvTable(ParamResources.CSV_HISTORY_TABLE_SETTLEMENTS, ParamResources.CSV_HISTORY_PATH, true);

        Dictionary<int, Settlement> settlements = new Dictionary<int, Settlement>();
        foreach (string[] row in settlementsTable)
        {
            int settlementKey = int.Parse(row[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
            string settlementName = row[1];
            int regionId = int.Parse(row[2], NumberStyles.Integer, CultureInfo.InvariantCulture);
            int pixelX = int.Parse(row[3], NumberStyles.Integer, CultureInfo.InvariantCulture);
            int pixelY = int.Parse(row[4], NumberStyles.Integer, CultureInfo.InvariantCulture);

            Vector2Int pixelCoordinates = new Vector2Int(pixelX, pixelY);

            settlements.Add(
                                settlementKey,
                                new Settlement(settlementName, regionId, pixelCoordinates)
             );
        }     

         return settlements;
    }


    /// <summary>
    /// Generate a queue of colors for the polity's colors
    /// </summary>
    /// <returns>Colors</returns>
    public Queue<Color32> GetColors()
    {
        List<string[]> colorsTable = new List<string[]>();
        colorsTable = GetCsvTable(ParamResources.CSV_HISTORY_TABLE_COLORS, ParamResources.CSV_HISTORY_PATH, true);

        List<Color32> colors = new List<Color32>();
        foreach (string[] row in colorsTable)
        {
            int r = int.Parse(row[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
            int g = int.Parse(row[1], NumberStyles.Integer, CultureInfo.InvariantCulture);
            int b = int.Parse(row[2], NumberStyles.Integer, CultureInfo.InvariantCulture);

            Color32 color = new Color32((byte)r, (byte)g, (byte)b, 255);
            colors.Add(color);
        }

         // Random items in the list
         Utilities.Shuffle(colors);

         return new Queue<Color32>(colors);
    }

    /// <summary>
    /// Gets information from the map regions to build a dictionary of regions using RGB colors/Id as the key and the Region class as the value
    /// </summary>
    /// <param name="timeline">Filter of the dates</param>
    /// <returns>Regions Dictionary</returns>
    public MultiKeyDictionary<Vector3Int, int, Region> GetInfoRegions(int timeline, int layerOption)
    {

        // Region table
        List<string[]> regionsTable = new List<string[]>();
        regionsTable = GetCsvTable(ParamResources.CSV_HISTORY_TABLE_REGIONS, ParamResources.CSV_HISTORY_PATH, true);

        // Territory History table
        List<TerritoryHistoryData> chronologyTable = GetTerritoryHistoryModel();

        // Terrain table
        List<TerrainData> terrainsTable = GetTerrainModel();

        // Terrain Type table
        List<TerrainTypeData> terrainTypesTable = GetTerrainTypeModel();

        MultiKeyDictionary<Vector3Int, int, Region> regions = new MultiKeyDictionary<Vector3Int, int, Region>();
        List<int> regionIdList = new List<int>();
        foreach (string[] row in regionsTable)
        {
            // Keys
            int regionId = int.Parse(row[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
            regionIdList.Add(regionId);
            int r = int.Parse(row[2], NumberStyles.Integer, CultureInfo.InvariantCulture);
            int g = int.Parse(row[3], NumberStyles.Integer, CultureInfo.InvariantCulture);
            int b = int.Parse(row[4], NumberStyles.Integer, CultureInfo.InvariantCulture);
            Vector3Int regionKey = new Vector3Int(r, g, b);

            // Region's name
            string regionName = row[1];

            // Selected terrain
            TerrainData terrain = terrainsTable.Where(t => t.TerrainId == row[5]).Select(t => t).FirstOrDefault();

            // The terrain type of this region (Land, sea, ...)
            string regionTerrainType = terrainTypesTable.Where(tt => tt.TerrainTypeId == terrain.TerrainTypeId).Select(tt => tt.TerrainTypeName).FirstOrDefault();

            // The terrain  of this region
            string regionTerrain = terrain.TerrainName;

            // Selected chronology
            TerritoryHistoryData chronology = chronologyTable.Where(c => c.RegionId == row[0] & c.StartDate <= timeline & c.EndDate >= timeline).Select(c => c).FirstOrDefault();

            // The settlement of this region
            int settlementId = 0;
            if (chronology != null) { settlementId = chronology.SettlementId == "" ? 0 : int.Parse(chronology.SettlementId, NumberStyles.Integer, CultureInfo.InvariantCulture); }

            // Owner
            int ownerId = 0;
            if (chronology != null)
            {
                ownerId = layerOption switch
                {
                    0 => string.IsNullOrWhiteSpace(chronology.L1_PolityParentId) ? 0 : int.Parse(chronology.L1_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture),
                    1 => string.IsNullOrWhiteSpace(chronology.L2_PolityParentId) ? (string.IsNullOrWhiteSpace(chronology.L1_PolityParentId) ? 0 : int.Parse(chronology.L1_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture)) : int.Parse(chronology.L2_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture),
                    2 => string.IsNullOrWhiteSpace(chronology.L3_PolityParentId) ? (string.IsNullOrWhiteSpace(chronology.L2_PolityParentId) ? (string.IsNullOrWhiteSpace(chronology.L1_PolityParentId) ? 0 : int.Parse(chronology.L1_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture)) : int.Parse(chronology.L2_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture)) : int.Parse(chronology.L3_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture),
                    3 => string.IsNullOrWhiteSpace(chronology.L4_PolityParentId) ? (string.IsNullOrWhiteSpace(chronology.L3_PolityParentId) ? (string.IsNullOrWhiteSpace(chronology.L2_PolityParentId) ? (string.IsNullOrWhiteSpace(chronology.L1_PolityParentId) ? 0 : int.Parse(chronology.L1_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture)) : int.Parse(chronology.L2_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture)) : int.Parse(chronology.L3_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture)) : int.Parse(chronology.L4_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture),
                    _ => 0
                };
            }


            //** Chronology of this region **//
           List<TerritoryHistoryData> chronologyFull = chronologyTable.Where(c_full => c_full.RegionId == row[0]).OrderBy(c_full => c_full.StartDate).Select(c_full => c_full).ToList();

            List<HistoryRegionRelation> history = new List<HistoryRegionRelation>();
            if (chronologyFull != null)
            {
                foreach (TerritoryHistoryData ch in chronologyFull)
                {
                    int stageId = int.Parse(ch.StageId, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int stageSettlementId = int.Parse(ch.SettlementId, NumberStyles.Integer, CultureInfo.InvariantCulture);

                    // Stage of History                
                    int startDate = ch.StartDate;
                    int endDate = ch.EndDate;
                    int polityParentId_L1 = string.IsNullOrWhiteSpace(ch.L1_PolityParentId) ? 0 : int.Parse(ch.L1_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int polityParentId_L2 = string.IsNullOrWhiteSpace(ch.L2_PolityParentId) ? 0 : int.Parse(ch.L2_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int polityParentId_L3 = string.IsNullOrWhiteSpace(ch.L3_PolityParentId) ? 0 : int.Parse(ch.L3_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int polityParentId_L4 = string.IsNullOrWhiteSpace(ch.L4_PolityParentId) ? 0 : int.Parse(ch.L4_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int polityTypeIdParent_L1 = string.IsNullOrWhiteSpace(ch.L1_PolityTypeIdParent) ? 0 : int.Parse(ch.L1_PolityTypeIdParent, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int polityTypeIdParent_L2 = string.IsNullOrWhiteSpace(ch.L2_PolityTypeIdParent) ? 0 : int.Parse(ch.L2_PolityTypeIdParent, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int polityTypeIdParent_L3 = string.IsNullOrWhiteSpace(ch.L3_PolityTypeIdParent) ? 0 : int.Parse(ch.L3_PolityTypeIdParent, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int polityTypeIdParent_L4 = string.IsNullOrWhiteSpace(ch.L4_PolityTypeIdParent) ? 0 : int.Parse(ch.L4_PolityTypeIdParent, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int capital_L1 = string.IsNullOrWhiteSpace(ch.L1_PolityCapital) ? 0 : int.Parse(ch.L1_PolityCapital, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int capital_L2 = string.IsNullOrWhiteSpace(ch.L2_PolityCapital) ? 0 : int.Parse(ch.L2_PolityCapital, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int capital_L3 = string.IsNullOrWhiteSpace(ch.L3_PolityCapital) ? 0 : int.Parse(ch.L3_PolityCapital, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int capital_L4 = string.IsNullOrWhiteSpace(ch.L4_PolityCapital) ? 0 : int.Parse(ch.L4_PolityCapital, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int policyPolityId = string.IsNullOrWhiteSpace(ch.PolicyPolityId) ? 0 : int.Parse(ch.PolicyPolityId, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int policyPolityTypeId = string.IsNullOrWhiteSpace(ch.PolicyPolityTypeId) ? 0 : int.Parse(ch.PolicyPolityTypeId, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int policyCapital = string.IsNullOrWhiteSpace(ch.PolicyCapital) ? 0 : int.Parse(ch.PolicyCapital, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    int isSymbolForDate= string.IsNullOrWhiteSpace(ch.IsSymbolForDate) ? 0 : int.Parse(ch.IsSymbolForDate, NumberStyles.Integer, CultureInfo.InvariantCulture);

                    HistoryStage stage = new HistoryStage(
                                        startDate, endDate,
                                        polityParentId_L1, polityParentId_L2, polityParentId_L3, polityParentId_L4,
                                        polityTypeIdParent_L1, polityTypeIdParent_L2, polityTypeIdParent_L3, polityTypeIdParent_L4,
                                        capital_L1, capital_L2, capital_L3, capital_L4,
                                        policyPolityId, policyPolityTypeId, policyCapital,
                                        isSymbolForDate
                    );
                    HistoryRegionRelation historyRegion = new HistoryRegionRelation(stageId, stageSettlementId, stage);
                    history.Add(historyRegion);
                }
            }
            

            /** REGION **/
            regions.Add(
                                regionKey,
                                regionId,
                                new Region(
                                                    regionName,
                                                    regionTerrainType,
                                                    regionTerrain,
                                                    settlementId,
                                                    ownerId,
                                                    history
                                 )
             ); ;            

            // List of identifier of the regions used
            MapController.Instance.regionsIdList.Add(regionId);
        }
            
        return regions;
    }
    public List<HistoryRegionRelation> GetHistoryByRegionId(int regionId)
    {
        // Territory History table
        List<TerritoryHistoryData> chronologyTable = GetTerritoryHistoryModel();

        //** Chronology of the region **//
        List<TerritoryHistoryData> chronologyRegion = chronologyTable.Where(c => c.RegionId == regionId.ToString()).OrderBy(c => c.StartDate).Select(c => c).ToList();

        List<HistoryRegionRelation> history = new List<HistoryRegionRelation>();
        if (chronologyRegion != null)
        {
            foreach (TerritoryHistoryData ch in chronologyRegion)
            {
                int stageId = int.Parse(ch.StageId, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int stageSettlementId = int.Parse(ch.SettlementId, NumberStyles.Integer, CultureInfo.InvariantCulture);

                // Stage of History                
                int startDate = ch.StartDate;
                int endDate = ch.EndDate;
                int polityParentId_L1 = string.IsNullOrWhiteSpace(ch.L1_PolityParentId) ? 0 : int.Parse(ch.L1_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int polityParentId_L2 = string.IsNullOrWhiteSpace(ch.L2_PolityParentId) ? 0 : int.Parse(ch.L2_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int polityParentId_L3 = string.IsNullOrWhiteSpace(ch.L3_PolityParentId) ? 0 : int.Parse(ch.L3_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int polityParentId_L4 = string.IsNullOrWhiteSpace(ch.L4_PolityParentId) ? 0 : int.Parse(ch.L4_PolityParentId, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int polityTypeIdParent_L1 = string.IsNullOrWhiteSpace(ch.L1_PolityTypeIdParent) ? 0 : int.Parse(ch.L1_PolityTypeIdParent, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int polityTypeIdParent_L2 = string.IsNullOrWhiteSpace(ch.L2_PolityTypeIdParent) ? 0 : int.Parse(ch.L2_PolityTypeIdParent, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int polityTypeIdParent_L3 = string.IsNullOrWhiteSpace(ch.L3_PolityTypeIdParent) ? 0 : int.Parse(ch.L3_PolityTypeIdParent, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int polityTypeIdParent_L4 = string.IsNullOrWhiteSpace(ch.L4_PolityTypeIdParent) ? 0 : int.Parse(ch.L4_PolityTypeIdParent, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int capital_L1 = string.IsNullOrWhiteSpace(ch.L1_PolityCapital) ? 0 : int.Parse(ch.L1_PolityCapital, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int capital_L2 = string.IsNullOrWhiteSpace(ch.L2_PolityCapital) ? 0 : int.Parse(ch.L2_PolityCapital, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int capital_L3 = string.IsNullOrWhiteSpace(ch.L3_PolityCapital) ? 0 : int.Parse(ch.L3_PolityCapital, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int capital_L4 = string.IsNullOrWhiteSpace(ch.L4_PolityCapital) ? 0 : int.Parse(ch.L4_PolityCapital, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int policyPolityId = string.IsNullOrWhiteSpace(ch.PolicyPolityId) ? 0 : int.Parse(ch.PolicyPolityId, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int policyPolityTypeId = string.IsNullOrWhiteSpace(ch.PolicyPolityTypeId) ? 0 : int.Parse(ch.PolicyPolityTypeId, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int policyCapital = string.IsNullOrWhiteSpace(ch.PolicyCapital) ? 0 : int.Parse(ch.PolicyCapital, NumberStyles.Integer, CultureInfo.InvariantCulture);
                int isSymbolForDate = string.IsNullOrWhiteSpace(ch.IsSymbolForDate) ? 0 : int.Parse(ch.IsSymbolForDate, NumberStyles.Integer, CultureInfo.InvariantCulture);

                HistoryStage stage = new HistoryStage(
                                    startDate, endDate,
                                    polityParentId_L1, polityParentId_L2, polityParentId_L3, polityParentId_L4,
                                    polityTypeIdParent_L1, polityTypeIdParent_L2, polityTypeIdParent_L3, polityTypeIdParent_L4,
                                    capital_L1, capital_L2, capital_L3, capital_L4,
                                    policyPolityId, policyPolityTypeId, policyCapital,
                                    isSymbolForDate
                );
            
                HistoryRegionRelation historyRegion = new HistoryRegionRelation(stageId, stageSettlementId, stage);
                history.Add(historyRegion);
            }
        }

        return history;
    }    

    /// <summary>
    /// Get the last ID inserted
    /// </summary>
    /// <param name="dataType">Data Collection Reference</param>
    /// <returns>Lastet ID</returns>
    public int GetLastIdAdded(EditorDataType dataType)
    {
        int lastId = 0;

        if (dataType == EditorDataType.PolityType)
        {
            List<string[]> politiesTypeTable = new List<string[]>();
            politiesTypeTable = GetCsvTable(ParamResources.CSV_HISTORY_TABLE_POLITIES_TYPE, ParamResources.CSV_HISTORY_PATH, true);
            lastId = int.Parse(politiesTypeTable.OrderByDescending( x => int.Parse(x[0], CultureInfo.InvariantCulture)).First()[0],CultureInfo.InvariantCulture);
        }
        else if (dataType == EditorDataType.Polity)
        {
            List<string[]> politiesTable = new List<string[]>();
            politiesTable = GetCsvTable(ParamResources.CSV_HISTORY_TABLE_POLITIES, ParamResources.CSV_HISTORY_PATH, true);
            lastId = int.Parse(politiesTable.OrderByDescending(x => int.Parse(x[0], CultureInfo.InvariantCulture)).First()[0], CultureInfo.InvariantCulture);
        }
        else if (dataType == EditorDataType.Settlement)
        {
            List<string[]> settlementsTable = new List<string[]>();
            settlementsTable = GetCsvTable(ParamResources.CSV_HISTORY_TABLE_SETTLEMENTS, ParamResources.CSV_HISTORY_PATH, true);
            lastId = int.Parse(settlementsTable.OrderByDescending(x => int.Parse(x[0], CultureInfo.InvariantCulture)).First()[0], CultureInfo.InvariantCulture);
        }
        else if (dataType == EditorDataType.StagePanel)
        {
            List<string[]> pchronologyTable = new List<string[]>();
            pchronologyTable = GetCsvTable(ParamResources.CSV_HISTORY_TABLE_CHRONOLOGY, ParamResources.CSV_HISTORY_PATH, true);
            lastId = int.Parse(pchronologyTable.OrderByDescending(x => int.Parse(x[0], CultureInfo.InvariantCulture)).First()[0], CultureInfo.InvariantCulture);
        }

        return lastId;
    }

    /// <summary>
    /// Get ID for collective text
    /// </summary>
    /// <returns>Collective ID</returns>
    public int GetCollectiveId()
    {
        List<PolicyData> policies = new List<PolicyData>();
        policies = GetPolicyModel();
        return int.Parse(policies.Where(p => p.PolicyName == ParamResources.DB_IS_COLLECTIVE).Select(p => p.PolicyId).FirstOrDefault(), CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Get ID for individual text
    /// </summary>
    /// <returns>Individual ID</returns>
    public int GetIndividualId()
    {
        List<PolicyData> policies = new List<PolicyData>();
        policies = GetPolicyModel();
        return int.Parse(policies.Where(p => p.PolicyName == ParamResources.DB_IS_INDIVIDUAL).Select(p => p.PolicyId).FirstOrDefault(), CultureInfo.InvariantCulture);
    }

    /************************************************
     * HELPER METHODS OPERATIONS 
    /************************************************/
    private void WriteSingleLine(string path, bool append, string line)
    {
        using (StreamWriter writer = new StreamWriter(path, append, System.Text.Encoding.UTF8))
        {
            writer.WriteLine(line);
        }
    }
    private int GetLineIndex(string path, string file, string id)
    {
        List<string[]> table = new List<string[]>();
        table = GetCsvTable(file, path, false);
        int i = 1, index = 0;
        foreach (string[] row in table)
        {
            if (row[0] == id)
            {
                index = i; break;
            }
            i++;
        }
        return index;
    }
    private void LineChanger(string newText, string fileName, int line_to_edit)
    {
        string[] arrLine = File.ReadAllLines(fileName);
        arrLine[line_to_edit - 1] = newText;
        File.WriteAllLines(fileName, arrLine);
    }
    public void ExportLocalization(string table, Locale locale, StringTable tableCollection)
    {
        string path = GameManager.Instance.STREAMING_FOLDER + ParamResources.LOCALIZATION_PATH + "/" + table + "_" + locale.Identifier + ".csv";

        using (StreamWriter writer = new StreamWriter(path, false, System.Text.Encoding.UTF8))
        {
            foreach (var valuePair in tableCollection)
            {
                string line = valuePair.Value.Key.ToString() + ";" + valuePair.Value.Value.ToString();
                writer.WriteLine(line);
            }
        }
    }

    /************************************************
     * ADD OPERATIONS 
    /************************************************/
    public void AddPolityType(string polityTypeName)
    {
        string path = GameManager.Instance.STREAMING_FOLDER + ParamResources.CSV_HISTORY_PATH + "/" + ParamResources.CSV_HISTORY_TABLE_POLITIES_TYPE;
        int lastId = GetLastIdAdded(EditorDataType.PolityType) + 1;
        string line = lastId.ToString() + ";" + polityTypeName;
        WriteSingleLine(path, true, line);
    }
    public void AddPolity(string polityName, bool isCollective)
    {
        string path = GameManager.Instance.STREAMING_FOLDER + ParamResources.CSV_HISTORY_PATH + "/" + ParamResources.CSV_HISTORY_TABLE_POLITIES;
        int policy = isCollective ? GetCollectiveId() : 1;
        int lastId = GetLastIdAdded(EditorDataType.Polity) + 1;
        string line = lastId.ToString() + ";" + polityName + ";" + "000.000.000" + ";" + policy.ToString();
        WriteSingleLine(path, true, line);
    }
    public void AddSettlement(string settlementName, int regionId, int x, int y)
    {
        string path = GameManager.Instance.STREAMING_FOLDER + ParamResources.CSV_HISTORY_PATH + "/" + ParamResources.CSV_HISTORY_TABLE_SETTLEMENTS;
        int lastId = GetLastIdAdded(EditorDataType.Settlement) + 1;
        string line = lastId.ToString() + ";" + settlementName + ";" + regionId.ToString() + ";" + x.ToString() + ";" + y.ToString();
        WriteSingleLine(path, true, line);
    }
    public void AddStage(
                        int regionId, int settlementId,
                        int startDate, int toDate,
                        int polityParentId_L1, int polityParentId_L2, int polityParentId_L3, int polityParentId_L4,
                        int polityTypeIdParent_L1, int polityTypeIdParent_L2, int polityTypeIdParent_L3, int polityTypeIdParent_L4,
                        int polityCapital_L1, int polityCapital_L2, int polityCapital_L3, int polityCapital_L4,
                        int policyPolityId, int policyPolityTypeId, int policyCapital, int isSymbolForDate)
    {
        string qPolityParentId_L1 = polityParentId_L1 == 0 ? "" : polityParentId_L1.ToString();
        string qPolityParentId_L2 = polityParentId_L2 == 0 ? "" : polityParentId_L2.ToString();
        string qPolityParentId_L3 = polityParentId_L3 == 0 ? "" : polityParentId_L3.ToString();
        string qPolityParentId_L4 = polityParentId_L4 == 0 ? "" : polityParentId_L4.ToString();
        string qPolityTypeIdParent_L1 = polityTypeIdParent_L1 == 0 ? "" : polityTypeIdParent_L1.ToString();
        string qPolityTypeIdParent_L2 = polityTypeIdParent_L2 == 0 ? "" : polityTypeIdParent_L2.ToString();
        string qPolityTypeIdParent_L3 = polityTypeIdParent_L3 == 0 ? "" : polityTypeIdParent_L3.ToString();
        string qPolityTypeIdParent_L4 = polityTypeIdParent_L4 == 0 ? "" : polityTypeIdParent_L4.ToString();
        string qPolityCapital_L1 = polityCapital_L1 == 0 ? "" : polityCapital_L1.ToString();
        string qPolityCapital_L2 = polityCapital_L2 == 0 ? "" : polityCapital_L2.ToString();
        string qPolityCapital_L3 = polityCapital_L3 == 0 ? "" : polityCapital_L3.ToString();
        string qPolityCapital_L4 = polityCapital_L4 == 0 ? "" : polityCapital_L4.ToString();
        string qPolicyPolityId = policyPolityId == 0 ? "" : policyPolityId.ToString();
        string qPolicyPolityTypeId = policyPolityTypeId == 0 ? "" : policyPolityTypeId.ToString();
        string qPolicyCapital = policyCapital == 0 ? "" : policyCapital.ToString();
        string qIsSymbolForDate = isSymbolForDate == 0 ? "0" : "1";

        string path = GameManager.Instance.STREAMING_FOLDER + ParamResources.CSV_HISTORY_PATH + "/" + ParamResources.CSV_HISTORY_TABLE_CHRONOLOGY;
        int lastId = GetLastIdAdded(EditorDataType.StagePanel) + 1;
        string line = lastId.ToString() + ";"
                            + regionId.ToString() + ";"
                            + settlementId.ToString() + ";"
                            + startDate.ToString() + ";"
                            + toDate.ToString() + ";"
                            + qPolityParentId_L1 + ";"
                            + qPolityParentId_L2 + ";"
                            + qPolityParentId_L3 + ";"
                            + qPolityParentId_L4 + ";"
                            + qPolityTypeIdParent_L1 + ";"
                            + qPolityTypeIdParent_L2 + ";"
                            + qPolityTypeIdParent_L3 + ";"
                            + qPolityTypeIdParent_L4 + ";"
                            + qPolityCapital_L1 + ";"
                            + qPolityCapital_L2 + ";"
                            + qPolityCapital_L3 + ";"
                            + qPolityCapital_L4 + ";"
                            + qPolicyPolityId + ";"
                            + qPolicyPolityTypeId + ";"
                            + qPolicyCapital + ";"
                            + qIsSymbolForDate;
        WriteSingleLine(path, true, line);
    }

    /************************************************
 * UPDATE OPERATIONS 
/************************************************/
    public void UpdatePolity(int polityId, string polityName, bool isCollective)
    {
        string path = GameManager.Instance.STREAMING_FOLDER + ParamResources.CSV_HISTORY_PATH + "/" + ParamResources.CSV_HISTORY_TABLE_POLITIES;
        int policy = isCollective ? GetCollectiveId() : 1;
        string newLine =polityId.ToString()+";"+polityName+";"+ "000.000.000"+";"+policy.ToString();
        int line = GetLineIndex(ParamResources.CSV_HISTORY_PATH, ParamResources.CSV_HISTORY_TABLE_POLITIES, polityId.ToString());
        LineChanger(newLine, path,line);
    }
    public void UpdateSettlement(int settlementId, string settlementName, int regionId, int settlementX, int settlementY)
    {
        string path = GameManager.Instance.STREAMING_FOLDER + ParamResources.CSV_HISTORY_PATH + "/" + ParamResources.CSV_HISTORY_TABLE_SETTLEMENTS;
        string newLine = settlementId.ToString() + ";" + settlementName + ";" + regionId.ToString() + ";" + settlementX.ToString() + ";" + settlementY.ToString();
        int line = GetLineIndex(ParamResources.CSV_HISTORY_PATH, ParamResources.CSV_HISTORY_TABLE_SETTLEMENTS, settlementId.ToString());
        LineChanger(newLine, path, line);
    }
    public void UpdateStage(
                        int stageId,
                        int startDate, int toDate,
                        int polityParentId_L1, int polityParentId_L2, int polityParentId_L3, int polityParentId_L4,
                        int polityTypeIdParent_L1, int polityTypeIdParent_L2, int polityTypeIdParent_L3, int polityTypeIdParent_L4,
                        int polityCapital_L1, int polityCapital_L2, int polityCapital_L3, int polityCapital_L4,
                        int policyPolityId, int policyPolityTypeId, int policyCapital,
                        int regionId, int settlementId, int isSymbolForDate
        )
    {
        string qPolityParentId_L1 = polityParentId_L1 == 0 ? "" : polityParentId_L1.ToString();
        string qPolityParentId_L2 = polityParentId_L2 == 0 ? "" : polityParentId_L2.ToString();
        string qPolityParentId_L3 = polityParentId_L3 == 0 ? "" : polityParentId_L3.ToString();
        string qPolityParentId_L4 = polityParentId_L4 == 0 ? "" : polityParentId_L4.ToString();
        string qPolityTypeIdParent_L1 = polityTypeIdParent_L1 == 0 ? "" : polityTypeIdParent_L1.ToString();
        string qPolityTypeIdParent_L2 = polityTypeIdParent_L2 == 0 ? "" : polityTypeIdParent_L2.ToString();
        string qPolityTypeIdParent_L3 = polityTypeIdParent_L3 == 0 ? "" : polityTypeIdParent_L3.ToString();
        string qPolityTypeIdParent_L4 = polityTypeIdParent_L4 == 0 ? "" : polityTypeIdParent_L4.ToString();
        string qPolityCapital_L1 = polityCapital_L1 == 0 ? "" : polityCapital_L1.ToString();
        string qPolityCapital_L2 = polityCapital_L2 == 0 ? "" : polityCapital_L2.ToString();
        string qPolityCapital_L3 = polityCapital_L3 == 0 ? "" : polityCapital_L3.ToString();
        string qPolityCapital_L4 = polityCapital_L4 == 0 ? "" : polityCapital_L4.ToString();
        string qPolicyPolityId = policyPolityId == 0 ? "" : policyPolityId.ToString();
        string qPolicyPolityTypeId = policyPolityTypeId == 0 ? "" : policyPolityTypeId.ToString();
        string qPolicyCapital = policyCapital == 0 ? "" : policyCapital.ToString();
        string qIsSymbolForDate = isSymbolForDate == 0 ? "0" : "1";

        string path = GameManager.Instance.STREAMING_FOLDER + ParamResources.CSV_HISTORY_PATH + "/" + ParamResources.CSV_HISTORY_TABLE_CHRONOLOGY;
        string newLine = stageId.ToString() + ";"
                                    + regionId.ToString() + ";"
                                    + settlementId.ToString() + ";"
                                    + startDate.ToString() + ";"
                                    + toDate.ToString() + ";"
                                    + qPolityParentId_L1 + ";"
                                    + qPolityParentId_L2 + ";"
                                    + qPolityParentId_L3 + ";"
                                    + qPolityParentId_L4 + ";"
                                    + qPolityTypeIdParent_L1 + ";"
                                    + qPolityTypeIdParent_L2 + ";"
                                    + qPolityTypeIdParent_L3 + ";"
                                    + qPolityTypeIdParent_L4 + ";"
                                    + qPolityCapital_L1 + ";"
                                    + qPolityCapital_L2 + ";"
                                    + qPolityCapital_L3 + ";"
                                    + qPolityCapital_L4 + ";"
                                    + qPolicyPolityId + ";"
                                    + qPolicyPolityTypeId + ";"
                                    + qPolicyCapital + ";"
                                    + qIsSymbolForDate;
        int line = GetLineIndex(ParamResources.CSV_HISTORY_PATH, ParamResources.CSV_HISTORY_TABLE_CHRONOLOGY, stageId.ToString());
        LineChanger(newLine, path, line);
    }

    /************************************************
 * REMOVE OPERATIONS 
/************************************************/
    public void RemovePolityType(int polityTypeId)
    {
        string path = GameManager.Instance.STREAMING_FOLDER + ParamResources.CSV_HISTORY_PATH + "/" + ParamResources.CSV_HISTORY_TABLE_POLITIES_TYPE;
        string item = polityTypeId.ToString() + ";";
        var lines = File.ReadLines(path).Where(line => !line.StartsWith(item)).ToArray();
        File.WriteAllLines(path, lines);
    }
    public void RemovePolity(int polityId)
    {
        string path = GameManager.Instance.STREAMING_FOLDER + ParamResources.CSV_HISTORY_PATH + "/" + ParamResources.CSV_HISTORY_TABLE_POLITIES;
        string item = polityId.ToString() + ";";
        var lines = File.ReadLines(path).Where(line => !line.StartsWith(item)).ToArray();
        File.WriteAllLines(path, lines);
    }
    public void RemoveSettlement(int settlementId)
    {
        string path = GameManager.Instance.STREAMING_FOLDER + ParamResources.CSV_HISTORY_PATH + "/" + ParamResources.CSV_HISTORY_TABLE_SETTLEMENTS;
        string item = settlementId.ToString() + ";";
        var lines = File.ReadLines(path).Where(line => !line.StartsWith(item)).ToArray();
        File.WriteAllLines(path, lines);
    }
    public void RemoveStage(int stageId)
    {
        string path = GameManager.Instance.STREAMING_FOLDER + ParamResources.CSV_HISTORY_PATH + "/" + ParamResources.CSV_HISTORY_TABLE_CHRONOLOGY;
        string item = stageId.ToString() + ";";
        var lines = File.ReadLines(path).Where(line => !line.StartsWith(item)).ToArray();
        File.WriteAllLines(path, lines);
    }

}
