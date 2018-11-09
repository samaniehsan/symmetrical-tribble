using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;

public class PlayerController : MonoBehaviour {

	// Create public variables for player speed, and for the Text UI game objects
	public float speed;
	public Text countText;
    public Text healthStatusText;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private int count;
    private int healthLevel = 100;

    // At the start of the game..
    void Start ()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();

		// Set the count to zero
		count = 0;

		// Run the SetCountText function to update the UI (see below)
		SetCountText ();

	}

	// Each physics step..
	void FixedUpdate ()
	{
		// Set some local float variables equal to the value of our Horizontal and Vertical Inputs
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		// Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		// Add a physical force to our Player rigidbody using our 'movement' Vector3 above,
		// multiplying it by 'speed' - our public player speed that appears in the inspector
		rb.AddForce (movement * speed);
	}

	// When this game object intersects a collider with 'is trigger' checked,
	// store a reference to that collider in a variable named 'other'..
	void OnTriggerEnter(Collider other)
	{
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag ("Pick Up"))
		{
            HandleGoldenBallPickup(other);
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

    void HandleGoldenBallPickup(Collider other)
    {
        // Make the other game object (the pick up) inactive, to make it disappear
        other.gameObject.SetActive(false);

        // Add one to the score variable 'count'
        count = count + 1;
				healthLevel += 10;

        // Run the 'SetCountText()' function (see below)
        SetCountText();

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

	void checkGameOverCondition() {
			Debug.Log("Checking game over condition - health level is " + healthLevel);
			if (this.healthLevel <= 0) {
					Debug.Log("Calling GameManager.gameOver()");
					GameManager.instance.gameOver();
			}
			else
				GameManager.instance.winCheck(count);
	}
}
