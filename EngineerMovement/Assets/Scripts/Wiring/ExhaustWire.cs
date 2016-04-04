using UnityEngine;
using System.Collections;

public class ExhaustWire : MonoBehaviour {

	public GameObject previousWire; // Either the wire before the one, or null
	public GameObject nextWire; // Either the wire after this one, or power

	public bool isWireOrigin;

	void start() {
		previousWire = null;
		nextWire = null;

		name = "ExhaustWire";
	}

	/**
	 * Function to check whether an ExhaustWire eventually connects to power.
	 * @return true/false
	 */
	public bool WireGetConnectsToExhaust () {
		// Check next blocks
		GameObject wire = nextWire;
		while (wire != null) {
			if (wire.tag == "Exhaust") {
				return true;
			}
			wire = wire.GetComponent<ExhaustWire>().nextWire;
		}

		// Check previous blocks
		wire = previousWire;
		while (wire != null) {
			if (wire.tag == "Exhaust") {
				return true;
			}
			wire = wire.GetComponent<ExhaustWire>().previousWire;
		}

		// Return false if we haven't found one
		return false;
	}

	/**
	 * Get the head of the current wire (not necessarily an origin wire).
	 * @return The head ExhaustWire.
	 */
	public GameObject WireGetHead () {
		GameObject pw = gameObject;
		while (pw.GetComponent<ExhaustWire>().previousWire != null) {
			pw = pw.GetComponent<ExhaustWire>().previousWire;
		}
		return pw;
	}

	/**
	 * Destroys this instance and sets the connected wire(s) to have null connections.
	 */
	public void DestroySelf () {
		if (previousWire != null) {
			previousWire.GetComponent<ExhaustWire>().nextWire = null;
		}
		if (nextWire != null) {
			nextWire.GetComponent<ExhaustWire>().previousWire = null;
		}

		Destroy(gameObject);
	}
}