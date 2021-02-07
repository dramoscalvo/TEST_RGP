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
    private Vector3 targetPosition;
    private Vector3 clickableTargetPosition;
    private bool isSelection;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        plane = new Plane(Vector3.up, 0);
    }

    private void Update() {
        SelectTarget();
        CancelSelectedTarget();
    }

    private void FixedUpdate() 
    {
        RotateCharacterToTarget();
        MoveCharacterNoSelection();  
    }

    /// <summary>
    /// Method to move the character on plane without rotation to follow the mouse
    /// </summary>
    private void MoveCharacterNoSelection() {
        
        float horizontal = Input.GetAxisRaw("Horizontal") ;
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 mousePositionOnWorld = Vector3.zero;
        
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
    private void RotateCharacterToTarget(){

        Vector3 mousePositionOnWorld = Vector3.zero;
        Vector3 targetPosition = Vector3.zero;
        Vector3 currentForwardDirection;
        float rotationSpeed;
        Quaternion rotation = Quaternion.identity;
        
        mousePositionOnWorld = GetMousePosition();

        if(!isSelection){
            targetPosition = mousePositionOnWorld - _rigidBody.position;
        } else{
            targetPosition = clickableTargetPosition - _rigidBody.position;   
        }

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
    /// Gets the position of the clicked target object
    /// </summary>
    void SelectTarget(){
        
        Ray rayCameraToPoint;
        bool isTargetHit;

        rayCameraToPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                isTargetHit = Physics.Raycast(rayCameraToPoint, out hit, 100);
                if (isTargetHit && hit.rigidbody.tag =="Target") {         
                    clickableTargetPosition = hit.rigidbody.position;
                    isSelection = true;
                }
                
        }
    }

    /// <summary>
    /// Gets the position of the clicked target object
    /// </summary>
    void CancelSelectedTarget(){
        
        if(Input.GetKeyDown(KeyCode.Q)){
            isSelection = false;
        }
    }



    /// <summary>
    /// DEPRECATED!! - Reflects a point across a line perpendicular to the lined formed by two points
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