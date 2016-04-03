using UnityEngine;
using System.Collections;

public class PowerWire : MonoBehaviour {

	public GameObject previousWire; // Either the wire before the one, or null
	public GameObject nextWire; // Either the wire after this one, or power

	public bool isWireOrigin;

	void start() {
		previousWire = null;
		nextWire = null;
	}

	/**
	 * Function to check whether a PowerWire eventually connects to power.
	 * @return true/false
	 */
	public bool WireGetConnectsToPower () {
		GameObject nw = nextWire;
		while (nw != null) {
			if (nw.tag == "Power") {
				return true;
			}
			nw = nw.GetComponent<PowerWire>().nextWire;
		}
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