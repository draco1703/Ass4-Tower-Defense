using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public Dictionary<string, Queue<GameObject>> poolDict;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPool Instance;
    #region Awake and start
    private void Awake()
    {
        Instance = this;
    }
    public List<Pool> pools;

    private void Start()
    {
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDict.Add(pool.tag, objectPool);
        }
    }
    #endregion
    public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion rotate)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.LogWarning("not here " + tag);
            return null;
        }

        GameObject obj = poolDict[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.position = pos;
        obj.transform.rotation = rotate;
        // if have time 
        poolDict[tag].Enqueue(obj);
        return obj;
    }

    public void Recycle(GameObject obj , string tag)
    {
        poolDict[tag].Enqueue(obj);
    }
}

