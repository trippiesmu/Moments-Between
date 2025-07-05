// FirstPersonController.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Bewegungseinstellungen")]
    public float walkSpeed = 2f;
    public float sprintSpeed = 4f;
    public float mouseSensitivity = 100f;

    [Header("ViewBobbing-Einstellungen (optional)")]
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

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (playerCamera == null) playerCamera = Camera.main.transform;
        defaultCameraY = playerCamera.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleViewBobbing();
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
        // Wenn Dialog aktiv → keine Bewegung
        if (DialogueTrigger.dialogueActive) return;

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
            var p = playerCamera.localPosition;
            p.y = defaultCameraY;
            playerCamera.localPosition = p;
            return;
        }

        bool moving = characterController.velocity.magnitude > 0.1f;
        if (moving)
        {
            bobTimer += Time.deltaTime * bobbingSpeed;
            float offset = Mathf.Sin(bobTimer) * bobbingAmount;
            var p = playerCamera.localPosition;
            p.y = defaultCameraY + offset;
            playerCamera.localPosition = p;
        }
        else
        {
            bobTimer = 0f;
            var p = playerCamera.localPosition;
            p.y = defaultCameraY;
            playerCamera.localPosition = p;
        }
    }
}
