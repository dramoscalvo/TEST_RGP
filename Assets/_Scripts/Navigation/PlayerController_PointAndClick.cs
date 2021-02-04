using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController_PointAndClick : MonoBehaviour
{
    private const string IS_MOVING = "isMoving";

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    
    private Ray rayCameraToPoint;
    private bool isFloorHit;


    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();   
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        CharacterMovement();
    }
    void CharacterMovement (){

        rayCameraToPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButton(0)) {
                RaycastHit hit;
                isFloorHit = Physics.Raycast(rayCameraToPoint, out hit, 100);
                if (isFloorHit) {         
                    _navMeshAgent.destination = hit.point;
                    _animator.SetBool("isMoving", true);
                }
        }

        if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance){
            _animator.SetBool("isMoving", false);
        } else {
            _animator.SetBool("isMoving", true);
        }
    }
}
