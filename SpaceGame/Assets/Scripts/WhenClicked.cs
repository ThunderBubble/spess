using UnityEngine;
using System.Collections;

public class WhenClicked : MonoBehaviour {

    public GameObject ship;

    private bool clicked;

	// Use this for initialization
	void Start () {
        clicked = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            clicked = true;
        }
        else clicked = false;
    }

    void OnMouseExit()
    {
        if(!Input.GetMouseButton(0))
        {
            clicked = false;
        }
    }

    public bool checkClick()
    {
        bool pressed = false;
        if(clicked) pressed = true;
        return pressed;
    }

    public void resetClick()
    {
        clicked = false;
    }
}
/*if(!clickState)
            {
                ship.GetComponent<ShipController>().setText("clicked");
                ship.GetComponent<AddBlock>().addBlock(gameObject);
                clickState = true;
            }
            else
            {
                ship.GetComponent<ShipController>().setText("");
                clickState = false;
            }*/
