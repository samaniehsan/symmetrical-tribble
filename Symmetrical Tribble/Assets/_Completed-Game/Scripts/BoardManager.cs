using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private System.Random r = new System.Random(System.DateTime.Now.Millisecond);
    public enum BoardItem
    {
        EmptySpace,
        Wall,
        BlueEnemyCapsule,       // always attack target
        OrangeEnemyCapsule,     // attach target once within a certain radius
        GreenEnemyCapsule,      // attack target - with raycast avoidance
        YellowPickUp,
        BombTrigger             // activates the overhead bomb
    }

    public enum WallConnection
    {
        North = 0x01,
        South = 0x02,
        East  = 0x04,
        West  = 0x08
    }

    public struct BoardItemData
    {
        public BoardItem Item;
        public int WallConnections;
    };

    const int ARRAYSIZE = 10;
    const int BLUECAPSULES = 2;//5;
    const int ORANGECAPSULES = 2;//5;
    const int GREENCAPSULES = 5;

    public BoardItemData[,] BoardItems = new BoardItemData[ARRAYSIZE, ARRAYSIZE];

    public GameObject Bomb;
    public int numberOfPickups;
    private GameObject _target;
    public GameObject pickupPrefab;

    // Use this for initialization
    public void setupScene(int level) {
        initializeBoard();

        // Attach Player's ball to Target
        _target = GameObject.FindGameObjectWithTag("Player");

        // Mark Wall Structures
        for (int i = 0; i < ARRAYSIZE * 2 / 3; ++i)
        {
            // Exclude first and last places of each quadrant
            int x = System.Math.Max(r.Next(ARRAYSIZE - 1), 1);
            int z = System.Math.Max(r.Next(ARRAYSIZE - 1), 1);
            if(BoardItems[x,z].Item == BoardItem.Wall)
            {
                // if randomly placed in existing location, try again
                --i;
                continue;
            }
            BoardItems[x,z].Item = BoardItem.Wall;
            //TODO: Connect Walls that are close together?
        }

        // Mark Blue Enemy Capsules
        for (int i = 0; i < BLUECAPSULES; ++i)
        {
            int x = r.Next(ARRAYSIZE);
            int z = r.Next(ARRAYSIZE);
            if (BoardItems[x, z].Item != BoardItem.EmptySpace)
            {
                --i;
                continue;
            }
            BoardItems[x, z].Item = BoardItem.BlueEnemyCapsule;
        }

        // Mark Orange Enemy Capsules
        for (int i = 0; i < ORANGECAPSULES; ++i)
        {
            int x = r.Next(ARRAYSIZE);
            int z = r.Next(ARRAYSIZE);
            if (BoardItems[x, z].Item != BoardItem.EmptySpace)
            {
                --i;
                continue;
            }
            BoardItems[x, z].Item = BoardItem.OrangeEnemyCapsule;
        }

        // Mark Green Enemy Capsules
        for (int i = 0; i < GREENCAPSULES; ++i)
        {
            int x = r.Next(ARRAYSIZE);
            int z = r.Next(ARRAYSIZE);
            if (BoardItems[x, z].Item != BoardItem.EmptySpace)
            {
                --i;
                continue;
            }
            BoardItems[x, z].Item = BoardItem.GreenEnemyCapsule;
        }

        // Mark Yellow PickUps
        if (pickupPrefab != null)
        {
            for (int i = 0; i < numberOfPickups; ++i)
            {
                int x = r.Next(ARRAYSIZE);
                int z = r.Next(ARRAYSIZE);
                if (BoardItems[x, z].Item != BoardItem.EmptySpace)
                {
                    --i;
                    continue;
                }
                BoardItems[x, z].Item = BoardItem.YellowPickUp;
            }
        }

        // Mark Bomb Trigger
        for (int i = 0; i < 1; ++i)
        {
            int x = r.Next(ARRAYSIZE);
            int z = r.Next(ARRAYSIZE);
            if (BoardItems[x, z].Item != BoardItem.EmptySpace)
            {
                --i;
                continue;
            }
            BoardItems[x, z].Item = BoardItem.BombTrigger;
        }

        placeItemsOnBoard();
    }

    // Initialize board
    void initializeBoard() {
        // Initialize
        for (int i = 0; i < ARRAYSIZE; ++i) {
            for (int j = 0; j < ARRAYSIZE; ++j) {
                BoardItems[i, j] = new BoardItemData {
                                         Item = BoardItem.EmptySpace,
                                         WallConnections = 0
                };
            }
        }
    }

    void placeItemsOnBoard() {
      // 10.0f is the size of one Quadrant
      float scaleX = 10.0f / ARRAYSIZE;// * 0.9f;
      float scaleZ = 10.0f / ARRAYSIZE;// * 0.9f;
      float scaleY = 1.5f;

      // Place Items on Map (mirror one quadrant's items about axes to cover board)
      for (int i = 0; i < ARRAYSIZE; ++i)
      {
          for (int j = 0; j < ARRAYSIZE; ++j)
          {
            switch(BoardItems[i,j].Item)
            {
                case BoardItem.Wall:
                    // North-East Quadrant
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(j, 0f, i);
                    cube.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                    cube.tag = "Wall";
                    // North-West
                    cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(-j, 0f, i);
                    cube.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                    cube.tag = "Wall";
                    // South-West
                    cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(-j, 0f, -i);
                    cube.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                    cube.tag = "Wall";
                    // South-East
                    cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(j, 0f, -i);
                    cube.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                    cube.tag = "Wall";
                    break;
                case BoardItem.BombTrigger:
                    GameObject capsuleTrigger = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    capsuleTrigger.transform.position = new Vector3(j, 0.5f, i);
                    capsuleTrigger.transform.localScale = new Vector3(scaleX / 2, scaleY / 4, scaleZ / 2);
                    capsuleTrigger.GetComponentInChildren<Renderer>().material.color = Color.red;
                    //Texture2D bombTex = Resources.Load("NuclearBomb_Spec") as Texture2D;
                    //capsuleTrigger.GetComponentInChildren<Renderer>().material.mainTexture = bombTex;
                    capsuleTrigger.AddComponent<BoxCollider>();
                    capsuleTrigger.GetComponentInChildren<BoxCollider>().isTrigger = true;
                    capsuleTrigger.tag = "BombTrigger";
                    capsuleTrigger.SetActive(true);
                    break;
                case BoardItem.YellowPickUp:
                    //Instantiate(YellowPickUp, new Vector3(j, 0.5f, i), Quaternion.identity);
                    GameObject yp = Instantiate(pickupPrefab) as GameObject;
                    yp.transform.position = new Vector3(j, 0.5f, i);
                    break;
                case BoardItem.BlueEnemyCapsule:
                    GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    capsule.transform.position = new Vector3(j, 0.5f, i);
                    capsule.transform.localScale = new Vector3(scaleX / 2, scaleY / 4, scaleZ / 2);
                    capsule.GetComponentInChildren<Renderer>().material.color = Color.blue;
                    capsule.AddComponent<BoxCollider>();
                    capsule.GetComponentInChildren<BoxCollider>().isTrigger = true;
                    capsule.tag = "Enemy Capsule";
                    if(_target != null)
                    {
                        capsule.AddComponent<BoardSeekRaycastBehavior>();
                        capsule.GetComponent<BoardSeekRaycastBehavior>().target = _target;
                        capsule.GetComponent<BoardSeekRaycastBehavior>().speed = 1.5f;
                        capsule.GetComponent<BoardSeekRaycastBehavior>().boundaryReached = true;
                        capsule.SetActive(true);
                    }
                    break;
                case BoardItem.OrangeEnemyCapsule:
                    GameObject capsuleOrange = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    capsuleOrange.transform.position = new Vector3(j, 0.5f, i);
                    capsuleOrange.transform.localScale = new Vector3(scaleX / 2, scaleY / 4, scaleZ / 2);
                    capsuleOrange.GetComponentInChildren<Renderer>().material.color = new Color(1.0f,0.5f,0f);
                    capsuleOrange.AddComponent<BoxCollider>();
                    capsuleOrange.GetComponentInChildren<BoxCollider>().isTrigger = true;
                    capsuleOrange.tag = "Enemy Capsule";
                    if (_target != null)
                    {
                        capsuleOrange.AddComponent<BoardSeekRaycastBehavior>();
                        capsuleOrange.GetComponent<BoardSeekRaycastBehavior>().target = _target;
                        capsuleOrange.GetComponent<BoardSeekRaycastBehavior>().speed = r.Next(1,3);
                        capsuleOrange.GetComponent<BoardSeekRaycastBehavior>().boundary = 5.0f;
                        capsuleOrange.GetComponent<BoardSeekRaycastBehavior>().boundaryReached = false;
                        capsuleOrange.SetActive(true);
                    }

                    break;
                case BoardItem.GreenEnemyCapsule:
                    GameObject capsuleGreen = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    capsuleGreen.transform.position = new Vector3(j, 0.5f, i);
                    capsuleGreen.transform.localScale = new Vector3(scaleX / 2, scaleY / 4, scaleZ / 2);
                    capsuleGreen.GetComponentInChildren<Renderer>().material.color = Color.green;
                    capsuleGreen.AddComponent<BoxCollider>();
                    capsuleGreen.GetComponentInChildren<BoxCollider>().isTrigger = true;
                    capsuleGreen.tag = "Enemy Capsule";
                    if (_target != null)
                    {
                        capsuleGreen.AddComponent<BoardSeekRaycastBehavior>();
                        capsuleGreen.GetComponent<BoardSeekRaycastBehavior>().target = _target;
                        capsuleGreen.GetComponent<BoardSeekRaycastBehavior>().speed = 1.0f;
                        capsuleGreen.GetComponent<BoardSeekRaycastBehavior>().boundaryReached = true;
                        capsuleGreen.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
          }
      }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
