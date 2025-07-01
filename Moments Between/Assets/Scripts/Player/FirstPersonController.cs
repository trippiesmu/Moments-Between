// FirstPersonController.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Bewegungseinstellungen")]
    public float walkSpeed = 2f;
    public float sprintSpeed = 4f;
    public float mouseSensitivity = 100f;

    [Header("ViewBobbing-Einstellungen")]
    public bool enableViewBobbing = true;
    public float bobbingSpeed = 5f;
    public float bobbingAmount = 0.05f;

    [Header("Weitere Einstellungen")]
    public Transform playerCamera;
    public float verticalRotationLimit = 80f;

    private CharacterController characterController;
    private float xRotation = 0f;
    private float defaultCameraLocalPosY;
    private float bobbingTimer = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (!playerCamera)
            playerCamera = Camera.main.transform;

        defaultCameraLocalPosY = playerCamera.localPosition.y;
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
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalRotationLimit, verticalRotationLimit);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.SimpleMove(move * currentSpeed);
    }

    void HandleViewBobbing()
    {
        if (!enableViewBobbing)
        {
            Vector3 camPos = playerCamera.localPosition;
            camPos.y = defaultCameraLocalPosY;
            playerCamera.localPosition = camPos;
            return;
        }

        bool isMoving = characterController.velocity.magnitude > 0.1f;

        if (isMoving)
        {
            bobbingTimer += Time.deltaTime * bobbingSpeed;
            float bobOffset = Mathf.Sin(bobbingTimer) * bobbingAmount;

            Vector3 camPos = playerCamera.localPosition;
            camPos.y = defaultCameraLocalPosY + bobOffset;
            playerCamera.localPosition = camPos;
        }
        else
        {
            bobbingTimer = 0f;
            Vector3 camPos = playerCamera.localPosition;
            camPos.y = defaultCameraLocalPosY;
            playerCamera.localPosition = camPos;
        }
    }
}