using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public float moveSmoothness;
    public float rotSmoothness;
    public Vector3 moveOffset;
    public Vector3 rotOffset;
    public Transform carTarget;
    void FixedUpdate() 
    {
        Follow();
    }
    void Follow()
    {
        Movement();
        Rotation();
    }
    void Movement()
    {
        Vector3 targetPos=new Vector3();
        targetPos=carTarget.TransformPoint(moveOffset);
        transform.position=Vector3.Lerp(transform.position,targetPos,moveSmoothness*Time.deltaTime);
    }
   void Rotation()
    {
        var dir=carTarget.position-transform.position;
        var rot=new Quaternion();
        rot=Quaternion.LookRotation(dir+rotOffset,Vector3.up);
        transform.rotation=Quaternion.Lerp(transform.rotation, rot, rotSmoothness*Time.deltaTime);
    }
}
