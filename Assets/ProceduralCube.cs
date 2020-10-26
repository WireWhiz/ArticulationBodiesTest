using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProceduralCube : MonoBehaviour
{
    public Transform target;
    public GameObject cubePrefab;
    public float scale;

    public Transform child;
    void Update()
    {
        if (!target || !cubePrefab)
            return;
        if (!child && cubePrefab)
        {
            child = Instantiate(cubePrefab, transform).transform;
        }

        child.position = (transform.position + target.position) / 2;
        child.rotation = Quaternion.LookRotation(target.position - transform.position, transform.up);
        child.localScale = new Vector3(scale, scale, (target.position - transform.position).magnitude);
    }
}
