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
        if (parentBlock.GetComponent<WhenClicked>().checkClick())
        {
            //parentBlock.GetComponent<WhenClicked>().ship.GetComponent<AddBlock>().addBlock(parentBlock);
            parentBlock.GetComponent<WhenClicked>().ship.GetComponent<AddBlock>().addBlock(parentBlock, direction);
            parentBlock.GetComponent<WhenClicked>().resetClick();
            //GameObject test = parentBlock.GetComponent<WhenClicked>().ship;
        }
    }
}
