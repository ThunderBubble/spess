using UnityEngine;
using System.Collections;

public class AddRotation : MonoBehaviour {
	public float force;

	private Rigidbody2D myBody;

	void Start () {
		myBody = GetComponent<Rigidbody2D>();
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey(KeyCode.LeftArrow)) {
			myBody.AddTorque(force);
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			myBody.AddTorque(-1*force);
		}
	}
}
