using UnityEngine;
using System.Collections;

public class BlockSetup : MonoBehaviour {

    public Transform parent;

	// Use this for initialization
	void Start () {
        gameObject.transform.SetParent(parent);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
