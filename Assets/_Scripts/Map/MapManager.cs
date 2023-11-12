using System.Collections.Generic;
using UnityEngine;
using Aron.Weiler;
using System.Linq;

public class MapManager : Singleton<MapManager>
{
    Material _material;

    // Map textures and arrays
    int width, height;
    Color32[] remapArr;
    Color32[] waterArr;
    Texture2D remapTex;
    Texture2D waterTex;
    Texture2D paletteTex;
    Texture2D mainTex;

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


    public GameObject flagMarker;


    void Start()
    {
        // Material & texture inicialization
        _material = GetComponent<Renderer>().material;
        mainTex = _material.GetTexture("_MainTex") as Texture2D;

        // Get Sql data
        LoadPolitiesTypeDictionaryFromDB();
        LoadPolitiesDictionaryFromDB();
        LoadSettlementsDictionaryFromDB();

        // Building map
        CreateMap();
        CreateRegions(0);

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

            // Avoid out of range error
            int marginBox = x + y * width;
            if (marginBox >= ParamMap.MAP_MARGIN_BOX_MIN && marginBox <= ParamMap.MAP_MARGIN_BOX_MAX)
            {
                Color32 remapColor = remapArr[x + y * width];

                Region region = regions[OnlyRGBColorByPosition(prevXY.x, prevXY.y)];

                // Show a post it note with information of the polity
                if (EditorUICanvasManager.Instance.uiStatus == UIStatus.Nothing || EditorUICanvasManager.Instance.uiStatus == UIStatus.PostItNote)
                {
                    if (region.Type == ParamUI.REGION_NAME_LAND && region.Owner != null)
                    {
                        EditorUICanvasManager.Instance.uiStatus = UIStatus.PostItNote;
                        EditorUICanvasManager.Instance.PostItPolityVisibility(mousePos, true, region);
                    }
                    else
                    {
                        EditorUICanvasManager.Instance.uiStatus = UIStatus.Nothing;
                        EditorUICanvasManager.Instance.PostItPolityVisibility(mousePos, false);
                    }
                }

                // Click in the region
                if (Input.GetMouseButtonDown(0) && (
                                            EditorUICanvasManager.Instance.uiStatus == UIStatus.InfoRegion ||
                                            EditorUICanvasManager.Instance.uiStatus == UIStatus.Nothing ||
                                            EditorUICanvasManager.Instance.uiStatus == UIStatus.PostItNote))
                {

                    // Change UI status if you touch land
                    // PostIt Note is deactivated
                    if (region.Type == ParamUI.REGION_NAME_LAND)
                    {
                        EditorUICanvasManager.Instance.uiStatus = UIStatus.InfoRegion;
                        EditorUICanvasManager.Instance.PostItPolityVisibility(mousePos, false);
                    }

                    // Remove others flag marker
                    RemoveFlagMarkers();

                    int redColor = OnlyRGBColorByPosition(x, y).x;
                    int greenColor = OnlyRGBColorByPosition(x, y).y;
                    int blueColor = OnlyRGBColorByPosition(x, y).z;

                    // Activate Region Panel and move Region Panel in your correct position
                    Vector2 panelPositionNew = EditorUICanvasManager.Instance.CalculateNewPositionPanel(mousePos);
                    if (region.Type == ParamUI.REGION_NAME_LAKE || region.Type == ParamUI.REGION_NAME_SEA) { EditorUICanvasManager.Instance.DeactivateRegionPanel(); } else { EditorUICanvasManager.Instance.ActivateRegionPanel(panelPositionNew.x, panelPositionNew.y); }

                    if (region.Type == ParamUI.REGION_NAME_LAND)
                    {

                        // Name Panel, only lands
                        EditorUICanvasManager.Instance.SetNameRegionPanel(region.Name);

                        // Info Panel
                        if (region.Settlement == null)
                        {
                            EditorUICanvasManager.Instance.SetSettlementRegionPanel(true); // Unknown settlement
                        }
                        else
                        {
                            EditorUICanvasManager.Instance.SetSettlementRegionPanel(true, region.Settlement.Name);
                        }

                        // Instantiate a flag marker in the center of the region (only lands)
                        GameObject flag = Instantiate(flagMarker);
                        flag.transform.position = new Vector3(region.CoordinatesCenter.x, region.CoordinatesCenter.y, -1);

                    }

                }

                // Mouse Right Button
                if (Input.GetMouseButton(1))
                {
                    EditorUICanvasManager.Instance.DeactivateRegionPanel();
                    RemoveFlagMarkers();
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

        width = mainTex.width;
        height = mainTex.height;

        var main2remap = new Dictionary<Color32, Color32>();
        remapArr = new Color32[mainArr.Length];
        waterArr = new Color32[mainArr.Length];
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
    Vector3Int OnlyRGBColorByRGBA(Color color)
    {
        int redColor = (int)Mathf.Round(color.r * ParamColor.COLOR32_MAX);
        int greenColor = (int)Mathf.Round(color.g * ParamColor.COLOR32_MAX);
        int bluecolor = (int)Mathf.Round(color.b * ParamColor.COLOR32_MAX);
        return new Vector3Int(redColor, greenColor, bluecolor);
    }

    /// <summary>
    /// Get information from database to colorize the regions
    /// </summary>
    /// <param name="timeTravelbutton">Event button</param>
    public void CreateRegions(int optionLayer, bool timeTravelbutton = false)
    {

        // Get current timeline
        int timeline = EditorUICanvasManager.Instance.GetCurrentTimeline(timeTravelbutton);

        // Create the list of regions with your information
        regions = MapSqlConnection.Instance.GetInfoRegions(timeline, optionLayer);

        // Colorize all regions of the worldmap
        ColorizeRegions();

    }

    /// <summary>
    /// Colorize the regions of the map
    /// </summary>
    /// <param name="singleRegion"></param>
    void ColorizeRegions(Region singleRegion = null)
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
        waterTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        waterTex.filterMode = FilterMode.Point;
        waterTex.SetPixels32(waterArr);
        waterTex.Apply(false);
        _material.SetTexture("_WaterTex", waterTex);

        // Palette texture update
        paletteTex.Apply(false);

    }

    /// <summary>
    /// Modify new color region
    /// </summary>
    /// <param name="regionId">region id</param>
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

    /*** Polities type methods ***/
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
    /***                        ***/

    /*** Polities methods ***/
    public Dictionary<int, Polity> GetPolities(int policy = 0)
    {
        if (policy == 0)
        {
            return polities;
        }
        else
        {
            bool policyValue = policy == MapSqlConnection.Instance.GetCollectiveId() ? true : false;
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
    /***                        ***/

    /*** Settlements methods ***/
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
    /***                        ***/


    void RemoveFlagMarkers()
    {
        GameObject[] flags = GameObject.FindGameObjectsWithTag(ParamUI.TAG_FLAG_MARKER);
        for (int i = 0; i < flags.Length; i++)
        {
            Destroy(flags[i].transform.parent.gameObject);
        }
    }

    /// Load data from database to dictionaries
    public void LoadPolitiesTypeDictionaryFromDB()
    {
        politiesType = MapSqlConnection.Instance.GetInfoPolitiesType();
    }
    public void LoadPolitiesDictionaryFromDB()
    {
        polities = MapSqlConnection.Instance.GetInfoPolities();
    }
    public void LoadSettlementsDictionaryFromDB()
    {
        settlements = MapSqlConnection.Instance.GetInfoSettlements();
    }
    public void LoadHistoryRegionDictionaryFromDB(int regionId)
    {
        regions[regionId].History = MapSqlConnection.Instance.GetHistoryByRegionId(regionId);
    }

    /// CHECKS
    public bool PolityIsRelated(int polityId)
    {
        bool result = false;

        foreach (var region in regions)
        {
            List<HistoryRegionRelation> history = region.Value.History;
            foreach (HistoryRegionRelation stage in history)
            {
                if (stage.Stage.PolicyId == polityId) { result = true; }
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
            foreach (HistoryRegionRelation stage in history)
            {
                if (stage.Stage.PolicyTypeId == polityTypeId) { result = true; }
            }
        }

        return result;
    }

}