using UnityEngine;

public class FullMapCameraController : MonoBehaviour
{

	private Vector3 mouseOrigin;    // Cursor position

	private bool isPanning;     // Camera movement in XY coordinates
	private bool isZooming;     // Is the camera zooming in?


	void Update()
	{

		// Deactivate the movement/zooming in when releasing the action
		if (!Input.GetMouseButton(1)) isPanning = false;
		if (Input.GetAxis(GameConst.MAP_MOUSE_SCROLL) == 0f) isZooming = false;

		// Right mouse button is activated (XY movement)
		if (Input.GetMouseButtonDown(1)) { mouseOrigin = Input.mousePosition; isPanning = true; }

		// The mouse wheel scroll is activated
		if (Input.GetAxis(GameConst.MAP_MOUSE_SCROLL) != 0f) { isZooming = true; }

		// Move the camera in XY
		if (isPanning)
		{

			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
			Vector3 move = new Vector3(pos.x * GameConst.MAP_PAN_SPEED, pos.y * GameConst.MAP_PAN_SPEED, 0);
			transform.Translate(move, Space.Self);

			// Movement within bounds
			transform.position = new Vector3(
				  Mathf.Clamp(transform.position.x, GameConst.MAP_X_MIN, GameConst.MAP_X_MAX),
				  Mathf.Clamp(transform.position.y, GameConst.MAP_Y_MIN, GameConst.MAP_Y_MAX),
				  transform.position.z
			);

		}

		// Camera zoom
		if (isZooming)
		{
			Camera.main.fieldOfView -= GameConst.MAP_ZOOM_SPEED * Input.GetAxis(GameConst.MAP_MOUSE_SCROLL);
			Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, GameConst.MAP_ZOOM_MIN, GameConst.MAP_ZOOM_MAX);
		}

	}

}
