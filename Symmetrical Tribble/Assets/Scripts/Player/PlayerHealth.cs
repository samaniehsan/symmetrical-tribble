using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    public Text healthStatusText;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
    // public AudioClip deathClip;

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    // PlayerShooting playerShooting;
    bool isDead;
    bool damaged;

    string[] healthLevel = new string[] {"Super strong", "Strong", "Ok", "Injured",
                            "Weak", "Running on fumes", "Like a zombie", "Dead"};

    void Awake() {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        // playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
    }

    void Update() {
        // if(damaged) {
        //     damageImage.color = flashColor;
        // } else {
        //     damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed);
        // }
        // damaged = false;
    }

    public void TakeDamage(int amount) {
        damaged = true;
        currentHealth -= amount;
        healthStatusText.text = healthLevel[(startingHealth - currentHealth) / healthLevel.Length];
        // healthStatusText.color =

        playerAudio.Play();
        if(currentHealth <= 0 && !isDead) {
            Death();
        }
    }

    void Death() {
        isDead = true;
        // anim.SetTrigger("Die");

        // playerAudio.clip = deathClip;
        // playerAudio.Play();

        // playerMovement.enabled = false;
        // playerShooting.enabled = false;
    }

    //
    // Color GetHealthColor(HealthLevel level)
    // {
    //     switch (level)
    //     {
    //         case HealthLevel.SuperStrong:
    //             return Color.green;
    //         case HealthLevel.Strong:
    //             return Color.blue;
    //         case HealthLevel.Ok:
    //             return Color.cyan;
    //         case HealthLevel.Injured:
    //             return Color.yellow;
    //         case HealthLevel.Weak:
    //             return Color.cyan;
    //         case HealthLevel.Fumes:
    //             return Color.red;
    //         case HealthLevel.Zombie:
    //             return Color.red;
    //     }
    //     return Color.red;
    // }
    // string GetHealthStatusText(int healthLevel)
    // {
    //     HealthLevel selection =
    //     switch(level)
    //     {
    //         case HealthLevel.SuperStrong:
    //             return "SuperStrong";
    //         case HealthLevel.Strong:
    //             return "Strong";
    //         case HealthLevel.Ok:
    //             return "Ok";
    //         case HealthLevel.Injured:
    //             return "Injured";
    //         case HealthLevel.Weak:
    //             return "Weak";
    //         case HealthLevel.Fumes:
    //             return "Fumes";
    //         case HealthLevel.Zombie:
    //             return "Zombie";
    //     }
    //     return "Dead";
    // }
}
