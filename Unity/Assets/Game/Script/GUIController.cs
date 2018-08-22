using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GUIController : MonoBehaviour {
   /*-------GUI Scripting----------*/
   [SerializeField] Text text_name;
   [SerializeField] Text text_team;
   [SerializeField] Text text_HP;
   [SerializeField] Text text_AP;
   [SerializeField] Text text_DE;
   [SerializeField] Text text_AT;
   [SerializeField] Text text_EV;
   [SerializeField] Image unitImgObject;
   [SerializeField] Sprite blankImg;
   [SerializeField] Sprite Harcher;
   [SerializeField] Sprite HHero;
   [SerializeField] Sprite HKnight;
   [SerializeField] Sprite HSiege;
   [SerializeField] Sprite HSoldier;
   [SerializeField] Sprite UDarcher;
   [SerializeField] Sprite UDHero;
   [SerializeField] Sprite UDKnight;
   [SerializeField] Sprite UDSiege;
   [SerializeField] Sprite UDSoldier;

   private GameObject[] panel;
   private GameObject[] skillGO;
   private GameObject[] skillN;
   private GameObject[] skillD;

   private Creature selectedCreature;
    
   private void Start(){
      GameController.guiController = this;
      unitImgObject.GetComponent<Image>().sprite = blankImg;
      panel = GameObject.FindGameObjectsWithTag("DescriptionPanel");
      skillGO = GameObject.FindGameObjectsWithTag("SkillObject").OrderBy(go => go.name).ToArray(); 
      skillN = GameObject.FindGameObjectsWithTag("SkillText").OrderBy(go => go.name).ToArray();
      skillD = GameObject.FindGameObjectsWithTag("SkillDescription").OrderBy(go => go.name).ToArray();
      selectBlank();
   }

   public void selectCreature(Creature creature){
      selectedCreature = creature;
      if(creature == null){
         selectBlank();
         return;
      }
      //Assigning Selected creature's value to diplay on GUI
      text_name.text = "Name: " + creature.getName();
      text_team.text = "Team: " + creature.getRaceName();
      text_HP.text = "HP: " + creature.health.ToString();
      text_AP.text = "AP: " + creature.actionPoints.ToString();
      text_DE.text = "DE: " + creature.defenseResistance.ToString();
      text_AT.text = "AT: " + creature.attackDamage.ToString();

      //Checking number of skills and displaying their names and description on GUI
      for(int aux = 0; aux < creature.skills.Length; aux++){
         if(creature.skills[aux] == null){
            skillGO[aux].SetActive(false);
            skillN[aux].GetComponent<Text>().text = null;
            skillD[aux].GetComponent<Text>().text = null;
            continue;
         }
         skillGO[aux].SetActive(true);
         var script = skillGO[aux].GetComponent<DisplaySkill>();
         script.skillFunction = creature.skills[aux].function;
         script.creature = creature;
         skillN[aux].GetComponent<Text>().text = creature.skills[aux].name;
         skillD[aux].GetComponent<Text>().text = creature.skills[aux].description;
      }
      
      //Assigning Selected creature's image file to display on GUI
      if (creature is HumanArcher)
         unitImgObject.GetComponent<Image>().sprite = Harcher;
      else if (creature is HumanHero)
         unitImgObject.GetComponent<Image>().sprite = HHero;
      else if (creature is HumanKnight)
         unitImgObject.GetComponent<Image>().sprite = HKnight;
      else if (creature is HumanSiege)
         unitImgObject.GetComponent<Image>().sprite = HSiege;
      else if (creature is HumanSoldier)
         unitImgObject.GetComponent<Image>().sprite = HSoldier;
      else if(creature is UndeadArcher)
         unitImgObject.GetComponent<Image>().sprite = UDarcher;
      else if (creature is UndeadHero)
         unitImgObject.GetComponent<Image>().sprite = UDHero;
      else if (creature is UndeadKnight)
         unitImgObject.GetComponent<Image>().sprite = UDKnight;
      else if (creature is UndeadSiege)
         unitImgObject.GetComponent<Image>().sprite = UDSiege;
      else if (creature is UndeadSoldier)
         unitImgObject.GetComponent<Image>().sprite = UDSoldier;
   }

   public void selectBlank(){
      text_name.text = "Name: ";
      text_team.text = "Team: ";
      text_HP.text = "HP: ";
      text_AP.text = "AP: ";
      text_DE.text = "DE: ";
      text_AT.text = "AT: ";
      text_EV.text = "EV: ";
      for (int i = 0; i < skillGO.Length; i++) skillGO[i].SetActive(false);
      for (int i = 0; i < panel.Length; i++) panel[i].SetActive(false);
      unitImgObject.GetComponent<Image>().sprite = blankImg;
   }
}
