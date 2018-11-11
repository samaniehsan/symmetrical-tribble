using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    // private System.Random r = new System.Random(System.DateTime.Now.Millisecond);
    public GameObject[] boardItems;
    // public enum BoardItem
    // {
    //     EmptySpace,
    //     Wall,
    //     BlueEnemyCapsule,       // always attack target
    //     OrangeEnemyCapsule,     // attach target once within a certain radius
    //     GreenEnemyCapsule,      // attack target - with raycast avoidance
    //     YellowPickUp,
    //     BombTrigger             // activates the overhead bomb
    // }
    //
    // public enum WallConnection
    // {
    //     North = 0x01,
    //     South = 0x02,
    //     East  = 0x04,
    //     West  = 0x08
    // }
    //
    // public struct BoardItemData
    // {
    //     public BoardItem Item;
    //     public int WallConnections;
    // };
    //
    // const int ARRAYSIZE = 10;
    // const int BLUECAPSULES = 2;//5;
    // const int ORANGECAPSULES = 2;//5;
    // const int GREENCAPSULES = 5;

    // public BoardItemData[,] BoardItems = new BoardItemData[ARRAYSIZE, ARRAYSIZE];

    public GameObject Bomb;
    public int numberOfPickups;
    public GameObject bombTriggerPrefab;

    private GameObject _target;
    private int boardSize = 10; // Set this dynamically from ground prefab at some point
    private int boardBuffer = 2; // Buffer to leave around the edge
    private int maxSpawnPoints = 3;
    private int maxWalls = 7;

    // Use this for initialization
    public void setupScene(int level) {
        // Attach Player's ball to Target
        _target = GameObject.FindGameObjectWithTag("Player");

        placeItemsOnBoard2();
    }

    void placeItemsOnBoard2() {
      float scaleX = 1.0f;// * 0.9f;
      float scaleZ = 1.0f;// * 0.9f;
      float scaleY = 1.5f;

      int placedSpawnPoints = 0;
      int placedPickups = 0;
      int placedWalls = 0;

      Vector3 position = new Vector3(0, 0, 0);

      for (int i = boardBuffer - boardSize; i < boardSize - boardBuffer; ++i) {
      // for (int i = -boardSize; i < boardSize; i+=4) {
          for (int j = boardBuffer - boardSize; j < boardSize - boardBuffer; ++j) {
          // for (int j = -boardSize; j < boardSize; j+=4) {
              int r = Random.Range(0, boardItems.Length*5);
              Vector3 scale = new Vector3(scaleX, scaleY, scaleZ);
              if (r >= 0 && r < boardItems.Length) {
                  string itemTag = boardItems[r].tag;

                  if (itemTag == "Pick Up" && placedPickups < numberOfPickups) {
                      //position = new Vector3(j, 1f, i);
                      position = new Vector3(i, 1f, j);
                      scale = new Vector3(0.5f, 0.5f, 0.5f);
                      ++placedPickups;
                      // j+=2;
                  } else if (itemTag == "SpawnPoint" && placedSpawnPoints < maxSpawnPoints) {
                      // position = new Vector3(j, 0.5f, i);
                      position = new Vector3(i, 0.01f, j);
                      scale = new Vector3(1f, 0.1f, 1f);
                      ++placedSpawnPoints;
                  } else if (itemTag == "Wall" && placedWalls < maxWalls){
                      // position = new Vector3(j, 0, i);
                      position = new Vector3(i, 0.5f, j);
                      ++placedWalls;
                  } else {
                      continue;
                  }


                  GameObject item = Instantiate(boardItems[r],
                                                position,
                                                Quaternion.identity) as GameObject;
                  item.transform.localScale = scale;

                  // try {
                  //     item.GetComponent<BoardSeekRaycastBehavior>().target = _target;
                  //     if (item.GetComponent<BoardSeekRaycastBehavior>().speed == 0) {
                  //         item.GetComponent<BoardSeekRaycastBehavior>().speed = Random.Range(1, 3);
                  //         item.GetComponent<BoardSeekRaycastBehavior>().boundary = 5;
                  //     }
                  // } catch {
                  //     // Either there is no player, or there is no BoardSeekRaycastBehavior script
                  // }
              }
          }
      }
      Debug.Log("Placed " + placedSpawnPoints + " spawn points and " + placedPickups + " pickups into the scene.");
    }

    // // Initialize board
    // void initializeBoard() {
    //     // Initialize
    //     for (int i = 0; i < ARRAYSIZE; ++i) {
    //         for (int j = 0; j < ARRAYSIZE; ++j) {
    //             BoardItems[i, j] = new BoardItemData {
    //                                      Item = BoardItem.EmptySpace,
    //                                      WallConnections = 0
    //             };
    //         }
    //     }
    //
    //     // Mark Wall Structures
    //     for (int i = 0; i < ARRAYSIZE * 2 / 3; ++i)
    //     {
    //         // Exclude first and last places of each quadrant
    //         int x = System.Math.Max(r.Next(ARRAYSIZE - 1), 1);
    //         int z = System.Math.Max(r.Next(ARRAYSIZE - 1), 1);
    //         if(BoardItems[x,z].Item == BoardItem.Wall)
    //         {
    //             // if randomly placed in existing location, try again
    //             --i;
    //             continue;
    //         }
    //         BoardItems[x,z].Item = BoardItem.Wall;
    //         //TODO: Connect Walls that are close together?
    //     }
    //
    //     // Mark Blue Enemy Capsules
    //     for (int i = 0; i < BLUECAPSULES; ++i)
    //     {
    //         int x = r.Next(ARRAYSIZE);
    //         int z = r.Next(ARRAYSIZE);
    //         if (BoardItems[x, z].Item != BoardItem.EmptySpace)
    //         {
    //             --i;
    //             continue;
    //         }
    //         BoardItems[x, z].Item = BoardItem.BlueEnemyCapsule;
    //     }
    //
    //     // Mark Orange Enemy Capsules
    //     for (int i = 0; i < ORANGECAPSULES; ++i)
    //     {
    //         int x = r.Next(ARRAYSIZE);
    //         int z = r.Next(ARRAYSIZE);
    //         if (BoardItems[x, z].Item != BoardItem.EmptySpace)
    //         {
    //             --i;
    //             continue;
    //         }
    //         BoardItems[x, z].Item = BoardItem.OrangeEnemyCapsule;
    //     }
    //
    //     // Mark Green Enemy Capsules
    //     for (int i = 0; i < GREENCAPSULES; ++i)
    //     {
    //         int x = r.Next(ARRAYSIZE);
    //         int z = r.Next(ARRAYSIZE);
    //         if (BoardItems[x, z].Item != BoardItem.EmptySpace)
    //         {
    //             --i;
    //             continue;
    //         }
    //         BoardItems[x, z].Item = BoardItem.GreenEnemyCapsule;
    //     }
    //
    //     // Mark Yellow PickUps
    //     if (pickupPrefab != null)
    //     {
    //         for (int i = 0; i < numberOfPickups; ++i)
    //         {
    //             int x = r.Next(ARRAYSIZE);
    //             int z = r.Next(ARRAYSIZE);
    //             if (BoardItems[x, z].Item != BoardItem.EmptySpace)
    //             {
    //                 --i;
    //                 continue;
    //             }
    //             BoardItems[x, z].Item = BoardItem.YellowPickUp;
    //         }
    //     }
    //
    //     // Mark Bomb Trigger
    //     for (int i = 0; i < 1; ++i)
    //     {
    //         int x = r.Next(ARRAYSIZE);
    //         int z = r.Next(ARRAYSIZE);
    //         if (BoardItems[x, z].Item != BoardItem.EmptySpace)
    //         {
    //             --i;
    //             continue;
    //         }
    //         BoardItems[x, z].Item = BoardItem.BombTrigger;
    //     }
    // }
    //
    // void placeItemsOnBoard() {
    //   // 10.0f is the size of one Quadrant
    //   float scaleX = 10.0f / ARRAYSIZE;// * 0.9f;
    //   float scaleZ = 10.0f / ARRAYSIZE;// * 0.9f;
    //   float scaleY = 1.5f;
    //
    //   // Place Items on Map (mirror one quadrant's items about axes to cover board)
    //   for (int i = 0; i < ARRAYSIZE; ++i)
    //   {
    //       for (int j = 0; j < ARRAYSIZE; ++j)
    //       {
    //         switch(BoardItems[i,j].Item)
    //         {
    //             case BoardItem.Wall:
    //                 // North-East Quadrant
    //                 GameObject cube = Instantiate(mazeWallPrefab,
    //                                               new Vector3(j, 0f, i),
    //                                               Quaternion.identity) as GameObject;
    //                 cube.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    //                 // North-West
    //                 cube = Instantiate(mazeWallPrefab,
    //                                    new Vector3(-j, 0f, i),
    //                                    Quaternion.identity) as GameObject;
    //                 cube.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    //                 // South-West
    //                 cube = Instantiate(mazeWallPrefab,
    //                                    new Vector3(-j, 0f, -i),
    //                                    Quaternion.identity) as GameObject;
    //                 cube.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    //                 // South-East
    //                 cube = Instantiate(mazeWallPrefab,
    //                                    new Vector3(j, 0f, -i),
    //                                    Quaternion.identity) as GameObject;
    //                 cube.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    //                 break;
    //             case BoardItem.BombTrigger:
    //                 GameObject capsuleTrigger = Instantiate(bombTriggerPrefab,
    //                                                         new Vector3(j, 0.5f, i),
    //                                                         Quaternion.identity) as GameObject;
    //                 capsuleTrigger.transform.localScale = new Vector3(scaleX / 2, scaleY / 4, scaleZ / 2);
    //                 //Texture2D bombTex = Resources.Load("NuclearBomb_Spec") as Texture2D;
    //                 //capsuleTrigger.GetComponentInChildren<Renderer>().material.mainTexture = bombTex;
    //                 break;
    //             case BoardItem.YellowPickUp:
    //                 //Instantiate(YellowPickUp, new Vector3(j, 0.5f, i), Quaternion.identity);
    //                 GameObject yp = Instantiate(pickupPrefab) as GameObject;
    //                 yp.transform.position = new Vector3(j, 0.5f, i);
    //                 break;
    //             case BoardItem.BlueEnemyCapsule:
    //                 GameObject capsuleBlue = Instantiate(enemyCapsulePrefab) as GameObject;
    //                 capsuleBlue.GetComponent<Renderer>().material.color = Color.blue;
    //                 capsuleBlue.transform.position = new Vector3(j, 0.5f, i);
    //                 capsuleBlue.transform.localScale = new Vector3(scaleX / 2, scaleY / 4, scaleZ / 2);
    //                 if(_target != null)
    //                 {
    //                     capsuleBlue.GetComponent<BoardSeekRaycastBehavior>().target = _target;
    //                     capsuleBlue.GetComponent<BoardSeekRaycastBehavior>().speed = 1.5f;
    //                     capsuleBlue.GetComponent<BoardSeekRaycastBehavior>().boundaryReached = true;
    //                 }
    //                 break;
    //             case BoardItem.OrangeEnemyCapsule:
    //                 GameObject capsuleOrange = GameObject.Instantiate(enemyCapsulePrefab) as GameObject;
    //                 capsuleOrange.transform.position = new Vector3(j, 0.5f, i);
    //                 capsuleOrange.transform.localScale = new Vector3(scaleX / 2, scaleY / 4, scaleZ / 2);
    //                 capsuleOrange.GetComponentInChildren<Renderer>().material.color = new Color(1.0f,0.5f,0f);
    //                 if (_target != null)
    //                 {
    //                     capsuleOrange.GetComponent<BoardSeekRaycastBehavior>().target = _target;
    //                     capsuleOrange.GetComponent<BoardSeekRaycastBehavior>().speed = r.Next(1,3);
    //                     capsuleOrange.GetComponent<BoardSeekRaycastBehavior>().boundary = 5.0f;
    //                     capsuleOrange.GetComponent<BoardSeekRaycastBehavior>().boundaryReached = false;
    //                 }
    //
    //                 break;
    //             case BoardItem.GreenEnemyCapsule:
    //                 GameObject capsuleGreen = GameObject.Instantiate(enemyCapsulePrefab) as GameObject;
    //                 capsuleGreen.transform.position = new Vector3(j, 0.5f, i);
    //                 capsuleGreen.transform.localScale = new Vector3(scaleX / 2, scaleY / 4, scaleZ / 2);
    //                 capsuleGreen.GetComponentInChildren<Renderer>().material.color = Color.green;
    //                 if (_target != null)
    //                 {
    //                     capsuleGreen.GetComponent<BoardSeekRaycastBehavior>().target = _target;
    //                     capsuleGreen.GetComponent<BoardSeekRaycastBehavior>().speed = 1.0f;
    //                     capsuleGreen.GetComponent<BoardSeekRaycastBehavior>().boundaryReached = true;
    //                 }
    //                 break;
    //             default:
    //                 break;
    //         }
    //       }
    //   }
    // }

    // Update is called once per frame
    void Update()
    {

    }
}
