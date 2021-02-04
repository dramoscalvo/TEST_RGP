using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyImpulse : MonoBehaviour
{
    public float impulseForce;
    private Rigidbody _rigidBody;
    private void Start() 
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate() {
        if(Input.GetKey("space"))
        {
            _rigidBody.AddForce(Vector3.right * impulseForce, ForceMode.Impulse);
        }
    }
}
