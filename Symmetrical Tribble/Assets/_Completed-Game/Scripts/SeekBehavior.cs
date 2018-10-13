using UnityEngine;
using System.Collections;

public class SeekBehavior : MonoBehaviour
{
    public GameObject target;   // target is set in the IDE
    public float speed;         // so is speed value

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null 
            || !gameObject.activeSelf) 
        {
            return;
        }

        // this behavior is closer to the "arrive" behavior in the book,
        // and still needs to add in acceleration of gravity, as it slows down when close to target
        Vector3 direction = Vector3.Normalize(target.transform.position - transform.position);
        transform.position += direction * speed * Time.deltaTime;
        // if bomb has reached the ground, remove it from view
        if(transform.position.y <= 1.0)
        {
            gameObject.SetActive(false);
        }
    }
}
