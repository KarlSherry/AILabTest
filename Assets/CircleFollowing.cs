using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleFollowing : MonoBehaviour {

	Vector3 force = Vector3.zero;
	Vector3 velocity = Vector3.zero;

	public float maxSpeed = 5.0f;
	public float mass = 1.0f;

	int currentWaypoint = 0;

	List<Vector3> wayPoints = new List<Vector3>();

	// Use this for initialization
	void Start () {
		CreatePath (10, 10);
	}
	
	// Update is called once per frame
	void Update () {
	
		force += FollowPath ();

		DrawPath ();

		Vector3 acceleration = force / mass;

		velocity += acceleration * Time.deltaTime;
		transform.position += velocity * Time.deltaTime;

		force = Vector3.zero;

		if (velocity.magnitude > float.Epsilon) {
			transform.forward = velocity.normalized;
		}

		velocity *= 0.99f;
	}

	private void CreatePath (int points, float radius) {
		float thetaInc = (2 * Mathf.PI) / points;

		for (int i = 0; i < points; i++) 
		{			
			float theta = thetaInc * i;

			float x = Mathf.Sin(theta) * radius;
			float z = Mathf.Cos(theta) * radius;

			wayPoints.Add(new Vector3(x, 0, z));
		}
	}

	private void DrawPath() {
		float maxPoints = wayPoints.Count;

		for (int i = 0; i < maxPoints; i++) 
		{
			int nextPos = i + 1;

			if(nextPos >= maxPoints)
			{
				nextPos = 0;
			}

			Vector3 startPos = wayPoints[i];
			Vector3 endPos = wayPoints[nextPos];

			Debug.DrawLine (startPos, endPos, Color.red);
		}
	}

	Vector3 Seek (Vector3 point) {
		Vector3 desiredVelocity = point - transform.position;
		desiredVelocity.Normalize ();
		desiredVelocity *= maxSpeed;

		return desiredVelocity - velocity;
	}

	Vector3 FollowPath() {
		Vector3 currentTarget = wayPoints [currentWaypoint];

		float distToTarget = (currentTarget - transform.position).magnitude;

		if (distToTarget < 1.0f) {
			currentWaypoint ++;
		}
		if (currentWaypoint >= wayPoints.Count) {
			currentWaypoint = 0;
		}

		return Seek(currentTarget);
	}
}
