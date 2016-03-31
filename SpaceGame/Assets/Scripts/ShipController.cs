using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

	private Rigidbody2D rb;

	public float speed;
	public float rotationalSpeed;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float rotate = Input.GetAxis ("Horizontal");
		float force = Input.GetAxis ("Vertical");
		float moveHorizontal = 0;
		float moveVertical = force;
		Vector2 movement = new Vector2(moveHorizontal, moveVertical);
		rb.AddRelativeForce (movement * speed);
		rb.AddTorque (-rotate * rotationalSpeed);
	}
}
