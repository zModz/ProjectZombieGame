#pragma strict

var recoiler : GameObject;

//How big it recoils
var verticalKick : float;
var sideKick : float;

//If you want your gun recoil to right side, change this var to 0<additionalSideKick 
//If you want your gun recoil to left side, change this var to 0>additionalSideKick 
//If you don't want it recoil to either way, set this var  = 0
var additionalSideKick : float;

private var recoilToVertical : float;
private var recoilToHorizontal : float;
private var additionalRecoilToSide : float;


function Start()
 {
  recoiler = GameObject.Find("Recoiler");
 }


function Update () 
 {
  recoilToVertical = verticalKick;
  recoilToHorizontal = sideKick;
  additionalRecoilToSide = additionalSideKick;

  if (Input.GetButton("Fire1"))
     {
       Debug.Log("Fired");
       Recoil();
     }
 }
 
 
function Recoil()
 {
  recoiler.transform.SendMessage("ApplyKickToVertical",recoilToVertical,SendMessageOptions.DontRequireReceiver);
  recoiler.transform.SendMessage("ApplyKickToHorizontal",recoilToHorizontal,SendMessageOptions.DontRequireReceiver);
  recoiler.transform.SendMessage("ApplyAdditionalKickToSide",additionalRecoilToSide,SendMessageOptions.DontRequireReceiver);
 }