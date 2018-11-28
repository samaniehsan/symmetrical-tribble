using UnityEngine;

public class PlayerThrowing : MonoBehaviour {
    Animator anim;
    HomingMissile missileScript;

    void Awake() {
        anim = GetComponent<Animator>();
        missileScript = GetComponent<HomingMissile>();
    }

    void Update() {
        if(Input.GetMouseButton(1)) {
            anim.SetTrigger("attacking");
        // } else {
        //     anim.SetBool("attacking", false);
        }
    }

    void spawnMissiles(GameObject prefab) {
        GameObject missile;
        missile = Instantiate(prefab, transform.position + new Vector3(0f, 0.5f, 0.8f), transform.rotation, transform);
        missileScript.target = findTarget(transform.TransformDirection(Vector3.forward));
    }

    Transform findTarget(Vector3 direction) {
        Transform target = null;

        // Tech debt: Place the physics sphere so that the robot is at the edge not the center
        Collider[] objectColliders = Physics.OverlapSphere(transform.position, missileScript.missileRange);
        for(int index = 0; index <= objectColliders.Length - 1; index++) {
            GameObject colliderObject = objectColliders[index].gameObject;
            if(colliderObject.tag == "Enemy") {
                RaycastHit hit;
                bool hitOccurred = Physics.Raycast(transform.position, colliderObject.transform.position.normalized, out hit);
                if(hitOccurred && hit.gameObject.tag == "Enemy") {
                    target = colliderObject.transform;
                }
            }
        }
        return transform;
    }
}
