#pragma strict

import SceneManagement;

var key : KeyCode = KeyCode.None;
var scene : String;

function Update(){
if(Input.GetKeyDown(key))	ChangeScene();
}

function ChangeScene(){
if(scene && scene != String.Empty)	ChangeScene(scene);
}

function ChangeScene(sceneName : String){
SceneManager.LoadScene(sceneName);
}

function ChangeScene(sceneNumber : int){
SceneManager.LoadScene(sceneNumber);
}