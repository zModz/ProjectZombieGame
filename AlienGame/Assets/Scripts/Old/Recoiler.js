#pragma strict

var recoilControlToVertical : float;
var recoilControlToHorizontal : float;

var maxVerticalRotation : float;
var maxHorizontalRotation : float;

private var verticalRotation : float;
private var horizontalRotation : float;

function Start () {

}

function Update () 
{
 RecoilControl();
 transform.localRotation.x = -verticalRotation/100;
 transform.localRotation.y = horizontalRotation/100;
}


public function ApplyKickToVertical(recoilToVertical : int)
 {
   verticalRotation += recoilToVertical;
 }

public function ApplyKickToHorizontal(recoilToHorizontal : int)
 {
   horizontalRotation += Random.Range(-recoilToHorizontal,recoilToHorizontal);
 }
 
public function ApplyAdditionalKickToSide(additionalRecoilToSide : int)
 {
   horizontalRotation += additionalRecoilToSide/2;
 }
 
 
function RecoilControl()
 {
   if(verticalRotation > 0)
      {
        verticalRotation -= recoilControlToVertical * Time.deltaTime;
      }
    
   if(horizontalRotation > 0)
      {
        horizontalRotation -= recoilControlToHorizontal * Time.deltaTime;
      }
    
   if(horizontalRotation < 0)
      {
        horizontalRotation += recoilControlToHorizontal * Time.deltaTime;
      }


   if(verticalRotation > maxVerticalRotation)
      {
        verticalRotation = maxVerticalRotation + Random.Range(-1,2);
      }
    
   if(horizontalRotation > maxHorizontalRotation)
      {
        horizontalRotation = maxHorizontalRotation + Random.Range(-1,1);;
      }
 }
 
 
 