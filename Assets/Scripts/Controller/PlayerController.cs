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
    public LayerMask touchableMask;
    public Transform pickupIcon;

    CharacterController characterController;
    bool grounded = true;
    const float gravity = -9.81f;
    const float groundDistance = 0.1f;
    const float interactDistance = 3f;
    const float touchDistance = 1f;

    float rotationX;

    Vector3 velocity;

    Transform self;
    GameObject playerObject;

    List<Note> collectedNotes = new List<Note>();
    Note currentNote;

    List<Guid> wallsPassed = new List<Guid>();

    private enum State
    {
        Normal,
        BurningNotes
    }

    State state;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Normal;
        self = transform;
        playerObject = gameObject;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Normal)
        {
            UpdateGrounded();
            UpdateMovement();
            UpdateLook();
            UpdatePickup();
            UpdateTouch();
        }
        else if (state == State.BurningNotes)
        {
            if (Input.GetButtonDown("Interact"))
            {
                NoteInteraction();
            }
        }
        
    }

    void NoteInteraction()
    {
        if (collectedNotes.Count == 0 && currentNote == null)
        {
            state = State.Normal;
            return;
        }

        if (currentNote == null)
        {
            Debug.Log("step1");
            currentNote = collectedNotes[0];
            collectedNotes.Remove(currentNote);
            NoteController.instance.DisplayNote(currentNote);
        }
        else
        {
            Debug.Log("step2");
            NoteController.instance.BurnNote();
            currentNote = null;
            MessageController.instance.DisplayMessage("You feel yourself become warmer...");
            DirectorController.instance.frozenPercent = 0;
        }
    }

    void UpdatePickup()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.position, cam.forward);

        if (Physics.Raycast(ray, out hit, interactDistance, interactMask))
        {
            pickupIcon.gameObject.SetActive(true);
            if (Input.GetButton("Interact"))
            {
                var hitGameobject = hit.transform.gameObject;
                if (hitGameobject.tag == "Note")
                {
                    Note note = hitGameobject.GetComponent<NoteEntity>().Note;
                    if (!collectedNotes.Contains(note))
                    {
                        collectedNotes.Add(note);
                        MessageController.instance.DisplayMessage("You picked up a note...");
                        Destroy(hitGameobject);
                    }
                    
                }
                if (hitGameobject.tag == "Fireplace" && collectedNotes.Count > 0)
                {
                    state = State.BurningNotes;
                    NoteInteraction();
                    pickupIcon.gameObject.SetActive(false);
                }
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
        Vector3 rightDir = self.TransformDirection(Vector3.right);
        Vector3 forwardsDir = self.TransformDirection(Vector3.forward);

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

    void UpdateTouch()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.position, cam.forward);

        if (Physics.SphereCast(ray, 1f, out hit, touchDistance, touchableMask))
        {


            var hitTransform = hit.transform;
            var hitObject = hitTransform.gameObject;
            Debug.Log("hit");

            switch (hitObject.tag)
            {
                case "LiminalWall":
                    var liminalEntity = hitObject.GetComponent<LiminalWallEntity>();
                    if (liminalEntity != null && !wallsPassed.Contains(liminalEntity.guid) && DirectorController.instance.eyesOpenPercent < 0.1f)
                    {
                        characterController.enabled = false;
                        wallsPassed.Add(liminalEntity.guid);
                        Vector3 target = liminalEntity.teleportTarget + hitTransform.position;
                        transform.position = target;
                        characterController.enabled = true;
                    }
                    break;
                case "Teleporter":
                    var teleporter = hitObject.GetComponent<TeleportEntity>();
                    if (teleporter != null)
                    {
                        if (teleporter.CheckDirection(self.position))
                        {
                            //characterController.enabled = false;
                            Vector3 difference = hitTransform.position - self.position;
                            Vector3 target = teleporter.TeleportPosition - difference;
                            transform.position = target;
                            //characterController.enabled = true;
                        }

                    }
                    break;
                default:
                    break;
            }

        }
    }

}
