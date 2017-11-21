#pragma strict

var maxRange : int = 1000;

function Start () {

}

function Update () {
if(transform.position.magnitude > maxRange){
	transform.position = Vector3.Lerp(transform.position, Vector3.ClampMagnitude(transform.position, maxRange), 0.5);
}
}