using UnityEngine;
using System.Collections;

public class WiringGlobal : MonoBehaviour {

	public string tagDesired;

	void Start() {
		if (tagDesired != "") {
			tag = tagDesired;
		}
	}

	/**
	 * Function to get the wiring object's type.
	 * @return String containing the type tag
	 */
	public string GetType() {
		return tag;
	}

	/**
	 * Function to set the wiring object's type.
	 * @param String containing the desired type tag
	 * @return Whether the given tag is a valid type
	 */
	public bool SetType(string type) {
		if (type == "Power" || type == "Exhaust") {
			tag = type;
			return true;
		}
		return false;
	}

	public bool IsWire() {
		if (tag == "PowerWire" || tag == "ExhaustWire") {
			return true;
		}
		return false;
	}
}
