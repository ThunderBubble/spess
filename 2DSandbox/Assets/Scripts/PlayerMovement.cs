using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float force;
	public float frictionKinetic;
	public float frictionStatic;
	public float maxSpeed;

	private Rigidbody2D myBody;

	public GameObject floor;
	private Rigidbody2D shipBody;

	private Vector2 relativeForce;

	// Use this for initialization
	void Start()
	{
		myBody = GetComponent<Rigidbody2D>();
		shipBody = floor.GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		relativeForce = (shipBody.velocity - myBody.velocity) / Time.deltaTime * myBody.mass; // dV/dt * m
		// Friction relative to ship
		if (relativeForce.magnitude > frictionStatic) { // Check if we have sufficient force to overcome static friction
			myBody.AddForce((shipBody.velocity - myBody.velocity).normalized * frictionKinetic); // Kinetic friction
		} else {
			myBody.AddForce(relativeForce); // Static friction - this should keep the player in place
		}

		// myBody.AddForce(relativeForce); // Stop the player relative to the ship

		// Movement with WASD
		if (Input.GetKey(KeyCode.W)) {
			myBody.AddForce(new Vector2(0, force));
		}
		if (Input.GetKey(KeyCode.A)) {
			myBody.AddForce(new Vector2(-1 * force, 0));
		}
		if (Input.GetKey(KeyCode.S)) {
			myBody.AddForce(new Vector2(0, -1 * force));
		}
		if (Input.GetKey(KeyCode.D)) {
			myBody.AddForce(new Vector2(force, 0));
		}

		myBody.AddForce(relativeForce / maxSpeed); // Cap the player's speed
	}
}