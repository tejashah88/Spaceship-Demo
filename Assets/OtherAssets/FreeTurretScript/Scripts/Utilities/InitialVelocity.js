#pragma strict

var startVelocity : Vector3;
var randomize : boolean = false;
var localSpace : boolean = false;

function Start () {
if(randomize)	startVelocity = Random.rotation * startVelocity;
if(GetComponent.<Rigidbody>() == null)	return;
yield WaitForFixedUpdate();
if(localSpace)	GetComponent.<Rigidbody>().velocity += transform.TransformDirection(startVelocity);
else	GetComponent.<Rigidbody>().velocity += startVelocity;
}