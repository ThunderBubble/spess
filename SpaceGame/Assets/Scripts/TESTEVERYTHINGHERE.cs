using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TESTEVERYTHINGHERE : MonoBehaviour {

    public Text clickTextIn;
    public bool isRoot;
    private static Text clickText;

    // Use this for initialization
    void Start () {
        if(isRoot)
        {
            clickText = clickTextIn;
        }
        else emptyText();
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    static void emptyText()
    {
        clickText.text = "";
    }

    void OnMouseEnter()
    {
        if (!isRoot)
        {
            mousedOver();
        }
        //Debug.Log("HI");
    }

    static void mousedOver()
    {
        clickText.text = "MOUSED OVER";
    }

    static void NotMousedOver()
    {
        clickText.text = "";
    }

    void OnMouseExit()
    {
        //Debug.Log("BYE");
        if (!isRoot)
        {
            NotMousedOver();
        }
    } 

    /*if (Input.GetMouseButton(0))
{
    mousePosition = Input.mousePosition;
}*/
}
