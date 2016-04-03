using UnityEngine;
using System.Collections;

public class ShipRoomUpdateDoors : MonoBehaviour
{
	private GameObject parentBlock;

	static float DOORSIZE = 0.667F;
	static float WALL_WIDTH = 0.1F;

	private float XSCALE;
	private float XOFFSET;
	private float YSCALE;
	private float YOFFSET;

	// Corners
	private BoxCollider2D northWest;
	private BoxCollider2D northEast;
	private BoxCollider2D eastNorth;
	private BoxCollider2D eastSouth;
	private BoxCollider2D southWest;
	private BoxCollider2D southEest;
	private BoxCollider2D westNorth;
	private BoxCollider2D westSouth;

	// Flat walls
	private BoxCollider2D north;
	private BoxCollider2D east;
	private BoxCollider2D south;
	private BoxCollider2D west;

	// Use this for initialization
	void Start()
	{
		parentBlock = transform.parent.gameObject;

		XSCALE = 0.5F - DOORSIZE / 2;
		XOFFSET = 0.5F - XSCALE / 2;

		YSCALE = WALL_WIDTH;
		YOFFSET = 0.5F - YSCALE / 2;

		// Set up the north walls
		northWest = gameObject.AddComponent<BoxCollider2D>();
		northWest.offset = new Vector2(-1.0F * XOFFSET, YOFFSET);
		northWest.size = new Vector2(XSCALE, YSCALE);

		SetDoors(true, true, true, true);
	}

	// Sets the room colliders. True = contains door on that side.
	private void SetDoors(bool doorNorth, bool doorEast, bool doorSouth, bool doorWest)
	{
		north.enabled = !doorNorth;
		northWest.enabled = doorNorth;
		northEast.enabled = doorNorth;

		east.enabled = !doorEast;
		eastNorth.enabled = doorEast;
		eastSouth.enabled = doorEast;

	}
}
