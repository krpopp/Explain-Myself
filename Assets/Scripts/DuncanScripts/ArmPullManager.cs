using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SegmentState{
	ATTACHED,
	DETACHED,
	GRABBED,
	FIXED
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

	public float pullForce = 1.2f;
	public float forwardPullForce = 0.75f;
	[Range(0,1)] public float drag = 0.05f;

	[SerializeField] public float gravity = -0.4f;

	public float grabPullForce = 0.5f;

	public float detachDistance = 4.5f;

	public float maxPullMagnitude;

	Vector3 rotateMomentum;

	public float magnitudePower = 2f;

	public float maxGrabMagnitude = 0.3f;

	public Vector2 visibleOne;
	public Vector2 hiddenOne;
	public Vector2 visibleTwo;
	public Vector2 hiddenTwo;

	bool somethingGrabbed = false;
	Transform grabbedSegment;

	[SerializeField] float armPullForce = 1f;
	[SerializeField] float gravityForce = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(somethingGrabbed){
			if(Input.GetMouseButtonUp(0)){
				somethingGrabbed = false;
			}

			Vector3 worldSpaceMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldSpaceMousePos.z = 0;

			Vector3 vecToMouse = worldSpaceMousePos - transform.position;
			Vector3 vecToFinger = grabbedSegment.position - transform.position;

			float angle = Vector3.SignedAngle(vecToFinger.normalized, vecToMouse.normalized, Vector3.forward);

			rotateMomentum.x += angle * armPullForce * Time.deltaTime;

		}
		rotateMomentum.x += Vector3.SignedAngle(-transform.up, Vector3.down, Vector3.forward) * gravityForce * Time.deltaTime;
		transform.Rotate(0,0,rotateMomentum.x);
	}

	void FixedUpdate(){
		rotateMomentum *= 0.9f;
	}

	Vector3 grabOffset;
	public void SomethingGrabbed(Transform segment){
		grabbedSegment = segment;
		somethingGrabbed = true;
		Vector3 worldSpaceMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		worldSpaceMousePos.z = 0;
		grabOffset = worldSpaceMousePos - transform.position;
	}

	public void LetGo(){
		somethingGrabbed = false;
		grabbedSegment = null;
	}

	void OnMouseEnter(){
		
	}
}
