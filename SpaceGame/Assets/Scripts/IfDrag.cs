using UnityEngine;
using System.Collections;

public class IfDrag : MonoBehaviour {

    public GameObject parentBlock;
    public BlockData.direction direct;

	private Transform obj;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseEnter()
    {
		//obj = parentBlock.transform.Find ("CenterCollider");

		if (parentBlock.transform.Find ("ColliderSet").Find("CenterCollider").gameObject.GetComponent<MouseController>().clicked)
		{
			parentBlock.GetComponent<BlockData>().ship.GetComponent<AddBlock>().addBlock(parentBlock, direct);
			parentBlock.transform.Find ("ColliderSet").Find("CenterCollider").gameObject.GetComponent<MouseController>().resetClick();
		}

        /*if (parentBlock.GetComponent<BlockController>().checkClick())
        {
            parentBlock.GetComponent<BlockData>().ship.GetComponent<AddBlock>().addBlock(parentBlock, direct);
            parentBlock.GetComponent<BlockController>().resetClick();
        }*/
    }
}
