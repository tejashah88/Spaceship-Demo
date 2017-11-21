#pragma strict
@script RequireComponent(Rigidbody);
@script RequireComponent(Collider);

var walkSpeed : float = 3f;
var runSpeed : float = 10f;
var jumpHeight : float = 2f;
var strength : float = 10f;
var walkOrRunKey : KeyCode = KeyCode.LeftShift;
var normallyRunning : boolean = true;
@Tooltip("Automatically orient character against curved terrains.")
var autoOrient : boolean = false;
@Tooltip("Character's local up direction, for curved terrains.")
var localUp : Vector3 = Vector3.up;
private var speed : float;
private var thrust : Vector3;
private var targetVelocity : Vector3;
private var flying : boolean = true;
private var frameRotation : Quaternion = Quaternion.identity;
private var oldVelocity : Vector3;

function OnCollisionStay(collision : Collision){
if(Input.GetButtonDown("Jump"))	return;
flying = false;
if(!autoOrient)	return;
localUp = (transform.position - collision.transform.position).normalized;
}

function OnCollisionExit(){
flying = true;
}

function FixedUpdate () {
if(Input.GetKey(KeyCode.E))	Debug.Break();
if(!flying && Input.GetButtonDown("Jump")){
	GetComponent.<Rigidbody>().AddRelativeForce(Vector3.up * Mathf.Sqrt(Physics.gravity.magnitude * jumpHeight * 2f), ForceMode.VelocityChange);
	flying = true;
	return;
}
if(Input.GetAxisRaw("Horizontal") == 0f && Input.GetAxisRaw("Vertical") == 0f){
	if(!flying && GetComponent.<Rigidbody>().velocity.sqrMagnitude > Physics.sleepThreshold){
		thrust = -GetComponent.<Rigidbody>().velocity.normalized;
		thrust += transform.TransformDirection(thrust).y * transform.up;
		GetComponent.<Rigidbody>().AddForce(thrust * strength, ForceMode.Acceleration);
	}
	return;
}
if(flying)	frameRotation = transform.rotation;
else	frameRotation = Quaternion.FromToRotation(Vector3.up, localUp);
frameRotation *= Quaternion.Euler(0f, (Quaternion.Inverse(frameRotation) * Camera.main.transform.rotation).eulerAngles.y + Mathf.Atan2(Input.GetAxisRaw("Vertical"), -Input.GetAxisRaw("Horizontal")) * Mathf.Rad2Deg - 90f, 0f);	//TODO: Make this adjust for non-flat worlds
transform.rotation = Quaternion.RotateTowards(transform.rotation, frameRotation, 90f * Time.fixedDeltaTime * strength);
if(Input.GetKey(walkOrRunKey) == normallyRunning || flying)	speed = walkSpeed;
else	speed = runSpeed;
targetVelocity = frameRotation * Vector3.forward * speed * 2f;
oldVelocity = GetComponent.<Rigidbody>().velocity;
thrust = targetVelocity - Vector3.Project(oldVelocity, targetVelocity) - oldVelocity;
thrust -= transform.TransformDirection(thrust).y * transform.up;
GetComponent.<Rigidbody>().AddForce(thrust * strength, ForceMode.Acceleration);
Debug.DrawRay(transform.position, oldVelocity, Color.red);
Debug.DrawLine(transform.position + oldVelocity, transform.position + Vector3.Project(oldVelocity, targetVelocity), Color.yellow);
Debug.DrawRay(transform.position, targetVelocity, Color.green);
Debug.DrawRay(transform.position, thrust, Color.blue);
}