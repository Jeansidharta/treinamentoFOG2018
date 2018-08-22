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
   [SerializeField] Sprite[] teamImg;
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
   private GameObject WinUI;
   private GameObject winText;
   private GameObject winImg;

    private Creature selectedCreature;
    
   private void Start(){
      GameController.guiController = this;
      unitImgObject.GetComponent<Image>().sprite = blankImg;
      panel = GameObject.FindGameObjectsWithTag("DescriptionPanel");
      skillGO = GameObject.FindGameObjectsWithTag("SkillObject").OrderBy(go => go.name).ToArray(); 
      skillN = GameObject.FindGameObjectsWithTag("SkillText").OrderBy(go => go.name).ToArray();
      skillD = GameObject.FindGameObjectsWithTag("SkillDescription").OrderBy(go => go.name).ToArray(); 
   }

   public void selectCreature(Creature creature){
      selectedCreature = creature;
      if(creature == null){
         selectBlank();
         return;
      }
      //Assigning Selected creature's value to diplay on GUI
      text_name.text = "Name: " + creature.name;
      text_team.text = "Team: " + creature.teamName;
      text_HP.text = "HP: " + creature.health.ToString();
      text_AP.text = "AP: " + creature.actionPoints.ToString();
      text_DE.text = "DE: " + creature.defenseResistance.ToString();
      text_AT.text = "AT: " + creature.attackDamage.ToString();

      //Checking number of skills and displaying their names and description on GUI
      for(int skillCount = 0; skillCount < creature.skills_names.Length; skillCount++){
         skillGO[skillCount].SetActive(true);
         skillN[skillCount].GetComponent<Text>().text = creature.skills_names[skillCount];
         skillD[skillCount].GetComponent<Text>().text = creature.skill_description[skillCount];
      }
      for(int skillCount = creature.skills_names.Length; skillCount < 3; skillCount++){
         skillGO[skillCount].SetActive(false);
         skillN[skillCount].GetComponent<Text>().text = null;
         skillD[skillCount].GetComponent<Text>().text = null;
      }

      //text_EV.text = "EV: " + creatureClicked.evasion.ToString();

      //Assigning Selected creature's image file to display on GUI
      //Humans Team
      var display0 = skillGO[0].GetComponent<DisplaySkill>();
      var display1 = skillGO[1].GetComponent<DisplaySkill>();
      var display2 = skillGO[2].GetComponent<DisplaySkill>();
      display0.skillFunction = null;
      display1.skillFunction = null;
      display2.skillFunction = null;
      display0.creature = creature;
      display1.creature = creature;
      display2.creature = creature;
      if (creature is HumanArcher){
         unitImgObject.GetComponent<Image>().sprite = Harcher;
         display0.skillFunction = (creature as HumanArcher).previewTrap;
      }
      else if (creature is HumanHero){
         unitImgObject.GetComponent<Image>().sprite = HHero;
         display0.skillFunction = (creature as HumanHero).previewFortress;
         display1.skillFunction = (creature as HumanHero).previewCorner;
         display2.skillFunction = (creature as HumanHero).lastResource;
      }
      else if (creature is HumanKnight){
         unitImgObject.GetComponent<Image>().sprite = HKnight;
         display0.skillFunction = (creature as HumanKnight).assault;
      }
      else if (creature is HumanSiege){
         unitImgObject.GetComponent<Image>().sprite = HSiege;
         display0.skillFunction = (creature as HumanSiege).toggleMount;
         display1.skillFunction = (creature as HumanSiege).habilityInvert;
         display2.skillFunction = (creature as HumanSiege).habilityPush;
      }
      else if (creature is HumanSoldier){
         unitImgObject.GetComponent<Image>().sprite = HSoldier;
         display0.skillFunction = (creature as HumanSoldier).raiseShields;
      }
      else if(creature is UndeadArcher){
         unitImgObject.GetComponent<Image>().sprite = UDarcher;
         display0.skillFunction = (creature as UndeadArcher).toxic;
      }
      else if (creature is UndeadHero){
         unitImgObject.GetComponent<Image>().sprite = UDHero;
      }
      else if (creature is UndeadKnight){
         unitImgObject.GetComponent<Image>().sprite = UDKnight;
      }
      else if (creature is UndeadSiege){
         unitImgObject.GetComponent<Image>().sprite = UDSiege;
         display1.skillFunction = (creature as UndeadSiege).previewSupress;
      }
      else if (creature is UndeadSoldier){
         unitImgObject.GetComponent<Image>().sprite = UDSoldier;
         display0.skillFunction = (creature as UndeadSoldier).previewCursedTouch;
      }
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

    public void gameOver(int player, int team ){
        //Setting WinUI Active
        WinUI = GameObject.FindGameObjectWithTag("WinUI");
        WinUI.SetActive(true);
        
        //Setting win text
        winText = GameObject.FindGameObjectWithTag("WinText");
        winText.GetComponent<Text>().text = "Player " + (player+1).ToString() + " wins";

        //Setting Winner Image
        winImg = GameObject.FindGameObjectWithTag("WinImg");
        winImg.GetComponent<Image>().sprite = teamImg[team];

    }

}
