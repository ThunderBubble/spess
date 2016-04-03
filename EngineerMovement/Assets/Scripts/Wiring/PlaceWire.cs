using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceWire : MonoBehaviour
{
	// Constants
	private const float GRIDSIZE = 1.0F;

	// Public variables
	public GameObject powerWirePrefab;
	public List<GameObject> wires;

	// Private variables
	private bool placingPowerWire;
	private bool placingExhaustWire;

	private GameObject previousWire;

	void Start()
	{
		ClearInputHist();
	}

	void Update()
	{
		// Check for input, check if the position is empty, and check if we placed a wire already
		if (Input.GetKeyDown(KeyCode.E)) {
			if (CheckPosition(transform.localPosition) == null) {
				if (placingPowerWire) {
					if (CheckDirections(transform.localPosition, previousWire)) {
						previousWire = AddConnectingWire(previousWire, transform.position);
						wires.Add(previousWire);
					}
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
	public GameObject AddConnectingWire(GameObject previousWire, Vector3 position)
	{
		// Create a new wire
		GameObject wire = Instantiate(previousWire, position, previousWire.transform.parent.rotation) as GameObject;
		wire.transform.SetParent(previousWire.transform.parent);

		previousWire.GetComponent<PowerWire>().nextWire = wire;
		wire.GetComponent<PowerWire>().previousWire = previousWire;
		wire.GetComponent<PowerWire>().isWireOrigin = false;

		return wire;
	}

	/***** Helper methods *****/

	// Clear the state of the wire placer
	void ClearInputHist()
	{
		placingPowerWire = false;
		placingExhaustWire = false;
		previousWire = null;
	}

	// Returns whether the given position has a wire in it
	GameObject CheckPosition(Vector3 position)
	{
		int i = 0;
		while (i < wires.Count) {
			if (wires[i].transform.localPosition == position) {
				return wires[i];
			}
			i++;
		}
		return null;
	}

	// Returns whether a matching wire is in any cardinal direction
	bool CheckDirections(Vector3 position, GameObject wire)
	{
		// North
		GameObject result = CheckPosition(position + new Vector3(0, GRIDSIZE));
		if (result != null) {
			if (result.tag == wire.tag) {
				return true;
			}
		}

		// East
		result = CheckPosition(position + new Vector3(GRIDSIZE, 0));
		if (result != null) {
			if (result.tag == wire.tag) {
				return true;
			}
		}

		// South
		result = CheckPosition(position + new Vector3(0, -1F * GRIDSIZE));
		if (result != null) {
			if (result.tag == wire.tag) {
				return true;
			}
		}

		// West
		result = CheckPosition(position + new Vector3(-1F * GRIDSIZE, 0));
		if (result != null) {
			if (result.tag == wire.tag) {
				return true;
			}
		}
		return false;
	}
}