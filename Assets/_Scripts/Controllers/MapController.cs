using System.Collections.Generic;
using UnityEngine;
using Aron.Weiler;
using System.Linq;
using System.IO;


public class MapController : Singleton<MapController>
{
    Material _material;

    // Map textures and arrays
    int width, height;
    Color32[] remapArr;
    Color32[] waterArr;
    Texture2D remapTex;
    Texture2D seaTex;
    Texture2D paletteTex;
    Texture2D mainTex;

    // Polity's symbols
    string symbolStreamingPath;
    List<SymbolTexture> symbolsTexture = new List<SymbolTexture>();

    // Data dictionaries
    Dictionary<int, PolityType> politiesType = new Dictionary<int, PolityType>();
    Dictionary<int, Polity> polities = new Dictionary<int, Polity>();
    Dictionary<int, Settlement> settlements = new Dictionary<int, Settlement>();
    MultiKeyDictionary<Vector3Int, int, Region> regions = new MultiKeyDictionary<Vector3Int, int, Region>();

    Color32 colorHighlight;

    private PlacementObjects placementObjects = new PlacementObjects();


    public List<int> regionsIdList = new List<int>();
    public Queue<Color32> polityColorList = new Queue<Color32>();


    void Start()
    {
        
        InitParams();

        // Get Csv data
        LoadPolitiesTypeDictionaryFromDB();
        LoadSettlementsDictionaryFromDB();

        // Building map
        CreateMap();
        CreateRegions(true);

        // Placement objects
        placementObjects = gameObject.GetComponentInParent<PlacementObjects>();
        ShowCapitalSymbols();
        ShowSettlementMarkers();

    }

    // Material, texture & properties inicialization
    private void InitParams()
    {
        /* Map textures init */
        _material = GetComponent<Renderer>().material;
        mainTex = _material.GetTexture("_RegionTex") as Texture2D;
        _material.SetFloat("_DrawRiver", 0f);


        /* Capital symbols preload */
        symbolStreamingPath = GameManager.Instance.STREAMING_FOLDER + ParamResources.SYMBOLS_FOLDER;
        CapitalSymbolLoad();

    }

    private void AddCapitalSymobl(string symbolPpath, string symbolName)
    {
        //Converts desired path into byte array
        byte[] pngBytes = File.ReadAllBytes(symbolPpath + symbolName);
        //Creates texture and loads byte array data to create image
        Texture2D symbolTex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        symbolTex.LoadImage(pngBytes);
        // Add image to symbols dictioanry
        SymbolTexture symbolTexture = new SymbolTexture(symbolName, symbolTex);
        symbolsTexture.Add(symbolTexture);
    }

    public void CapitalSymbolLoad()
    {        
        // Subdirectories
        string[] directories = Directory.GetDirectories(symbolStreamingPath);

        symbolsTexture.Clear();
        for (int i = 0; i < directories.Length; i++)
        {

            // Files in the subdirectory
            DirectoryInfo symbolsDir = new DirectoryInfo(directories[i] + "/");
            FileInfo[] symbolsInfo = symbolsDir.GetFiles("*.png");
            foreach (FileInfo symbolFilename in symbolsInfo)
            {
                AddCapitalSymobl(directories[i] + "/", symbolFilename.Name);
            }

        }
    }

    /// <summary>
    /// Black and white base on which the map is built
    /// </summary>
    void CreateMap()
    {
        var mainArr = mainTex.GetPixels32();
        waterArr = new Color32[mainArr.Length];

        width = mainTex.width;
        height = mainTex.height;

        var main2remap = new Dictionary<Color32, Color32>();
        remapArr = new Color32[mainArr.Length];
        //waterArr = new Color32[mainArr.Length];
        int idx = 0;
        for (int i = 0; i < mainArr.Length; i++)
        {
            var mainColor = mainArr[i];
            if (!main2remap.ContainsKey(mainColor))
            {
                var low = (byte)(idx % (ParamColor.COLOR32_MAX + 1));
                var high = (byte)(idx / (ParamColor.COLOR32_MAX + 1));
                main2remap[mainColor] = new Color32(low, high, 0, (byte)ParamColor.COLOR32_MAX);
                idx++;
            }
            var remapColor = main2remap[mainColor];
            remapArr[i] = remapColor;
        }
        
        var paletteArr = new Color32[256 * 256];
        for (int i = 0; i < paletteArr.Length; i++)
        {
            paletteArr[i] = ParamColor.COLOR_WHITE;
        }
        
        // Rempa texture update
        remapTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        remapTex.filterMode = FilterMode.Point;
        remapTex.SetPixels32(remapArr);
        remapTex.Apply(false);
        _material.SetTexture("_RemapTex", remapTex);

        // Water texture update
        seaTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        seaTex.filterMode = FilterMode.Point;

        // Palette texture update
        paletteTex = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        paletteTex.filterMode = FilterMode.Point;
        paletteTex.SetPixels32(paletteArr);
        paletteTex.Apply(false);
        _material.SetTexture("_PaletteTex", paletteTex);

    }

    /// <summary>
    /// Color replacement
    /// </summary>
    /// <param name="remapColor">Color to replace </param>
    /// <param name="showColor">Color to be painted</param>
    public void ChangeColor(Color32 remapColor, Color32 showColor)
    {
        int xp = remapColor[0];
        int yp = remapColor[1];
        paletteTex.SetPixel(xp, yp, showColor);
    }

    /// <summary>
    /// Returns a vector with the RGB colors of an XY position without taking into account the Alpha
    /// </summary>
    /// <param name="x">Color X position</param>
    /// <param name="y">Color Y position</param>
    /// <returns>RGB color</returns>
    public Vector3Int OnlyRGBColorByPosition(int x, int y)
    {
        Color color = mainTex.GetPixel(x, y);
        int redColor = (int)Mathf.Round(color.r * ParamColor.COLOR32_MAX);
        int greenColor = (int)Mathf.Round(color.g * ParamColor.COLOR32_MAX);
        int bluecolor = (int)Mathf.Round(color.b * ParamColor.COLOR32_MAX);
        return new Vector3Int(redColor, greenColor, bluecolor);
    }

    /// <summary>
    /// Returns a vector with the RGB colors of an RGBA color
    /// </summary>
    /// <param name="color">RGBA color</param>
    /// <returns>RGB color</returns>
    /*Vector3Int OnlyRGBColorByRGBA(Color color)
    {
        int redColor = (int)Mathf.Round(color.r * ParamColor.COLOR32_MAX);
        int greenColor = (int)Mathf.Round(color.g * ParamColor.COLOR32_MAX);
        int bluecolor = (int)Mathf.Round(color.b * ParamColor.COLOR32_MAX);
        return new Vector3Int(redColor, greenColor, bluecolor);
    }*/

    // Get pixel color by position from the map
    public Color32 GetRemapColorByPosition(int x, int y)
    {
        return remapArr[x + y * ParamMap.MAP_SIZE_WIDTH];
    }

    // Apply changes in the map
    public void ApplyPaletteTexture(bool updateMipmaps)
    {
        paletteTex.Apply(updateMipmaps);
    }


    ///********************************************************************************************************************************************///
    /// REGIONS METHODS 
    ///********************************************************************************************************************************************///
    
    /// <summary>
    /// Colorize the regions of the map
    /// </summary>
    /// <param name="singleRegion"></param>
    private void ColorizeRegions(Region singleRegion = null)
    {
        // Initial values to colorize all regions or single region
        int ix = 0, iy = 0;
        int ex = width, ey = height;
        if (singleRegion != null)
        {
            ix = singleRegion.XminCoordinates;
            iy = singleRegion.YminCoordinates;
            ex = singleRegion.XmaxCoordinates;
            ey = singleRegion.YmaxCoordinates;
        }

        // Fill color of regions
        for (int x = ix; x < ex; x++)
        {
            for (int y = iy; y < ey; y++)
            {
                bool mustBeColored = false;
                Color32 regionColor;
                if (singleRegion == null)
                {// If region is null, we want to colorize all regions
                    if(regions.ContainsKey(OnlyRGBColorByPosition(x, y)))
                    {
                        Region region = regions[OnlyRGBColorByPosition(x, y)];

                        // Minimum, maximum coordinates of the region
                         if (x <= region.XminCoordinates) { region.XminCoordinates = x; }
                         if (x >= region.XmaxCoordinates) { region.XmaxCoordinates = x; }
                         if (y <= region.YminCoordinates) { region.YminCoordinates = y; }
                         if (y >= region.YmaxCoordinates) { region.YmaxCoordinates = y; }

                        mustBeColored = true;
                        regionColor = region.Rgb32;

                        // Indicator water terrain white-water, black-land
                        if (region.Type == ParamUI.REGION_NAME_LAND)
                        {
                            waterArr[x + y * width] = ParamColor.COLOR_WHITE;
                        }
                        else
                        {
                            waterArr[x + y * width] =  ParamColor.COLOR_BLACK;
                        }

                    }
                    else
                    {
                        regionColor = ParamColor.COLOR_WHITE;
                    }

                }
                else
                {// Single region
                    Region compareRegion = regions[OnlyRGBColorByPosition(x, y)];
                    mustBeColored = compareRegion.Equals(singleRegion) ? true : false;
                    regionColor = singleRegion.Rgb32;

                    // Indicator water terrain white-water, black-land
                    if (singleRegion.Type == ParamUI.REGION_NAME_LAND)
                    {
                        waterArr[x + y * width] = ParamColor.COLOR_WHITE;
                    }
                    else
                    {
                        waterArr[x + y * width] = ParamColor.COLOR_BLACK;
                    }
                }

                if (mustBeColored)
                {
                    var remapColor = remapArr[x + y * width];
                    ChangeColor(remapColor, regionColor);
                }
            }
        }

        // Water texture update
        seaTex.SetPixels32(waterArr);
        seaTex.Apply(false);
        _material.SetTexture("_SeaTex", seaTex);

        // Palette texture update
        paletteTex.Apply(false);
    }

    private void UpdateRegionsTimeline(int timeline, int optionLayer)
    {
        foreach(int regionId in regionsIdList)
        {
            Region region = regions[regionId];
            List<HistoryRegionRelation> historyList = region.History;
            if (historyList.Count > 0) // This region has history
            {
                HistoryRegionRelation history = historyList.Where(h => h.Stage.StartDate <= timeline & h.Stage.EndDate >= timeline).Select(c => c).FirstOrDefault();
                if (history != null) // This region has history in selected dates
                {
                    Settlement settlement = settlements[history.SettlementId];
                    regions[regionId].Settlement = settlement;
                    HistoryStage stage = history.Stage;
                    int ownerId = optionLayer switch
                    {
                        0 => Utilities.EitherInt(stage.PolityParentId_L1, 0),
                        1 => Utilities.EitherInt(stage.PolityParentId_L2, Utilities.EitherInt(stage.PolityParentId_L1, 0)),
                        2 => Utilities.EitherInt(stage.PolityParentId_L3, Utilities.EitherInt(stage.PolityParentId_L2, Utilities.EitherInt(stage.PolityParentId_L1, 0))),
                        3 => Utilities.EitherInt(stage.PolityParentId_L4, Utilities.EitherInt(stage.PolityParentId_L3, Utilities.EitherInt(stage.PolityParentId_L2, Utilities.EitherInt(stage.PolityParentId_L1, 0)))),
                        _ => 0
                    };
                    Polity polity = polities[ownerId];
                    regions[regionId].Owner = polity;

                    regions[regionId].ColorRecalculate();
                }
                else
                {
                    // Clear the region if not exist data history
                    regions[regionId].Settlement = null;
                    regions[regionId].Owner = null;
                    regions[regionId].ColorRecalculate();
                }
            }            
        }
    }

    /// 
    /// Get information from csv to colorize the regions
    ///
    private void CreateRegions(bool isInitial)
    {
        ColorsList(); // Generate the list of colors for the polities                      
        int timeline = GameManager.Instance.UI_GetCurrentTimeline(false); // Get current timeline        
        LoadPolitiesDictionaryFromDB(timeline, 1); // Uptade Polities (Important, before regions)        
        regions = CsvConnection.Instance.GetInfoRegions(timeline, 0); // Initial regions
        ColorizeRegions(); // Colorize all regions of the worldmap
    }
    public void CreateRegions(int optionLayer, bool timeTravelbutton = false, float drawRiver = 0f)
    {
        ColorsList(); // Generate the list of colors for the polities        
        int timeline = GameManager.Instance.UI_GetCurrentTimeline(timeTravelbutton); // Get current timeline
        UpdateRegionsTimeline(timeline, optionLayer); // Update the regions dictionary to optimize memory used        
        _material.SetFloat("_DrawRiver", drawRiver); // Show rivers
        ColorizeRegions(); // Colorize all regions of the worldmap
    }
    private void ColorsList()
    {        
        polityColorList.Clear();
        polityColorList.TrimExcess();
        polityColorList = CsvConnection.Instance.GetColors();
    }

    /// <summary>
    /// Modify new color region
    /// </summary>
    /// <param name="regionId">region id</param>
    /// <param name="owner">owner</param>
    public void ColorizeRegionsById(int regionId, Polity owner)
    {
        // Modify dictionary        
        regions[regionId].Owner = owner;

        // Modify new color region
        regions[regionId].ColorRecalculate();
        ColorizeRegions(regions[regionId]);
    }

    /// <summary>
    /// Get a region by a key
    /// </summary>
    /// <param name="id">key int</param>
    /// <returns></returns>
    public Region GetRegionById(int id)
    {
        return regions[id];
    }

    /// <summary>
    /// Get a region by position
    /// </summary>
    /// <param name="x">position x</param>
    /// <param name="y">position y</param>
    /// <returns>Region in x,y position</returns>
    public Region GetRegionByPosition(int x, int y)
    {
        return regions[OnlyRGBColorByPosition(x, y)];
    }

    ///********************************************************************************************************************************************///
    /// POLITIES TYPE METHODS 
    ///********************************************************************************************************************************************///
    public Dictionary<int, PolityType> GetPolitiesType()
    {
        return politiesType;
    }
    public PolityType GetPolityTypeById(int id)
    {
        return politiesType[id];
    }
    public string GetPolitiesTypeLocaleKeyById(int id)
    {
        string name = politiesType.Where(x => x.Key.Equals(id)).Select(x => x.Value.Name).FirstOrDefault();
        return name;
    }
    public List<int> GetPolityTypesByPolity(int polityId)
    {
        List<int> polityTypes = new List<int>();

        foreach (var region in regions)
        {
            List<HistoryRegionRelation> history = region.Value.History;         
            if(history != null)
            {
                foreach (HistoryRegionRelation stage in history)
                {
                    if (stage.Stage.PolityParentId_L1 == polityId) { polityTypes.Add(stage.Stage.PolityTypeIdParent_L1); }
                    if (stage.Stage.PolityParentId_L2 == polityId) { polityTypes.Add(stage.Stage.PolityTypeIdParent_L2); }
                    if (stage.Stage.PolityParentId_L3 == polityId) { polityTypes.Add(stage.Stage.PolityTypeIdParent_L3); }
                    if (stage.Stage.PolityParentId_L4 == polityId) { polityTypes.Add(stage.Stage.PolityTypeIdParent_L4); }
                }
            }
        }
        return polityTypes.Distinct().ToList();
    }
    public List<int> GetPolityTypesByPolicy(int policyId)
    {
        List<int> polityTypes = new List<int>();

        foreach (var region in regions)
        {
            List<HistoryRegionRelation> history = region.Value.History;
            if (history != null)
            {
                foreach (HistoryRegionRelation stage in history)
                {
                    if (stage.Stage.PolicyId == policyId) { polityTypes.Add(stage.Stage.PolicyTypeId); }
                }
            }
        }
        return polityTypes.Distinct().ToList();
    }


    ///********************************************************************************************************************************************///
    /// POLITIES METHODS 
    ///********************************************************************************************************************************************///
    public Dictionary<int, Polity> GetPolities(int policy = 0)
    {
        if (policy == 0)
        {
            return polities;
        }
        else
        {
            bool policyValue = policy == CsvConnection.Instance.GetCollectiveId() ? true : false;
            return polities.Where(x => x.Value.IsCollective.Equals(policyValue)).ToDictionary(x => x.Key, x => x.Value);
        }
    }
    public Polity GetPolityById(int id)
    {
        return polities[id];
    }
    public string GetPolitiesLocaleKeyById(int id)
    {
        string name = polities.Where(x => x.Key.Equals(id)).Select(x => x.Value.Name).FirstOrDefault();
        return name;
    }
   

    ///********************************************************************************************************************************************///
    /// SETTLEMENTS METHODS 
    ///********************************************************************************************************************************************///
    public Dictionary<int, Settlement> GetSettlements()
    {
        return settlements;
    }
    public Settlement GetSettlementById(int id)
    {
        return settlements[id];
    }
    public string GetSettlementsLocaleKeyById(int id)
    {
        string name = settlements.Where(x => x.Key.Equals(id)).Select(x => x.Value.Name).FirstOrDefault();
        return name;
    }
    ///********************************************************************************************************************************************///
    ///********************************************************************************************************************************************///
    ///********************************************************************************************************************************************///
    ///********************************************************************************************************************************************///


    /// Load data from database to dictionaries
    public void LoadPolitiesTypeDictionaryFromDB()
    {
        politiesType = CsvConnection.Instance.GetInfoPolitiesType();
    }
    public void LoadPolitiesDictionaryFromDB(int currentTime, int polityLayer)
    {
        polities = CsvConnection.Instance.GetInfoPolities(currentTime.ToString(), polityLayer);
    }
    public void LoadSettlementsDictionaryFromDB()
    {
        settlements = CsvConnection.Instance.GetInfoSettlements();
    }
    public void LoadHistoryRegionDictionaryFromDB(int regionId)
    {
        regions[regionId].History = CsvConnection.Instance.GetHistoryByRegionId(regionId);
    }

    /// CHECKS
    public bool PolityIsRelated(int polityId)
    {
        bool result = false;

        foreach (var region in regions)
        {
            List<HistoryRegionRelation> history = region.Value.History;
            if(history != null)
            {
                foreach (HistoryRegionRelation stage in history)
                {
                    if (stage.Stage.PolicyId == polityId) { result = true; }
                }
            }
        }

        return result;
    }
    public bool PolityTypeIsRelated(int polityTypeId)
    {
        bool result = false;

        foreach (var region in regions)
        {
            List<HistoryRegionRelation> history = region.Value.History;
            if(history != null)
            {
                foreach (HistoryRegionRelation stage in history)
                {
                    if (stage.Stage.PolicyTypeId == polityTypeId) { result = true; }
                }
            }
        }

        return result;
    }

    /// SYMBOLS
    public Texture2D GetSymbolTexture(string symbolFilename)
    {
        string currentSymbolName = Utilities.PascalStrings(symbolFilename) + ".png";
        return symbolsTexture.Where(s => s.Name == currentSymbolName).Select(s => s.Texture).FirstOrDefault();
    }
    public void ShowCapitalSymbols()
    {
        // Remove previous symbols
        placementObjects.RemoveMapObjects(ParamMap.MAPTAG_CAPITAL_SYMBOL);
        
        // Current layer selected
        int currentLevelLayer = GameManager.Instance.UI_GetLayerValue() + 1;

        int isCapital;
        string polityCapital;
        string polityTypeCapital;
        // Find regions with capital symbol
        foreach(int id in regionsIdList)
        {
            Region region = regions[id];
            List<HistoryRegionRelation> history = region.History;
            if(history != null)
            {
                foreach (HistoryRegionRelation h in history)
                {
                    switch (currentLevelLayer)
                    {
                        case 1:
                            isCapital = h.Stage.Capital_L1; 
                            polityCapital = GetPolityById(h.Stage.PolityParentId_L1).Name; 
                            polityTypeCapital= GetPolityTypeById(h.Stage.PolityTypeIdParent_L1).Name;  
                            break;
                        case 2:
                            isCapital = Utilities.EitherInt(h.Stage.Capital_L2, h.Stage.Capital_L1); 
                            polityCapital = GetPolityById(Utilities.EitherInt(h.Stage.PolityParentId_L2, h.Stage.PolityParentId_L1)).Name; 
                            polityTypeCapital = GetPolityTypeById(Utilities.EitherInt(h.Stage.PolityTypeIdParent_L2, h.Stage.PolityTypeIdParent_L1)).Name; 
                            break;
                        case 3: 
                            isCapital = Utilities.EitherInt(Utilities.EitherInt(h.Stage.Capital_L3, h.Stage.Capital_L2), h.Stage.Capital_L1); 
                            polityCapital = GetPolityById(Utilities.EitherInt(Utilities.EitherInt(h.Stage.PolityParentId_L3, h.Stage.PolityParentId_L2), h.Stage.PolityParentId_L1)).Name; 
                            polityTypeCapital = GetPolityTypeById(Utilities.EitherInt(Utilities.EitherInt(h.Stage.PolityTypeIdParent_L3, h.Stage.PolityTypeIdParent_L2), h.Stage.PolityTypeIdParent_L1)).Name; 
                            break;
                        case 4: 
                            isCapital = Utilities.EitherInt(Utilities.EitherInt(Utilities.EitherInt(h.Stage.Capital_L4, h.Stage.Capital_L3), h.Stage.Capital_L2), h.Stage.Capital_L1); 
                            polityCapital = GetPolityById(Utilities.EitherInt(Utilities.EitherInt(Utilities.EitherInt(h.Stage.PolityParentId_L4, h.Stage.PolityParentId_L3), h.Stage.PolityParentId_L2), h.Stage.PolityParentId_L1)).Name; 
                            polityTypeCapital = GetPolityTypeById(Utilities.EitherInt(Utilities.EitherInt(Utilities.EitherInt(h.Stage.PolityTypeIdParent_L4, h.Stage.PolityTypeIdParent_L3), h.Stage.PolityTypeIdParent_L2), h.Stage.PolityTypeIdParent_L1)).Name; 
                            break;
                        default: isCapital = 0; polityCapital = ""; polityTypeCapital = ""; break;
                    }
                    
                    if (isCapital == 1 && GameManager.Instance.UI_IsDateCurrent(h.Stage.StartDate, h.Stage.EndDate))
                    {
                        string startEra = h.Stage.StartDate > 0 ? "A" : "B";
                        string endEra = h.Stage.EndDate > 0 ? "A" : "B";
                        string symbolFilename = h.Stage.IsSymbolForDate == 0 ? polityCapital + "_" + polityTypeCapital : polityCapital + "_" + polityTypeCapital + "_" + startEra + h.Stage.StartDate.ToString().PadLeft(8, '0') + "_" + endEra + h.Stage.EndDate.ToString().PadLeft(8, '0');
                        Texture2D currentSymbol = GetSymbolTexture(symbolFilename);
                        placementObjects.PutMapObjectsCustomSprites(ParamMap.MAPTAG_CAPITAL_SYMBOL, region.Settlement.PixelCoordinates, currentSymbol, region.Settlement.Name);                 
                    }
                
                }
            }
        }

    }

    /// SETTLEMENT MARKERS
    public void ShowSettlementMarkers()
    {
        // Remove previous markers
        placementObjects.RemoveMapObjects(ParamMap.MAPTAG_SETTLEMENT_MARKER);

        // Current layer selected
        int currentLevelLayer = GameManager.Instance.UI_GetLayerValue() + 1;

        int isCapital;

        // Find regions with marker
        foreach (int id in regionsIdList)
        {
            Region region = regions[id];
            List<HistoryRegionRelation> history = region.History;
            if (history != null)
            {
                foreach (HistoryRegionRelation h in history)
                {
                    switch (currentLevelLayer)
                    {
                        case 1:
                            isCapital = h.Stage.Capital_L1;
                            break;
                        case 2:
                            isCapital = Utilities.EitherInt(h.Stage.Capital_L2, h.Stage.Capital_L1);
                            break;
                        case 3:
                            isCapital = Utilities.EitherInt(Utilities.EitherInt(h.Stage.Capital_L3, h.Stage.Capital_L2), h.Stage.Capital_L1);
                            break;
                        case 4:
                            isCapital = Utilities.EitherInt(Utilities.EitherInt(Utilities.EitherInt(h.Stage.Capital_L4, h.Stage.Capital_L3), h.Stage.Capital_L2), h.Stage.Capital_L1);
                            break;
                        default: isCapital = 0; break;
                    }

                    Settlement settlement = GetSettlementById(h.SettlementId);
                    if(isCapital == 0 && settlement != null && settlement.RegionId != 0 && settlement.PixelCoordinates != Vector2.zero & GameManager.Instance.UI_IsDateCurrent(h.Stage.StartDate, h.Stage.EndDate))
                    {
                        placementObjects.PutMapObjects(ParamMap.MAPTAG_SETTLEMENT_MARKER, settlement.PixelCoordinates, settlement.Name);
                    }
                }
            }
        }

    }

    // GENERIC PLACEMENT OBJECTS
    public void RemoveMapObjects(string name)
    {
        placementObjects.RemoveMapObjects(name);
    }
    public void PutMapObjects(string name, Region region)
    {
        placementObjects.PutMapObjects(name, region);
    }

}