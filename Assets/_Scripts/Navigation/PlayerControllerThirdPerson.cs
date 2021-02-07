using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerThirdPerson : MonoBehaviour
{
    private const string IS_MOVING = "isMoving";

    private Animator _animator;
    private Rigidbody _rigidBody;


    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private float movementSpeed;
    private Vector3 movement;
    private Vector3 product;
    private Quaternion rotation = Quaternion.identity;
    private Vector3 worldPosition = Vector3.zero;
    public float m_DistanceZ;
    private static Vector3 cameraPosition;
    private Plane plane;
    private Vector3 desiredPosition;
    

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        plane = new Plane(Vector3.up, 0);
    }

    private void Update() 
    {
        CharacterRotation();
        CharacterMovement();
        
    }
        

/// <summary>
/// Method to move the character on plane without rotation
/// </summary>
    private void CharacterMovement() {
        float horizontal = Input.GetAxisRaw("Horizontal") ;
        float vertical = Input.GetAxisRaw("Vertical");
        float characterYAngle = transform.eulerAngles.y;
        
        movement.Set(vertical * Mathf.Sin(characterYAngle), 0, vertical * Mathf.Cos(characterYAngle));
        movement.Normalize();
        
        _animator.SetFloat("Horizontal", horizontal);
        _animator.SetFloat("Vertical", vertical);

        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }

        if(vertical < 0){
            worldPosition = 2*transform.position - worldPosition;
        }

        // if(Mathf.Approximately(vertical, 0)){
        //     worldPosition = transform.position;
        // }else{
        //     worldPosition *= vertical;
        // }

        // worldPosition *= vertical;

        // transform.position += movement * movementSpeed * Time.fixedDeltaTime;
        


        if(Mathf.Approximately(vertical, 0)){
            desiredPosition = transform.position;
        }else{
            desiredPosition = Vector3.MoveTowards(transform.position, worldPosition, movementSpeed * Time.deltaTime);
        }
        
        
        Debug.Log("Desired: " + desiredPosition);
        Debug.Log("Transform: " + transform.position);
        Debug.Log("World: " + worldPosition);
        Debug.Log("Diff: " + (worldPosition - transform.position));
        // Debug.Log("Movement: " + movement);
           transform.position = desiredPosition;
        // _rigidBody.MovePosition(desiredPosition);
        // lastPosition = _rigidBody.position;
    }

/// <summary>
/// Method to rotate character to constantly look at mouse pointer
/// </summary>
    private void CharacterRotation(){

        // Plane plane = new Plane(Vector3.up, Vector3.zero);

        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, worldPosition - transform.position, turnSpeed * Time.deltaTime, 0);
        rotation = Quaternion.LookRotation(desiredForward);
        transform.rotation = rotation;
        // _rigidBody.MoveRotation(rotation);
        
    }
}