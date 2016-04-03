using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceWire : MonoBehaviour
{
	public GameObject powerWirePrefab;

	private bool placingPowerWire;
	private bool placingExhaustWire;

	private GameObject previousWire;

	public List<GameObject> wires;

	void Start()
	{
		ClearInputHist();
	}

	void Update()
	{
		// Check for input, check if the position is empty, and check if we placed a wire already
		if (Input.GetKeyDown(KeyCode.E)) {
			if (CheckPositionEmpty(transform.position)) {
				if (placingPowerWire) {
					previousWire = AddConnectingWire(previousWire, transform.position);
					wires.Add(previousWire);
				} else {
					previousWire = AddNewWire(powerWirePrefab, transform.position, transform.parent);
					wires.Add(previousWire);
					placingPowerWire = true;
				}
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

	void ClearInputHist()
	{
		placingPowerWire = false;
		placingExhaustWire = false;
		previousWire = null;
	}

	// Returns whether the given position has a wire in it
	bool CheckPositionEmpty(Vector3 position)
	{
		int i = 0;
		while (i < wires.Count) {
			if (wires[i].transform.localPosition == position) {
				return false;
			}
		}
		return true;
	}
}