using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
    private PlayerHealth health;


    void Awake() {
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
        GameManager.instance.checkWinCondition();

    }

    void HandleEnemyCollision(Collider enemy)
    {
        if (enemy.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if(enemyHealth != null) {
                enemyHealth.TakeDamage(40);
            }
            health.TakeDamage(10);
        }
        else
        {
            if (enemy.gameObject.CompareTag("Bomb"))
                health.currentHealth = 0;
        }

    }
}
