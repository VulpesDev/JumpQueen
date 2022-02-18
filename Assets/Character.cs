using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CharacterController))]

public class Character : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    float charge = 0.0f;
    public float maxCharge = 3.0f;
    public float chargeRate = 0.01f;
    bool holding = false;
    bool grounded;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeedBase;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;
    [HideInInspector]
    public bool canLook = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //lookSpeed = SeneManagement.sensitivity;
        //lookSpeedBase = lookSpeed;
    }

    void Update()
    {
        Effects();
        grounded = characterController.isGrounded;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed)
            * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed)
            * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            StartCoroutine(JumpCheck());
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (canLook)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    IEnumerator JumpCheck()
    {
        holding = true;
        if (Input.GetButtonUp("Jump"))
        {
            moveDirection.y = jumpSpeed * charge;
            holding = false;
            yield return new WaitUntil(() => characterController.isGrounded != true);
            StartCoroutine(PushForwards());
        }
        yield return new WaitForSeconds(0.01f);
        if(charge < maxCharge)
        charge += chargeRate;
        if (holding) StartCoroutine(JumpCheck());
    }

    IEnumerator PushForwards()
    {
        StartCoroutine(Cooldown());
        while (!characterController.isGrounded)
        {
            characterController.Move(transform.TransformDirection(Vector3.forward)
                * walkingSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator Cooldown()
    {
        while (charge > 0.0f)
        {
            charge -= 0.4f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void FixedUpdate()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.fixedDeltaTime;
            characterController.Move(new Vector3(0, moveDirection.y, 0) * Time.fixedDeltaTime);
        }
        if(!holding && characterController.isGrounded)
        characterController.Move(moveDirection * Time.fixedDeltaTime);
    }

    void Effects()
    {
        Volume vol = GameObject.Find("Volume").GetComponent<Volume>();
        
        if (vol.profile.TryGet(out Vignette vignette2))
        {
            Vignette vignette = vignette2;
            vignette.intensity.Override(charge / 4f);
        }
        
    }
}