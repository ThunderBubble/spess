using UnityEngine;
using System.Collections;

public class WiringGlobal : MonoBehaviour {
	
	/**
	 * Function to get the wiring object's type.
	 * @return String containing the type tag
	 */
	public string GetWireType() {
		return tag;
	}

	/**
	 * Returns whether the wiring object is a wire.
	 * @return True/false
	 */
	public bool IsWire() {
		if (tag == "PowerWire" || tag == "ExhaustWire") {
			return true;
		}
		return false;
	}

	/**
	 * Returns whether the wiring object is a source.
	 * @return True/false
	 */
	public bool IsSource() {
		if (tag == "Power" || tag == "Exhaust") {
			return true;
		}
		return false;
	}
}
