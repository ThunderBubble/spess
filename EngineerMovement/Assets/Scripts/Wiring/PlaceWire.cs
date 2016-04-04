using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceWire : MonoBehaviour
{
	// Constants
	private const float GRIDSIZE = 1.0F;

	// Public variables
	public GameObject powerWirePrefab;
	public GameObject exhaustWirePrefab;
	public List<GameObject> wires = new List<GameObject>();

	// Private variables
	private bool placingPowerWire;
	private bool placingExhaustWire;

	private GameObject previousWire;

	// Controls
	private const KeyCode placePower = KeyCode.E;
	private const KeyCode placeExhaust = KeyCode.Q;
	private const KeyCode cancel = KeyCode.F;

	void Start()
	{
		ClearInputHist();
	}

	void Update()
	{
		/***** Code for adding power wires *****/

		// Check for input, check if the position is empty, and check if we placed a wire already
		if (Input.GetKeyDown(placePower)) {
			GameObject thisSquare = CheckPosition(transform.localPosition);

			// Nothing in this square; check to see if we can add a new block
			if (thisSquare == null) {
				if (placingPowerWire) {
					if (CheckDirections(transform.localPosition, previousWire)) {
						previousWire = AddConnectingPowerWire(previousWire, transform.position);
						wires.Add(previousWire);
					}
				} else if (placingExhaustWire == false) {
					previousWire = AddNewPowerWire(powerWirePrefab, transform.position, transform.parent);
					wires.Add(previousWire);
					placingPowerWire = true;
				}
			} 

			// Power in this square; check if we can connect a wire to power
			else if (thisSquare.tag == "Power") {
				if (placingPowerWire && CheckDirections(transform.localPosition, previousWire)) {
					previousWire.GetComponent<PowerWire>().nextWire = thisSquare;
					placingPowerWire = false;
				}
			}
		}

		/***** Code for adding exhaust wires *****/

		// Check for input, check if the position is empty, and check if we placed a wire already
		if (Input.GetKeyDown(placeExhaust)) {
			GameObject thisSquare = CheckPosition(transform.localPosition);

			// Nothing in this square; check to see if we can add a new block
			if (thisSquare == null) {
				if (placingExhaustWire) {
					if (CheckDirections(transform.localPosition, previousWire)) {
						previousWire = AddConnectingExhaustWire(previousWire, transform.position);
						wires.Add(previousWire);
					}
				} else if (placingPowerWire == false) {
					previousWire = AddNewExhaustWire(exhaustWirePrefab, transform.position, transform.parent);
					wires.Add(previousWire);
					placingExhaustWire = true;
				}
			} 

			// Exhaust in this square; check if we can connect a wire to power
			else if (thisSquare.tag == "Exhaust") {
				if (placingExhaustWire && CheckDirections(transform.localPosition, previousWire)) {
					previousWire.GetComponent<ExhaustWire>().nextWire = thisSquare;
					placingExhaustWire = false;
				}
			}
		}

		/***** Code for cancelling the current wire *****/

		if (Input.GetKeyDown(cancel)) {
			ClearInputHist();
		}
	}

	/**
	 * Creates the start of a power wire.
	 * @param Prefab wire
	 * @param Position to create the new wire at
	 * @param Parent (probably the ship)
	 * @return The new wire
	 */
	public GameObject AddNewPowerWire(GameObject prefab, Vector3 position, Transform parent)
	{
		GameObject wire = Instantiate(prefab, position, parent.rotation) as GameObject;
		wire.transform.SetParent(parent);

		wire.GetComponent<PowerWire>().isWireOrigin = true;
		wire.GetComponent<PowerWire>().wireController = gameObject;

		return wire;
	}

	/**
	 * Creates the start of an exhaust wire.
	 * @param Prefab wire
	 * @param Position to create the new wire at
	 * @param Parent (probably the ship)
	 * @return The new wire
	 */
	public GameObject AddNewExhaustWire(GameObject prefab, Vector3 position, Transform parent)
	{
		GameObject wire = Instantiate(prefab, position, parent.rotation) as GameObject;
		wire.transform.SetParent(parent);

		wire.GetComponent<ExhaustWire>().isWireOrigin = true;
		wire.GetComponent<ExhaustWire>().wireController = gameObject;

		return wire;
	}

	/**
	 * Creates a power wire connecting to connectedWire at position offset
	 * @param The original wire
	 * @param Position to create the new wire at
	 * @return The new wire
	 */
	public GameObject AddConnectingPowerWire(GameObject previousWire, Vector3 position)
	{
		// Create a new wire
		GameObject wire = Instantiate(previousWire, position, previousWire.transform.parent.rotation) as GameObject;
		wire.transform.SetParent(previousWire.transform.parent);

		previousWire.GetComponent<PowerWire>().nextWire = wire;
		wire.GetComponent<PowerWire>().previousWire = previousWire;
		wire.GetComponent<PowerWire>().isWireOrigin = false;
		wire.GetComponent<PowerWire>().wireController = gameObject;

		return wire;
	}

	/**
	 * Creates an exhaust wire connecting to connectedWire at position offset
	 * @param The original wire
	 * @param Position to create the new wire at
	 * @return The new wire
	 */
	public GameObject AddConnectingExhaustWire(GameObject previousWire, Vector3 position)
	{
		// Create a new wire
		GameObject wire = Instantiate(previousWire, position, previousWire.transform.parent.rotation) as GameObject;
		wire.transform.SetParent(previousWire.transform.parent);

		previousWire.GetComponent<ExhaustWire>().nextWire = wire;
		wire.GetComponent<ExhaustWire>().previousWire = previousWire;
		wire.GetComponent<ExhaustWire>().isWireOrigin = false;
		wire.GetComponent<ExhaustWire>().wireController = gameObject;

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
			if ((wires[i].transform.localPosition - position).magnitude < 0.05) {
				return wires[i];
			}
			i++;
		}
		return null;
	}

	// Returns whether a matching wire is in any cardinal direction
	bool CheckDirections(Vector3 position, GameObject wire)
	{
		GameObject resultWire;

		// North
		resultWire = CheckPosition(position + new Vector3(0, GRIDSIZE));
		if (resultWire == wire) return true;

		// East
		resultWire = CheckPosition(position + new Vector3(GRIDSIZE, 0));
		if (resultWire == wire) return true;

		// South
		resultWire = CheckPosition(position + new Vector3(0, -1F * GRIDSIZE));
		if (resultWire == wire) return true;

		// West
		resultWire = CheckPosition(position + new Vector3(-1F * GRIDSIZE, 0));
		if (resultWire == wire) return true;
		return false;
	}
}