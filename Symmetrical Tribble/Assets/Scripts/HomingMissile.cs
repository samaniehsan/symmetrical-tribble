using UnityEngine;

public class HomingMissile : MonoBehaviour {
    public Transform target;
    public float angleChangingSpeed;
    public float movementSpeed;
    public float missileRange;

    Rigidbody rb;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        if (target != null) {
            Vector3 direction = target.position - rb.position;
            direction.Normalize();
            Vector3 rotateAmount = Vector3.Cross(direction, transform.up);
            rb.angularVelocity = -angleChangingSpeed * rotateAmount;
            rb.velocity = transform.up * movementSpeed;
        } else {
            // no target assigned, so random movement for a few seconds
            // then play explode animation
            // and destroy the object
        }
    }
}
