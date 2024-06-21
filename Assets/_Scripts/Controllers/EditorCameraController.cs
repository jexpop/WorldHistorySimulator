using UnityEngine;

public class EditorCameraController : Singleton<EditorCameraController>
{

	private Vector3 mouseOrigin;    // Cursor position

	private bool isPanning;     // Camera movement in XY coordinates
	private bool isZooming;     // Is the camera zooming in?
	private bool isZoomingEnabled = true;

    private void Start()
    {
		int xCameraPosition = GameManager.Instance.playerEditorData.xCameraPosition;
        int yCameraPosition = GameManager.Instance.playerEditorData.yCameraPosition;
        int fieldOfView = GameManager.Instance.playerEditorData.fovCamera;
		Camera.main.transform.position = new Vector3(xCameraPosition, yCameraPosition, Camera.main.transform.position.z);
		Camera.main.fieldOfView = fieldOfView;
    }


    void Update()
	{

        // Deactivate the movement/zooming in when releasing the action
        if (!Input.GetMouseButton(1)) isPanning = false;
		if (Input.GetAxis(ParamMap.MAP_MOUSE_SCROLL) == 0f) isZooming = false;

		// Right mouse button is activated (XY movement)
		if (Input.GetMouseButtonDown(1)) { mouseOrigin = Input.mousePosition; isPanning = true; }

		// The mouse wheel scroll is activated
		if (Input.GetAxis(ParamMap.MAP_MOUSE_SCROLL) != 0f) { isZooming = true; }

		// Move the camera in XY
		if (isPanning)
		{

			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
			Vector3 move = new Vector3(pos.x * ParamMap.MAP_PAN_SPEED, pos.y * ParamMap.MAP_PAN_SPEED, 0);
			transform.Translate(move, Space.Self);

			// Movement within bounds
            transform.position = new Vector3(
                  Mathf.Clamp(transform.position.x, ParamMap.MAP_X_MIN, ParamMap.MAP_X_MAX),
                  Mathf.Clamp(transform.position.y, ParamMap.MAP_Y_MIN, ParamMap.MAP_Y_MAX),
                  transform.position.z
            );

			SaveCameraPosition();

        }

		// Camera zoom
		if (isZooming & isZoomingEnabled)
		{
			Camera.main.fieldOfView -= ParamMap.MAP_ZOOM_SPEED * Input.GetAxis(ParamMap.MAP_MOUSE_SCROLL);
			Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, ParamMap.MAP_ZOOM_MIN, ParamMap.MAP_ZOOM_MAX);
			SaveCameraPosition();

        }

	}

	public void ChangeZoomingEnabled()
	{
		isZoomingEnabled = !isZoomingEnabled;
	}

	private void SaveCameraPosition()
	{
        GameManager.Instance.playerEditorData.xCameraPosition = (int)Camera.main.transform.position.x;
        GameManager.Instance.playerEditorData.yCameraPosition = (int)Camera.main.transform.position.y;
        GameManager.Instance.playerEditorData.fovCamera = (int)Camera.main.fieldOfView;
        GameManager.Instance.SavePlayerEditorConfiguration();
    }

}
