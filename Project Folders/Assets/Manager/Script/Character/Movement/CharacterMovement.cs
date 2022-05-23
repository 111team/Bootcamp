using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] float speedValue = 5f;
    [Range(1f, 5f)]
    [SerializeField] float runMultiplier = 2f;
    [SerializeField] float smoothRun = 2f;
    [Range(3f, 20f)]
    [SerializeField] float smoothRotation = 1f;
    [Space(10)]
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float jumpForce = 100f;


    [Header("Character Sense")]
    [SerializeField] Transform wallCheckTarget;
    [SerializeField] float wallCheckLength = 1f;
    [Space(10)]
    [Range(0.1f, 1f)]
    [SerializeField] float groundSphereRadius = 0.3f;
    [SerializeField] LayerMask groundMask;


    //Kompanentler
    PlayerInput playerInput; //Player Input türünde referans
    CharacterController _characterController;
    Animator _animator;

    //Character Movement
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 movementDirection;
    Vector3 upVelocity;

    //Karakter hareket deðerleri
    float run;
    float speed;


    //Karakter hareketlerini kontrol eden deðiþkenler
    bool isMovementPressed;
    bool isWalking;
    bool isSprint;
    bool isJumping;
    bool isGrounded;
    bool isWallCheck;


    private void Awake()
    {
        playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        //Move Input ayarlarýný deðiþkene atama
        playerInput.Player.Move.started += OnMovementInput;
        playerInput.Player.Move.performed += OnMovementInput;
        playerInput.Player.Move.canceled += OnMovementInput;

        playerInput.Player.Run.started += OnSprint;
        playerInput.Player.Run.canceled += OnSprint;

        playerInput.Player.Jump.started += Jump;
        playerInput.Player.Jump.canceled += Jump;

        run = 1f;
    }

    private void Start()
    {
        //Oyun baþladýðýnda fare kilitlenir.
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Move girdi ayarlama fonksiyonu
    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement = new Vector3(currentMovementInput.x, 0f, currentMovementInput.y);
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;


        switch (context.phase)
        {
            case InputActionPhase.Started:
                isWalking = true;
                speed = speedValue;
                break;

            case InputActionPhase.Performed:
                break;

            case InputActionPhase.Canceled:
                isWalking = false;
                speed = 0f;
                break;

            default:
                break;
        }
    }

    //Sprint girdi ayarlama fonksiyonu
    void OnSprint(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            isSprint = !isSprint;
        }
    }

    //Zýplama
    void Jump(InputAction.CallbackContext context)
    {
        isJumping = context.started ? true : false;
    }

    //Zýplama özelliklerinin ayarlandýðý fonksiyon.
    void SetJumpProperties()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundSphereRadius, groundMask);

        if (isGrounded && isJumping && !isWallCheck)
        {
            upVelocity.y = jumpForce;
        }
    }

    //Karakteri koþma durumuna geçiren fonksiyon.
    void SetSprint()
    {
        if (!isSprint)
        {
            float currentSpeed = _characterController.velocity.magnitude;
            run = Mathf.Lerp(currentSpeed, 1f, smoothRun);
        }
        else if (isSprint)
        {
            float currentSpeed = _characterController.velocity.magnitude;
            run = Mathf.Lerp(currentSpeed, runMultiplier, smoothRun);
        }
    }

    //Karakterin animator deðiþkenlerini ayarlayan fonksiyon.
    void SetAnimator()
    {
        _animator.SetBool("walk", isWalking);
        _animator.SetBool("run", isSprint);
        _animator.SetBool("isMovement", isMovementPressed);
        _animator.SetBool("isInAir", isGrounded);
    }

    //Karakterin hareket ederken rotasyonunu ayarlayan fonksiyon.
    void SetRotation()
    {
        Quaternion currentRotation = transform.rotation;
        float targetAngle = Mathf.Atan2(currentMovement.x, currentMovement.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        Quaternion targetDirection = Quaternion.Euler(0f, targetAngle, 0f);

        if (isMovementPressed)
        {
            transform.rotation = Quaternion.Slerp(currentRotation, targetDirection, smoothRotation * Time.deltaTime);
            movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
    }

    //Yer çekimi deðiþkenlerini kontrol eden fonksiyon.
    void SetGravity()
    {
        if (_characterController.isGrounded)
        {
            float groundedGravity = -0.05f;
            upVelocity.y = groundedGravity * Time.deltaTime;
        }
        else if (!_characterController.isGrounded)
        {
            upVelocity.y -= gravity * Time.deltaTime;
        }
    }

    //Karakterin önünde bir ray çizerek herhangi bir obje ile çarpýþýp çarpýþmadýðýný kontrol eder.
    void DrawWallCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(wallCheckTarget.position, wallCheckTarget.forward, out hit, wallCheckLength, groundMask))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                isWallCheck = true;
            }
        }
        else
        {
            isWallCheck = false;
        }
        Debug.DrawRay(wallCheckTarget.position, wallCheckTarget.forward * wallCheckLength, Color.red);
    }

    private void OnEnable()
    {
        //Player Input aktifleþtirilir...
        playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        //Player Input pasifleþtirilir...
        playerInput.Player.Disable();
    }

    private void Update()
    {
        SetGravity();
        SetAnimator();
        SetRotation();
        SetSprint();
        SetJumpProperties();
        DrawWallCheck();

        _characterController.Move(movementDirection * speed * run * Time.deltaTime);
        _characterController.Move(upVelocity * Time.deltaTime);

    }

    private void OnDrawGizmos()
    {
        //Karakterin zemin ile temasýný kontrol eden kürenin çizdirilmesi
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, groundSphereRadius);
    }
}
