#pragma strict

var startVelocity : Vector3;
var randomize : boolean = false;

function Start () {
if(randomize)	startVelocity = Random.rotation * startVelocity;
if(GetComponent.<Rigidbody>() == null)	return;
yield WaitForFixedUpdate();
GetComponent.<Rigidbody>().angularVelocity += startVelocity;
}