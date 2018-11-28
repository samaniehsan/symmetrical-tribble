using UnityEngine;

public class PlayerThrowing : MonoBehaviour {
    Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
    }

    void Update() {
        if(Input.GetMouseButton(1)) {
            anim.SetTrigger("attacking");
        // } else {
        //     anim.SetBool("attacking", false);
        }
    }
}
