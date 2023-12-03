using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{

    [SerializeField] private Texture2D[] cursorTextureArray;
    [SerializeField] private int frameCount;
    [SerializeField] private float frameRate;

    private int currentFrame;
    private float frameTimer;
    private bool isWaiting;


    private void Start()
    {
        Cursor.SetCursor(cursorTextureArray[0], new Vector2(10, 10), CursorMode.Auto);
        isWaiting = true;
    }

    private void Update()
    {
        if (isWaiting)
        {
            frameTimer -= Time.deltaTime;
            if (frameTimer <= 0f)
            {
                frameTimer += frameRate;
                currentFrame = (currentFrame + 1) % frameCount;
                Cursor.SetCursor(cursorTextureArray[currentFrame], new Vector2(10, 10), CursorMode.Auto);
            }
        }

    }

    public void ChangeWaiting()
    {
        isWaiting = !isWaiting;
    }

}
