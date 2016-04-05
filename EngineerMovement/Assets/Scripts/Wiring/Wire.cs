using UnityEngine;
using System.Collections;

public class Wire : MonoBehaviour
{
	private GameObject previousWire; // Either the wire before the one, or null
	private GameObject nextWire; // Either the wire after this one, null, or power

	private bool isWireOrigin;

	void start()
	{
		previousWire = null;
		nextWire = null;

		tag = "";
	}

	public GameObject GetPrevious()
	{
		return previousWire;
	}

	public void SetPrevious(GameObject wire)
	{
		previousWire = wire;
	}

	public GameObject GetNext()
	{
		return nextWire;
	}

	public void SetNext(GameObject wire)
	{
		nextWire = wire;
	}

	public bool IsOrigin()
	{
		return isWireOrigin;
	}

	public void SetOrigin(bool isOrigin)
	{
		isWireOrigin = isOrigin;
	}

	/**
	 * Function to check whether a PowerWire eventually connects to power.
	 * @return true/false
	 */
	public bool WireGetConnectsToPower()
	{
		// Check next blocks
		GameObject wire = nextWire;
		while (wire != null) {
			if (wire.tag == "Power") {
				return true;
			}
			wire = wire.GetComponent<Wire>().GetNext();
		}

		// Check previous blocks
		wire = previousWire;
		while (wire != null) {
			if (wire.GetComponent<WiringGlobal>().GetType() == "Power") {
				return true;
			}
			wire = wire.GetComponent<Wire>().GetPrevious();
		}

		// Return false if we haven't found one
		return false;
	}

	/**
	 * Get the head of the current wire (not necessarily an origin wire).
	 * @return The head PowerWire.
	 */
	public GameObject WireGetHead()
	{
		GameObject pw = gameObject;
		while (pw.GetComponent<Wire>().previousWire != null) {
			pw = pw.GetComponent<Wire>().previousWire;
		}
		return pw;
	}
}