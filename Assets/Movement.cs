using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR;
using UnityEngine.XR;

public class Movement : MonoBehaviour
{
    public Transform HeadTransform;
    public Transform MovementArticulationBodyRoot;
    public ArticulationBody LegSpring;
    public ArticulationBody foot;
    public float speed;

    private Vector3 headLastPose;
    private float footRaduis;
    private FixedJoint movementJoint;
    private void Start()
    {
        footRaduis = foot.GetComponent<SphereCollider>().radius;
        movementJoint = gameObject.AddComponent<FixedJoint>();
        movementJoint.connectedArticulationBody = MovementArticulationBodyRoot.GetComponent<ArticulationBody>();
    }
    void FixedUpdate()
    {
        transform.rotation = Quaternion.identity;
        MovementArticulationBodyRoot.position = HeadTransform.position;
        MovementArticulationBodyRoot.rotation = Quaternion.identity;
        Vector3 delta = HeadTransform.position - transform.position - headLastPose + GetMovement() ;
        Vector3 flatDelta = Vector3.ProjectOnPlane(delta, Vector3.up);
        movementJoint.autoConfigureConnectedAnchor = false;
        movementJoint.connectedAnchor = -(MovementArticulationBodyRoot.position - transform.position);
        
        ArticulationDrive drive = LegSpring.yDrive;
        drive.target = -(MovementArticulationBodyRoot.position - transform.position).y + footRaduis;
        LegSpring.yDrive = drive;
        RotateFoot(delta); 
        Walk(flatDelta.magnitude);

        headLastPose = HeadTransform.position - transform.position;
        
    }
    void Walk(float distance)
    {
        Debug.Log(distance + " " + distance * footRaduis  * 2* Mathf.Rad2Deg);
        ArticulationDrive drive = foot.xDrive;
        drive.target += distance / footRaduis * Mathf.Rad2Deg;
        foot.xDrive = drive;
    }
    void RotateFoot(Vector3 delta)
    {
        if(delta != Vector3.zero)
            RotateFoot(Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg);
    }
    void RotateFoot(float angle)
    {
        LegSpring.transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);
        LegSpring.anchorRotation = LegSpring.transform.rotation;
    }
    Vector3 GetMovement()
    {
        Vector3 output = default;
        var leftHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);

        if (leftHandDevices.Count == 1)
        {
            InputDevice device = leftHandDevices[0];
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 thumbstick);
            output = Quaternion.AngleAxis(HeadTransform.rotation.eulerAngles.y, Vector3.up) * new Vector3(thumbstick.x, 0, thumbstick.y) * speed * Time.deltaTime;
        }

        return output;
    }
}
