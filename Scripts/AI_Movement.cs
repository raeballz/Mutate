using UnityEngine;
using System.Collections;

public class AI_Movement : MonoBehaviour {

	public Transform pointA;
	public Transform pointB;
	public float patrolTime;
	private Vector3 pointAPos;
	private Vector3 pointBPos;


	IEnumerator Start()
	{
		var pointAPos = pointA.transform.position;
		var pointBPos = pointB.transform.position;
		while (true) {
			yield return StartCoroutine(MoveObject(transform, pointAPos, pointBPos, patrolTime));
			yield return StartCoroutine(MoveObject(transform, pointBPos, pointAPos, patrolTime));
		}
	}
	
	IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
	{
		var i= 0.0f;
		var rate= 1.0f/time;
		while (i < 1.0f) {
			i += Mathf.Sin(Time.deltaTime * rate);
			thisTransform.position = Vector3.Lerp(startPos, endPos, i);
			yield return null; 
		}
	}
}
