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
		// Direction the ground is moving relative to the player (rotational only)
		Vector3 groundDirection = Vector3.Cross(shipBody.worldCenterOfMass - myBody.worldCenterOfMass, Vector3.forward).normalized;
		// Net velocity of the ground relative to the player
		Vector2 netGroundVelocity = shipBody.velocity + new Vector2(groundDirection.x,groundDirection.y) * shipBody.angularVelocity * (shipBody.worldCenterOfMass - myBody.worldCenterOfMass).magnitude * Mathf.PI / 180F;
		// Net force needed to keep the player stationary relative to the ship
		relativeForce = (netGroundVelocity - myBody.velocity) * myBody.mass / Time.fixedDeltaTime;

		// Friction relative to ship
		if (relativeForce.magnitude > frictionStatic * myBody.mass) { // Check if we have sufficient force to overcome static friction
			myBody.AddForce(relativeForce.normalized * frictionKinetic); // Kinetic friction
		} else {
			myBody.AddForce(relativeForce); // Static friction - this should keep the player in place with respect to the ship
		}

		Vector2 moveDir = new Vector2(0, 0); // Vector to hold the direction of our movement

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
		if(moveDir.magnitude > 0)
			myBody.AddForce(moveDir.normalized * myBody.mass * speed * (1 - (relativeForce.magnitude / maxSpeed)));
	}
}