using UnityEngine;
using System.Collections;

public class AddBlock : MonoBehaviour {

    public float blockSize;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
    }

    //Sets up the public variables of the new block's mouse colliders
    void setMouseColliders(GameObject parent)//parent is a reference to the parent of the colliders
    {
        GameObject colliderSet = parent.transform.Find("ColliderSet").gameObject;//colliderSet is the parent for the mouse colliders
        GameObject collider;

        collider = colliderSet.transform.Find("ForeCollider").gameObject;//get the child object named collider
        collider.GetComponent<IfDrag>().parentBlock = parent;//set child object's script to hold a reference to the block it's attatched to
        collider.GetComponent<IfDrag>().direct = BlockData.direction.fore;//tell the child object's script what direction it's pointed in
        //Same for the rest
        collider = colliderSet.transform.Find("PortCollider").gameObject;
        collider.GetComponent<IfDrag>().parentBlock = parent;
        collider.GetComponent<IfDrag>().direct = BlockData.direction.port;

        collider = colliderSet.transform.Find("AftCollider").gameObject;
        collider.GetComponent<IfDrag>().parentBlock = parent;
        collider.GetComponent<IfDrag>().direct = BlockData.direction.aft;

        collider = colliderSet.transform.Find("StarbordCollider").gameObject;
        collider.GetComponent<IfDrag>().parentBlock = parent;
        collider.GetComponent<IfDrag>().direct = BlockData.direction.starbord;
    }

    //Finds the location that the new block should be, offset to take in account what side the block is being added to
    Vector3 getLocationOffset(GameObject connectedBlock, BlockData.direction direct)
    {
        float angleOffset = 90 * (int)direct;
        float angle = connectedBlock.transform.eulerAngles.z + angleOffset;
        angle = angle % 360;

        //use some trig to find the location where the center of the new block should be, and then offset it by the location of the previous block
        float X = connectedBlock.transform.position.x + (blockSize * (Mathf.Cos((angle * Mathf.PI) / 180)));
        float Y = connectedBlock.transform.position.y + (blockSize * (Mathf.Sin((angle * Mathf.PI) / 180)));

        Vector3 locationOffset = new Vector3(X, Y, 0);

        return locationOffset;
    }

    //Master function that does everything required to set up a new block
        //connectedBlock is the block that the new block will be adjacent to
        //direct is the direction that the new block will be added in
    public void addBlock(GameObject connectedBlock, BlockData.direction direct)
    {
        if (gameObject.GetComponent<GridData>().getAdjacent(connectedBlock.GetComponent<BlockData>().locate, direct) == null)//if the location where the new block will be placed is vacant
        {
            Vector3 locationOffset = new Vector3();
            locationOffset = getLocationOffset(connectedBlock, direct);

            Transform clone = Instantiate(gameObject.GetComponent<GridData>().blockPrefab, locationOffset, connectedBlock.transform.rotation) as Transform;
            if (clone != null)//make sure that the creation was successful
            {
                clone.transform.SetParent(gameObject.transform);
                clone.GetComponent<BlockData>().ship = gameObject;
                clone.GetComponent<BlockData>().setDirection(connectedBlock.GetComponent<BlockData>().locate, direct);
                setMouseColliders(clone.gameObject);

                gameObject.GetComponent<ShipController>().recalcCenterOfMass();//recalculate the center of mass, taking into account the new block
            }
        }
    }
}
