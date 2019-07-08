using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyableUI : MonoBehaviour
{
    public Text BuyInfo;
    public BuyScript buy;
    public Transform eyesPoint;

    void Start()
    {
        BuyInfo.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        //BuyInfo.text = "";

        RaycastHit hit;
        if (Physics.Raycast(eyesPoint.position, eyesPoint.transform.forward, out hit, 10))
        {
            if (hit.transform == gameObject.transform)
            {
                Debug.Log(hit.transform + " == " + buy.transform);
                BuyInfo.text = buy.weaponToBuy.GetComponent<WeaponScript>().WeaponName.ToString();
            }
        }
    }
}
