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

    // Color detection
    Color32 prevColor;
    Vector2Int prevXY = new Vector2Int();
    bool selectAny = false;

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

        // Placement objects script
        placementObjects = gameObject.GetComponentInParent<PlacementObjects>();

        // Building map
        CreateMap();
        CreateRegions(true);

        // Put capital symbols
        ShowCapitalSymbols();
       
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
        DirectoryInfo symbolsDir = new DirectoryInfo(symbolStreamingPath);
        FileInfo[] symbolsInfo = symbolsDir.GetFiles("*.png");
        foreach (FileInfo symbolFilename in symbolsInfo)
        {
            //Converts desired path into byte array
            byte[] pngBytes = File.ReadAllBytes(symbolStreamingPath + symbolFilename.Name);
            //Creates texture and loads byte array data to create image
            Texture2D symbolTex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            symbolTex.LoadImage(pngBytes);
            // Add image to symbols dictioanry
            SymbolTexture symbolTexture = new SymbolTexture(symbolFilename.Name, symbolTex);
            symbolsTexture.Add(symbolTexture);
        }
    }

    void Update()
    {
        
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 p = hitInfo.point;
            int x = (int)Mathf.Floor(p.x) + width / 2;
            int y = (int)Mathf.Floor(p.y) + height / 2;

            GameManager.Instance.UI_SetCoordinates(x.ToString(), y.ToString());

            // Avoid out of range error
            int marginBox = x + y * width;
            if (marginBox >= ParamMap.MAP_MARGIN_BOX_MIN && marginBox <= ParamMap.MAP_MARGIN_BOX_MAX)
            {
                Color32 remapColor = remapArr[x + y * width];

                Region region = regions[OnlyRGBColorByPosition(prevXY.x, prevXY.y)];
                
                // Show a post it note with information of the polity
                // Also show the map coordinates
                if (GameManager.Instance.UI_GetUIStatus() == UIStatus.Nothing || GameManager.Instance.UI_GetUIStatus() == UIStatus.PostItNote)
                {
                    // Post It
                    if (region.Type == ParamUI.REGION_NAME_LAND && region.Owner != null)
                    {
                        GameManager.Instance.UI_SetUIStatus(UIStatus.PostItNote);
                        GameManager.Instance.UI_PostItPolityVisibility(mousePos, true, region);
                    }
                    else
                    {
                        GameManager.Instance.UI_SetUIStatus(UIStatus.Nothing);
                        GameManager.Instance.UI_PostItPolityVisibility(mousePos, false);
                    }
                }
                
                // Click in the region
                if (Input.GetMouseButtonDown(0) && (
                                            GameManager.Instance.UI_GetUIStatus() == UIStatus.InfoRegion ||
                                            GameManager.Instance.UI_GetUIStatus() == UIStatus.Nothing ||
                                            GameManager.Instance.UI_GetUIStatus() == UIStatus.PostItNote))
                {

                    // Change UI status if you touch land
                    // PostIt Note is deactivated
                    if (region.Type == ParamUI.REGION_NAME_LAND)
                    {
                        GameManager.Instance.UI_SetUIStatus(UIStatus.InfoRegion);
                        GameManager.Instance.UI_PostItPolityVisibility(mousePos, false);
                    }

                    // Remove others flag marker
                    placementObjects.RemoveMapObjects(ParamMap.MAPTAG_FLAG_MARKER);

                    int redColor = OnlyRGBColorByPosition(x, y).x;
                    int greenColor = OnlyRGBColorByPosition(x, y).y;
                    int blueColor = OnlyRGBColorByPosition(x, y).z;

                    // Activate Region Panel and move Region Panel in your correct position
                    Vector2 panelPositionNew = GameManager.Instance.UI_CalculateNewPositionPanel(mousePos);
                    if (region.Type == ParamUI.REGION_NAME_LAKE || region.Type == ParamUI.REGION_NAME_SEA) { GameManager.Instance.UI_DeactivateRegionPanel(); } else { GameManager.Instance.UI_ActivateRegionPanel(panelPositionNew.x, panelPositionNew.y); }

                    if (region.Type == ParamUI.REGION_NAME_LAND)
                    {

                        // Name and Image Panel, only lands
                        GameManager.Instance.UI_SetNameAndImageRegionPanel(region.Name, region.Terrain);

                        // For debugging, show color of the owner
                        string debugColor = region.Rgb32.r.ToString() + "-" + region.Rgb32.g.ToString() + "-" + region.Rgb32.b.ToString();
                        GameManager.Instance.UI_ShowRgbOwner(debugColor);

                        // Info Panel
                        if (region.Settlement == null)
                        {
                            GameManager.Instance.UI_SetSettlementRegionPanel(true); // Unknown settlement
                        }
                        else
                        {
                            GameManager.Instance.UI_SetSettlementRegionPanel(true, region.Settlement.Name);
                        }

                        // Instantiate a flag marker in the center of the region (only lands)
                        placementObjects.PutMapObjects(ParamMap.MAPTAG_FLAG_MARKER, region);

                    }

                }
                
                // Mouse Right Button
                if (Input.GetMouseButton(1))
                {
                    GameManager.Instance.UI_DeactivateRegionPanel();
                    placementObjects.RemoveMapObjects(ParamMap.MAPTAG_FLAG_MARKER);
                }
 
                if ( !selectAny || !prevColor.Equals(remapColor) )
                {
                    if (selectAny)
                    {
                        ChangeColor(prevColor, region.Rgb32);

                    }
                    selectAny = true;
                    prevColor = remapColor;
                    prevXY = new Vector2Int(x, y);

                    // Define the color to the highlight to the land regions
                    Region regionHighlight = regions[OnlyRGBColorByPosition(x, y)];
                    if (regionHighlight.Type == ParamUI.REGION_NAME_LAND)
                    {
                        ChangeColor(remapColor, ParamColor.COLOR_REGION_HIGHLIGHT);
                    }

                    paletteTex.Apply(false);
                }
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
    void ChangeColor(Color32 remapColor, Color32 showColor)
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
    Vector3Int OnlyRGBColorByPosition(int x, int y)
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
    public Texture2D GetSymbolTexture(string polityCapital, string polityTypeCapital)
    {
        string currentSymbolName = Utilities.PascalStrings(polityCapital + "_" + polityTypeCapital) + ".png";
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
                            polityTypeCapital = GetPolityTypeById(Utilities.EitherInt(h.Stage.PolityTypeIdParent_L2, h.Stage.PolityTypeIdParent_L1)).Name; break;
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
                    
                    if (isCapital == 1 & GameManager.Instance.UI_IsDateCurrent(h.Stage.StartDate, h.Stage.EndDate))
                    {
                        Texture2D currentSymbol = GetSymbolTexture(polityCapital, polityTypeCapital);
                        placementObjects.PutMapObjectsCustomSprites(ParamMap.MAPTAG_CAPITAL_SYMBOL, region, currentSymbol);                 
                    }
                
                }
            }
        }

    }

}