using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Please use the BoardSeekRaycastBehavior, as it will not collide with walls
public class BoardSeekBehavior : MonoBehaviour
{
    public GameObject target;
    public float speed;

    public float boundary;
    public bool boundaryReached;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            return;
        }

        if(!boundaryReached)
        {
            float distance = Vector3.Distance(this.transform.position,
                                              target.transform.position);
            if(distance < boundary)
            {
                boundaryReached = true;
            }
        }

        if(!boundaryReached)
        {
            return;
        }

        float step = speed * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(this.transform.position,
                                                      target.transform.position,
                                                      step);
    }
}
