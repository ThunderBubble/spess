﻿using UnityEngine;
using System.Collections;

public class Source : MonoBehaviour {
	public GameObject wireController;



	void Start () {
		wireController.GetComponent<WireController>().TrackWireObject(gameObject);
	}
}