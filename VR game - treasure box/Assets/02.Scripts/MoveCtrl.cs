using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCtrl : MonoBehaviour
{   
    public enum MoveType
    {
        WAY_POINT,
        LOOK_AT,
        DAYDREAM,
    }

    public MoveType moveType = MoveType.WAY_POINT; // 이동 방식 
    public float speed = 1.0f; // 이동 속도 
    public float damping = 3.0f; // 회전 속도 

    public Transform[] points; // 웨이포인트 저장 배열 

    private Transform tr; // transform 컴포넌트 저장 

    private CharacterController cc;
    
    private Transform camTr; // main camera 의 transform component

    private int nextIdx = 1; // 다음에 이동해야 할 위치 idx

    public static bool isStopped = false; // 컴포넌트화 하지 않아도 변수를 가져올 수 있음 

    

    void Start()
    {
        tr = GetComponent<Transform>();

        cc = GetComponent<CharacterController>();

        camTr = Camera.main.GetComponent<Transform>();

        GameObject wayPointGroup = GameObject.Find("WayPointGroup");

        if(wayPointGroup != null)
        {
            points = wayPointGroup.GetComponentsInChildren<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isStopped) return;

        switch(moveType)
        {
            case MoveType.WAY_POINT :
                MoveWayPoint();
                break;
            case MoveType.LOOK_AT :
                MoveLookAt(1);
                break;
            case MoveType.DAYDREAM :
                break;
        }
    }

    void MoveWayPoint()
    {
        Vector3 direction = points[nextIdx].position - tr.position;
        Quaternion rot = Quaternion.LookRotation(direction);
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime*damping);

        tr.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    void MoveLookAt(int facing)
    {
        Vector3 heading = camTr.forward;
        heading.y = 0.0f;

        Debug.DrawRay(tr.position, heading.normalized*1.0f, Color.red);

        cc.SimpleMove(heading*speed*facing);
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.CompareTag("WAY_POINT"))
        {
            nextIdx = (++nextIdx >= points.Length) ? 1 : nextIdx;
        }
    }
}
