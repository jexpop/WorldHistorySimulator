using TMPro;
using UnityEngine;

public class MouseController : Singleton<MouseController>
{

    // TODO check 
    [SerializeField] private Texture2D[] cursorTextureArray;
    [SerializeField] private int frameCount;
    [SerializeField] private float frameRate;

    private Vector3Int lastCursorMovement;
    private Vector2Int prevXY = new Vector2Int();
    private bool selectAny = false;
    private Color32 prevColor;

    /*private int currentFrame;
    private float frameTimer;
    private bool isWaiting;*/


    private void Start()
    {
        lastCursorMovement = Vector3Int.FloorToInt(Input.mousePosition);

        //Cursor.SetCursor(cursorTextureArray[0], new Vector2(10, 10), CursorMode.Auto);
        //isWaiting = true;
    }

    private void Update()
    {
        Vector3Int currentMousePosition = Vector3Int.FloorToInt(Input.mousePosition);
        if (lastCursorMovement != currentMousePosition) 
        {
            lastCursorMovement = currentMousePosition;
            RegionDetect(currentMousePosition);
        }


        /*if (isWaiting)
        {
            frameTimer -= Time.deltaTime;
            if (frameTimer <= 0f)
            {
                frameTimer += frameRate;
                currentFrame = (currentFrame + 1) % frameCount;
                Cursor.SetCursor(cursorTextureArray[currentFrame], new Vector2(10, 10), CursorMode.Auto);
            }
        }*/

    }

    /*
    public void ChangeWaiting()
    {
        isWaiting = !isWaiting;
    }*/

    public void LocalizationSetTextName(TextMeshProUGUI nameText, string name)
    {
        GameManager.Instance.LOC_AddLocalizeString(nameText, "LOC_TABLE_HIST_SETTLEMENTS", name);
    }

    private void RegionDetect(Vector3Int currentMousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(currentMousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 p = hitInfo.point;
            int x = (int)Mathf.Floor(p.x) + ParamMap.MAP_SIZE_WIDTH / 2;
            int y = (int)Mathf.Floor(p.y) + ParamMap.MAP_SIZE_HIGHT / 2;

            // Show coordinates of the map
            GameManager.Instance.UI_SetCoordinates(((int)p.x).ToString(), ((int)p.y).ToString());

            // Avoid out of range error
            int marginBox = x + y * ParamMap.MAP_SIZE_WIDTH;
            if (marginBox >= ParamMap.MAP_MARGIN_BOX_MIN && marginBox <= ParamMap.MAP_MARGIN_BOX_MAX)
            {
                Color32 remapColor = GameManager.Instance.MAP_GetRemapColorByPosition(x, y);

                Region region = GameManager.Instance.MAP_GetRegionByPosition(prevXY.x, prevXY.y);

                // TO DO - Check PostIT hidden in the menu mode
                if (GameManager.Instance.UI_GetUIStatus() == UIStatus.TopPanel)
                {
                    GameManager.Instance.UI_PostItPolityVisibility(currentMousePosition, false);
                }

                // Show a post it note with information of the polity               
                if (GameManager.Instance.UI_GetUIStatus() == UIStatus.Nothing || GameManager.Instance.UI_GetUIStatus() == UIStatus.PostItNote)
                {
                    // Post It
                    if (region.Type == ParamUI.REGION_NAME_LAND && region.Owner != null)
                    {
                        GameManager.Instance.UI_SetUIStatus(UIStatus.PostItNote);
                        GameManager.Instance.UI_PostItPolityVisibility(currentMousePosition, true, region);
                    }
                    else
                    {
                        GameManager.Instance.UI_SetUIStatus(UIStatus.Nothing);
                        GameManager.Instance.UI_PostItPolityVisibility(currentMousePosition, false);
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
                        GameManager.Instance.UI_PostItPolityVisibility(currentMousePosition, false);
                    }

                    // Remove others flag marker
                    GameManager.Instance.MAP_RemoveMapObjects(ParamMap.MAPTAG_FLAG_MARKER);

                    Vector3Int colorPosition = GameManager.Instance.MAP_OnlyRGBColorByPosition(x, y);
                    int redColor = colorPosition.x;
                    int greenColor = colorPosition.y;
                    int blueColor = colorPosition.z;

                    // Activate Region Panel and move Region Panel in your correct position
                    Vector2 panelPositionNew = GameManager.Instance.UI_CalculateNewPositionPanel(currentMousePosition);
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
                        GameManager.Instance.MAP_PutMapObjects(ParamMap.MAPTAG_FLAG_MARKER, region);

                    }

                }
                
                // Mouse Right Button
                if (Input.GetMouseButton(1))
                {
                    GameManager.Instance.UI_DeactivateRegionPanel();
                    GameManager.Instance.MAP_RemoveMapObjects(ParamMap.MAPTAG_FLAG_MARKER);
                }
 
                if ( !selectAny || !prevColor.Equals(remapColor) )
                {
                    if (selectAny)
                    {
                        GameManager.Instance.MAP_ChangeColor(prevColor, region.Rgb32);

                    }
                    selectAny = true;
                    prevColor = remapColor;
                    prevXY = new Vector2Int(x, y);

                    // Define the color to the highlight to the land regions
                    Region regionHighlight = GameManager.Instance.MAP_GetRegionByPosition(x, y);
                    if (regionHighlight.Type == ParamUI.REGION_NAME_LAND)
                    {
                        GameManager.Instance.MAP_ChangeColor(remapColor, ParamColor.COLOR_REGION_HIGHLIGHT);
                    }

                    GameManager.Instance.MAP_ApplyPaletteTexture(false);
                }
            }

        }
    }


}
