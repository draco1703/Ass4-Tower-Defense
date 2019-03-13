using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowNode : MonoBehaviour
{
    public int x, z;

    private bool walkable = true;
    public bool Buildable = true;
    




    public void boolUpdatate(bool _walkable)
    {
        walkable = _walkable;
        Grid.instance.updateNode(x, z, walkable);
        updateEnemy();
        Pathfinding.instace.updatePath();

    }
    public void setXY(int _x, int _z, bool _Buildable)
    {
        x = _x;
        z = _z;
     
        Buildable = _Buildable;
    }
    void updateEnemy()
    {
     //   Debug.Log(GameObject.FindGameObjectsWithTag("Enemy").Length);
        foreach (var enermy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            
            enermy.GetComponent<Enemy>().updatePath();
        }

    }
}
