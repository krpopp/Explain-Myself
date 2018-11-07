using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SkinBackface : MonoBehaviour {

	List<Transform> allVertChildren = new List<Transform>();
	List<Vector2> uvs = new List<Vector2>();

	MeshFilter meshFilter;
	Mesh skinMesh;
	MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
		meshFilter = GetComponent<MeshFilter>();
		skinMesh = new Mesh();
		meshFilter.mesh = skinMesh;
		meshRenderer = GetComponent<MeshRenderer>();

		GetVertChildrenRecursive(transform);
		Debug.Log("Num children: " + allVertChildren.Count);

		SetMesh(skinMesh);		

		//triangles: 0,1,2; 3,2,1; 2,3,4; 5,4,3; 4,5,6; 7,6,5
		bool forward = false;
		int forwardCount = 1;
		int backwardCount = 0;

		int triangleCount = 6 * (allVertChildren.Count - 2) / 2;
		int[] tris = new int[triangleCount];
		for(int i = 0; i < tris.Length; i++){
			if(i % 3 == 0){
				forward = !forward;
				if(forward){
					forwardCount--;
				} else{
					backwardCount = forwardCount;
				}
			}
			if(forward){
				tris[i] = forwardCount++;
			} else{
				tris[i] = backwardCount--;
			}
			Debug.Log(tris[i]);
		}
		skinMesh.triangles = tris;
		skinMesh.RecalculateNormals();
	}

	void Update(){
		SetMesh(skinMesh);
	}

	Transform GetVertChildrenRecursive(Transform trans)
	{
		int childCount = trans.childCount;
		if(childCount == 0){
			return trans;
		}
		for(int i = childCount - 1; i >= 0; i--){
			Transform childTrans = GetVertChildrenRecursive(trans.GetChild(i));
			if(childTrans != null){
				if(childTrans.gameObject.name.Contains("Verts")){
					Debug.Log(childTrans.gameObject.name);
					allVertChildren.Add(childTrans);
				}
			}
		}
		return null;
	}
	void SetMesh(Mesh mesh){
		Vector3[] newVertPositions = new Vector3[allVertChildren.Count];

		for(int i = 0; i < newVertPositions.Length; i++){
			newVertPositions[i] = transform.InverseTransformPoint(allVertChildren[i].position);
		}

		mesh.vertices = newVertPositions;
		mesh.RecalculateBounds();
	}
	

	void OnDrawGizmos(){
		if(!UnityEditor.EditorApplication.isPlaying)
			return;
		for(int i = 0; i < skinMesh.vertices.Length; i++){
			Gizmos.DrawCube(skinMesh.vertices[i], Vector3.one * 0.05f);
		}
	}
}
