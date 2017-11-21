#pragma strict

@Tooltip("Euler rotation to apply in degrees per second.")
var rotation : Vector3 = Vector3.zero;
@Tooltip("Rotate in local space rather than world space.")
var local : boolean = false;

function Update () {
transform.Rotate(rotation * Time.deltaTime, local ? Space.Self : Space.World);
}