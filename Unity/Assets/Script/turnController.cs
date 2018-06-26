﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController{
   public static int turn = 0;
   public GameObject player1;
   public GameObject player2;

   public static void nextTurn() {
      turn = 1 - turn;
      foreach (Creature creature in Creature.allCreatures) {
         creature.newTurn();
      }
   }
}