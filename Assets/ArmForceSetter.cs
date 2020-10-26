using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmForceSetter : MonoBehaviour
{
    public ArticulationBody[] articulationBodies;
    public float[] stiffnesses;
    public float[] maxForces;
    private void OnValidate()
    {
        if(articulationBodies.Length / 2 != stiffnesses.Length)
        {
            stiffnesses = new float[articulationBodies.Length / 2];
        }
        if (articulationBodies.Length / 2 != maxForces.Length)
        {
            maxForces = new float[articulationBodies.Length / 2];
        }
        for(int i =0; i < articulationBodies.Length; i++)
        {
            if (!articulationBodies[i])
                continue;
            ArticulationDrive drive = articulationBodies[i].xDrive;
            drive.stiffness = stiffnesses[i % (articulationBodies.Length / 2)];
            drive.forceLimit = maxForces[i % (articulationBodies.Length / 2)];
            articulationBodies[i].xDrive = drive;
        }
    }
}
