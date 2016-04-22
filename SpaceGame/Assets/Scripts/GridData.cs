using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridData : MonoBehaviour {

    public Transform blockPrefab;
    public Transform debriesPrefab;
    public List<GameObject> blocks;
    public GameObject firstBlock;

    // Use this for initialization
    void Start () {
        firstBlock.GetComponent<BlockData>().locate = new Vector2(0, 0);
    }
	
	// Update is called once per frame
	void Update () {


	
	}

	public void recalcGridColliders()
	{
		for(int i = 0; i < blocks.Count; i++)
		{
			blocks[i].GetComponent<BlockData>().recalcValidColliders();
		}
	}

    //Marks all blocks connected to the currentBlock as connected
    void checkGridConnected(GameObject currentBlock)
    {
        BlockData block = currentBlock.GetComponent<BlockData>();
        block.check = true;
        GameObject adjacent;

        for (int i = 0; i < 4; i++)//i serves as the direction of the check
        {
            adjacent = getAdjacent(block.locate, (BlockData.direction)i);
            if (adjacent != null)
            {
                if (!adjacent.GetComponent<BlockData>().check)
                {
                    checkGridConnected(adjacent);
                }
            }
        }
    }

    //Runs through the list of blocks and resets the flags
    void resetGridConnected()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].GetComponent<BlockData>().check = false;
        }
    }

    //For finding specific elements of a List of blocks that has changed in size while iterating through it
    int findBlockIndex(GameObject block)
    {
        int j = -1;
        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] == block)
            {
                j = i;
            }
        }
        return j;
    }

    //Takes any detatched blocks and reparents them to a debries object
    void recalcGridConnected()
    {
        checkGridConnected(firstBlock);

        //Find the velocity and location to apply to the new debries object by averaging the velocity and position of all blocks not marked as connected
        Vector3 location = new Vector3(0, 0, 0);
        Vector3 velocity = new Vector2(0, 0);
        int j = 0;

        for (int i = 0; i < blocks.Count; i++)
        {
            if (!blocks[i].GetComponent<BlockData>().check)
            {
                j++;
                location += blocks[i].transform.localPosition;
                velocity += blocks[i].GetComponent<BlockData>().velocity;
            }
        }

        if (j > 0)//Only continue if there is at least one block to detatch
        {
            location = location / j;
            velocity = velocity / j;
            Transform debries = Instantiate(debriesPrefab, location, gameObject.transform.rotation) as Transform;
            debries.gameObject.GetComponent<Rigidbody2D>().AddForce(velocity / Time.fixedDeltaTime);

            //Remove detatched blocks and parent them to the debries
            List<GameObject> blocksCopy = new List<GameObject>();
            for (int i = 0; i < blocks.Count; i++)
            {
                blocksCopy.Add(blocks[i]);
            }

            for (int i = 0; i < blocksCopy.Count; i++)
            {
                if (!blocksCopy[i].GetComponent<BlockData>().check)
                {
                    blocks[findBlockIndex(blocksCopy[i])].transform.parent = debries;
                    blocks.Remove(blocks[findBlockIndex(blocksCopy[i])]);
                }
            }

            gameObject.GetComponent<ShipController>().recalcCenterOfMass();
        }
        resetGridConnected();
    }

    //Bundles together some housekeeping for removing blocks from the List
    public void removeBlock(GameObject block)
    {
        blocks.Remove(block);
        Destroy(block);
        recalcGridConnected();
    }

    //Self explanitory
    GameObject getBlockOfLocation(Vector2 loc)
    {
        GameObject block = null;

        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i].GetComponent<BlockData>().locate == loc)
            {
                block = blocks[i];
            }
        }

        return block;
    }

    //Takes the address of the current block and returns the address of an adjacent block in a given direction
    public GameObject getAdjacent(Vector2 prevLoc, BlockData.direction direct)
    {
        GameObject block = null;
        Vector2 loc = new Vector2();

        if (direct == BlockData.direction.fore)//fore
        {
            loc = new Vector2(prevLoc.x, prevLoc.y + 1);
        }

        if (direct == BlockData.direction.aft)//aft
        {
            loc = new Vector2(prevLoc.x, prevLoc.y - 1);
        }

        if (direct == BlockData.direction.starbord)//starbord
        {
            loc = new Vector2(prevLoc.x + 1, prevLoc.y);
        }

        if (direct == BlockData.direction.port)//port
        {
            loc = new Vector2(prevLoc.x - 1, prevLoc.y);
        }
			
		block = getBlockOfLocation(loc);

        return block;
    }
}


//I can get the same functionality from getAdjacent, but I'm saving this for now
/*public bool checkAdjacent(Vector2 prevLoc, BlockData.direction direct)//prevLoc is the location of the previous block, direct tells the function which direction to check
{
    bool isAdjacent = false;
    Vector2 loc;//Vector that gets filled with the grid space to check

    if (direct == BlockData.direction.fore)
    {
        loc = new Vector2(prevLoc.x, prevLoc.y + 1);
        if (findBlockOfLocation(loc) != null)
        {
            isAdjacent = true;
        }
    }
    else if (direct == BlockData.direction.aft)
    {
        loc = new Vector2(prevLoc.x, prevLoc.y - 1);
        if (findBlockOfLocation(loc) != null)
        {
            isAdjacent = true;
        }
    }
    else if (direct == BlockData.direction.starbord)
    {
        loc = new Vector2(prevLoc.x + 1, prevLoc.y);
        if (findBlockOfLocation(loc) != null)
        {
            isAdjacent = true;
        }

    }
    else if (direct == BlockData.direction.port)
    {
        loc = new Vector2(prevLoc.x - 1, prevLoc.y);
        if (findBlockOfLocation(loc) != null)
        {
            isAdjacent = true;
        }
    }

    return isAdjacent;
}*/
