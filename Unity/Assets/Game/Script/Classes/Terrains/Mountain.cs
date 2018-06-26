using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : Terrain {
   const int _walkSpeed = -1;
   public Mountain(GameObject prefab, int x, int y) : base(prefab, x, y, _walkSpeed) {
   }
}