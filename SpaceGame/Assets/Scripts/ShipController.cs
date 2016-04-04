using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;



public class ShipController : MonoBehaviour {

	private Rigidbody2D rb;

	public float speed;
	public float rotationalSpeed;
    public Transform prefab;
    public float L;
    public List<GameObject> blocks;
    public GameObject firstBlock;

    // Use this for initialization
    void Start () {
        //blocks.Add(firstBlock);
        firstBlock.GetComponent<BlockController>().locate = new Vector2(0, 0);
		rb = GetComponent<Rigidbody2D> ();
	}

    // Update is called once per frame
    void FixedUpdate () {

        if(Input.GetMouseButtonDown(1))
        {
            checkGridConnected(firstBlock);
        }

        movement();
	}


    void movement()
    {
        float rotate = Input.GetAxis("Horizontal");
        float force = Input.GetAxis("Vertical");
        float moveHorizontal = 0;
        float moveVertical = force;
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb.AddRelativeForce(movement * speed);
        rb.AddTorque(-rotate * rotationalSpeed);
    }

    public bool checkForLocation(Vector2 loc)
    {
        bool found = false;
        int i = 0;
        while (!found && i < blocks.Count)
        {
            if(blocks[i].GetComponent<BlockController>().locate.x == loc.x && blocks[i].GetComponent<BlockController>().locate.y == loc.y)
            {
                found = true;
            }

            i++;
        }

        return found;
    }



    //For adding blocks

    void checkGridConnected(GameObject currentBlock)
    {
        BlockController block = currentBlock.GetComponent<BlockController>();

        if (checkAdjacent(block.locate, 1))
        {
            block.check = true;
            checkGridConnected(getAdjacent(block.locate, 1));
        }

        if (checkAdjacent(block.locate, 2))
        {
            block.check = true;
            checkGridConnected(getAdjacent(block.locate, 2));
        }

        if (checkAdjacent(block.locate, 3))
        {
            block.check = true;
            checkGridConnected(getAdjacent(block.locate, 3));
        }

        if (checkAdjacent(block.locate, 4))
        {
            block.check = true;
            checkGridConnected(getAdjacent(block.locate, 4));
        }

    }

    GameObject findBlockOfLocation(Vector2 loc)
    {
        GameObject block = null;

        for(int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i].GetComponent<BlockController>().locate == loc)
            {
                block = blocks[i];
            }
        }

        return block;
    }

    void recalcCenterOfMass()
    {
        //average the locations of all the blocks
        float averageX = 0;
        float averageY = 0;
        for (int i = 0; i < blocks.Count; i++)
        {
            averageX += blocks[i].transform.localPosition.x;
            averageY += blocks[i].transform.localPosition.y;
        }
        averageX = averageX / (blocks.Count - 1);
        averageY = averageY / (blocks.Count - 1);

        //set center off mass to that location
        rb.centerOfMass.Set(averageX, averageY);
    }

    bool checkAdjacent(Vector2 prevLoc, int direction)
    {
        bool isAdjacent = false;
        Vector2 loc;

        if(direction == 1)//fore
        {
            loc = new Vector2(prevLoc.x, prevLoc.y + 1);
            if (findBlockOfLocation(loc) != null)
            {
                isAdjacent = true;
            }
        }

        if(direction == 3)//aft
        {
            loc = new Vector2(prevLoc.x, prevLoc.y - 1);
            if (findBlockOfLocation(loc) != null)
            {
                isAdjacent = true;
            }

        }

        if(direction == 4)//starbord
        {
            loc = new Vector2(prevLoc.x + 1, prevLoc.y);
            if (findBlockOfLocation(loc) != null)
            {
                isAdjacent = true;
            }

        }

        if(direction == 2)//port
        {
            loc = new Vector2(prevLoc.x - 1, prevLoc.y);
            if (findBlockOfLocation(loc) != null)
            {
                isAdjacent = true;
            }
        }

        return isAdjacent;
    }

    GameObject getAdjacent(Vector2 prevLoc, int direction)//assumes that there is an adjacent block
    {
        GameObject block = null;
        Vector2 loc = new Vector2();

        if (direction == 1)//fore
        {
            loc = new Vector2(prevLoc.x, prevLoc.y + 1);
        }

        if (direction == 3)//aft
        {
            loc = new Vector2(prevLoc.x, prevLoc.y - 1);
        }

        if (direction == 4)//starbord
        {
            loc = new Vector2(prevLoc.x + 1, prevLoc.y);
        }

        if (direction == 2)//port
        {
            loc = new Vector2(prevLoc.x - 1, prevLoc.y);
        }

        block = findBlockOfLocation(loc);

        return block;
    }

    void setColliders(GameObject colliderSet, GameObject parent)
    {
        GameObject direction = colliderSet.transform.Find("ForeCollider").gameObject;
        direction.GetComponent<IfDrag>().parentBlock = parent;
        direction.GetComponent<IfDrag>().direction = 1;

        direction = colliderSet.transform.Find("PortCollider").gameObject;
        direction.GetComponent<IfDrag>().parentBlock = parent;
        direction.GetComponent<IfDrag>().direction = 2;

        direction = colliderSet.transform.Find("AftCollider").gameObject;
        direction.GetComponent<IfDrag>().parentBlock = parent;
        direction.GetComponent<IfDrag>().direction = 3;

        direction = colliderSet.transform.Find("StarbordCollider").gameObject;
        direction.GetComponent<IfDrag>().parentBlock = parent;
        direction.GetComponent<IfDrag>().direction = 4;
    }

    void setLocationOffset(ref Vector3 loc, GameObject connectedBlock, int direction)
    {
        float angleOffset = 90 * direction;
        float angle = connectedBlock.transform.eulerAngles.z + angleOffset;
        angle = angle % 360;
        float X = connectedBlock.transform.position.x + (L * (Mathf.Cos((angle * Mathf.PI) / 180)));
        float Y = connectedBlock.transform.position.y + (L * (Mathf.Sin((angle * Mathf.PI) / 180)));
        loc.Set(X, Y, 0);
    }

    public void addBlock(GameObject connectedBlock, int direction = 1)
    {
        if(!checkAdjacent(connectedBlock.GetComponent<BlockController>().locate, direction))
        {
            Vector3 locationOffset = new Vector3();
            setLocationOffset(ref locationOffset, connectedBlock, direction);

            Transform clone = Instantiate(prefab, locationOffset, connectedBlock.transform.rotation) as Transform;
            if (clone != null)
            {
                clone.transform.SetParent(gameObject.transform);
                clone.GetComponent<BlockController>().ship = gameObject;
                clone.GetComponent<BlockController>().setDirection(connectedBlock, direction, connectedBlock.GetComponent<BlockController>().locate);
                GameObject colliderSet = clone.transform.Find("ColliderSet").gameObject;
                setColliders(colliderSet, clone.gameObject);
                recalcCenterOfMass();
            }
        }
    }
}
