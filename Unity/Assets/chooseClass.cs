using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chooseClass : MonoBehaviour {

    [SerializeField] Text player;
    private static int current = 0;

    public static int[] teams = new int[2];

    private void Start()
    {
        player.text = "Player " + (current + 1).ToString() + ", select your team";
    }

    public void selectTeam(int t)
    {
        teams[current] = t;
        current = current + 1;
        print(current);
        if (current == teams.Length)
        {
            new startBtn().AbrirJogo();
        }
        else player.text = "Player " + (current + 1).ToString() + ", select your team";
    }

}
