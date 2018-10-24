using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmPullScript : MonoBehaviour {

	[SerializeField] bool detached = false;
	bool grabbed = false;

	[SerializeField] ArmPullScript neighborSegment;

	Vector3 lastMousePosition;
	Vector3 offset;
	Vector3 momentum;
	[SerializeField] [Range(0,1)] float drag = 0.1f;

	[SerializeField] [Range(0,2)] float force = 1f;

	Vector3 startPos;
	Vector3 startOffset;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		if(neighborSegment != null)
			startOffset = transform.position - neighborSegment.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(grabbed){ //TODO: use a state machine instead
			if(Input.GetMouseButtonUp(0)){
				grabbed = false;
			}

			Vector3 worldSpaceMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldSpaceMouse.z = 0;

			offset = transform.position - worldSpaceMouse;

			momentum -= offset * force * Time.deltaTime;

			

		} else if(neighborSegment != null){
			offset = (transform.position - neighborSegment.transform.position) - startOffset;
			momentum -= offset * force * Time.deltaTime;
		}
		transform.position += momentum;
	}

	void FixedUpdate(){
		momentum *= 1 - drag;
	}

	void OnMouseDown(){
		if(detached){
			grabbed = true;
			lastMousePosition = transform.position;
			// Vector3 worldSpaceMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			// worldSpaceMouse.z = 0;
			// offset = transform.position - worldSpaceMouse;
		}
	}

	//TODO: add a light pull in OnMouseOver
}
