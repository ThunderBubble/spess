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

	public enum Event
	{
		ERROR,
		STARTED_POWER_FROM_ORIGIN,
		STARTED_POWER_FROM_SOURCE,
		CONTINUED_POWER,
		FINISHED_POWER}

	;

	// Constants
	private const float GRIDSIZE = 1.0F;

	// Public variables
	public GameObject wirePrefab;

	// Private variables
	private State state = State.PENDING;

	private GameObject previous;
	private GameObject objectHere;

	private List<GameObject> wires = new List<GameObject>();

	// Controls
	public KeyCode placePower = KeyCode.E;
	public KeyCode placeExhaust = KeyCode.Q;
	public KeyCode cancel = KeyCode.F;

	void Start()
	{
		PowerWireCancel();
	}

	void Update()
	{
		objectHere = CheckPosition(transform.localPosition);

		switch (state) {
			case State.PENDING:
				if (Input.GetKeyDown(placePower)) {
					WireControllerStartPowerWire(transform.position);
				}
				break;

			case State.PLACING_POWER_FROM_ORIGIN:
				if (Input.GetKeyDown(placePower)) {
					WireControllerContinuePowerWire(transform.localPosition);
				}

				// Reset to NONE
				if (Input.GetKeyDown(cancel)) {
					PowerWireCancel();
				}
				break;

			case State.PLACING_POWER_FROM_SOURCE:
				if (Input.GetKeyDown(placePower)) {
					WireControllerContinuePowerWire(transform.localPosition);
				}
				// Reset to NONE
				if (Input.GetKeyDown(cancel)) {
					PowerWireCancel();
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
	 * Start placing a power wire at local position position.
	 * @param Position to place the new wire
	 * @return Wire event corresponding to placing from origin, placing from source, or invalid location
	 */
	public Event WireControllerStartPowerWire(Vector3 position)
	{
		// Start placing a new power wire
		if (objectHere == null) {
			previous = AddNewWire("PowerWire", position, transform.parent);
			state = State.PLACING_POWER_FROM_ORIGIN;
			return Event.STARTED_POWER_FROM_ORIGIN;
		}

		// Start placing a power wire from a power source
		else if (objectHere.GetComponent<WiringGlobal>().GetWireType() == "Power") {
			previous = objectHere;
			state = State.PLACING_POWER_FROM_SOURCE;
			return Event.STARTED_POWER_FROM_SOURCE;
		} 

		// Couldn't place a wire
		else {
			return Event.ERROR;
		}
	}

	/**
	 * Continue a power wire at local position position.
	 * @param Position to place the new wire
	 * @return Wire event
	 */
	public Event WireControllerContinuePowerWire(Vector3 position)
	{
		// Currently placing from origin
		if (state == State.PLACING_POWER_FROM_ORIGIN) {
			// Make sure we're next to the previous wire
			if (CheckDirections(position, previous)) {
				// Make a new wire
				if (objectHere == null) {
					previous = AddConnectingWire("PowerWire", previous, null, transform.position, transform.parent);
					return Event.CONTINUED_POWER;
				}

				// Connect a wire to a power source
				else if (objectHere.GetComponent<WiringGlobal>().GetWireType() == "Power") {
					previous.GetComponent<Wire>().SetNext(objectHere);

					// Test code
//					if (previous.GetComponent<Wire>().WireGetHead().GetComponent<Wire>().WireGetConnectsToPower()) {
//						Debug.Log("Connected to power!");
//					} else {
//						Debug.Log("Not connected to power!");
//					}

					PowerWireCancel();
					return Event.FINISHED_POWER;
				}
			}
		}

		// Currently placing from source
		if (state == State.PLACING_POWER_FROM_SOURCE) {
			// Continue an existing wire
			if (objectHere == null && CheckDirections(transform.localPosition, previous)) {
				previous = AddConnectingWire("PowerWire", null, previous, transform.position, transform.parent);
				return Event.CONTINUED_POWER;
			}
		}
		return Event.ERROR;
	}

	/**
	 * Clears the input state of the wire controller.
	 */
	public void PowerWireCancel()
	{
		state = State.PENDING;
		previous = null;
	}

	/**
	 * Creates the start of a power wire.
	 * @param Type ("PowerWire" or "ExhaustWire")
	 * @param Global osition to create the new wire at
	 * @param Parent (probably the ship)
	 * @return The new wire
	 */
	private GameObject AddNewWire(string type, Vector3 position, Transform parent)
	{
		GameObject wire = InstantiateWire(type, position, parent);
		wire.GetComponent<Wire>().SetOrigin(true);
		return wire;
	}

	/**
	 * Creates a power wire connecting to connectedWire at position offset
	 * @param Previous wire (or null)
	 * @param Next wire/source (or null)
	 * @param Global position to create the new wire at
	 * @return The new wire
	 */
	private GameObject AddConnectingWire(string type, GameObject previousWire, GameObject nextWire, Vector3 position, Transform parent)
	{
		GameObject wire = InstantiateWire(type, position, parent);

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
		wire.GetComponent<Wire>().SetPrevious(previousWire);
		wire.GetComponent<Wire>().SetOrigin(false);

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

	// Returns the wire in the given position in local space (null if none)
	GameObject CheckPosition(Vector3 position)
	{
		int i = 0;
		while (i < wires.Count) {
			if ((wires[i].transform.localPosition - position).magnitude < 0.5) {
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
		resultWire = CheckPosition(position + new Vector3(GRIDSIZE, 0));
		if (resultWire == wire)
			return true;

		// East
		resultWire = CheckPosition(position + new Vector3(0, GRIDSIZE));
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

	Vector3 LocalToGlobal(Vector3 localPosition, Transform parent) {
		float angle = parent.rotation.z * Mathf.PI / 180F;
		float x = localPosition.x * Mathf.Cos(angle) + localPosition.y * Mathf.Sin(angle);
		float y = localPosition.x * Mathf.Sin(angle) + localPosition.y * Mathf.Cos(angle);

		return new Vector3(parent.position.x + x, parent.position.y + y);
	}

	// Creates a wire and does some setup
	GameObject InstantiateWire(string type, Vector3 position, Transform parent) {
		GameObject wire = Instantiate(wirePrefab, position, parent.rotation) as GameObject;

		wire.transform.SetParent(parent);
		wire.GetComponent<WiringGlobal>().SetType(type);
		wires.Add(wire);

		return wire;
	}
}
	