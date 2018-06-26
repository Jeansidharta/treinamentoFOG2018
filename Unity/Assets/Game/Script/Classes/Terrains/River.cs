using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River : Terrain {
   const int _walkSpeed = 3;
   public River(GameObject prefab, int x, int y) : base(prefab, x, y, _walkSpeed) {
   }
}