using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uween;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sensitivity;
    public float jumpHeight = 2.0f;
    public Transform cam;
    public Transform groundCheck;
    public LayerMask groundMask;
    public LayerMask interactMask;
    public Transform pickupIcon;

    CharacterController characterController;
    bool grounded = true;
    const float gravity = -9.81f;
    const float groundDistance = 0.1f;
    const float interactDistance = 300f;

    float rotationX;

    Vector3 velocity;

    Transform self;
    GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        self = transform;
        playerObject = gameObject;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGrounded();
        UpdateMovement();
        UpdateLook();
        UpdatePickup();
        //UpdateJump();
    }

    void UpdatePickup()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.position, cam.forward);
        Debug.DrawRay(cam.position, cam.forward, Color.red);

        if (Physics.Raycast(ray, out hit, interactDistance, interactMask))
        {
            pickupIcon.gameObject.SetActive(true);
            if (Input.GetButton("Interact"))
            {
                var hitGameobject = hit.transform.gameObject;
                Destroy(hitGameobject);
            }
        }
        else
        {
            pickupIcon.gameObject.SetActive(false);
        }
    }

    void UpdateGrounded()
    {
        if (!grounded)
        {
            grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
        

    }

    void UpdateMovement()
    {
        Vector3 rightDir = transform.TransformDirection(Vector3.right);
        Vector3 forwardsDir = transform.TransformDirection(Vector3.forward);

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveBy = rightDir * x + forwardsDir * z;

        characterController.Move(moveBy.normalized * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    void UpdateLook()
    {
        float x = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotationX -= y;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        cam.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(0f, x, 0f);

    }

    void UpdateJump()
    {
        if (Input.GetAxisRaw("Jump") > 0 && grounded)
        {
            grounded = false;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
