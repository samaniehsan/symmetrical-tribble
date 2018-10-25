using UnityEngine;

public class BoardGeneration : MonoBehaviour
{
    public enum BoardItem 
    {
        EmptySpace,
        Wall,
        YellowPickupCapsule,
        BombTrigger
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
    public BoardItemData[,] BoardItems = new BoardItemData[ARRAYSIZE, ARRAYSIZE];

    public GameObject Bomb;
    public GameObject Target;

    // Use this for initialization
    void Start()
    {
        System.Random r = new System.Random(System.DateTime.Now.Millisecond);

        // Initialize
        for (int i = 0; i < ARRAYSIZE; ++i)
        {
            for (int j = 0; j < ARRAYSIZE; ++j)
            {
                BoardItems[i, j] = new BoardItemData
                                       {
                                           Item = BoardItem.EmptySpace,
                                           WallConnections = 0
                                       };
            }
        }

        // Attach Player's ball to Target
        Target = GameObject.FindGameObjectWithTag("Player");

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

        // Mark Yellow PickUp Capsules
        for (int i = 0; i < ARRAYSIZE / 2; ++i)
        {
            int x = r.Next(ARRAYSIZE);
            int z = r.Next(ARRAYSIZE);
            if (BoardItems[x, z].Item != BoardItem.EmptySpace)
            {
                --i;
                continue;
            }
            BoardItems[x, z].Item = BoardItem.YellowPickupCapsule;
        }

        // Mark Bomb Trigger
        if(Bomb != null)
        {
            BoardItems[r.Next(ARRAYSIZE), 
                       r.Next(ARRAYSIZE)].Item = BoardItem.BombTrigger;
        }

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
                        // North-West
                        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = new Vector3(-j, 0f, i);
                        cube.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                        // South-West
                        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = new Vector3(-j, 0f, -i);
                        cube.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                        // South-East
                        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = new Vector3(j, 0f, -i);
                        cube.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                        break;
                    case BoardItem.BombTrigger:
                        break;
                    case BoardItem.YellowPickupCapsule:
                        GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                        capsule.transform.position = new Vector3(j, 0.5f, i);
                        capsule.transform.localScale = new Vector3(scaleX / 2, scaleY / 4, scaleZ / 2);
                        capsule.GetComponentInChildren<Renderer>().material.color = Color.blue;
                        capsule.AddComponent<BoxCollider>();
                        capsule.GetComponentInChildren<BoxCollider>().isTrigger = true;
                        capsule.tag = "Pick Up";
                        if(Target != null)
                        {
                            capsule.AddComponent<SeekBehavior>();
                            capsule.GetComponent<SeekBehavior>().target = Target;
                            capsule.GetComponent<SeekBehavior>().speed = 1.0f;
                            capsule.SetActive(true);
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
