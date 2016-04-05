using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WireController : MonoBehaviour
{
	// Datatypes
	enum State
	{
		NONE,
		PENDING,
		PLACING_POWER_FROM_ORIGIN,
		PLACING_POWER_FROM_SOURCE,
		PLACING_EXHAUST_FROM_ORIGIN,
		PLACING_EXHAUST_FROM_SOURCE}

	;

	// Constants
	private const float GRIDSIZE = 1.0F;

	// Public variables
	public GameObject wirePrefab;

	// Private variables
	private State state = State.PENDING;

	private GameObject previous;

	private List<GameObject> wires = new List<GameObject>();

	// Controls
	public KeyCode placePower = KeyCode.E;
	public KeyCode placeExhaust = KeyCode.Q;
	public KeyCode cancel = KeyCode.F;

	void Start()
	{
		ClearState();
	}

	void Update()
	{
		GameObject objectHere = CheckPosition(transform.position);

		switch (state) {
			case State.PENDING:
				if (Input.GetKeyDown(placePower)) {
					// Start placing a new power wire
					if (objectHere == null) {
						previous = AddNewWire("PowerWire", transform.position, transform.parent);
						state = State.PLACING_POWER_FROM_ORIGIN;
					}

					// Start placing a power wire from a power source
					else if (objectHere.GetComponent<WiringGlobal>().GetType() == "Power") {
						previous = objectHere;
						state = State.PLACING_POWER_FROM_SOURCE;
					}
				}
				break;

			case State.PLACING_POWER_FROM_ORIGIN:
				if (Input.GetKeyDown(placePower)) {
					// Continue an existing wire
					if (objectHere == null) {
						if (CheckDirections(transform.position, previous)) {
							previous = AddConnectingWire("PowerWire", previous, null, transform.position, transform.parent);
						}
					}

					// Connect a wire to a power source
					else if (objectHere.GetComponent<WiringGlobal>().GetType() == "Power") {
						previous.GetComponent<Wire>().SetNext(objectHere);

						// Test code
						//if (previous.GetComponent<Wire>().WireGetHead().GetComponent<Wire>().WireGetConnectsToPower()) {
						//	Debug.Log("Connected to power!");
						//} else {
						//	Debug.Log("Not connected to power!");
						//}

						ClearState();
					}
				}

				// Reset to NONE
				if (Input.GetKeyDown(cancel)) {
					ClearState();
				}
				break;

			case State.PLACING_POWER_FROM_SOURCE:
				if (Input.GetKeyDown(placePower)) {
					// Continue an existing wire
					if (objectHere == null && CheckDirections(transform.position, previous)) {
						previous = AddConnectingWire("PowerWire", null, previous, transform.position, transform.parent);
					}
				}

				// Reset to NONE
				if (Input.GetKeyDown(cancel)) {
					ClearState();
				}
				break;
		}
	}

	/**
	 * Adds an object to the tracked list.
	 * @param GameObject to be added
	 */
	public void TrackWireObject(GameObject obj)
	{
		wires.Add(obj);
	}

	/**
	 * Creates the start of a power wire.
	 * @param Type ("PowerWire" or "ExhaustWire")
	 * @param Position to create the new wire at
	 * @param Parent (probably the ship)
	 * @return The new wire
	 */
	public GameObject AddNewWire(string type, Vector3 position, Transform parent)
	{
		GameObject wire = Instantiate(wirePrefab, position, parent.rotation) as GameObject;
		wire.transform.SetParent(parent);

		wire.GetComponent<WiringGlobal>().SetType(type);
		wire.GetComponent<Wire>().SetOrigin(true);

		wires.Add(wire);
		return wire;
	}

	/**
	 * Creates a power wire connecting to connectedWire at position offset
	 * @param Previous wire (or null)
	 * @param Next wire/source (or null)
	 * @param Position to create the new wire at
	 * @return The new wire
	 */
	public GameObject AddConnectingWire(string type, GameObject previousWire, GameObject nextWire, Vector3 position, Transform parent)
	{
		GameObject wire = Instantiate(wirePrefab, position, parent.rotation) as GameObject;
		wire.transform.SetParent(parent);

		// Previous wire
		if (previousWire != null) {
			if (previousWire.GetComponent<WiringGlobal>().IsWire()) {
				previousWire.GetComponent<Wire>().SetNext(wire);
			}
		}

		// Next wire
		if (nextWire != null) {
			if (nextWire.GetComponent<WiringGlobal>().IsWire()) {
				nextWire.GetComponent<Wire>().SetPrevious(wire);
			}
		}
		
		// New wire
		wire.GetComponent<WiringGlobal>().SetType(type);
		wire.GetComponent<Wire>().SetPrevious(previousWire);
		wire.GetComponent<Wire>().SetOrigin(false);

		wires.Add(wire);
		return wire;
	}

	/**
	 * Destroys a wire and sets its neighbors' links to null.
	 * @param Wire to destroy
	 */
	public void DestroyWire(GameObject wire)
	{
		GameObject previous = wire.GetComponent<Wire>().GetPrevious();
		if (previous != null)
			previous.GetComponent<Wire>().SetNext(null);
		GameObject next = wire.GetComponent<Wire>().GetNext();
		if (next != null)
			next.GetComponent<Wire>().SetNext(null);

		wires.Remove(wire);
		Destroy(wire);
	}

	/***** Helper methods *****/

	// Clear the state of the wire placer
	void ClearState()
	{
		state = State.PENDING;
		previous = null;
	}

	// Returns the wire in the given position in relative space (null if none)
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

	// Returns whether a certain wire is in any cardinal direction
	bool CheckDirections(Vector3 position, GameObject wire)
	{
		GameObject resultWire;

		// North
		resultWire = CheckPosition(position + new Vector3(0, GRIDSIZE));
		if (resultWire == wire)
			return true;

		// East
		resultWire = CheckPosition(position + new Vector3(GRIDSIZE, 0));
		if (resultWire == wire)
			return true;

		// South
		resultWire = CheckPosition(position + new Vector3(0, -1F * GRIDSIZE));
		if (resultWire == wire)
			return true;

		// West
		resultWire = CheckPosition(position + new Vector3(-1F * GRIDSIZE, 0));
		if (resultWire == wire)
			return true;
		return false;
	}
}