using UnityEngine;
using System.Collections;



public class BlockController : MonoBehaviour {

    public GameObject ship;
    public Vector2 locate;
    public bool check;

    //GameObject powerWire
    //GameObject exaustWire

    public bool clicked;

    void Start()
    {
        ship.GetComponent<ShipController>().blocks.Add(gameObject);
        clicked = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setDirection(GameObject parent, int direction, Vector2 prevLoc)
    {
        if (direction == 1)//fore
        {
            locate = new Vector2(prevLoc.x, prevLoc.y + 1);
        }

        if (direction == 3)//aft
        {
            locate = new Vector2(prevLoc.x, prevLoc.y - 1);
        }

        if (direction == 4)//starbord
        {
            locate = new Vector2(prevLoc.x + 1, prevLoc.y);
        }

        if (direction == 2)//port
        {
            locate = new Vector2(prevLoc.x - 1, prevLoc.y);
        }
    }

    //For Mouse Related Ventures:
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
        if (!Input.GetMouseButton(0))
        {
            clicked = false;
        }
    }
    public bool checkClick()
    {
        bool pressed = false;
        if (clicked) pressed = true;
        return pressed;
    }
    public void resetClick()
    {
        clicked = false;
    }
}


