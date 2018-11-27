using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerHealth : MonoBehaviour {

    public int startingHealth = 100;
    // [HideInInspector]
    public int currentHealth;
    public Text healthStatusText;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
    // public AudioClip deathClip;

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    // Rigidbody rb;
    Collider c;
    // PlayerShooting playerShooting;
    bool isDead;
    bool damaged;

    [System.Serializable]
    public class HealthLevel {
        public string text;
        public Color color;

        public HealthLevel(string t, Color c) {
            text = t;
            color = c;
        }
    }

    HealthLevel[] healthLevel =  new HealthLevel[] {
        new HealthLevel("Super strong", Color.green), new HealthLevel("Strong", Color.green),
        new HealthLevel("Ok", Color.blue), new HealthLevel("Injured", Color.cyan),
        new HealthLevel("Weak", Color.yellow), new HealthLevel("Running on fumes", Color.yellow),
        new HealthLevel("Like a zombie", Color.red), new HealthLevel("Dying", Color.red)
    };

    void Awake() {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        // rb = GetComponent<Rigidbody>();
        c = GetComponent<CapsuleCollider>();
        // playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
        setHealthText();
    }

    void Update() {
        if(damaged) {
            damageImage.color = flashColor;
        } else {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed);
        }
        damaged = false;
    }

    public void TakeDamage(int amount) {
        damaged = true;
        currentHealth -= amount;
        setHealthText();
        // playerAudio.Play();
        if(currentHealth <= 0 && !isDead) {
            Death();
        }
    }

    void setHealthText() {
        int statusIndex = Mathf.Clamp((startingHealth - currentHealth) / healthLevel.Length, 0, healthLevel.Length-1);
        healthStatusText.text = healthLevel[statusIndex].text;
        healthStatusText.color = healthLevel[statusIndex].color;
    }

    void Death() {
        isDead = true;
        anim.SetTrigger("die");
        c.isTrigger = true;
        // rb.isKinematic = true;
        // rb.detectCollisions = false;
        // playerAudio.clip = deathClip;
        // playerAudio.Play();

        playerMovement.enabled = false;
        GameManager.instance.gameOver();
        // anim.enabled = false;
        // playerShooting.enabled = false;
    }
}
