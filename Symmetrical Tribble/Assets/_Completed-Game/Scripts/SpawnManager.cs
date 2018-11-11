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
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }


    void Spawn ()
    {
        // Select enemy from list of enemy prefabs
        GameObject enemy = enemies[Random.Range (0, enemies.Length)];

        // Find a random index between zero and one less than the number of spawn points.
        // Select spawn point from list of spawn points
        int spawnPointIndex = Random.Range (0, spawnPoints.Length);

        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate (enemy, spawnPoints[spawnPointIndex].transform.position, spawnPoints[spawnPointIndex].transform.rotation);
    }
}
