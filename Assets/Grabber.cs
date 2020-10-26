using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Grabber : MonoBehaviour
{
    public XRNode hand;
    public InputSources inputSource;
    [Range(1,0)]
    public float triggerThreshold;
    public Collider mainCollider;
    [HideInInspector]
    public Interactible grabbedObject;

    public enum InputSources
    {
        trigger,
        grip
    }


    private List<Interactible> interactibles = new List<Interactible>();
    private FixedJoint joint;
    private ArticulationBody handBody;
    private float handStiffnessX;
    private float handStiffnessY;
    private float handStiffnessZ;
    void Start()
    {
        handBody = GetComponentInParent<ArticulationBody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbedObject)
        {
            if (!GetIfGrabbing())
                Release();
        }
        else
        {
            if (GetIfGrabbing())
                Grab();
        }
    }

    void Grab()
    {
        Interactible grabbing = GetClosestInteractiable();
        if (grabbing)
        {
            joint = grabbing.gameObject.AddComponent<FixedJoint>();
            joint.connectedArticulationBody = handBody;
            if (grabbing.GetComponent<Rigidbody>().isKinematic)
            {
                SetHandStiffness(false);
            }
            mainCollider.enabled = false;
            
            grabbedObject = grabbing;
        }
    }

    void Release()
    {
        Destroy(joint);
        mainCollider.enabled = true;
        grabbedObject = null;
        SetHandStiffness(true);
    }

    void SetHandStiffness(bool stiff)
    {
        if (stiff)
        {
            if (handStiffnessZ == 0)
                return;
            ArticulationDrive drive = handBody.xDrive;
            drive.stiffness = handStiffnessZ;
            handBody.xDrive = drive;

            ArticulationBody parent = handBody.transform.parent.GetComponentInParent<ArticulationBody>();
            drive = parent.xDrive;
            drive.stiffness = handStiffnessX;
            parent.xDrive = drive;

            parent = parent.transform.parent.GetComponentInParent<ArticulationBody>();
            drive = parent.xDrive;
            drive.stiffness = handStiffnessY;
            parent.xDrive = drive;
        }
        else
        {
            ArticulationDrive drive = handBody.xDrive;
            handStiffnessZ = drive.stiffness;
            drive.stiffness = 0;
            handBody.xDrive = drive;

            ArticulationBody parent = handBody.transform.parent.GetComponentInParent<ArticulationBody>();
            drive = parent.xDrive;
            handStiffnessX = drive.stiffness;
            drive.stiffness = 0;
            parent.xDrive = drive;

            parent = parent.transform.parent.GetComponentInParent<ArticulationBody>();
            drive = parent.xDrive;
            handStiffnessY = drive.stiffness;
            drive.stiffness = 0;
            parent.xDrive = drive;
        }
    }

    Interactible GetClosestInteractiable()
    {
        float distance = float.MaxValue;
        Interactible closest = null;
        foreach(Interactible i in interactibles)
        {
            float sqrDistance = (i.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance < distance)
            {
                distance = sqrDistance;
                closest = i;
            }
        }
        return closest;
    }

    bool GetIfGrabbing()
    {
        var HandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(hand, HandDevices);

        if (HandDevices.Count == 1)
        {
            InputDevice device = HandDevices[0];
            switch (inputSource)
            {
                case InputSources.trigger:
                    device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
                    return triggerValue > triggerThreshold;
                    break;
                case InputSources.grip:
                    device.TryGetFeatureValue(CommonUsages.grip, out float gripValue);
                    return gripValue > triggerThreshold;
                    break;
            }
            
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactible i = other.GetComponent<Interactible>();
        if (i && !interactibles.Contains(i))
        {
            interactibles.Add(i);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Interactible i = other.GetComponent<Interactible>();
        if (i)
        {
            interactibles.Remove(i);
        }
    }
}
