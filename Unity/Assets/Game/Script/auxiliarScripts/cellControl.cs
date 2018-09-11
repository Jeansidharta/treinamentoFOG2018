using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class cellControl : MonoBehaviour {

   public Terrain cell;
   void Start () {
	}
	
	void Update () {
   }
   void OnMouseDown() {
      if(!EventSystem.current.IsPointerOverGameObject()){
         cell.mouseDown();
            Debug.Log("CLICK");
      }
   }
   void OnMouseUp() {
      if(!EventSystem.current.IsPointerOverGameObject()){
         cell.mouseUp();
      }
   }
}
