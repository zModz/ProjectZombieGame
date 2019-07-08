using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectInfo : MonoBehaviour {

    public string MapName;
    public Text MapName_Text;
    public string MapDesc;
    public Text MapDesc_Text;
    public Sprite MapImage_Sprite;
    public Image MapImage;

    public void OnMouseClick()
    {
        MapName_Text.text = MapName.ToString();
        MapDesc_Text.text = MapDesc.ToString();
        MapImage_Sprite = MapImage.sprite;
    }
}
