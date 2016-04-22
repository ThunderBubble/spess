using UnityEngine;
using System.Collections;

public class Source : MonoBehaviour {
	public GameObject wireController;

	void Start () {
		wireController.GetComponent<WireController>().TrackWireObject(gameObject);
	}

	/**
	 * Function to set the source's type.
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
}