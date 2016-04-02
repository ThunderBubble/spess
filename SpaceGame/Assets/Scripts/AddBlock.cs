using UnityEngine;
using System.Collections;

public class AddBlock : MonoBehaviour {

    public Transform prefab;
    public float L;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
    }

    void setColliders(GameObject colliderSet, GameObject parent)
    {
        GameObject direction = colliderSet.transform.Find("NorthCollider").gameObject;
        direction.GetComponent<IfDrag>().parentBlock = parent;
        direction.GetComponent<IfDrag>().direction = 1;

        direction = colliderSet.transform.Find("PortCollider").gameObject;
        direction.GetComponent<IfDrag>().parentBlock = parent;
        direction.GetComponent<IfDrag>().direction = 2;

        direction = colliderSet.transform.Find("SouthCollider").gameObject;
        direction.GetComponent<IfDrag>().parentBlock = parent;
        direction.GetComponent<IfDrag>().direction = 3;

        direction = colliderSet.transform.Find("StarbordCollider").gameObject;
        direction.GetComponent<IfDrag>().parentBlock = parent;
        direction.GetComponent<IfDrag>().direction = 4;

        


    }

    public void addBlock(GameObject connectedBlock, int direction = 1)
    {
        float angleOffset = 90*direction;
        float angle = connectedBlock.transform.eulerAngles.z + angleOffset;//ZRotation
        angle = angle % 360;
        float X = connectedBlock.transform.position.x + (L * (Mathf.Cos((angle * Mathf.PI) / 180)));//X
        float Y = connectedBlock.transform.position.y + (L * (Mathf.Sin((angle * Mathf.PI) / 180)));//Y
        Vector3 locationOffset = (new Vector3(X, Y, 0));

        Transform clone = Instantiate(prefab, locationOffset, connectedBlock.transform.rotation) as Transform;
        if (clone != null)
        {
            clone.gameObject.transform.SetParent(gameObject.transform);//gameObject.transform should be the ship
            clone.GetComponent<WhenClicked>().ship = gameObject;
            GameObject colliderSet = clone.gameObject.transform.Find("ColliderSet").gameObject;
            setColliders(colliderSet, clone.gameObject);
        }
    }
}



//FixedJoint2D joint = clone.GetComponent<FixedJoint2D>();
//joint.connectedBody = gameObject.transform.GetComponent<Rigidbody2D>();
