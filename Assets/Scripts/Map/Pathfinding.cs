using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding instace;
    public Path path;
    public GameObject Startpos, Targetpos;
    private Node node;

    #region awaka and start
    private void Awake()
    {
        instace = this;

    }
    private void Start()
    {
        updatePath();
    }
    #endregion

    public void updatePath()
    {

        PathRequestManager.RequestPath(new PathRequest(Startpos.transform.position, Targetpos.transform.position, OnPathFound));

    }

    public void OnPathFound(Vector3[] waypoints, bool PathSuccessful)
    {

        if (PathSuccessful)
        {
            path = new Path(waypoints, Targetpos.transform.position, 0);
            node = null;
        }
        else
        {
            node.Refund();
            node = null;
        }

    }
    public void giveNode(Node _node)
    {
        node = _node;
    }

    public void FindPath(PathRequest request, Action<PathResult> callback)
    {
        Stopwatch sw = new Stopwatch();
        //sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSucess = false;


        NodePosition StartNode = Grid.instance.NodeFromWorldPoint(request.pathStart);
        NodePosition TargetNode = Grid.instance.NodeFromWorldPoint(request.pathEnd);
        StartNode.ParentNode = StartNode;

        if (StartNode.walkable && TargetNode.walkable)
        {
            Heap<NodePosition> openSet = new Heap<NodePosition>(Grid.instance.MaxSize);
            HashSet<NodePosition> ClosedList = new HashSet<NodePosition>();
            openSet.Add(StartNode);
            while (openSet.Count > 0)
            {
                NodePosition CurrentNode = openSet.RemoveFirst();
                ClosedList.Add(CurrentNode);
                if (CurrentNode == TargetNode)
                {
                    //sw.Stop();
                    // print("path found: " + sw.ElapsedMilliseconds + "ms");
                    pathSucess = true;
                    break;
                }
                foreach (NodePosition NeighborNode in Grid.instance.GetNeighboringNodes(CurrentNode))
                {
                    if (!NeighborNode.walkable || ClosedList.Contains(NeighborNode))
                        continue;
                    int MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, NeighborNode);

                    if (MoveCost < NeighborNode.gCost || !openSet.Contains(NeighborNode))
                    {
                        NeighborNode.gCost = MoveCost;
                        NeighborNode.hCost = GetManhattenDistance(NeighborNode, TargetNode);
                        NeighborNode.ParentNode = CurrentNode;

                        if (!openSet.Contains(NeighborNode))
                            openSet.Add(NeighborNode);
                        else
                            openSet.UpdateItem(NeighborNode);
                    }
                }

            }

        }
        if (pathSucess)
        {
            waypoints = GetFinalPath(StartNode, TargetNode);
            pathSucess = waypoints.Length > 0;
        }

        callback(new PathResult(waypoints, pathSucess, request.callback));
    }


    Vector3[] GetFinalPath(NodePosition a_StartingNode, NodePosition a_EndNode)
    {
        List<NodePosition> FinalPath = new List<NodePosition>();
        NodePosition CurrentNode = a_EndNode;
        while (CurrentNode != a_StartingNode)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.ParentNode;
        }
        Vector3[] waypoints = SimplefyPath(FinalPath);
        Array.Reverse(waypoints);
        return waypoints;


    }

    Vector3[] SimplefyPath(List<NodePosition> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directioinNew = new Vector2(path[i - 1].iGridX - path[i].iGridX, path[i - 1].iGridY - path[i].iGridY);
            if (directioinNew != directionOld)
                waypoints.Add(path[i].vPosition);
            directionOld = directioinNew;
        }
        return waypoints.ToArray();
    }

    int GetManhattenDistance(NodePosition a_nodeA, NodePosition a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.iGridX - a_nodeB.iGridX);
        int iy = Mathf.Abs(a_nodeA.iGridY - a_nodeB.iGridY);

        return ix + iy;
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {

            for (int i = 0; i < path.lookPoints.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path.lookPoints[i], Vector3.one);
                if (i == 0)
                {
                    Gizmos.DrawLine(Startpos.transform.position, path.lookPoints[i]);
                }
                else
                {
                    Gizmos.DrawLine(path.lookPoints[i - 1], path.lookPoints[i]);
                }
            }
        }
    }
}
