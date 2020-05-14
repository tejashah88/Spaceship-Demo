using UnityEngine;
using System.Collections;
public class SmoothFollow2 : MonoBehaviour {
    public Transform target;
    public float distance = 3.0f;
    public float height = 3.0f;
    public float damping = 5.0f;
    public bool smoothRotation = true;
    public bool followBehind = true;
    public float rotationDamping = 10.0f;
    public float rotationLookOffset;

    public void setDamping(float newDamping) {
        damping = newDamping;
    }

    void FixedUpdate() {
        Vector3 wantedPosition;
        if (followBehind)
            wantedPosition = target.TransformPoint(0, height, -distance);
        else
            wantedPosition = target.TransformPoint(0, height, distance);

        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

        if (smoothRotation) {
            Quaternion wantedRotation = Quaternion.LookRotation(
                target.position - transform.position + Vector3.Scale(
                    Vector3.one * rotationLookOffset,
                    target.transform.forward
                ),
                target.up
            );
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
        } else
            transform.LookAt(target, target.up);
    }
}