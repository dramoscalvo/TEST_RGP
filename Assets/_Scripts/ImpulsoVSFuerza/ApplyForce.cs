using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour
{
    public float constForce;
    private Rigidbody _rigidBody;
    private void Start() 
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate() {
        if(Input.GetKey("space"))
        {
            _rigidBody.AddForce(Vector3.right * constForce, ForceMode.Force);
        }
    }
}
