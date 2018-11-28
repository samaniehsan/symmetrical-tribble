using UnityEngine;
using System.Linq;

public class PlayerThrowing : MonoBehaviour {
    Animator anim;
    HomingMissile missileScript;
    float radius;
    Vector3 center;

    void Awake() {
        anim = GetComponent<Animator>();
    }

    void Update() {
        if(Input.GetMouseButton(1)) {
            anim.SetTrigger("attacking");
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, radius);
    }

    void spawnMissiles(GameObject prefab) {
        GameObject missile;
        missile = Instantiate(prefab, transform.position + new Vector3(0f, 0.5f, 0.8f), transform.rotation, transform);
        missileScript = missile.GetComponent<HomingMissile>();
        missileScript.target = findTarget(transform.TransformDirection(Vector3.forward));
    }

    Transform findTarget(Vector3 direction) {
        Transform target = null;
        LayerMask enemiesMask = LayerMask.GetMask("enemies");
        radius = missileScript.range;
        center = transform.position + (direction * radius);

        // Find closest visible target in front of player
        Collider[] objectColliders = Physics.OverlapSphere(center, radius, enemiesMask);
        Debug.Log(objectColliders.Length + " enemies nearby");
        var nearest = objectColliders
          .OrderBy(t => (t.gameObject.transform.position - transform.position).sqrMagnitude)
          .FirstOrDefault();

        return nearest.gameObject.transform;
    }
}
