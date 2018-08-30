using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour {

    public Image healthbar;

    private void Start()
    {
        healthbar.fillAmount = 1;
    }

    public void updateHealthBar(float hp, float maxhp)
    {
       healthbar.fillAmount =  hp / maxhp;
    }
}
