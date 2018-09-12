using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class blockButonController : MonoBehaviour,IPointerUpHandler {

   private Creature selectedCreature;
   
   public void selectCreature(Creature creature){
      selectedCreature = creature;
   }

   public void OnPointerUp(PointerEventData p){
      selectedCreature.defend();
      GameController.console.Log(selectedCreature.getName() + " is blocking");
   }

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
