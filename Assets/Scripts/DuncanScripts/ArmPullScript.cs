using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmPullScript : MonoBehaviour {


	[SerializeField] SegmentState currentState = SegmentState.ATTACHED;

	delegate void StateUpdate();
	StateUpdate stateUpdate;

	void SetState(SegmentState newState){
		currentState = newState;

		switch(currentState){
			case SegmentState.ATTACHED:
				InitAttached();
				stateUpdate = AttachedUpdate;
				break;
			case SegmentState.DETACHED:
				InitDetached();
				stateUpdate = DetachedUpdate;
				break;
			case SegmentState.GRABBED:
				InitGrabbed();
				stateUpdate = GrabbedUpdate;
				break;
		}
	}

	[SerializeField] ArmPullScript forwardNeighbor;
	[SerializeField] ArmPullScript backNeighbor;

	Vector3 lastMousePosition;
	Vector3 offset;
	Vector3 momentum;

	Vector3 startPos;
	Vector3 startOffset;

	// Use this for initialization
	void Start () {
		SetState(currentState);
	}
	
	// Update is called once per frame
	void Update () {
		if(stateUpdate != null){
			stateUpdate();
		}
	}

	void FixedUpdate(){
		momentum *= 1 - ArmPullManager.Instance.drag;
	}

	#region ATTACHED
	void InitAttached(){
		startPos = transform.position;
		if(forwardNeighbor != null)
			startOffset = transform.position - forwardNeighbor.transform.position;
	}

	void AttachedUpdate(){
		if(Vector3.Distance(forwardNeighbor.transform.position, transform.position) > ArmPullManager.Instance.detachDistance){
			SetState(SegmentState.DETACHED);
		}
	}
	#endregion

	#region DETACHED
	void InitDetached(){

	}

	void DetachedUpdate(){
		if(forwardNeighbor != null){
			offset = (transform.position - forwardNeighbor.transform.position) - startOffset;
			if(offset.magnitude > ArmPullManager.Instance.maxSegmentDistance){
				transform.position = forwardNeighbor.transform.position + offset.normalized * ArmPullManager.Instance.maxSegmentDistance;
				momentum = Vector3.zero;
				Debug.Log("HELL0?");
			} else{
				momentum -= offset * ArmPullManager.Instance.pullForce * Time.deltaTime;
			}
		}
		transform.position += momentum;
	}
	#endregion

	#region GRABBED
	void InitGrabbed(){
		lastMousePosition = transform.position;
		momentum = Vector3.zero;
	}

	void GrabbedUpdate(){
		if(Input.GetMouseButtonUp(0)){
			SetState(SegmentState.DETACHED);
		}

		Vector3 worldSpaceMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		worldSpaceMouse.z = 0;

		offset = transform.position - worldSpaceMouse;

		momentum -= offset * ArmPullManager.Instance.pullForce * Time.deltaTime;
		
		transform.position += momentum;
	}
	#endregion

	void OnMouseDown(){
		if(currentState == SegmentState.DETACHED){
			SetState(SegmentState.GRABBED);
		}
	}

	//TODO: add a light pull in OnMouseOver
}
