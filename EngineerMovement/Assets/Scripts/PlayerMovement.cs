using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float speed;
	public float maxSpeed;
	public float frictionKinetic;
	public float frictionStatic;

	private Rigidbody2D myBody;

	public GameObject floor;
	private Rigidbody2D shipBody;

	private Vector2 relativeForce;

	// Use this for initialization
	void Start()
	{
		myBody = GetComponent<Rigidbody2D>();
		shipBody = floor.GetComponent<Rigidbody2D>();

		myBody.freezeRotation = true; // Keep our player from rotating because of collisions
	}

	void FixedUpdate()
	{
		Vector2 moveDir = new Vector2(0, 0);

		Vector2 linearVelocity = shipBody.velocity - myBody.velocity;
		Vector3 angularDirection = Vector3.Cross(shipBody.position - myBody.position, Vector3.forward).normalized; // Direction to compute angular velocity difference in
		// Magnitude of difference in angular velocity
		Vector2 angularVelocity = new Vector2(angularDirection.x,angularDirection.y) * shipBody.angularVelocity * (shipBody.position - myBody.position).magnitude * Mathf.PI / 180;
		// Force = velocity(linear component + angular component) * mass / time
		relativeForce = (linearVelocity + angularVelocity) * myBody.mass / Time.deltaTime;

		// Friction relative to ship
		if (relativeForce.magnitude > frictionStatic * myBody.mass) { // Check if we have sufficient force to overcome static friction
			myBody.AddForce(relativeForce.normalized * frictionKinetic); // Kinetic friction
		} else {
			myBody.AddForce(relativeForce); // Static friction - this should keep the player in place with respect to the ship
		}

		// Movement with WASD
		if (Input.GetKey(KeyCode.W)) {
			moveDir += new Vector2(0, 1);
		}
		if (Input.GetKey(KeyCode.A)) {
			moveDir += new Vector2(-1, 0);
		}
		if (Input.GetKey(KeyCode.S)) {
			moveDir += new Vector2(0, -1);
		}
		if (Input.GetKey(KeyCode.D)) {
			moveDir += new Vector2(1, 0);
		}
		if (moveDir.magnitude != 0)
			myBody.AddForce(moveDir.normalized * myBody.mass * speed);
		myBody.AddForce(relativeForce / maxSpeed); // Cap the player's speed
	}
}