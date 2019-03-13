using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathResult> results = new Queue<PathResult>();
    public static PathRequestManager instance;

    #region Awake and start
    private void Awake()
    {
        instance = this;
        //  pathfinding = Pathfinding.instace;
    }

    private void Update()
    {
        if (results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock (results)
            {
                for (int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }
    #endregion


    public static void RequestPath(PathRequest request)
    {

        ThreadStart threadStart = delegate
        {
            Pathfinding.instace.FindPath(request, instance.FinishProcessingPath);
        };
        threadStart.Invoke();

    }

    public void FinishProcessingPath(PathResult result)
    {
        lock (results)
        {
            results.Enqueue(result);
        }

    }

}

public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}



public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callback;

    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
    {
        pathStart = _start;
        pathEnd = _end;
        callback = _callback;
    }
}

