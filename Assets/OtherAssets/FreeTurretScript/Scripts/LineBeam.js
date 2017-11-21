#pragma strict

var maxLength : float = 100f;
var requireRaycastHit : boolean = false;
private var lineRenderer : LineRenderer;
private var closestPoint : float;

function Start () {
lineRenderer = GetComponent.<LineRenderer>();
if(!lineRenderer){	Debug.LogWarning("The 'LineBeam' script on " + gameObject.name + " requires a line renderer! Deactivating.");	this.enabled = false;	}
lineRenderer.positionCount = 2;
lineRenderer.useWorldSpace = false;
}

function Update () {
if(!lineRenderer)	Start();
if(requireRaycastHit)	lineRenderer.enabled = Physics.Raycast(transform.position, transform.forward, maxLength);
if(!lineRenderer.enabled)	return;
//lineRenderer.SetPosition(0, transform.position);
closestPoint = maxLength;
for(var hit : RaycastHit in Physics.RaycastAll(transform.position, transform.forward, maxLength))
	if((hit.point - transform.position).sqrMagnitude < closestPoint * closestPoint)
		closestPoint = (hit.point - transform.position).magnitude;
lineRenderer.SetPosition(1, Vector3.forward * closestPoint);
}