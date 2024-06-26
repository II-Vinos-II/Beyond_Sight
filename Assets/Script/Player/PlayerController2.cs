using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
//using Unity.VisualScripting.ReorderableList;
using UnityEditor;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using UnityEngine.UIElements;
//using UnityEngine.WSA;

public class PlayerController2 : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    [Header("Moves")]
    Vector3 movement;
    public float rotationSpeed;
    public float jumpForce = 20f;
    public float moveSpeed = 55f;
    private float actualMoveSpeed;
    public float dragX;
    public Vector3 velocity;

    [Header("Boolean")]
    public bool isGrounded;
    public bool isCrouch;
    public bool canJump;
    public bool canMove;
    public bool canLand;
    public bool onLadder;
    public bool debutLadder; //changé dans le script LadderDetection
    public bool debutClimb; //changé dans le script ClimbDetection
    public bool isWithle;
    public bool goingRight;
    public bool isTurning;
    public bool isFrozen;
    public bool canScanWithInput;
    public bool isDoingATuto;

    public bool onKeyboard;

    public LayerMask layerExclude;

    public enum STATUS
    {
        MOVING,
        JUMPING,
        FALLING,
        CLIMBING,
        LADDER
    }
    public STATUS status;


    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        canJump = true;
        canScanWithInput = true;
        rb.drag = 0;

        actualMoveSpeed = moveSpeed;
        Physics.gravity = new Vector3(0, -9.81f, 0) * 2f;

        if (!GameObject.Find("TitleScreen"))
        {
            EnableMove();
            animator.Play("IdleTree");
        }
    }

    void Update()
    {
        if (InputManager._anyKeyDown)
        {
            onKeyboard = true;
        }

        if (InputManager._anyConDown)
        {
            onKeyboard = false;
        }

        if (movement.x == 0 && isGrounded && 
            (animator.GetCurrentAnimatorStateInfo(0).IsName("IdleTree") || animator.GetCurrentAnimatorStateInfo(0).IsName("Cinematic_Idle")))
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }

        if (isCrouch)
        {
            GetComponent<CapsuleCollider>().height = 2.4f;
            GetComponent<CapsuleCollider>().center = new Vector3(0, 1.1f, 0);
        }
        else
        {
            GetComponent<CapsuleCollider>().height = 3.8f;
            GetComponent<CapsuleCollider>().center = new Vector3(0, 1.8f, 0);
        }

        AnimationControl();
        StatusControl();
        JumpAndCross();

        if(status != STATUS.CLIMBING && !isTurning)
        {
            BoxcastCheck();
        }


        if (!isFrozen)
        {
            //Set Variables each tic
            //movement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            movement = new Vector3(InputManager._moveHorizontal, 0, 0);
            transform.position = new Vector3(transform.position.x, transform.position.y, -.14f);

            //ladder
            if (onLadder)
            {
                status = STATUS.LADDER;
                canMove = false;
                //rb.velocity += Input.GetAxis("Vertical") * Time.fixedDeltaTime * Vector3.up * 40;
                rb.velocity += InputManager._moveVertical * Time.fixedDeltaTime * Vector3.up * 40;
                rb.drag = 12;
                rb.useGravity = false;
            }
            else
            {
                velocity = rb.velocity;
                velocity.x = Mathf.Lerp(velocity.x, 0f, dragX * Time.fixedDeltaTime);

                if (!rb.isKinematic)
                {
                    rb.velocity = velocity;
                }

                if (!animator.applyRootMotion)
                {
                    rb.useGravity = true;
                }
                rb.drag = 0;
            }
        }

        //landing
        if (canLand && isGrounded)
        {
            animator.SetTrigger("Land");
            canLand = false;
            status = STATUS.MOVING;
        }
    }

    public void StatusControl()
    {
        switch (status)
        {
            case STATUS.MOVING:
                Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up), Color.red);
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up), Color.red, 30f);

                if (canMove && !isFrozen)
                {
                    Moving();
                }

                if ((animator.GetCurrentAnimatorStateInfo(0).IsName("IdleTree") || isWithle) && !isCrouch)
                {
                    Withle();
                }

                dragX = 5f;
                canMove = true;
                break;

            case STATUS.JUMPING:
                Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up), Color.green);

                dragX = 0f;
                canMove = false;
                break;

            case STATUS.FALLING:
                Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * 1000, Color.white); //Ground not detected

                dragX = 0f;
                canMove = false;
                canLand = true;
                break;

            case STATUS.LADDER:
                dragX = 40f;
                break;

            case STATUS.CLIMBING:
                canMove = false;
                break;
        }
    }

    public void AnimationControl()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("goingRight", goingRight);
        if (!isDoingATuto)
        {
            //animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
            animator.SetFloat("Speed", Mathf.Abs(InputManager._moveHorizontal));
        }

        if (!isFrozen && status == STATUS.LADDER)
        { 
            //animator.SetFloat("Y", Input.GetAxis("Vertical"));
            animator.SetFloat("Y", InputManager._moveVertical);
        }
        else
        {
            animator.SetFloat("Y", 0);
        }


        if (status == STATUS.MOVING && canMove && !isFrozen)
        {
            //Crouch
            if (InputManager._crouch)
            {
                if (!isCrouch)
                {
                    actualMoveSpeed = moveSpeed / 1.3f;
                    StartCoroutine(CrouchFloatTrans(true));
                }
            }
            else
            {
                if (isCrouch)
                {
                    RaycastHit hitUp;
                    Vector3 castStart = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                    Vector3 castSize = Vector3.one * 0.4f;

                    DrawBoxCastBox(castStart, castSize, Quaternion.identity, Vector3.up, 2f, Color.yellow);

                    if (Physics.BoxCast(castStart, castSize, Vector3.up, out hitUp, Quaternion.identity, 2f, layerExclude))
                    {
                        if (hitUp.transform.CompareTag("UpObstacle"))
                        {
                            actualMoveSpeed = moveSpeed / 1.3f;
                            StartCoroutine(CrouchFloatTrans(true));
                        }
                    }
                    else
                    {
                        actualMoveSpeed = moveSpeed;
                        StartCoroutine(CrouchFloatTrans(false));
                    }

                }
            }



            animator.SetBool("Fall", false);
            animator.SetBool("Ladder", false);
        }

        else if (status == STATUS.JUMPING)
        {
            animator.SetBool("Fall", false);
            animator.SetBool("Ladder", false);
        }

        else if (status == STATUS.FALLING)
        {
            animator.SetBool("Fall", true);
            animator.SetBool("Ladder", false);
        }

        else if (status == STATUS.LADDER)
        {
            animator.SetBool("Ladder", true);
            animator.SetBool("Fall", false);
        }

        else if (status == STATUS.CLIMBING)
        {
            animator.SetBool("Fall", false);
            //animator.SetBool("Ladder", false);
        }
    }

    public void Moving()
    {
        //movement
        if (!rb.isKinematic)
        {
            rb.velocity += actualMoveSpeed * Time.fixedDeltaTime * movement;
        }

        //Demi-Tour
        if (((movement.x > 0 && !goingRight) || (movement.x < 0 && goingRight)) && !isTurning && !isDoingATuto &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Vanne") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Vanne_Inverse"))
        {
            goingRight = !goingRight;
            isTurning = true;
            DisableMove();

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("IdleTree") || animator.GetCurrentAnimatorStateInfo(0).IsName("RunTree"))
            {
                animator.SetTrigger("TurnTrigger");
            }
            else
            {
                ClampRotation();
            }
        }
    }

    public void JumpAndCross()
    {
        if (status != STATUS.LADDER && status != STATUS.CLIMBING &&
            isGrounded && canJump && !onLadder && !isCrouch && !isFrozen && !isTurning && InputManager._jumpDown)
        {
            status = STATUS.JUMPING;
            animator.SetTrigger("JumpTrigger");
            StartCoroutine(WaitToJump());
        }

        //Ladder
        if (debutLadder && status != STATUS.LADDER && status != STATUS.CLIMBING)
        {
            status = STATUS.LADDER;
            animator.SetTrigger("LadderStart");
            if (!isGrounded)
            {
                PointLightScanner.GetInstance().StartScanner(new Vector3(transform.localPosition.x + 0.5f, transform.localPosition.y + 2, transform.localPosition.z), 5f, 0.5f);
                PlayerWalkSounds.GetInstance().PlayRandom(0);
                PlayerWalkSounds.GetInstance().PlayRandom(0);
                PlayerWalkSounds.GetInstance().PlayRandom(0);
            }
            onLadder = true;
            debutLadder = false;
        }

        //Climb
        if (debutClimb && status != STATUS.CLIMBING)
        {
            debutClimb = false;
            onLadder = false;
            status = STATUS.CLIMBING;
            animator.SetTrigger("Climb");
            DisableRootMotion();
        }

    }

    public void Withle()
    {
        if (/*InputManager._whistle*/InputManager._whistleDown && canScanWithInput && !isDoingATuto)
        {
            //ShakeInstance.instance.CamShake(2f, 0.5f);
            if (!isWithle)
            {
                StartCoroutine(WhistleSound());
            }

            isWithle = true;
            DisableMove();
            animator.SetBool("Echo", true);
            canScanWithInput = false;
        }


        /*if (InputManager._whistleUp && isWithle)
        {
            isWithle = false;
            EnableMove();
            animator.SetBool("Echo", false);
            StartCoroutine(WaitToScan(0.5f));
        }*/
    }

    public IEnumerator WhistleSound()
    {
        GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/Whistle/WhistleFull"));
        yield return new WaitForSeconds(Resources.Load<AudioClip>("Sounds/Whistle/WhistleFull").length - 0.07f);

        //press once addition
        isWithle = false;
        EnableMove();
        animator.SetBool("Echo", false);
        StartCoroutine(WaitToScan(0.5f));
        //press once addition

        while (isWithle)
        {
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/Whistle/WhistleLoop"));
            yield return new WaitForSeconds(Resources.Load<AudioClip>("Sounds/Whistle/WhistleLoop").length - 0.07f);
            GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/Whistle/WhistleEnd"));
        }
    }

    #region [The Waits Coroutine]
    IEnumerator WaitToScan(float waitTime)
    {
        canScanWithInput = false;
        yield return new WaitForSeconds(waitTime);
        canScanWithInput = true;
    }

    public IEnumerator WaitToTurn()
    {
        yield return new WaitForSeconds(0.1f);
        isTurning = false;
    }

    public IEnumerator WaitToJump()
    {
        canJump = false;
        yield return new WaitForSeconds(1.1f);
        canJump = true;
    }
    #endregion

    #region [Animation Events]
    public void AnimCanMoveOnPos()
    {
        canJump = true;
        onLadder = false;
        isGrounded = true;
        debutLadder = false;
        status = STATUS.MOVING;
        EnableMove();
    }

    public void EnableRootMotion()
    {     
        animator.applyRootMotion = true;
    }

    public void DisableRootMotion()
    {
        animator.applyRootMotion = false;
    }

    public void EnableTrigger()
    {
        this.GetComponent<Collider>().isTrigger = true;
        rb.useGravity = false;
    }

    public void DisableTrigger()
    {
        this.GetComponent<Collider>().isTrigger = false;
        rb.useGravity = true;
    }

    public void EnableMove()
    {
        if (!isDoingATuto)
        {
            isFrozen = false;
        }
    }

    public void DisableMove()
    {
        isFrozen = true;
    }

    public void ClampRotation()
    {
        if (goingRight)
        {
            transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(transform.rotation.y, 90, 1), 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(transform.rotation.y, -90, 1), 0);
        }

        gameObject.transform.Find("HitClimb").transform.rotation = new Quaternion(0, 0, 0, 0);
        gameObject.transform.Find("LadderDetection").transform.rotation = new Quaternion(0, 90, 0, 0);

        EnableMove();
        StartCoroutine(WaitToTurn());
    }

    public void PlayerToPoint(Vector3 pointA, float waitTime)
    {
        StartCoroutine(MoveToSpot(pointA, waitTime));
    }

    IEnumerator MoveToSpot(Vector3 pointA, float waitTime)
    {
        float elapsedTime = 0;
        Vector3 currentPos = transform.position;
        DisableMove();

        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(currentPos, pointA, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Make sure we got there
        transform.position = pointA;
        yield return null;
        EnableMove();
    }
    #endregion


    #region [Raycast + Draw]
    //BoxcastGround
    public void BoxcastCheck()
    {
        RaycastHit hitGround;
        Vector3 castStart = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Vector3 castSize = Vector3.one * 0.4f;

        DrawBoxCastBox(castStart, castSize, Quaternion.identity, -Vector3.up, 0.8f, Color.yellow);

        if (Physics.BoxCast(castStart, castSize, -Vector3.up, out hitGround, Quaternion.identity, 0.8f, layerExclude))
        {
            //Debug.Log(hitGround.transform.gameObject);

            if (hitGround.transform.CompareTag("Ground"))
            {
                isGrounded = true;
                canMove = true;

                if ((status == STATUS.JUMPING || status == STATUS.FALLING) && status != STATUS.LADDER)
                {
                    status = STATUS.MOVING;
                }
            }
        }
        else if (Physics.BoxCast(castStart, castSize, -Vector3.up, out hitGround, Quaternion.identity, 5f, layerExclude))
        {
            if (hitGround.transform.CompareTag("Ground"))
            {
                isGrounded = false;
                status = STATUS.JUMPING;
            }
        }
        else if (Physics.BoxCast(castStart, castSize, -Vector3.up, Quaternion.identity, 1000f, layerExclude) && !isTurning)
        {
            isGrounded = false;
            status = STATUS.FALLING;
        }
    }

    //Draws the full box from start of cast to its end distance. Can also pass in hitInfoDistance instead of full distance
    public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, Color color)
    {
        direction.Normalize();
        Box bottomBox = new Box(origin, halfExtents, orientation);
        Box topBox = new Box(origin + (direction * distance), halfExtents, orientation);

        Debug.DrawLine(bottomBox.backBottomLeft, topBox.backBottomLeft, color);
        Debug.DrawLine(bottomBox.backBottomRight, topBox.backBottomRight, color);
        Debug.DrawLine(bottomBox.backTopLeft, topBox.backTopLeft, color);
        Debug.DrawLine(bottomBox.backTopRight, topBox.backTopRight, color);
        Debug.DrawLine(bottomBox.frontTopLeft, topBox.frontTopLeft, color);
        Debug.DrawLine(bottomBox.frontTopRight, topBox.frontTopRight, color);
        Debug.DrawLine(bottomBox.frontBottomLeft, topBox.frontBottomLeft, color);
        Debug.DrawLine(bottomBox.frontBottomRight, topBox.frontBottomRight, color);

        DrawBox(bottomBox, color);
        DrawBox(topBox, color);
    }

    public static void DrawBox(Box box, Color color)
    {
        Debug.DrawLine(box.frontTopLeft, box.frontTopRight, color);
        Debug.DrawLine(box.frontTopRight, box.frontBottomRight, color);
        Debug.DrawLine(box.frontBottomRight, box.frontBottomLeft, color);
        Debug.DrawLine(box.frontBottomLeft, box.frontTopLeft, color);

        Debug.DrawLine(box.backTopLeft, box.backTopRight, color);
        Debug.DrawLine(box.backTopRight, box.backBottomRight, color);
        Debug.DrawLine(box.backBottomRight, box.backBottomLeft, color);
        Debug.DrawLine(box.backBottomLeft, box.backTopLeft, color);

        Debug.DrawLine(box.frontTopLeft, box.backTopLeft, color);
        Debug.DrawLine(box.frontTopRight, box.backTopRight, color);
        Debug.DrawLine(box.frontBottomRight, box.backBottomRight, color);
        Debug.DrawLine(box.frontBottomLeft, box.backBottomLeft, color);
    }

    public struct Box
    {
        public Vector3 localFrontTopLeft { get; private set; }
        public Vector3 localFrontTopRight { get; private set; }
        public Vector3 localFrontBottomLeft { get; private set; }
        public Vector3 localFrontBottomRight { get; private set; }
        public Vector3 localBackTopLeft { get { return -localFrontBottomRight; } }
        public Vector3 localBackTopRight { get { return -localFrontBottomLeft; } }
        public Vector3 localBackBottomLeft { get { return -localFrontTopRight; } }
        public Vector3 localBackBottomRight { get { return -localFrontTopLeft; } }

        public Vector3 frontTopLeft { get { return localFrontTopLeft + origin; } }
        public Vector3 frontTopRight { get { return localFrontTopRight + origin; } }
        public Vector3 frontBottomLeft { get { return localFrontBottomLeft + origin; } }
        public Vector3 frontBottomRight { get { return localFrontBottomRight + origin; } }
        public Vector3 backTopLeft { get { return localBackTopLeft + origin; } }
        public Vector3 backTopRight { get { return localBackTopRight + origin; } }
        public Vector3 backBottomLeft { get { return localBackBottomLeft + origin; } }
        public Vector3 backBottomRight { get { return localBackBottomRight + origin; } }

        public Vector3 origin { get; private set; }

        public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
        {
            Rotate(orientation);
        }
        public Box(Vector3 origin, Vector3 halfExtents)
        {
            this.localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
            this.localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

            this.origin = origin;
        }


        public void Rotate(Quaternion orientation)
        {
            localFrontTopLeft = RotatePointAroundPivot(localFrontTopLeft, Vector3.zero, orientation);
            localFrontTopRight = RotatePointAroundPivot(localFrontTopRight, Vector3.zero, orientation);
            localFrontBottomLeft = RotatePointAroundPivot(localFrontBottomLeft, Vector3.zero, orientation);
            localFrontBottomRight = RotatePointAroundPivot(localFrontBottomRight, Vector3.zero, orientation);
        }
    }

    static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        Vector3 direction = point - pivot;
        return pivot + rotation * direction;
    }
    #endregion

    //Transition fluide quand on appuie sur la touche de crouch
    public IEnumerator CrouchFloatTrans(bool transition)
    {
        float elapsedTime;
        isTurning = true;

        if (transition && !isCrouch)
        {
            elapsedTime = 0f;

            while (elapsedTime < 1)
            {
                animator.SetFloat("CrouchFloat", elapsedTime);
                elapsedTime += 0.05f;
                yield return null;
            }

            isCrouch = true;
            animator.SetFloat("CrouchFloat", 1f);
        }
        else if (!transition && isCrouch)
        {
            elapsedTime = 1f;

            while (elapsedTime > 0f)
            {
                animator.SetFloat("CrouchFloat", elapsedTime);
                elapsedTime -= 0.05f;
                yield return null;
            }

            isCrouch = false;
            animator.SetFloat("CrouchFloat", 0f);
        }

        isTurning = false;
    }
}
