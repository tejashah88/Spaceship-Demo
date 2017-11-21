#pragma strict

var yawSegment : Transform;
var pitchSegment : Transform;
var yawSpeed : float = 30f;
var pitchSpeed : float = 30f;
var yawLimit : float = 90f;
var pitchLimit : float = 90f;
var target : Vector3;
private var yawSegmentStartRotation : Quaternion;
private var pitchSegmentStartRotation : Quaternion;

function Start () {
yawSegmentStartRotation = yawSegment.localRotation;
pitchSegmentStartRotation = pitchSegment.localRotation;
}

function Update () {
var angle : float;
var targetRelative : Vector3;
var targetRotation : Quaternion;
if(yawSegment && yawLimit != 0f){
	targetRelative = yawSegment.InverseTransformPoint(target);
	angle = Mathf.Atan2(targetRelative.x, targetRelative.z) * Mathf.Rad2Deg;
	if(angle >= 180f)	angle = 180f - angle;	if(angle <= -180f)	angle = -180f + angle;
	targetRotation = yawSegment.rotation * Quaternion.Euler(0f, Mathf.Clamp(angle, -yawSpeed * Time.deltaTime, yawSpeed * Time.deltaTime), 0f);
	if(yawLimit < 360f && yawLimit > 0f)	yawSegment.rotation = Quaternion.RotateTowards(yawSegment.parent.rotation * yawSegmentStartRotation, targetRotation, yawLimit);
	else	yawSegment.rotation = targetRotation;
}
if(pitchSegment && pitchLimit != 0f){
	targetRelative = pitchSegment.InverseTransformPoint(target);
	angle = -Mathf.Atan2(targetRelative.y, targetRelative.z) * Mathf.Rad2Deg;
	if(angle >= 180f)	angle = 180f - angle;	if(angle <= -180f)	angle = -180f + angle;
	targetRotation = pitchSegment.rotation * Quaternion.Euler(Mathf.Clamp(angle, -pitchSpeed * Time.deltaTime, pitchSpeed * Time.deltaTime), 0f, 0f);
	if(pitchLimit < 360f && pitchLimit > 0f)	pitchSegment.rotation = Quaternion.RotateTowards(pitchSegment.parent.rotation * pitchSegmentStartRotation, targetRotation, pitchLimit);
	else	pitchSegment.rotation = targetRotation;
}
Debug.DrawLine(pitchSegment.position, target, Color.red);
Debug.DrawRay(pitchSegment.position, pitchSegment.forward * (target - pitchSegment.position).magnitude, Color.green);
}

function Target(target : Vector3){
this.target = target;
}