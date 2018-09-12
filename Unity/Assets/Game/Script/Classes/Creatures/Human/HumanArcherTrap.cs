using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanArcherTrap : Trap{
   public static GameObject prefab;

   const int _totalLifeSpam = 15;
   const int _snareAmmount = 1;
   
   private bool isHidden = false;
   
	public HumanArcherTrap(int x, int y, int team) : base(prefab, x, y, team, _totalLifeSpam, 0){
      Transform t = spriteInstance.GetComponent<Transform>();
      Vector3 scale = t.localScale;
      t.localScale = new Vector3(Terrain._terrainSize * 10, Terrain._terrainSize * 10, scale.z);
	}

   public override void newTurn(int turn){
      base.newTurn(turn);
      if(!isHidden) hide();
   }

   public override void activate(Creature creature){
      if(creature is UndeadKnight && (creature as UndeadKnight).isImmaterial){
         GameController.console.Log("Cannot snare immaterial undead knight\n");
         return;
      }
      creature.snareDuration += _snareAmmount;
      creature.actionPoints = 0;
      base.activate(creature);
      GameController.console.Log("trap activated");
   }

   public void hide(){
      spriteInstance.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
      isHidden = true;
   }
}
