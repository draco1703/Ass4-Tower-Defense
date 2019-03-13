using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtRot : MonoBehaviour
{
    //Vector3 startRot;
    Quaternion iniRot;

    // Start is called before the first frame update
    void Awake()
    {
        //Vector3 startRot = transform.rotation.eulerAngles;
        iniRot = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = iniRot;
        //transform.rotation = Quaternion.Euler(new Vector3(transform.x, ;
    }
}
