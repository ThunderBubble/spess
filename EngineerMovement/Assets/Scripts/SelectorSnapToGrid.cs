using UnityEngine;
using System.Collections;

public class SelectorSnapToGrid : MonoBehaviour
{
	public GameObject player;
	public GameObject ship;
	
	// Update is called once per frame
	void Update()
	{
		Vector3 offset = Quaternion.Euler(0, 0, ship.transform.rotation.eulerAngles.z * -1.0F) * (player.transform.position - ship.transform.position);

		float offset_x = Mathf.Round(offset.x);
		float offset_y = Mathf.Round(offset.y);

		offset = new Vector3(offset_x, offset_y);
		offset = Quaternion.Euler(0, 0, ship.transform.rotation.eulerAngles.z) * offset;

		transform.position = offset + ship.transform.position;

		transform.rotation = ship.transform.rotation;

	}
}