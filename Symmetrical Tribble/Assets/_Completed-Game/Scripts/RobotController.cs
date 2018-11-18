using UnityEngine;
using System.Collections;

public class RobotController : MonoBehaviour {
    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");
        anim.SetFloat("Direction", h);
        float v = Input.GetAxis("Vertical");
        anim.SetFloat("Speed", v);
    }

}
