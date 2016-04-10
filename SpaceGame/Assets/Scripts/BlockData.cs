using UnityEngine;
using System.Collections;

public class BlockData : MonoBehaviour {

    public GameObject ship;
    public Vector2 locate;
    public bool check;
    public Vector3 velocity;
    private Vector3 previousPosition;

    public enum direction {starbord, fore, port, aft}

    // Use this for initialization
    void Start () {
        ship.GetComponent<GridData>().blocks.Add(gameObject);
        velocity = new Vector3(0, 0, 0);
		ship.GetComponent<GridData>().recalcGridColliders();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //Keep track of velocity of the block
        velocity.Set((transform.position - previousPosition).x, (transform.position - previousPosition).y, 0);
        velocity = velocity / Time.deltaTime;
        previousPosition = transform.position;
    }

	public void recalcValidColliders()
	{
		GameObject colliderSet = gameObject.transform.Find("ColliderSet").gameObject;//colliderSet is the parent for the mouse colliders
		GameObject collider;
		GameObject ShipRoomHitbox = gameObject.transform.Find("ShipRoomHitbox").gameObject;

		Debug.Log(locate.ToString());

		collider = colliderSet.transform.Find("ForeCollider").gameObject;//get the child object named collider
		if (ship.GetComponent<GridData> ().getAdjacent (gameObject.GetComponent<BlockData> ().locate, BlockData.direction.fore) != null) 
		{
			Debug.Log("Fore");
			collider.GetComponent<BoxCollider2D> ().enabled = false;
			ShipRoomHitbox.transform.Find("ForeShipDoorHitbox").GetComponent<BoxCollider2D>().enabled = false;
		}
		else
		{
			collider.GetComponent<BoxCollider2D> ().enabled = true;
			ShipRoomHitbox.transform.Find("ForeShipDoorHitbox").GetComponent<BoxCollider2D>().enabled = true;
		}

		//Same for the rest
		collider = colliderSet.transform.Find ("PortCollider").gameObject;
		if (ship.GetComponent<GridData> ().getAdjacent (gameObject.GetComponent<BlockData> ().locate, BlockData.direction.port) != null)
		{
			Debug.Log("Port");
			collider.GetComponent<BoxCollider2D> ().enabled = false;
			ShipRoomHitbox.transform.Find("PortShipDoorHitbox").GetComponent<BoxCollider2D>().enabled = false;
		}
		else
		{
			collider.GetComponent<BoxCollider2D> ().enabled = true;
			ShipRoomHitbox.transform.Find("PortShipDoorHitbox").GetComponent<BoxCollider2D>().enabled = true;
		}

		collider = colliderSet.transform.Find ("AftCollider").gameObject;
		if (ship.GetComponent<GridData> ().getAdjacent (gameObject.GetComponent<BlockData> ().locate, BlockData.direction.aft) != null) 
		{
			Debug.Log("Aft");
			collider.GetComponent<BoxCollider2D> ().enabled = false;
			ShipRoomHitbox.transform.Find("AftShipDoorHitbox").GetComponent<BoxCollider2D>().enabled = false;
		}
		else
		{
			collider.GetComponent<BoxCollider2D> ().enabled = true;
			ShipRoomHitbox.transform.Find("AftShipDoorHitbox").GetComponent<BoxCollider2D>().enabled = true;
		}


		collider = colliderSet.transform.Find("StarbordCollider").gameObject;
		if (ship.GetComponent<GridData> ().getAdjacent (gameObject.GetComponent<BlockData> ().locate, BlockData.direction.starbord) != null)  
		{
			Debug.Log("Starbord");
			collider.GetComponent<BoxCollider2D> ().enabled = false;
			ShipRoomHitbox.transform.Find("StarbordShipDoorHitbox").GetComponent<BoxCollider2D>().enabled = false;
		}
		else
		{
			collider.GetComponent<BoxCollider2D> ().enabled = true;
			ShipRoomHitbox.transform.Find("StarbordShipDoorHitbox").GetComponent<BoxCollider2D>().enabled = true;
		}
	}

    public void setDirection(Vector2 prevLoc, direction direct)
    {
        if (direct == direction.fore)//fore
        {
            locate = new Vector2(prevLoc.x, prevLoc.y + 1);
        }

        if (direct == direction.aft)//aft
        {
            locate = new Vector2(prevLoc.x, prevLoc.y - 1);
        }

        if (direct == direction.starbord)//starbord
        {
            locate = new Vector2(prevLoc.x + 1, prevLoc.y);
        }

        if (direct == direction.port)//port
        {
            locate = new Vector2(prevLoc.x - 1, prevLoc.y);
        }
    }
}
