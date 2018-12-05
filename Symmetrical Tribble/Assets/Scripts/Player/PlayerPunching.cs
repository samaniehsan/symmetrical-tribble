using UnityEngine;

public class PlayerPunching : MonoBehaviour {

    void OnCollisionEnter(Collision other) {
        Debug.Log("Player Punching collision detected");
        if (other.gameObject.CompareTag("Enemy")) {
            other.gameObject.SendMessageUpwards("TakeDamage", 10, SendMessageOptions.DontRequireReceiver);
        }
    }
}
