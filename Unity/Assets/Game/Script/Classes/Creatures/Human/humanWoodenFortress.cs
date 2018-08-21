using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanWoodenFortress{
   public static GameObject prefab;

   public int additionalDefense = 30;
   public int team;
   public Terrain terrain;
   const int _maxLifeSpan = 15;
   public int lifespan = _maxLifeSpan;
   public GameObject spriteInstance;

   public static List<HumanWoodenFortress> allFortresses = new List<HumanWoodenFortress>();

   public HumanWoodenFortress(Terrain terrain, int team){
      this.team = team;
      this.terrain = terrain;
      this.terrain.fortress = this;
      
      this.spriteInstance = MonoBehaviour.Instantiate(prefab);
      this.spriteInstance.transform.localScale = new Vector3(0.6f, 0.6f, 0.5f);
      this.spriteInstance.transform.position = this.terrain.spriteInstance.transform.position + new Vector3(0, 0, -0.00005f);

      if(terrain.creature != null && terrain.creature.team == team){
         terrain.creature.defenseResistance += additionalDefense;
      }

      allFortresses.Add(this);
   }

   public void newTurn(int turn){
      lifespan--;
      if(lifespan == 0){
         terrain.fortress = null;
         allFortresses[allFortresses.IndexOf(this)] = null;
         MonoBehaviour.Destroy(spriteInstance);
         if(terrain.creature != null && terrain.creature.team == team)
            terrain.creature.defenseResistance -= additionalDefense;
         Debug.Log("fortress dead");
      }
   }
}
