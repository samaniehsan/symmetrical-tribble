using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // public PlayerHealth playerHealth;       // Reference to the player's heatlh.
    [Range(2, 10)]
    public float spawnTime = 3f;            // How long between each spawn.
    public GameObject[] enemies;            // An array of the enemy prefabs to be spawned.
    private GameObject[] spawnPoints;         // An array of the spawn points this enemy can spawn from.


    void Start ()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Debug.Log(spawnPoints);
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }


    void Spawn ()
    {
        try {
            // Select enemy from list of enemy prefabs
            GameObject enemy = enemies[Random.Range (0, enemies.Length)];

            // Select spawn point from list of spawn points
            int spawnPointIndex = Random.Range (0, spawnPoints.Length);

            if (! enemy.GetComponent<BoardSeekRaycastBehavior>().boundaryReached) {
                enemy.GetComponent<BoardSeekRaycastBehavior>().speed = Random.Range(1, 4);
                enemy.GetComponent<BoardSeekRaycastBehavior>().boundary = Random.Range(3, 6);
                enemy.GetComponent<BoardSeekRaycastBehavior>().target = GameManager.instance.getTarget();
            }

            Vector3 spawnPosition = spawnPoints[spawnPointIndex].transform.position + new Vector3(0, 0.5f, 0);
            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            GameObject spawned = Instantiate (enemy, spawnPosition, spawnPoints[spawnPointIndex].transform.rotation);
            spawned.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        } catch {}
    }
}
