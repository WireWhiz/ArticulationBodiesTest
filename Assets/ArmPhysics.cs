using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmPhysics : MonoBehaviour
{
    public Transform targetShoulder;
    public Transform targetElbow;
    public Transform targetHand;
    [Space(10)]
    public ArticulationBody[] shoulder;
    public ArticulationBody elbow;
    public ArticulationBody[] hand;

    void FixedUpdate()
    {
        SetDrive(shoulder[0], targetShoulder.localRotation.eulerAngles.x);
        SetDrive(shoulder[1], targetShoulder.localRotation.eulerAngles.y);
        SetDrive(shoulder[2], targetShoulder.localRotation.eulerAngles.z);
        SetDrive(elbow, targetElbow.localRotation.eulerAngles.y);
        SetDrive(hand[0], targetHand.localRotation.eulerAngles.x);
        SetDrive(hand[1], targetHand.localRotation.eulerAngles.y);
        SetDrive(hand[2], targetHand.localRotation.eulerAngles.z);
    }

    void SetDrive(ArticulationBody body, float target)
    {
        ArticulationDrive drive = body.xDrive;
        drive.target = FixAngleJump(target, drive.target);
        body.xDrive = drive;
    }

    //this removes the jump that happens when the angle goes from 0 to 359 and changes it to 0 to -1 it does this for any angle jump though it limits itself to ten rotations in any direction for performance reasons.
    float FixAngleJump(float target, float past)
    {
        //if (past > 360 * 10 || past < -360 * 10)
            //return target;
        if(target > past)
        {
            float output = target;
            while (output - past > 180)
            {
                
                output -= 360;
            }
            target = output;
        }
        else if(target < past)
        {
            float output = target;
            while (output - past < -180)
            {

                output += 360;
            }
            target = output;
        }
        return target;
    }
}
 