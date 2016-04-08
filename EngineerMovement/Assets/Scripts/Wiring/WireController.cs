using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WireController : MonoBehaviour
{
	// Datatypes
	public enum State
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
		FINISHED_POWER,
		STARTED_EXHAUST_FROM_ORIGIN,
		STARTED_EXHAUST_FROM_SOURCE,
		CONTINUED_EXHAUST,
		FINISHED_EXHAUST}

	;

	// Constants
	private const float GRIDSIZE = 1.0F;

	// Public variables
	public GameObject wirePrefab;

	// Private variables
	public State state = State.PENDING;

	public GameObject previous;
	public GameObject objectHere;

	private List<GameObject> wires = new List<GameObject>();

	// Controls
	public KeyCode placePower = KeyCode.E;
	public KeyCode placeExhaust = KeyCode.Q;
	public KeyCode finishWire = KeyCode.F;
	public KeyCode cancel = KeyCode.X;

	void Start()
	{
		WireControllerReset();
	}

	void Update()
	{
		objectHere = CheckPosition(transform.localPosition);

		if (Input.GetKeyDown(cancel)) {
			WireControllerReset();
		}

		switch (state) {
			case State.PENDING:
				// Start a power wire
				if (Input.GetKeyDown(placePower)) {
					WireControllerStartPowerWire();
				}
				// Start an exhaust wire
				if (Input.GetKeyDown(placeExhaust)) {
					WireControllerStartExhaustWire();
				}
				break;

			case State.PLACING_POWER_FROM_ORIGIN:
			case State.PLACING_POWER_FROM_SOURCE:
				// Continue a power wire
				if (Input.GetKeyDown(placePower)) {
					WireControllerContinuePowerWire();
				}
				// Finish power wire
				if (Input.GetKeyDown(finishWire)) {
					WireControllerFinishPowerWire();
				}
				break;

			case State.PLACING_EXHAUST_FROM_ORIGIN:
			case State.PLACING_EXHAUST_FROM_SOURCE:
				// Continue a exhaust wire
				if (Input.GetKeyDown(placeExhaust)) {
					WireControllerContinueExhaustWire();
				}
				// Finish exhaust wire
				if (Input.GetKeyDown(finishWire)) {
					WireControllerFinishExhaustWire();
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
	 * Start placing a power wire at the wire controller's position. Returns a wire event corresponding to placing from origin, placing from source, or invalid location.
	 * @return Power start from origin event, power start from source event, error event
	 */
	public Event WireControllerStartPowerWire()
	{
		// Start placing a new power wire
		if (objectHere == null) {
			previous = AddNewWire("PowerWire", transform.position, transform.parent);
			state = State.PLACING_POWER_FROM_ORIGIN;
			return Event.STARTED_POWER_FROM_ORIGIN;
		}

		// Start placing a power wire from a power source
		else if (objectHere.GetComponent<WiringGlobal>().GetWireType() == "Power") {
			previous = objectHere;
			state = State.PLACING_POWER_FROM_SOURCE;
			return Event.STARTED_POWER_FROM_SOURCE;
		}

		// Start placing a power wire from an unfinished wire
		else if (objectHere.GetComponent<WiringGlobal>().GetWireType() == "PowerWire") {
			// If next is empty we can start placing from here no matter what; we do this by default
			if (objectHere.GetComponent<Wire>().GetNext() == null) {
				previous = objectHere;
				state = State.PLACING_POWER_FROM_ORIGIN;
				return Event.CONTINUED_POWER;
			} 

			// If previous is empty we can start placing from here only if the block is not an origin
			else if (objectHere.GetComponent<Wire>().GetPrevious() == null && objectHere.GetComponent<Wire>().IsOrigin() == false) {
				previous = objectHere;
				state = State.PLACING_POWER_FROM_SOURCE;
				return Event.CONTINUED_POWER;
			}
		}

		// Couldn't place a wire
		return Event.ERROR;
	}

	/**
	 * Start placing a exhaust wire at the wire controller's position. Returns a wire event corresponding to placing from origin, placing from source, or invalid location.
	 * @return Exhaust start from origin event, exhaust start from source event, error event
	 */
	public Event WireControllerStartExhaustWire()
	{
		// Start placing a new exhaust wire
		if (objectHere == null) {
			previous = AddNewWire("ExhaustWire", transform.position, transform.parent);
			state = State.PLACING_EXHAUST_FROM_ORIGIN;
			return Event.STARTED_EXHAUST_FROM_ORIGIN;
		}

		// Start placing a exhaust wire from a exhaust source
		else if (objectHere.GetComponent<WiringGlobal>().GetWireType() == "Exhaust") {
			previous = objectHere;
			state = State.PLACING_EXHAUST_FROM_SOURCE;
			return Event.STARTED_EXHAUST_FROM_SOURCE;
		}

		// Start placing a exhaust wire from an unfinished wire
		else if (objectHere.GetComponent<WiringGlobal>().GetWireType() == "ExhaustWire") {
			// If next is empty we can start placing from here no matter what; we do this by default
			if (objectHere.GetComponent<Wire>().GetNext() == null) {
				previous = objectHere;
				state = State.PLACING_EXHAUST_FROM_ORIGIN;
				return Event.CONTINUED_EXHAUST;
			} 

			// If previous is empty we can start placing from here only if the block is not an origin
			else if (objectHere.GetComponent<Wire>().GetPrevious() == null && objectHere.GetComponent<Wire>().IsOrigin() == false) {
				previous = objectHere;
				state = State.PLACING_EXHAUST_FROM_SOURCE;
				return Event.CONTINUED_EXHAUST;
			}
		}

		// Couldn't place a wire
		return Event.ERROR;
	}

	/**
	 * Continue a power wire at the WireController's position. Returns a wire event corresponding to continuing a wire, finishing a wire, or invalid location. Returns an error if not currently placing a wire.
	 * @return Continued power event or error event
	 */
	public Event WireControllerContinuePowerWire()
	{
		// Currently placing from origin
		if (state == State.PLACING_POWER_FROM_ORIGIN) {
			// Make sure we're next to the previous wire
			if (CheckDirections(transform.localPosition, previous)) {
				// Make a new wire
				if (objectHere == null) {
					previous = AddConnectingWire("PowerWire", previous, null, transform.position, transform.parent);
					return Event.CONTINUED_POWER;
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
	 * Continue a exhaust wire at the wire controller's position. Returns a wire event corresponding to continuing a wire, finishing a wire, or invalid location. Returns an error if not currently placing a wire.
	 * @return Continued exhaust event, finished exhaust event, error event
	 */
	public Event WireControllerContinueExhaustWire()
	{
		// Currently placing from origin
		if (state == State.PLACING_EXHAUST_FROM_ORIGIN) {
			// Make sure we're next to the previous wire
			if (CheckDirections(transform.localPosition, previous)) {
				// Make a new wire
				if (objectHere == null) {
					previous = AddConnectingWire("ExhaustWire", previous, null, transform.position, transform.parent);
					return Event.CONTINUED_EXHAUST;
				}
			}
		}

		// Currently placing from source
		if (state == State.PLACING_EXHAUST_FROM_SOURCE) {
			// Continue an existing wire
			if (objectHere == null && CheckDirections(transform.localPosition, previous)) {
				previous = AddConnectingWire("ExhaustWire", null, previous, transform.position, transform.parent);
				return Event.CONTINUED_EXHAUST;
			}
		}

		return Event.ERROR;
	}

	/**
	 * Finish a power wire either by placing an origin wire or connecting to a power source. Position to be checked is taken as the WireController's position. Returns error if the appropriate operation could not be completed.
	 * @return Finished power event or error
	 */
	public Event WireControllerFinishPowerWire()
	{
		// Placing from origin
		if (state == State.PLACING_POWER_FROM_ORIGIN) {
			// Connect a wire to a power source
			if (objectHere != null && CheckDirections(transform.localPosition, previous) && objectHere.GetComponent<WiringGlobal>().GetWireType() == "Power") {
				previous.GetComponent<Wire>().SetNext(objectHere);

				WireControllerReset();
				return Event.FINISHED_POWER;
			}
		}

		// Placing from source
		if (state == State.PLACING_POWER_FROM_SOURCE) {
			if (objectHere == null && CheckDirections(transform.localPosition, previous)) {
				previous = AddOriginWire("PowerWire", previous, transform.position, transform.parent);

				WireControllerReset();
				return Event.FINISHED_POWER;
			}
		}

		return Event.ERROR;
	}

	/**
	 * Finish a exhaust wire either by placing an origin wire or connecting to a exhaust source. Position to be checked is taken as the WireController's position. Returns error if the appropriate operation could not be completed.
	 * @return Finished exhaust event or error
	 */
	public Event WireControllerFinishExhaustWire()
	{
		// Placing from origin
		if (state == State.PLACING_EXHAUST_FROM_ORIGIN) {
			// Connect a wire to a exhaust source
			if (objectHere != null && CheckDirections(transform.localPosition, previous) && objectHere.GetComponent<WiringGlobal>().GetWireType() == "Exhaust") {
				previous.GetComponent<Wire>().SetNext(objectHere);

				WireControllerReset();
				return Event.FINISHED_EXHAUST;
			}
		}

		// Placing from source
		if (state == State.PLACING_EXHAUST_FROM_SOURCE) {
			if (objectHere == null && CheckDirections(transform.localPosition, previous)) {
				previous = AddOriginWire("ExhaustWire", previous, transform.position, transform.parent);

				WireControllerReset();
				return Event.FINISHED_EXHAUST;
			}
		}

		return Event.ERROR;
	}

	/**
	 * Clears the input state of the wire controller.
	 */
	public void WireControllerReset()
	{
		state = State.PENDING;
		previous = null;
	}

	/**
	 * Creates the start of a wire.
	 * @param Type ("PowerWire" or "ExhaustWire")
	 * @param Global position to create the new wire at
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
	 * Creates a wire connecting to connectedWire at position position
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
		wire.GetComponent<Wire>().SetNext(nextWire);
		wire.GetComponent<Wire>().SetOrigin(false);

		return wire;
	}

	/**
	 * Creates an origin wire connecting to the given previousWire
	 * @param Type ("PowerWire" or "ExhaustWire")
	 * @param Global position to create the new wire at
	 * @param Parent (probably the ship)
	 * @return The new wire
	 */
	private GameObject AddOriginWire(string type, GameObject previousWire, Vector3 position, Transform parent)
	{
		GameObject wire = InstantiateWire(type, position, parent);

		// Next wire in the linked list, referred to as previousWire by the function declaration
		if (previousWire.GetComponent<WiringGlobal>().IsWire()) {
			previousWire.GetComponent<Wire>().SetPrevious(wire);
		}

		// New wire
		wire.GetComponent<Wire>().SetNext(previousWire);
		wire.GetComponent<Wire>().SetOrigin(true);
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
	GameObject CheckPosition(Vector3 localPosition)
	{
		int i = 0;
		while (i < wires.Count) {
			if ((wires[i].transform.localPosition - localPosition).magnitude < 0.05) {
				return wires[i];
			}
			i++;
		}
		return null;
	}

	// Returns whether a certain wire is in any cardinal direction
	bool CheckDirections(Vector3 position, GameObject wire)
	{
		// North
		if (CheckPosition(position + new Vector3(GRIDSIZE, 0)) == wire)
			return true;

		// East
		if (CheckPosition(position + new Vector3(0, GRIDSIZE)) == wire)
			return true;

		// South
		if (CheckPosition(position + new Vector3(0, -1F * GRIDSIZE)) == wire)
			return true;

		// West
		if (CheckPosition(position + new Vector3(-1F * GRIDSIZE, 0)) == wire)
			return true;

		return false;
	}

	Vector3 LocalToGlobal(Vector3 localPosition, Transform parent)
	{
		float angle = Mathf.Deg2Rad * parent.rotation.z;
		float x = localPosition.x * Mathf.Cos(angle) + localPosition.y * Mathf.Sin(angle);
		float y = localPosition.x * Mathf.Sin(angle) + localPosition.y * Mathf.Cos(angle);

		return new Vector3(parent.position.x + x, parent.position.y + y);
	}

	// Creates a wire and does some setup
	GameObject InstantiateWire(string type, Vector3 position, Transform parent)
	{
		GameObject wire = Instantiate(wirePrefab, position, parent.rotation) as GameObject;

		wire.transform.SetParent(parent);
		wire.GetComponent<Wire>().SetType(type);
		wires.Add(wire);

		return wire;
	}
}