using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;



public class ShipController : MonoBehaviour {

	private Rigidbody2D rb;
	public float speed;
	public float rotationalSpeed;
    public bool controlOn;
    
    void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}

    void FixedUpdate () {
        if(controlOn)
        {
            movement();
        }

	}

    //Turns input into forces
    void movement()
    {
        rb.AddRelativeForce(new Vector2(0, Input.GetAxis("Vertical")) * speed);//Use the vertical axis of input to move the ship forwards and backwards
        rb.AddTorque(-Input.GetAxis("Horizontal") * rotationalSpeed);//Use the horizontal axis of input to rotate the ship
    }

    //Finds the center of mass between the ship's blocks and sets the Rigidbody2D's center of mass accordingly
    public void recalcCenterOfMass()
    {
        List<GameObject> blocks = gameObject.GetComponent<GridData>().blocks;

        //average the locations of all the blocks
        float averageX = 0;
        float averageY = 0;
        for (int i = 0; i < blocks.Count; i++)
        {
            averageX += blocks[i].transform.localPosition.x;
            averageY += blocks[i].transform.localPosition.y;
        }
        if (blocks.Count - 1 > 0)
        {
            averageX = averageX / (blocks.Count - 1);
            averageY = averageY / (blocks.Count - 1);
        }
        Vector2 vec = new Vector2(averageX, averageY);

        //set the center of mass
        rb.centerOfMass = vec;
    }
}