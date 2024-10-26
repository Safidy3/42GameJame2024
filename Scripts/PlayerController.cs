using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    [Header("Look Settings")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;

    [Header("References")]
    public Transform cameraTransform;

    private GameObject currentKeyPickup = null;
    private GameObject currentCardPickup = null;
    // public GameObject door = null;
    // public GameObject prefabOldLight = null;
    // public GameObject prefabNewLight = null;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private float verticalRotation = 0f;
	private bool gotKey = false;
    public Camera FPSCamera;
    public Camera BFPSCamera;

    private void ShowBFPSView()
    {
        FPSCamera.enabled = false;
        BFPSCamera.enabled = true;
    }

    private void ShowFPSView()
    {
        FPSCamera.enabled = true;
        BFPSCamera.enabled = false;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
		// prefabNewLight.SetActive(false);
        if (controller == null)
        {
            Debug.LogError("CharacterController component missing!");
            enabled = false;
            return;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform reference missing!");
            enabled = false;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ShowFPSView();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
		HandlePickupInput();
        HandleCamera();
    }

    private void HandleMovement()
    {
        float moveSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        move = Vector3.ClampMagnitude(move, 1f);
        controller.Move(move * moveSpeed * Time.deltaTime);

    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandlePickupInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentKeyPickup != null)
            {
                Debug.Log("Key collected!");
                currentKeyPickup.SetActive(false);
                currentKeyPickup = null;
				gotKey = true;
            }
            if (currentCardPickup != null && gotKey)
            {
                Debug.Log("Card collected!");
                currentCardPickup.SetActive(false);
                currentCardPickup = null;
				// door.transform.Rotate(new Vector3(0,0,120));
				// prefabOldLight.SetActive(false);
				// prefabNewLight.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUpKey"))
        {
            Debug.Log("Near key - Press E to collect");
            currentKeyPickup = other.gameObject;
        }
        else if (other.CompareTag("PickUpCard"))
        {
            Debug.Log("Near card - Press E to collect");
            currentCardPickup = other.gameObject;
        }
        else if (other.CompareTag("EnemyTag"))
        {
            Debug.Log("GAME OVEEEEEEEER");
            SceneManager.LoadSceneAsync("GameOver");
        }
        else if (other.CompareTag("desk1"))
            Debug.Log("desk 1");
        else if (other.CompareTag("desk2"))
            Debug.Log("desk 2");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickUpKey") && other.gameObject == currentKeyPickup)
            currentKeyPickup = null;
        else if (other.CompareTag("PickUpCard") && other.gameObject == currentCardPickup)
            currentCardPickup = null;
    }

    private void HandleCamera()
    {
        if (Input.GetKeyDown(KeyCode.C))
            ShowBFPSView();
        else if (Input.GetKeyUp(KeyCode.C))
            ShowFPSView();
    }
}
