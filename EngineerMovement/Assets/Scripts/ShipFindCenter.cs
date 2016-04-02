using UnityEngine;
using System.Collections;

public class ShipFindCenter : MonoBehaviour
{
	private GameObject[] parts;

	public GameObject referencePart;

	void Start()
	{
	
	}

	void Update()
	{
		Vector3 center = new Vector3(0, 0, 0);
		float totalMass = 0;

		parts = GameObject.FindGameObjectsWithTag("ShipPiece");
		// Calculate the center of mass of all the ship pieces
		if (parts.Length > 0) {
			foreach (GameObject part in parts) {
				center += part.transform.position * part.GetComponent<Rigidbody2D>().mass;
				totalMass += part.GetComponent<Rigidbody2D>().mass;
			}
		}
		center /= totalMass;

		transform.position = center;

		// Calculate the angular velocity of the ship

	}
}
