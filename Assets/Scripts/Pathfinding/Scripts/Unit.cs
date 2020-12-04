using UnityEngine;
using System.Collections;
using System;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private float speed = 20;
	Vector3[] path;
	int targetIndex;
    private Enemy myEnemy;

    //public event Action OnPathNotSuccessful;
    /// <summary>
    /// If waypoint is last point - true; else - false
    /// </summary>
    public Action<bool> OnWaypointAchieved;
    void Update()
    {

    }

    void Start() {
        myEnemy = GetComponent<Enemy>();
	}

    public void MoveToTarget(Vector2 myPos, Vector2 _target)
    {
        Vector3 targetPosition = new Vector3(_target.x, _target.y, transform.position.z);
        Vector3 myPosition = new Vector3(myPos.x, myPos.y, transform.position.z);
        PathRequestManager.RequestPath(myPosition, targetPosition, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
            Debug.Log("PathSuccessful!");
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
        }
        else
        {
            Debug.Log("PathNotSuccessful!");
            //PathNotSuccessful();
        }
	}

    //void PathNotSuccessful()
    //{
    //    Debug.Log("PathNotSuccessful! Calling callback..");
    //    if (OnPathNotSuccessful != null)
    //        OnPathNotSuccessful();
    //}

	IEnumerator FollowPath() {
        targetIndex = 0;
        Vector3 currentWaypoint = path[0];

		while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex ++;
				if (targetIndex >= path.Length) {

                    if(OnWaypointAchieved != null)
                        OnWaypointAchieved(true);

                    yield break;
				}
                if(OnWaypointAchieved != null)
                    OnWaypointAchieved(false);

				currentWaypoint = path[targetIndex];
			}

			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
			yield return null;

		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}
