using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortress : Terrain {
   const int _walkSpeed = 1;
   public Fortress(GameObject prefab, int x, int y) : base(prefab, x, y, _walkSpeed) {
   }
}