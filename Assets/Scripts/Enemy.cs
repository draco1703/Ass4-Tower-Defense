using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Optinal")]
    public float startSpeed = 10f;

    public float startHealth;
    public int worth = 50;

    [Header("Unit Stuff")]
    public Image healthBar;

    public float turnDst = 1f;
    public float turnSpeed = 3;

    #region hideninspector
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public Path path;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float health;
    #endregion

    int pathfound;
    private void Awake()
    {
        target = GameObject.Find("EndPos").GetComponent<Transform>();
    }

    void Start()
    {
        speed = startSpeed;
        health = startHealth;
        path = Pathfinding.instace.path;
        StartCoroutine("FollowPath");

    }
    //void Update()
    //{
    //    speed = startSpeed;

    //}



    //public void OnPathFound(Vector3[] newPath, bool PathSuccessful)

    public void OnPathFound(Vector3[] waypoints, bool PathSuccessful)
    {
        if (PathSuccessful)
        {

            path = new Path(waypoints, transform.position, turnDst);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");

        }
    }


    public void TakkeDamager(float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
            Die();
        }
    }
    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    private void Die()
    {
        PlayerStats.Money += worth;

        Recycle();
    }


    void EndPath()
    {
        PlayerStats.Lives--;

        Recycle();
    }
    private void Recycle()
    {
        WaveSpawner.instance.EnemisAlive--;

        Destroy(gameObject);
    }


    // Skal update på turret build or sell. not movement for target prosisien 
    public void updatePath()
    {

        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

    }


    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);

        while (followingPath)
        {

            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }
            if (followingPath)
            {

                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
                speed = startSpeed;

            }
            yield return null;
        }
        EndPath();
    }


    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
            //for (int i = targetIndex; i < path.Length; i++)
            //{
            //    Gizmos.color = Color.black;
            //    Gizmos.DrawCube(path[i], Vector3.one);
            //    if (i == targetIndex)
            //    {
            //        Gizmos.DrawLine(transform.position, path[i]);
            //    }
            //    else
            //    {
            //        Gizmos.DrawLine(path[i - 1], path[i]);
            //    }
            //}
        }
    }

}

