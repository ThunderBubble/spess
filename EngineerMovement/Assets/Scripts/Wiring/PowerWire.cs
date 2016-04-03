using UnityEngine;
using System.Collections;

public class PowerWire : MonoBehaviour {

	public GameObject previousWire; // Either the wire before the one, or null
	public GameObject nextWire; // Either the wire after this one, or power

	public bool isWireOrigin;

	void start() {
		previousWire = null;
		nextWire = null;

		name = "PowerWire";
	}

	/**
	 * Function to check whether a PowerWire eventually connects to power.
	 * @return true/false
	 */
	public bool WireGetConnectsToPower () {
		// Check next blocks
		GameObject wire = nextWire;
		while (wire != null) {
			if (wire.tag == "Power") {
				return true;
			}
			wire = wire.GetComponent<PowerWire>().nextWire;
		}

		// Check previous blocks
		wire = previousWire;
		while (wire != null) {
			if (wire.tag == "Power") {
				return true;
			}
			wire = wire.GetComponent<PowerWire>().previousWire;
		}

		// Return false if we haven't found one
		return false;
	}

	/**
	 * Get the head of the current wire (not necessarily an origin wire).
	 * @return The head PowerWire.
	 */
	public GameObject WireGetHead () {
		GameObject pw = gameObject;
		while (pw.GetComponent<PowerWire>().previousWire != null) {
			pw = pw.GetComponent<PowerWire>().previousWire;
		}
		return pw;
	}
}