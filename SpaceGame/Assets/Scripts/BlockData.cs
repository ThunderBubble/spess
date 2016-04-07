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
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //Keep track of velocity of the block
        velocity.Set((transform.position - previousPosition).x, (transform.position - previousPosition).y, 0);
        velocity = velocity / Time.deltaTime;
        previousPosition = transform.position;
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
