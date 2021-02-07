using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControler : MonoBehaviour
{
    private const string DOOR_TRIGGER = "DoorTriggered";

    private Animator _animator;
    public Transform player;
    private bool canPlayerInteract;
    // Start is called before the first frame update
    private void Start() {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.transform == player){
            canPlayerInteract = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.transform == player){
            canPlayerInteract = false;
        }
    }

    private void Update() {
        if(canPlayerInteract){
            if(Input.GetKeyDown("space")){
                _animator.SetTrigger(DOOR_TRIGGER);
            }
        }
    }
}
