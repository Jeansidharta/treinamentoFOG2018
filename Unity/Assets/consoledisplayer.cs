﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class consoledisplayer : MonoBehaviour {

    string[] lines = new string[7];
    [SerializeField] Text panel;
    int current_line = 0;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < 7; i++) lines[i] = " ";


    }

    // Update is called once per frame
    void Update () {
        updatePanel();
	}

    void shift()
    {
        for(int i = 1; i < 7; i++)
        {
            lines[i - 1] = lines[i];
        }
    }

    void panelLog(string t)
    {
        if (current_line > 6)
        {
            shift();
            lines[6] = t;

        }
        else {
            lines[current_line] = t;
            current_line++;
        }
    }

    void updatePanel()
    {
        panel.text = lines[0] + lines[1] + lines[2] + lines[3] + lines[4] + lines[5] + lines[6];
    }

}