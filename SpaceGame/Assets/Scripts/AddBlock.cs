using UnityEngine;
using System.Collections;

public class AddBlock : MonoBehaviour {

    public bool isRoot;
    public Transform prefab;
    private bool isLatest;
    //private static Transform myPrefab;
    //public Transform parent;

	// Use this for initialization
	void Start () {
        isLatest = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && isLatest)
        {
            addBlock();
        }
    }

    void addBlock()
    {
        Transform tClone = Instantiate(prefab, new Vector3(0, 12.5F, 0), Quaternion.identity) as Transform;
        GameObject clone = tClone.gameObject;

        if (clone != null)
        {
            isLatest = false;
            clone.transform.SetParent(gameObject.transform);//Switch this for the block itself//and use static joint instead
            FixedJoint2D joint = clone.GetComponent<FixedJoint2D>();
            joint.connectedBody = gameObject.transform.GetComponent<Rigidbody2D>();
            clone.GetComponent<AddBlock>().prefab = prefab;
        }
    }
}
