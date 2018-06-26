using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plains : Terrain{
   const int _walkSpeed = 1;
   public Plains(GameObject prefab, int x, int y) : base(prefab, x, y, _walkSpeed) {
   }
}