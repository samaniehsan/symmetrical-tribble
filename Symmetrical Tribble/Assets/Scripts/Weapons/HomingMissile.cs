using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HomingMissile : MonoBehaviour {
    public Transform target;
    public float torque = 5f;
    public float speed = 10f;
    public float range;

    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        if (target != null) {
            // Vector3 direction = target.position - rb.position;
            // direction.Normalize();
            // Vector3 rotateAmount = Vector3.Cross(direction, transform.up);
            // rb.angularVelocity = -torque * rotateAmount;
            rb.velocity = transform.up * speed;

            // rb.velocity = new Vector3(0, 0, speed);
            //
            // float step = torque * Time.deltaTime;
            //
            // Vector3 direction = target.position - transform.position;
            // Quaternion targetRotation = Quaternion.LookRotation(direction);
            // Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
            //
            // rb.MoveRotation(rb.rotation * rotation);
        } else {
            // Debug.Log("No target");
            Destroy(gameObject, 2f);
        }
    }

    // void OnCollisionEnter(Collision collision) {
    //     GameObject targetObj = target.gameObject;
    //     if (collision.collider.gameObject == targetObj) {
    //         Debug.Log("Attacking enemy " + collision.gameObject.name + " for 10 damage");
    //         collision.gameObject.SendMessageUpwards("TakeDamage", 10, SendMessageOptions.DontRequireReceiver);
    //         Destroy(gameObject);
    //     } else (if collision.collider.tag == "Wall") {
    //
    //     }
    // }
}
