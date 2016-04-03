using UnityEngine;
using System.Collections;

public class PlaceWire : MonoBehaviour
{
	public GameObject powerWirePrefab;
	public GameObject ship;

	private bool placingPowerWire = false;
	private bool placingExhaustWire = false;

	private GameObject previousWire;

	void Update()
	{
		if (!gameObject.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Wiring"))) {
			if (Input.GetKeyDown(KeyCode.E)) {
				AddNewWire(powerWirePrefab, transform.position, ship.transform);
			}
		}
	}

	/**
	 * Creates the start of a wire.
	 * @param Prefab wire
	 * @param Position to create the new wire at
	 * @param Parent (probably the ship)
	 * @return The new wire
	 */
	public GameObject AddNewWire(GameObject prefab, Vector3 position, Transform parent)
	{
		GameObject wire = Instantiate(prefab, position, parent.rotation) as GameObject;
		wire.transform.SetParent(parent);
		wire.GetComponent<PowerWire>().isWireOrigin = true;
		return wire;
	}

	/**
	 * Creates a wire connecting to connectedWire at position offset
	 * @param The original wire
	 * @param Position to create the new wire at
	 * @return The new wire
	 */
	public GameObject AddConnectingWire(GameObject connectingWire, Vector3 position)
	{
		// Create a new wire
		GameObject wire = Instantiate(connectingWire, position, connectingWire.transform.parent.rotation) as GameObject;
		// Set up the wire's parent
		wire.transform.SetParent(connectingWire.transform.parent);
		connectingWire.GetComponent<PowerWire>().nextWire = wire;
		wire.GetComponent<PowerWire>().previousWire = connectingWire;
		wire.GetComponent<PowerWire>().isWireOrigin = false;
		return wire;
	}
}