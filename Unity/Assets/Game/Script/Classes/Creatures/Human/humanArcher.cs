using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanArcher : Creature {
   public static GameObject prefab;
   const int _maxHealth = 300;
   const int _maxActionPoints = 3;
   const int _baseDodge = 10;
   const int _defenseHeal = 30;
   const int _defenseResistance = 20;
   const int _attackDamage = 60;
   const int _attackRange = 2;

   const int _maxTrapCooldown = 6;
   const int _minTrapAP = 1;
   private Surroundings preview = null;

   private bool movedLastTurn = true;
   private bool movedThisTurn = false;

   private consoledisplayer cnl = GameObject.FindGameObjectWithTag("Console").GetComponent<consoledisplayer>();


   public HumanArcher(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {
      skills[0] = new Skill("Armadilha de Rede", "Armadilha de rede: (Cd = 6) (Ap = 1) (alcance = 1\n\nColoca uma armadilha no chão que é visível por 1 turno e depois se torna invisível. Tal armadilha ira paralisar o alvo impedindo de se movimentar no próximo turno do jogador. A armadilha em si dura 15 turnos.\n", previewTrap, this, _minTrapAP, _maxTrapCooldown);
   }

   public override void newTeamTurn() {
      base.newTeamTurn();

      movedLastTurn = movedThisTurn;
      movedThisTurn = false;

      if (!movedLastTurn) this.attackRange = _attackRange + 1;
      else this.attackRange = _attackRange;
   }

   public override void move(int x, int y, int distance) {
      base.move(x, y, distance);
      movedThisTurn = true;
   }

   public override void attack(Creature victim) {
      if (!movedLastTurn) {
         Surroundings surroundings = terrain.expandByDistance(attackRange);

         for(int aux = 0; aux < surroundings.creatures.Count; aux ++){
            Pair<int, Creature> target = surroundings.creatures[aux];
            if(target.second == victim){
               if(target.first == 3){
                  int buffer = attackDamage;
                  attackDamage /= 2;
                  base.attack(victim);
                  attackDamage = buffer;
                  return;
               }
               break;
            }
         }
      }
      base.attack(victim);
   }

   //public Surroundings previewTrap(){
   public void previewTrap(){
      if(!skills[0].canUse()) return;
      cnl.Log("Previewing trap\n");
      preview = terrain.expandByDistance(terrain.movePointsRequired);
      GameController.overrideClick(trySetTrap);
      preview.paint(Color.blue);
   }

   public void setTrap(int x, int y){
      cnl.Log("Setting trap\n");
      new HumanArcherTrap(x, y, team);
      skills[0].use();
   }

    public void trySetTrap(Terrain terrain)
    {
        if (preview.hasTerrain(terrain.x, terrain.y))
        {
            if (terrain.creature == null && terrain.trap == null)
            {
                if (!(terrain is Mountain))
                {
                    this.setTrap(terrain.x, terrain.y);
                }
                else cnl.Log("Cant place trap in mountain\n");
            }
            else cnl.Log("Cant place trap over another creature or trap\n");
        }
        else cnl.Log("position out of range\n");
        preview.clear();
        preview = null;
    }

}
