using UnityEngine;

public class MeshDeformerInput : MonoBehaviour {
	
	public static float force = 10f;
	public static float forceOffset = 0.1f;
	
	void Update () {
		if (Input.GetMouseButton(0)) {
			HandleInput(10);
		}
	}

	public static void HandleInput (float moreForce) {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast(inputRay, out hit)) {
			MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
			if (deformer) {
				Vector3 point = hit.point;
				point += hit.normal * forceOffset;
                deformer.AddDeformingForce(point, (force * moreForce));
			}
		}
	}
}