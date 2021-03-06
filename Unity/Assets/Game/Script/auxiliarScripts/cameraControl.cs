﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour {

   float scrollAmmount = -100;

	public void middleCamera () {
		float tx = Terrain.terrainWidth * Terrain._terrainSize * 13 / 2;
      float ty = Terrain.terrainHeight * Terrain._terrainSize * 13 / 2;
      float tz = transform.position.z;
      transform.position = new Vector3(tx, ty, tz);
	}
	
	// Update is called once per frame
	void Update () {
      if (Input.GetMouseButton(1)) {

         float dx = Input.GetAxisRaw("Mouse X");
         float dy = Input.GetAxisRaw("Mouse Y");
         Vector3 deltaCamera = new Vector3(-dx, -dy, 0) * -scrollAmmount / 5;
         transform.position += deltaCamera;
      }
      scrollAmmount += Input.mouseScrollDelta.y * 10;
      if (scrollAmmount > -0.5f) scrollAmmount = -0.5f;
      float x = transform.position.x;
      float y = transform.position.y;
      transform.position = new Vector3(x, y, scrollAmmount);
   }
}
