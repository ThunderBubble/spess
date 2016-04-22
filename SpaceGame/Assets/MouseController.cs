using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

	public bool clicked;

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

		if(Input.GetMouseButton(1))
		{
			gameObject.GetComponent<BlockData>().ship.GetComponent<GridData>().removeBlock(gameObject);
		}
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
