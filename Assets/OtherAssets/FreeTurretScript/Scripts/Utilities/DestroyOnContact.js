#pragma strict

var explosion : GameObject;
var ignoreTags : String[];
var destroySelf : boolean = true;
var detachChildren : boolean = true;
var destroyOther : boolean = false;

function OnCollisionEnter(collision : Collision){
for(var ignoreTag : String in ignoreTags)
	if(collision.gameObject.tag == ignoreTag)	return;
if(destroySelf){
	Destroy(gameObject);
	if(detachChildren)	transform.DetachChildren();
}
if(destroyOther)	Destroy(collision.gameObject);
if(explosion != null)	Instantiate(explosion, collision.contacts[0].point, Random.rotation);
}