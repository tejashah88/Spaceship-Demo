#pragma strict

var key : KeyCode;
var directory : String = "Screenshots/";
var superSize : int = 0;

function Update () {
if(!Input.GetKeyDown(key))	return;
var now : System.DateTime = System.DateTime.Now;
var zero : char = char.Parse("0");
var nameString : String = directory + now.Year + now.Month.ToString().PadLeft(2, zero) + now.Day.ToString().PadLeft(2, zero) + now.Hour.ToString().PadLeft(2, zero) + now.Minute.ToString().PadLeft(2, zero) + now.Second.ToString().PadLeft(2, zero) + now.Millisecond.ToString().PadLeft(3, zero) + ".png";
print("Saving " + nameString + "...");
ScreenCapture.CaptureScreenshot(nameString, superSize);
}