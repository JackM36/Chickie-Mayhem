using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject player1Panel;
    public Text nameTxt1;
    public Text ammoTxt1;
    public Image charImg1;
    public Image charImgCross1;
    public GameObject player2Panel;
    public Text nameTxt2;
    public Text ammoTxt2;
    public Image charImg2;
    public Image charImgCross2;

    private void Start()
    {
        int players = GameManager.GetPlayers().Count;

        player1Panel.SetActive(true);
        if (players > 1)
        {
            player2Panel.SetActive(true);

        }
    }

    private void Update()
    {
        List<Player> players = GameManager.GetPlayers();

        foreach(Player player in players)
        {
            if(player.playerID == 1)
            {
                ammoTxt1.text = player.weapon == null ? "Ammo: -" : "Ammo: " + player.weapon.ammo.ToString();
                charImg1.sprite = player.characterImg;

                if(players.Count == 1)
                {
                    charImgCross2.gameObject.SetActive(true);
                }
            }
            if (player.playerID == 2)
            {
                ammoTxt2.text = player.weapon == null ? "Ammo: -" : "Ammo: " + player.weapon.ammo.ToString();
                charImg2.sprite = player.characterImg;

                if (players.Count == 1)
                {
                    charImgCross1.gameObject.SetActive(true);
                }
            }
        }
    }
}
