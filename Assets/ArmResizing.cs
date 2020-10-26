using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ArmResizing : MonoBehaviour
{
    public float length;
    public Transform[] l50;
    public Transform[] l25;
    void Update()
    {
        foreach(Transform t in l50)
        {
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, length / 2);
        }
        foreach (Transform t in l25)
        {
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, length / 4);
        }
    }
}
