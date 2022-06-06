using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(CharacterController))]

public class CharacterMovement : MonoBehaviour
{
    #region Serialize Variables

    [Header("Movement Properties")]
    [SerializeField] float speedValue = 5f;
    [Range(3f, 20f)]
    [SerializeField] float smoothRotation = 1f;
    [Space(10)]
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float jumpForce = 100f;
    [SerializeField] float doubleJumpMultiplier = .5f;


    [Header("Character Sense")]
    [SerializeField] Transform wallCheckTarget;
    [SerializeField] float wallCheckLength = 1f;
    [Space(10)]
    [Range(0.1f, 1f)]
    [SerializeField] float groundSphereRadius = 0.3f;
    [SerializeField] LayerMask groundMask;

    #endregion



    //Kompanentler
    PlayerInput playerInput; //Player Input türünde referans
    CharacterController _characterController;
    Animator _animator;

    #region Character Control Variables

    //Character Movement
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 movementDirection;
    Vector3 upVelocity;

    //Karakter hareket deðerleri
    float speed;


    //Karakter hareketlerini kontrol eden deðiþkenler
    bool isMovementPressed;
    bool isWalking;
    bool isSprint;
    bool isJumping;
    bool isGrounded;
    bool isWallCheck;
    bool canDoubleJump;

    #endregion


    private void Awake()
    {
        playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        //Move Input ayarlarýný deðiþkene atama
        playerInput.Player.Move.started += OnMovementInput;
        playerInput.Player.Move.performed += OnMovementInput;
        playerInput.Player.Move.canceled += OnMovementInput;

    }

    private void Start()
    {
        //Oyun baþladýðýnda fare kilitlenir.
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Karakterin animator deðiþkenlerini ayarlayan fonksiyon.
    void SetAnimator()
    {
        _animator.SetBool("walk", isWalking);
        _animator.SetBool("run", isSprint);
        _animator.SetBool("isMovement", isMovementPressed);
        _animator.SetBool("isInAir", isGrounded);
    }

    //Move girdi ayarlama fonksiyonu
    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement = new Vector3(currentMovementInput.x, 0f, currentMovementInput.y);
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;

    }

    void OnMovement()
    {
        if (isMovementPressed)
        {
            speed = speedValue;
            isSprint = true;
        }
        else
        {
            speed = 0f;
            isSprint = false;
        }
    }

    //Zýplama
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(isGrounded || (!isGrounded && canDoubleJump))
            isJumping = true;
        }
    }

    #region Set Jump Properties

    //Zýplama özelliklerinin ayarlandýðý fonksiyon.
    void SetJumpProperties()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundSphereRadius, groundMask);

        if(isGrounded)
        {
            if (isJumping && !isWallCheck)
            {
                upVelocity.y = jumpForce;
                canDoubleJump = true;
                isJumping = false;
            } 
        }
        else
        {
            if (isJumping & canDoubleJump)
            {
                _animator.SetTrigger("doubleJump");
                upVelocity.y = jumpForce * doubleJumpMultiplier;
                canDoubleJump = false;
                isJumping = false;
            }
        }
    }

    #endregion

  
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
        OnMovement();
        SetGravity();
        SetAnimator();
        SetRotation();
        SetJumpProperties();
        //DrawWallCheck();

        _characterController.Move(movementDirection * speed * Time.deltaTime);
        _characterController.Move(upVelocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        //Karakterin zemin ile temasýný kontrol eden kürenin çizdirilmesi
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, groundSphereRadius);
    }
}
