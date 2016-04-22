using UnityEngine;
using System.Collections;

public class CameraFollowShip : MonoBehaviour
{
	public GameObject ship;
	private Vector3 offset;
	// Use this for initialization
	void Start()
	{
		offset = transform.position - ship.transform.position;
	}
	
	// Update is called once per frame
	void Update()
	{
		transform.position = ship.transform.position + offset;
	}
}
