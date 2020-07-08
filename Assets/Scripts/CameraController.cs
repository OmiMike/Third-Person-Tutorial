using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [Header("Camera")]
    [SerializeField] bool isGrounded;
    [SerializeField] bool groundChecked;
    float groundCheckDeadzone = 0.7f;
    Vector2 newRotation;
    Vector2 rotation;
    Vector3 dampVelocity;

    public Transform targetOverride;
    public Transform camOverride;

    public Vector2 rotSpeed = new Vector2(2.5f, 3.5f);
    public Vector2 pitchClamp = new Vector2(70f, -11.5f);

    public float turnSpeed = 15f;
    public float dampTime = 0.07f;
    public float maxCamDistance = 3f;
    public float minCamDistance = 1f;

    public float camCollisionDistance = 2f;
    public float camCollisionDampRate = 10f;
    public float camCollisionReturnDampTime = 0.16f;

    public LayerMask notPlayerMask;

    public float currentCamDistance;
    public float collisionDampVelocity;


    void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        float yawCamera = camOverride.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);


        newRotation.y += Input.GetAxis("Mouse X") * rotSpeed.y;
        newRotation.x -= Input.GetAxis("Mouse Y") * rotSpeed.x;
        newRotation.x = Mathf.Clamp(newRotation.x, pitchClamp.y, pitchClamp.x);
        rotation = Vector3.SmoothDamp(rotation, newRotation, ref dampVelocity, dampTime);
        camOverride.eulerAngles = rotation;

        Vector3 head = camOverride.position - targetOverride.position;
        RaycastHit hit;
        if (Physics.Raycast(targetOverride.position, (camOverride.position - targetOverride.position).normalized, out hit, maxCamDistance, notPlayerMask))
        {
            SetCamDistanceToRay(hit);
        }
        else if (Physics.Raycast(camOverride.position, camOverride.up, out hit, 2f))
        {
            SetCamDistanceToRay(hit);
        }
        else
            currentCamDistance = Mathf.SmoothDamp(currentCamDistance, maxCamDistance, ref collisionDampVelocity, camCollisionReturnDampTime);

        camOverride.position = (targetOverride.position - camOverride.forward * currentCamDistance);
    }

    public void SetCamDistanceToRay(RaycastHit hit)
    {
        currentCamDistance = Mathf.Lerp(currentCamDistance, Vector3.Distance(hit.point, targetOverride.position), camCollisionDampRate * Time.deltaTime);
        collisionDampVelocity = 0;
    }
}
