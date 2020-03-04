using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkSlotBehaviour : MonoBehaviour
{
    public Player_Script player;

    public Image overlay;
    public Image prkIcn;
    public Image Actover;
    public Color ActiveCLR;

    public bool isActive;
    public int scoreToGet;
    public GameObject button;
    public Text input;
    public KeyCode inputbtn;

    private void Awake()
    {
        button.SetActive(false);
    }

    void Update()
    {
        if(player.ScoreSlider.value == scoreToGet)
        {
            isActive = true;
        }


        if (isActive)
        {
            button.SetActive(true);
            input.text = inputbtn.ToString();

            Actover.color = ActiveCLR;
        }
    }
}
