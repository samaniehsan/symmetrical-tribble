using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSeekRaycastBehavior : MonoBehaviour
{
    private const bool DEBUGMODE = true;
    private const int FRAMERATE = 1;//3;
    private const float SPEED = 0.7f;
    private const float RAYLEN = 0.5f;

    private GameObject _target;
    private int _framerate;
    private bool _blockedByWall;

    // Start is called before the first frame update
    void Start()
    {
        _framerate = 0;
        _blockedByWall = false;
        if(_target == null)
        {
            _target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(++_framerate % FRAMERATE != 0)
        {
            return;
        }
        float step = SPEED * Time.deltaTime;

        // rotation
        Vector3 targetDir = _target.transform.position - this.transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 2.0f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
        Vector3 rightDir = Quaternion.AngleAxis(30f, transform.up) * transform.forward;
        Vector3 leftDir = Quaternion.AngleAxis(-30f, transform.up) * transform.forward;
        if(DEBUGMODE)
        {
            Debug.DrawRay(this.transform.position, newDir, Color.yellow, step);
            Debug.DrawRay(this.transform.position, rightDir, Color.blue, step);
            Debug.DrawRay(this.transform.position, leftDir, Color.cyan, step);
        }
        // position
        Vector3 nextStep = Vector3.MoveTowards(this.transform.position,
                                               _target.transform.position,
                                               step);
        var ray = new Ray(this.transform.position, newDir);
        if (Physics.Raycast(ray, out RaycastHit hit, SPEED))
        {
            if (DEBUGMODE)
            {
                Debug.DrawRay(this.transform.position, newDir, Color.green);
            }
            if (hit.collider.tag.Equals("Wall"))
            {
                _blockedByWall = true;
            }
            else
            {
                _blockedByWall = false;
            }
        }
        else
        {
            _blockedByWall = false;
        }

        if(!_blockedByWall)
        {
            this.transform.position = nextStep;
        }
    }
}
