#pragma strict

var delay : float = 1f;
var explosion : GameObject;
var detachChildren : boolean = true;

function Start () {
Invoke("Die", delay);
}

function Die () {
if(detachChildren)	transform.DetachChildren();
if(explosion != null)	Instantiate(explosion, transform.position, transform.rotation);
Destroy(gameObject);
}