using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    public float restartLevelDelay = 1f;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	  private int count;
    private PlayerHealth health;


    void Awake() {
        count = 0;

        health = GetComponent<PlayerHealth>();
        // Run the SetCountText function to update the UI (see below)
    }

    void Update() {}

    void OnTriggerEnter(Collider other) {
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
    switch(other.gameObject.tag) {
        case "Pick Up":
            HandlePickup(other);
            break;
        case "BombTrigger":
            HandleBombTriggerCollision(other);
            break;
        default:
            HandleEnemyCollision(other);
            break;
    }
	}

    private void HandleBombTriggerCollision(Collider collider)
    {
        GameManager.instance.triggerBomb();
    }

    void HandlePickup(Collider other)
    {
        // Make the other game object (the pick up) inactive, to make it disappear
        other.gameObject.SetActive(false);

        // Add one to the score variable 'count'
        ScoreManager.score += 1;

        // Run the 'SetCountText()' function (see below)
        // SetCountText();
        checkGameOverCondition();

    }

    void HandleEnemyCollision(Collider enemy)
    {
        if (enemy.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if(enemyHealth != null) {
                enemyHealth.TakeDamage(20);
            }
            health.TakeDamage(10);
            // enemy.gameObject.SetActive(false);
        }
        else
        {
            if (enemy.gameObject.CompareTag("Bomb"))
                health.currentHealth = 0;
        }

    }

	public void Restart () {
			SceneManager.LoadScene(0);
	}

	void checkGameOverCondition() {
			Debug.Log("Checking game over condition");/* - health level is " + healthLevel);*/
			if (health.currentHealth <= 0) {
					Debug.Log("Calling GameManager.gameOver()");
					GameManager.instance.gameOver();
			} else {
					Debug.Log("Checking for win condition");
					if (count >= GameManager.instance.getNumPickups()) {
							Invoke("Restart", restartLevelDelay);
							enabled = false;
					}
			}

	}

}
