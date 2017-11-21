
var targeter : Camera;
var bullet: GameObject;
var weaponRange = 10;
private var hit : RaycastHit;
private var target : Vector3;

function Update(){
if(Physics.Linecast(targeter.ScreenToWorldPoint(Vector3(Input.mousePosition.x, Input.mousePosition.y, targeter.nearClipPlane)), targeter.ScreenToWorldPoint(Vector3(Input.mousePosition.x, Input.mousePosition.y, weaponRange)), hit))
	target = hit.point;
else	target = targeter.GetComponent.<Camera>().ScreenToWorldPoint(Vector3(Input.mousePosition.x, Input.mousePosition.y, weaponRange));
transform.root.BroadcastMessage("Target", target);
if(Input.GetButtonDown("Fire1"))
	Instantiate(bullet, transform.position, transform.rotation);
}