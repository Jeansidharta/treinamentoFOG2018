using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateTerrain : MonoBehaviour {
   const int squareHeight = 40;
   const int squareWidth = 64;

   public GameObject plainsPrefab;
   public GameObject forestPrefab;
   public GameObject mountainPrefab;
   public GameObject riverPrefab;
   public GameObject fortressPrefab;
   public GameObject soldierPrefab;
   public TextAsset terrainFile;

   void GenerateTerrain(string[] tokens) {
      int terrainHeight = int.Parse(tokens[0]);
      int terrainWidth = int.Parse(tokens[1]);
      Terrain[][] allTiles;
      allTiles = new Terrain[terrainHeight][];

      Terrain.allTiles = allTiles;
      Terrain.terrainHeight = terrainHeight;
      Terrain.terrainWidth = terrainWidth;

      for (int height = 0; height < terrainHeight; height++) {
         allTiles[height] = new Terrain[terrainWidth];
         for (int width = 0; width < terrainWidth; width++) {
            int tokenPos = height * terrainWidth + width + 2;
            if (tokens[tokenPos] == "p") // planice
               allTiles[height][width] = new Terrain(plainsPrefab, width, height, 1);
            else if (tokens[tokenPos] == "f") // floresta
               allTiles[height][width] = new Terrain(forestPrefab, width, height, 2);
            else if (tokens[tokenPos] == "m") // montanha, impassavel
               allTiles[height][width] = new Terrain(mountainPrefab, width, height, -1);
            else if (tokens[tokenPos] == "r") // rio
               allTiles[height][width] = new Terrain(riverPrefab, width, height, 3);
            else if (tokens[tokenPos] == "t") // fortaleza
               allTiles[height][width] = new Terrain(fortressPrefab, width, height, 1);
            else Debug.Log(tokens[tokenPos]);
         }
      }
      for(int aux = terrainHeight * terrainWidth + 2; aux < tokens.Length; aux += 4) {
         if(tokens[aux] == "s") // soldado, unidade unica
            new Creature(soldierPrefab, int.Parse(tokens[aux + 3]), int.Parse(tokens[aux + 2]), 8, int.Parse(tokens[aux + 1]));
      }
   }

   void Start() {
      string terrainString = terrainFile.text;
      string[] terrainTokens;
      terrainString = terrainString.Replace(" ", string.Empty);
      terrainString = terrainString.Replace("\n", string.Empty);
      terrainString = terrainString.Replace("\r", string.Empty);

      terrainTokens = terrainString.Split(',');
      GenerateTerrain(terrainTokens);
   }
}