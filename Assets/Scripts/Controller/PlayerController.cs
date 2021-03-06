using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uween;
using UnityEngine.UI;

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
    public Transform handIconObject;
    public Sprite pickupSprite;
    public Sprite pushSprite;
    public Transform enemyTransform;
    public Transform finalDoorRoot;

    private Image handIcon;

    CharacterController characterController;
    bool grounded = true;
    const float gravity = -9.81f;
    const float groundDistance = 0.1f;
    const float interactDistance = 3f;
    const float touchDistance = 1f;
    const float footstepFrequency = .6f;
    float footstepTimer = 0;

    float rotationX;

    Vector3 velocity;

    public static Transform self;
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
        handIcon = handIconObject.GetComponent<Image>();
        Cursor.lockState = CursorLockMode.Locked;

        MessageController.instance.EnqueueMessage("WASD to move, mouse to look");
        MessageController.instance.EnqueueMessage("Hold space to close your eyes");
        MessageController.instance.EnqueueMessage("You are constantly freezing");
        MessageController.instance.EnqueueMessage("Find notes to burn at the fireplace");
        
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

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "SaferoomTrigger":
                AudioController.instance.ChangeMusic(AudioController.MusicTrack.Saferoom);
                break;
            case "AmbienceTrigger":
                AudioController.instance.ChangeMusic(AudioController.MusicTrack.Ambience);
                break;
            case "ChaseTrigger":
                AudioController.instance.ChangeMusic(AudioController.MusicTrack.Chase);
                break;
            case "StingerTrigger":
                AudioController.instance.PlayStinger();
                Destroy(other.gameObject);
                break;
            default:
                break;
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
            currentNote = collectedNotes[0];
            collectedNotes.Remove(currentNote);
            NoteController.instance.DisplayNote(currentNote);
            MessageController.instance.DisplayMessage("Click to burn...");
            AudioController.instance.PlayNotePickup();
        }
        else
        {
            CheckNoteForFlags(currentNote);
            NoteController.instance.BurnNote();
            currentNote = null;
            MessageController.instance.DisplayMessage("You feel yourself become warmer...");
            DirectorController.instance.frozenPercent = 0;
            AudioController.instance.PlayBurnNote();
            if (collectedNotes.Count == 0)
            {
                state = State.Normal;
            }
        }
    }

    void CheckNoteForFlags(Note note)
    {
        switch (note.Title)
        {
            case "The Walls Themselves":
                DirectorController.instance.CompleteFlag(DirectorController.Flags.FirstNoteTaken);
                break;
            case "Letting Go":
                DirectorController.instance.CompleteFlag(DirectorController.Flags.Win);
                break;
            default:
                break;
        }
    }

    void UpdatePickup()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.position, cam.forward);

        if (Physics.Raycast(ray, out hit, interactDistance, interactMask))
        {
            

            var hitGameobject = hit.transform.gameObject;
            if (hitGameobject.tag == "Note")
            {
                handIconObject.gameObject.SetActive(true);
                handIcon.sprite = pickupSprite;
                if (Input.GetButton("Interact"))
                {
                    var noteEntity = hitGameobject.GetComponent<NoteEntity>();
                    Note note = noteEntity.Note;
                    if (noteEntity.pickupFlag != DirectorController.Flags.None)
                    {
                        DirectorController.instance.CompleteFlag(noteEntity.pickupFlag);
                    }
                    if (!collectedNotes.Contains(note))
                    {
                        AudioController.instance.PlayNotePickup();
                        collectedNotes.Add(note);
                        MessageController.instance.DisplayMessage("You picked up a note...");
                        Destroy(hitGameobject);
                    }
                }
            }
            if (hitGameobject.tag == "Fireplace" && collectedNotes.Count > 0)
            {
                handIconObject.gameObject.SetActive(true);
                handIcon.sprite = pickupSprite;
                if (Input.GetButton("Interact"))
                {
                    state = State.BurningNotes;
                    NoteInteraction();
                    handIconObject.gameObject.SetActive(false);
                }
            }
            if (hitGameobject.tag == "Door")
            {
                
                DoorController doorController = hitGameobject.GetComponentInParent<DoorController>();
                bool flagCompleted = doorController.requiredFlag == DirectorController.Flags.None || DirectorController.instance.IsFlagCompleted(doorController.requiredFlag);
                if (!doorController.isOpen)
                {
                    handIconObject.gameObject.SetActive(true);
                    handIcon.sprite = pushSprite;
                    if (Input.GetButton("Interact"))
                    {
                        if (flagCompleted)
                        {
                            AudioController.instance.PlayDoorOpen();
                            doorController.isOpen = true;
                            doorController.OpenDoor(transform.position);
                        }
                        else
                        {
                            MessageController.instance.DisplayMessage("It won't budge");
                        }
                    }
                }
            }
            if (hitGameobject.tag == "WinWall")
            {
                handIconObject.gameObject.SetActive(true);
                handIcon.sprite = pushSprite;
                if (Input.GetButton("Interact"))
                {
                    DirectorController.instance.Win();
                }
            }
            
        }
        else
        {
            handIconObject.gameObject.SetActive(false);
        }
    }

    void UpdateGrounded()
    {
        RaycastHit hit;
        grounded = Physics.SphereCast(groundCheck.position, groundDistance, Vector3.down, out hit, groundMask);
        if (grounded) velocity.y = 0;

        if (grounded && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer > footstepFrequency)
            {
                footstepTimer = 0;
                switch (hit.collider.gameObject.tag)
                {
                    case "Carpet":
                        AudioController.instance.PlayCarpetFootstep();
                        break;
                    case "Invisible":
                        break;
                    default:
                        AudioController.instance.PlayHardFootstep();
                        break;
                }
            }
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
                        bool flagCompleted = teleporter.requiredFlag == DirectorController.Flags.None || DirectorController.instance.IsFlagCompleted(teleporter.requiredFlag);

                        if (teleporter.CheckDirection(self.position) && flagCompleted)
                        {
                            //characterController.enabled = false;
                            Vector3 difference = hitTransform.position - self.position;
                            Vector3 target = teleporter.TeleportPosition - difference;
                            transform.position = target;
                            //characterController.enabled = true;
                        }

                    }
                    break;
                case "Trigger":
                    switch (hitObject.name)
                    {
                        case "ChaseTrigger":
                            enemyTransform.gameObject.SetActive(true);
                            break;
                        case "ChaseEndTrigger":
                            enemyTransform.gameObject.SetActive(false);
                            finalDoorRoot.localRotation = Quaternion.Euler(Vector3.zero);
                            break;
                        case "KillTrigger":
                            DirectorController.instance.GameOver();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

        }
    }

}
