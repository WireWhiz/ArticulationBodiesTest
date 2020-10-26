using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayPhysics : MonoBehaviour
{
    public float delay;
    void Start()
    {

        GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(StartPhysics());
    }
    IEnumerator StartPhysics()
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
