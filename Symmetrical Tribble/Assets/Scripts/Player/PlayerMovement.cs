using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    Animator anim;
    public float rotateSpeed = 20.0f;
    private Rigidbody rb;

    void Awake() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
      if (Input.GetMouseButton(0)) {
        lookAtMouse();
      }
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



}
