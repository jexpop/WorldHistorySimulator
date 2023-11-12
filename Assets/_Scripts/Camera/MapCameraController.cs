using UnityEngine;

public class MapCameraController : MonoBehaviour
{

	private Vector3 mouseOrigin;    // Cursor position

	private bool isPanning;     // Camera movement in XY coordinates


	void Update()
	{

		// Deactivate the movement/zooming in when releasing the action
		if (!Input.GetMouseButton(1)) isPanning = false;

		// Right mouse button is activated (XY movement)
		if (Input.GetMouseButtonDown(1)) { mouseOrigin = Input.mousePosition; isPanning = true; }

		// Move the camera in XY
		if (isPanning)
		{

			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
			Vector3 move = new Vector3(pos.x * GameConst.MAP_PAN_SPEED, pos.y * GameConst.MAP_PAN_SPEED, 0);
			transform.Translate(move, Space.Self);

			// Movement within bounds (Only Y limits)
			transform.position = new Vector3(
                  transform.position.x, //Mathf.Clamp(transform.position.x, GameConst.MAP_X_MIN, GameConst.MAP_X_MAX),
                  Mathf.Clamp(transform.position.y, GameConst.MAP_Y_MIN, GameConst.MAP_Y_MAX),
				  transform.position.z
			);

		}

	}

}