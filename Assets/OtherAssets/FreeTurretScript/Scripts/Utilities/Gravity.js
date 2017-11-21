#pragma strict

@script RequireComponent(Rigidbody);

private var SOI : float;
static var precision : int = 1000;

function Start(){
	SOI = Mathf.Sqrt(Physics.gravity.magnitude * GetComponent.<Rigidbody>().mass * precision);
	print("SOI of " + name + " is " + SOI);
	var children : Rigidbody[] = gameObject.GetComponentsInChildren.<Rigidbody>();
	yield WaitForFixedUpdate();
	yield WaitForFixedUpdate();
	for(var child : Rigidbody in children)
		if(child != GetComponent.<Rigidbody>())	child.velocity += GetComponent.<Rigidbody>().velocity;
}

function FixedUpdate () {
for(var object : Collider in Physics.OverlapSphere(transform.position, SOI)){
	Debug.DrawLine(transform.position, object.transform.position, Color(0.1, 0.1, 0.1));
	if(object.attachedRigidbody == null || object.gameObject == gameObject)	continue;
	object.attachedRigidbody.AddForce(Physics.gravity.magnitude * GetComponent.<Rigidbody>().mass / (object.transform.position - transform.position).sqrMagnitude * (transform.position - object.transform.position).normalized, ForceMode.Acceleration);
}
}

function OnDrawGizmosSelected(){
	Gizmos.color = Color.gray;
	Gizmos.DrawWireSphere(transform.position, SOI);
}