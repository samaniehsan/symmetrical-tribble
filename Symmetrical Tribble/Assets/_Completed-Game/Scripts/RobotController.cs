using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class RobotController : MonoBehaviour {
    Animator anim;
    // CharacterController controller;
    public Text countText;
    public Text healthStatusText;
    public float restartLevelDelay = 1f;
    // public float speed;
    public float rotateSpeed = 20.0f;

    // Vector3 smoothDeltaPosition = Vector3.zero;
    // Vector2 velocity = Vector2.zero;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private int count;
    private int healthLevel = 100;


    void Start() {
        anim = GetComponent<Animator>();
        // controller = GetComponent<CharacterController>();

        count = 0;

        // Run the SetCountText function to update the UI (see below)
        SetCountText ();
        SetHealthIndicator();
    }

    void lookAtMouse() {
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float hitdist = 0.0f;

        if (playerPlane.Raycast(ray, out hitdist)) {
            Vector3 targetPoint = ray.GetPoint(hitdist);

            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    void Update() {
      // float rotation = Input.GetAxis("Mouse X") * rotateSpeed;

      // rotation *= Time.deltaTime;
      if (Input.GetMouseButton(0)) {
        // transform.Rotate(0, rotation, 0);
        lookAtMouse();
      }

    }
    void OnTriggerEnter(Collider other)
	{
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag ("Pick Up"))
		{
            HandlePickup(other);
        }
        else if(other.gameObject.CompareTag("BombTrigger"))
        {
            HandleBombTriggerCollision(other);
        }
        else
        {
            HandleEnemyCollision(other);
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
        count = count + 1;

        // Run the 'SetCountText()' function (see below)
        SetCountText();
		checkGameOverCondition();

    }

    void HandleEnemyCollision(Collider enemy)
    {
        if (enemy.gameObject.CompareTag("Enemy Capsule"))
        {
            this.healthLevel -= 10;
            enemy.gameObject.SetActive(false);
        }
        else
        {
            if (enemy.gameObject.CompareTag("Bomb"))
                this.healthLevel = 0;
        }
        SetHealthIndicator();

		// Check if the stage is over
		checkGameOverCondition();

    }
    void SetHealthIndicator()
    {
        var healthLevel = GetHealthLevel();
        healthStatusText.text = GetHealthStatusText(healthLevel);
        healthStatusText.color = GetHealthColor(healthLevel);
    }

    enum HealthLevel
    {
        Dead,
        Zombie,
        Fumes,
        Weak,
        Injured,
        Ok,
        Strong,
        SuperStrong
    }

    HealthLevel GetHealthLevel()
    {
        if (this.healthLevel >= 100)
        {
            return HealthLevel.SuperStrong;
        }
        else if (this.healthLevel >= 80)
        {
            return HealthLevel.Strong;
        }
        else if (this.healthLevel >= 60)
        {
            return HealthLevel.Ok;
        }
        else if (this.healthLevel >= 40)
        {
            return HealthLevel.Injured;
        }
        else if (this.healthLevel >= 30)
        {
            return HealthLevel.Weak;
        }
        else if (this.healthLevel >= 20)
        {
            return HealthLevel.Fumes;
        }
        else if (this.healthLevel >= 10)
        {
            return HealthLevel.Zombie;
        }
        return HealthLevel.Dead;
    }

    Color GetHealthColor(HealthLevel level)
    {
        switch (level)
        {
            case HealthLevel.SuperStrong:
                return Color.green;
            case HealthLevel.Strong:
                return Color.blue;
            case HealthLevel.Ok:
                return Color.cyan;
            case HealthLevel.Injured:
                return Color.yellow;
            case HealthLevel.Weak:
                return Color.cyan;
            case HealthLevel.Fumes:
                return Color.red;
            case HealthLevel.Zombie:
                return Color.red;
        }
        return Color.red;
    }
    string GetHealthStatusText(HealthLevel level)
    {
        switch(level)
        {
            case HealthLevel.SuperStrong:
                return "SuperStrong";
            case HealthLevel.Strong:
                return "Strong";
            case HealthLevel.Ok:
                return "Ok";
            case HealthLevel.Injured:
                return "Injured";
            case HealthLevel.Weak:
                return "Weak";
            case HealthLevel.Fumes:
                return "Fumes";
            case HealthLevel.Zombie:
                return "Zombie";
        }
        return "Dead";
    }

	// Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
	void SetCountText()
	{
		// Update the text field of our 'countText' variable
		countText.text = "Count: " + count.ToString ();

		// Check if our 'count' is equal to or exceeded 12
		/*
		if (count >= 12)
		{
			// Set the text value of our 'winText'
			winText.text = "You Win!";
        } */
	}

	public void Restart () {
			SceneManager.LoadScene(0);
	}

	void checkGameOverCondition() {
			Debug.Log("Checking game over condition - health level is " + healthLevel);
			if (this.healthLevel <= 0) {
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
