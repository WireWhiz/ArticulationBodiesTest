using UnityEngine;

public class TorsoController : MonoBehaviour
{
    public Transform head;
    public Transform torso;
    public Transform leftShoulder;
    public ArticulationBody leftShoulderBody;
    public Transform rightShoulder;
    public ArticulationBody rightShoulderBody;
    public float neckLength;
    public float shoulderWidth;
    public float shoulderRotationSmoothTime;

    private float torsoRotation;
    private float torsoVelocity;

    private ArticulationBody joint;
    private void Start()
    {
        joint = torso.GetComponent<ArticulationBody>();
        if (leftShoulderBody)
        {
            leftShoulder.localPosition = new Vector3(-shoulderWidth / 2, 0, 0);
            leftShoulderBody.anchorPosition = Quaternion.Inverse(leftShoulder.localRotation) * new Vector3(shoulderWidth / 2, 0, 0);
            leftShoulderBody.anchorRotation = Quaternion.Inverse(leftShoulder.localRotation);
            //leftShoulderBody.parentAnchorRotation = leftShoulder.localRotation;

        }
        if (rightShoulderBody)
        {
            rightShoulder.localPosition = new Vector3(shoulderWidth / 2, 0, 0);
            rightShoulderBody.anchorPosition = Quaternion.Inverse(rightShoulder.localRotation) * new Vector3(-shoulderWidth / 2, 0, 0);
            rightShoulderBody.anchorRotation = Quaternion.Inverse(rightShoulder.localRotation);
            //rightShoulderBody.parentAnchorRotation = rightShoulder.localRotation;

        }
    }

    void Update()
    {
        torsoRotation = Mathf.SmoothDampAngle(torsoRotation, head.rotation.eulerAngles.y, ref torsoVelocity, shoulderRotationSmoothTime);
        joint.anchorRotation = Quaternion.AngleAxis(-torsoRotation, Vector3.up);
        joint.anchorPosition = Vector3.up * neckLength;
    }
}
