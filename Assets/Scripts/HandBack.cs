using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBack : MonoBehaviour {

	LineRenderer lineRnd;
	public GameObject pointOne;
	public GameObject pointTwo;

	public float zPointOne;
	public float zPointTwo;

	// Use this for initialization
	void Start () {
		lineRnd = gameObject.GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		lineRnd.SetPosition(0, new Vector3(pointOne.transform.position.x, pointOne.transform.position.y,zPointOne));
		lineRnd.SetPosition(1, new Vector3 (pointTwo.transform.position.x,pointTwo.transform.position.y,zPointTwo));
	}
}
