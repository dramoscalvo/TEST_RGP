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
    private float speed;
    private Plane plane;
    
    

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        plane = new Plane(Vector3.up, 0);
    }

    private void FixedUpdate() 
    {
        RotateCharacter();
        MoveCharacter();
        
    }

    /// <summary>
    /// Method to move the character on plane without rotation
    /// </summary>
    private void MoveCharacter() {
        
        float horizontal = Input.GetAxisRaw("Horizontal") ;
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 mousePositionOnWorld = Vector3.zero;
        Vector3 targetPosition;
        Vector3 currentPosition;
        float movementSpeed;
        Vector3 keyboardInput = Vector3.zero;
        Vector3 keyboardInputInLocals;
        float characterRotation;
        Vector3 desiredPosition;
        
        keyboardInput.Set(horizontal, 0, vertical);
         
        _animator.SetFloat("Horizontal", horizontal);
        _animator.SetFloat("Vertical", vertical);
        currentPosition = _rigidBody.position;
        movementSpeed = speed * Time.fixedDeltaTime;
        characterRotation = _rigidBody.transform.eulerAngles.y;
        keyboardInputInLocals = RotateVector(keyboardInput, characterRotation, Vector3.up);
        
        targetPosition = currentPosition + keyboardInputInLocals;
        

        if(keyboardInputInLocals == Vector3.zero){
            desiredPosition = _rigidBody.position;
        }else{
            desiredPosition = Vector3.MoveTowards(currentPosition, targetPosition, movementSpeed);
        }
        
        _rigidBody.MovePosition(desiredPosition);
    }

    /// <summary>
    /// Method to rotate character to constantly look at mouse pointer
    /// </summary>
    private void RotateCharacter(){

        Vector3 mousePositionOnWorld = Vector3.zero;
        Vector3 targetPosition;
        Vector3 currentForwardDirection;
        float rotationSpeed;
        Quaternion rotation = Quaternion.identity;
        
        mousePositionOnWorld = GetMousePosition();
        targetPosition = mousePositionOnWorld - _rigidBody.position;
        currentForwardDirection = _rigidBody.transform.forward;
        rotationSpeed = turnSpeed * Time.fixedDeltaTime;

        Vector3 desiredForward = Vector3.RotateTowards(currentForwardDirection, targetPosition, rotationSpeed, 0);
        rotation = Quaternion.LookRotation(desiredForward);

        _rigidBody.MoveRotation(rotation);
        
    }

    /// <summary>
    /// Locates mouse cursor on World coordinates
    /// </summary>
    /// <returns>Coordinates of the mouse in world reference</returns>
    private Vector3 GetMousePosition(){

        Vector3 mousePositionOnWorld = Vector3.zero;
        float distance;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            mousePositionOnWorld = ray.GetPoint(distance);
        }

        return mousePositionOnWorld;
    }

    /// <summary>
    /// Rotates a given vector
    /// </summary>
    /// <param name="vector">Vector to rotate</param>
    /// <param name="angle">Angle of rotation</param>
    /// <param name="rotationAxis">Axis around to which rotate</param>
    /// <returns>Rotated vector</returns>
    private Vector3 RotateVector (Vector3 vector, float angle, Vector3 rotationAxis){

        vector = Quaternion.AngleAxis(angle, rotationAxis) * vector;
        return vector;


    }
    
    /// <summary>
    /// DEPRECATED Reflects a point across a line perpendicular to the lined formed by two points
    /// </summary>
    /// <param name="reflectPivot">Intersection point between reflection line and the perpendicular</param>
    /// <param name="pointToReflect">Point to be reflected</param>
    /// <returns>Reflected point</returns>
    private Vector3 RefecltPointAcrossLine (Vector3 reflectPivot, Vector3 pointToReflect){
        
        Vector3 mirroredPoint;

        mirroredPoint = 2*reflectPivot - pointToReflect;

        return mirroredPoint;
    }
}