using UnityEngine;
using System.Collections;

public class IfDrag : MonoBehaviour {

    public GameObject parentBlock;
    public BlockData.direction direct;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseEnter()
    {
        if (parentBlock.GetComponent<BlockController>().checkClick())
        {
            parentBlock.GetComponent<BlockData>().ship.GetComponent<AddBlock>().addBlock(parentBlock, direct);
            parentBlock.GetComponent<BlockController>().resetClick();
        }
    }
}
