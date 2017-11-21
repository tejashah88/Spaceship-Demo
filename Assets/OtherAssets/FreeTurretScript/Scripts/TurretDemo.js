#pragma strict

var turrets : GameObject[];

function Start () {

}

function Update () {
for(var turret : GameObject in turrets as GameObject[])
	turret.SendMessage("Target", transform.position);
}