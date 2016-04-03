using UnityEngine;
using System.Collections;

public class ShipMovement : MonoBehaviour {
	
	private Rigidbody2D body;
	public float thrust;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey(KeyCode.Space)) {
			body.AddForce(transform.right.normalized * thrust); // Apply thrust forward
		}
	}
}
