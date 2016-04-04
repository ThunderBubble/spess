using UnityEngine;
using System.Collections;

public class IfDrag : MonoBehaviour {

    public GameObject parentBlock;
    public int direction;

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
            parentBlock.GetComponent<BlockController>().ship.GetComponent<ShipController>().addBlock(parentBlock, direction);
            parentBlock.GetComponent<BlockController>().resetClick();
        }
    }
}
