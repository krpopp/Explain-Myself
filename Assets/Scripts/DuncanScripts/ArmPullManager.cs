using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SegmentState{
	ATTACHED,
	DETACHED,
	GRABBED
}

public class ArmPullManager : MonoBehaviour {

	private static ArmPullManager instance;
	public static ArmPullManager Instance{
		get{
			if(instance == null){
				instance = FindObjectOfType<ArmPullManager>();
			}
			return instance;
		}
		set { instance = value; }
	}

	public float detachDistance;
	[Range(0,2)] public float pullForce = 1.2f;
	[Range(0,1)] public float drag = 0.05f;
	public float maxSegmentDistance = 4f;

	Vector3 rotateMomentum;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,0,rotateMomentum.magnitude);
	}

	void FixedUpdate(){
		rotateMomentum *= 0.9f;
	}

	Vector3 lastMousePos;
	void OnMouseEnter(){
		lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		lastMousePos.z = 0;
	}
	void OnMouseOver(){
		Debug.Log("HEYEYEYEY");
		Vector3 worldSpaceMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		worldSpaceMousePos.z = 0;

		if(Vector3.Distance(lastMousePos, worldSpaceMousePos) > 0.1f){
			rotateMomentum += (worldSpaceMousePos - lastMousePos);
		}
		lastMousePos = worldSpaceMousePos;
	}
}
