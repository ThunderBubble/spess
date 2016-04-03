using UnityEngine;
using System.Collections;

public class SelectorSnapToGrid : MonoBehaviour
{
	public GameObject player;
	public GameObject ship;

	void Update()
	{
		// Create a vector which is the vector between the center of the ship and the player rotated to face right
		//Vector3 offset = Quaternion.Euler(0, 0, ship.transform.rotation.eulerAngles.z * -1.0F) * (player.transform.position - ship.transform.position);

		// We use this vector to find the rounded x and y components of the offset relative to the ship
		//float offset_x = Mathf.Round(offset.x);
		//float offset_y = Mathf.Round(offset.y);

		// Create a vector with these components then rotate it back to the original direction
		//offset = new Vector3(offset_x, offset_y);
		//offset = Quaternion.Euler(0, 0, ship.transform.rotation.eulerAngles.z) * offset;

		// Update our position and rotate to allign with the ship
		//transform.position = offset + ship.transform.position;
		//transform.rotation = ship.transform.rotation;

		// Significantly simpler script using parents
		transform.localPosition = new Vector3(Mathf.Round(player.transform.localPosition.x), Mathf.Round(player.transform.localPosition.y));
	}
}