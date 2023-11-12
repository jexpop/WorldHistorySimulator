public class ParamMap
{

    /* Map constants */
    public static string MAP_MOUSE_SCROLL = "Mouse ScrollWheel";
    public static int MAP_SIZE_WIDTH = 5632;
    public static int MAP_SIZE_HIGHT = 2048;

    // Raycast limit to avoid out-of-range (off-map) errors on shader
    public static float MAP_MARGIN_BOX_MIN = 2885.0f;
    public static float MAP_MARGIN_BOX_MAX = 11534192.0f;

    // Positions, minimum and maximum possible movement
    public static float MAP_PAN_SPEED = 10.0f;
    public static float MAP_X_MIN = -2615.0f, MAP_Y_MIN = -940.0f;
    public static float MAP_X_MAX = 2615.0f, MAP_Y_MAX = 940.0f;

    // Camera zoom speed, minimum and maximum
    public static float MAP_ZOOM_SPEED = 100.0f;
    public static float MAP_ZOOM_MIN = 20.0f;
    public static float MAP_ZOOM_MAX = 100.0f;

}