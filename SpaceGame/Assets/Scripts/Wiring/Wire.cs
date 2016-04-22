using UnityEngine;
using System.Collections;

public class Wire : MonoBehaviour
{
	public GameObject previousWire; // Either the wire before the one, or null
	public GameObject nextWire; // Either the wire after this one, null, or power

	public bool isWireOrigin;

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
	 * Function to set the wire's type. Assigns the type and returns true if the type was valid, otherwise returns false.
	 * @param String containing the desired type tag
	 * @return Whether the given tag is a valid type
	 */
	public bool SetType(string type) {
		if (type == "PowerWire" || type == "ExhaustWire") {
			tag = type;
			return true;
		}
		return false;
	}

	/**
	 * Function to check whether a Wire eventually connects to power. Returns false if given an invalid tag.
	 * @return true/false
	 */
	public bool WireGetConnectsToSource()
	{
		GameObject wire;
		string typeDesired;

		// Set up the tag to check
		if (tag == "PowerWire")
			typeDesired = "Power";
		else
			typeDesired = "Exhaust";
			
		// Check next blocks
		wire = nextWire;
		while (wire != null) {
			if (wire.GetComponent<WiringGlobal>().GetWireType() == typeDesired) {
				return true;
			}
			wire = wire.GetComponent<Wire>().GetNext();
		}

		// Check previous blocks
		wire = previousWire;
		while (wire != null) {
			if (wire.GetComponent<WiringGlobal>().GetWireType() == typeDesired) {
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