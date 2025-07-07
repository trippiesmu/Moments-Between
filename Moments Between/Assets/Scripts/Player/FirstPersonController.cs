// FirstPersonController.cs
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Bewegungseinstellungen")]
    public float walkSpeed = 2f;
    public float sprintSpeed = 4f;
    public float mouseSensitivity = 100f;

    [Header("Footstep Settings")]
    [Tooltip("Fußschritte aktivieren/deaktivieren")]
    public bool enableFootsteps = false;
    [Tooltip("AudioSource für Fußschritte")]
    public AudioSource footstepAudioSource;
    [Tooltip("Fußschritt-Audio-Clips")]
    public AudioClip[] footstepClips;
    [Tooltip("Intervall (Sekunden) zwischen zwei Schritten beim Gehen")]
    public float footstepInterval = 0.5f;

    [Header("ViewBobbing-Einstellungen")]
    public bool enableViewBobbing = true;
    public float bobbingSpeed = 5f;
    public float bobbingAmount = 0.05f;

    [Header("Referenzen")]
    public Transform playerCamera;
    public float verticalRotationLimit = 80f;

    private CharacterController characterController;
    private float xRotation = 0f;
    private float defaultCameraY;
    private float bobTimer = 0f;
    private float footstepTimer = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (playerCamera == null)
            playerCamera = Camera.main.transform;
        defaultCameraY = playerCamera.localPosition.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleViewBobbing();
        HandleFootsteps();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation - mouseY, -verticalRotationLimit, verticalRotationLimit);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        // Sperrung während eines aktiven Dialogs
        if (DialogueManager.Instance != null && DialogueManager.Instance.DialogueActive)
            return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.SimpleMove(move * speed);
    }



    void HandleViewBobbing()
    {
        if (!enableViewBobbing)
        {
            Vector3 p = playerCamera.localPosition;
            p.y = defaultCameraY;
            playerCamera.localPosition = p;
            return;
        }

        bool isMoving = characterController.velocity.magnitude > 0.1f;
        if (isMoving)
        {
            bobTimer += Time.deltaTime * bobbingSpeed;
            float offset = Mathf.Sin(bobTimer) * bobbingAmount;
            Vector3 p = playerCamera.localPosition;
            p.y = defaultCameraY + offset;
            playerCamera.localPosition = p;
        }
        else
        {
            bobTimer = 0f;
            Vector3 p = playerCamera.localPosition;
            p.y = defaultCameraY;
            playerCamera.localPosition = p;
        }
    }

    void HandleFootsteps()
    {
        if (!enableFootsteps || footstepAudioSource == null || footstepClips.Length == 0)
            return;

        // Nur auf dem Boden und bei Bewegung
        if (!characterController.isGrounded ||
            characterController.velocity.magnitude < 0.1f)
        {
            footstepTimer = 0f;
            return;
        }

        footstepTimer += Time.deltaTime;
        if (footstepTimer >= footstepInterval)
        {
            footstepTimer = 0f;
            // Zufälligen Clip auswählen und abspielen
            var clip = footstepClips[Random.Range(0, footstepClips.Length)];
            footstepAudioSource.PlayOneShot(clip);
        }
    }
}
