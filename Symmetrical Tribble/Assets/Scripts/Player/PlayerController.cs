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

        GameManager.instance.checkWinCondition();

    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = c.gameObject.GetComponent<EnemyHealth>();
            if(enemyHealth != null) {
                enemyHealth.TakeDamage(40);
            }
            health.TakeDamage(10);
        }
        else
        {
            if (c.gameObject.CompareTag("Bomb"))
                health.TakeDamage(health.currentHealth);
        }

    }
}
