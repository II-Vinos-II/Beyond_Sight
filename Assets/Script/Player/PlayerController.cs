using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
//using Unity.VisualScripting.ReorderableList;
using UnityEditor;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public Animator animator;
    public GameObject uIPrefab;
    public bool uIPrefabBool;

    [Header("Moves")]
    Vector3 movement;
    public bool isTurning;
    public float rotationSpeed;
    public bool isLanded;
    public bool isGrounded;
    public bool isCroutch;
    public bool isFalling;
    public bool isJumping;
    public bool canJump;
    public bool canMove;
    public bool crossable;
    public bool canLand;
    public bool isLanding;
    public bool canDetectGround;
    public GameObject playerHips;
    public float climbLerpDuractionA = .05f;
    public float dragX;
    public float elapsedTimeA;
    public float elapsedTimeRotation = 5;
    public float t;
    public float percentageCompleteA;
    public Vector3 toPointAClimb;
    public Vector3 velocity;
    public bool debutClimb;
    public bool finClimb;

    public float vitesseGlissade;
    public float vitesseGlissadeMax = 50;


    /*[Header("Shoot")]
    public float aimForce = 0f;
    public float aimForceMax = 5f;
    float pressionAim;
    public bool canShoot;

    [Header("Cursor")]
    public float cursorSpeed = 15f;
    Cursor_Controller cursorCtrl;
    public bool isAiming;*/

    [Header("Scan")]
    public bool canScanWithInput;
    public bool isWithle;

    public float jumpForce = 20f;
    public float moveSpeed = 55f;
    public float moveSpeedRun = 110F;

    public Obstacle_Controller obstacle_Controller;
    public LadderController ladder_Controller;
    public GlissadeController glissade_Controller;
    public monsterDeclanchement monstre;

    public enum STATUS
    {
        MOVING,
        JUMPING,
        FALLING,
        CLIMBING,
        LADDER,
        AIM,
        GLISSADE
    }
    public STATUS status;

    public bool goingRight;
    public float timeToLerp;
    private float elapsedTime;

    public bool isFrozen;

    private void Awake()
    {
        canJump = true;
        isFrozen = false;
    }

    void Start()
    {
        uIPrefabBool = true;
        rb = GetComponent<Rigidbody>();
        //cursorCtrl = FindObjectOfType<Cursor_Controller>();
        rb.drag = 0;
        Physics.gravity = new Vector3(0, -9.81f, 0) * 2f;
    }

    void Update()
    {
        //Set Variables each tic
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y, -.14f);

        elapsedTimeRotation += Time.deltaTime;
        t = Mathf.Clamp01(elapsedTimeRotation / 0.5f);

        velocity = rb.velocity;
        velocity.x = Mathf.Lerp(velocity.x, 0f, dragX * Time.deltaTime);
        rb.velocity = velocity;

        //Call Functions
        if (!isFrozen)
        {
            AnimationControl();
            StatusControl();
        }


        /*if (uIPrefabBool)
        {
            uIPrefab.SetActive(true);
            canMove = false;
        }
        if ((Input.GetKeyDown(KeyCode.JoystickButton7)|| Input.GetKeyDown(KeyCode.Return)) && uIPrefabBool)
        {
            uIPrefab.SetActive(false);
            uIPrefabBool = false;
            canMove = true;
        }*/

        //AimAndShoot();
        //pressionAim = Input.GetAxis("AimH") + Input.GetAxis("AimV");

        //ladder ???
        if (ladder_Controller)
        {
            status = STATUS.LADDER;
            canMove = false;
            rb.velocity += Input.GetAxis("Vertical") * Time.deltaTime * Vector3.up * 40;
            rb.drag = 12;
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
            animator.speed = 1;
            rb.drag = 0;
        }

        if (isLanding && status == STATUS.MOVING)
        {
            animator.SetBool("Land", true);
            isLanding = false;
        }

        if (glissade_Controller != null)
        {
            status = STATUS.GLISSADE;
        }
    }

    public void StatusControl()
    {
        switch (status)
        {
            case STATUS.MOVING:
                Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up), Color.red);
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up), Color.red, 30f);

                Withle();

                JumpAndCross();
                Moving();
                rb.drag = 0;
                dragX = 15f;
                isGrounded = true;
                canMove = true;
                //canJump = true;
                isJumping = false;
                isFalling = false;
                debutClimb = false;
                /*isAiming = false;
                cursorCtrl.gameObject.SetActive(false);
                canShoot = false;*/
                break;

            case STATUS.JUMPING:
                Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up), Color.green);
                JumpAndCross();
                dragX = 0f;
                isJumping = true;
                canMove = false;
                isGrounded = false;
                debutClimb = false;
                break;

            case STATUS.FALLING:
                Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * 1000, Color.white); //Ground not detected
                JumpAndCross();
                dragX = 0f;
                isGrounded = false;
                canMove = false;
                isJumping = false;
                canJump = true;
                isLanding = true;   //Reference in Update() Method
                isFalling = true;   //Reference in Update() Method
                debutClimb = false;
                break;

            case STATUS.LADDER:
                JumpAndCross();
                dragX = 40f;
                isGrounded = true;
                canJump = true;
                isJumping = false;
                debutClimb = false;
                break;

            /*case STATUS.AIM:
                canMove = false;
                isAiming = true;
                cursorCtrl.gameObject.SetActive(true);
                canShoot = true;
                if (pressionAim == 0) // if it equal to zero
                {
                    status = STATUS.MOVING;
                }
                break;*/

            case STATUS.CLIMBING:
                debutClimb = true;
                if (percentageCompleteA < 1)
                {
                    debutClimb = true;
                    canMove = false;
                    canJump = false;
                    isFalling = false;

                    if (!animator.applyRootMotion)
                    {
                        elapsedTimeA += Time.deltaTime;
                        percentageCompleteA = elapsedTimeA / climbLerpDuractionA;
                        transform.position = Vector3.Lerp(toPointAClimb, obstacle_Controller.pointA.transform.position, percentageCompleteA);
                    }
                }
                else
                {
                    if (!animator.applyRootMotion)
                    {
                        transform.position = obstacle_Controller.pointA.transform.position;
                    }
                }
                break;

            case STATUS.GLISSADE:
                canMove = false;
                canJump = false;
                isFalling = false;
                isJumping = false;
                rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
                rb.drag = 0;
                dragX = 0;
                vitesseGlissade = 5;
                if (vitesseGlissade < vitesseGlissadeMax)
                    rb.AddForce(rb.velocity * vitesseGlissade * Time.deltaTime, ForceMode.Acceleration);
                break;

        }
    }

    public void Withle()
    {
        if ((Input.GetKey(KeyCode.JoystickButton3) || Input.GetKey(KeyCode.A)) && canScanWithInput)
        {
            isWithle = true;
            PointLightScanner.GetInstance().StartScanner(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), 100f, 1f);
            StartCoroutine(WaitToScanManually());
        }
        else if ((Input.GetKeyUp(KeyCode.JoystickButton3) || Input.GetKeyUp(KeyCode.A)) && canScanWithInput)
        {
            isWithle = false;
        }

    }
    public void AnimationControl()
    {
        if (status == STATUS.MOVING && canMove)
        {
            //if (!uIPrefabBool)
            //animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
            //else
            //animator.SetFloat("Speed", 0);

            if (Input.GetKey(KeyCode.JoystickButton1) || Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = 55f;
                animator.SetBool("Croutch", true);
                StartCoroutine(CroutchFloatTrans(true));
                //animator.SetFloat("CroutchFloat", 1f);
                //isCroutch = true;
            }
            else
            {
                RaycastHit hitUp;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hitUp, 2f))
                {
                    if (hitUp.transform.CompareTag("UpObstacle"))
                    {
                        moveSpeed = 55f;
                        animator.SetBool("Croutch", true);
                        StartCoroutine(CroutchFloatTrans(true));
                        //animator.SetFloat("CroutchFloat", 1f);
                        //isCroutch = true;
                    }
                }
                moveSpeed = 110f;
                animator.SetBool("Croutch", false);
                StartCoroutine(CroutchFloatTrans(false));
                //animator.SetFloat("CroutchFloat", 0f);
                //isCroutch = false;
            }

            animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
            animator.SetBool("Jump", false);
            animator.SetBool("Fall", false);
            animator.SetBool("FrondeCharge", false);
            animator.SetBool("Ladder", false);
        }

        else if (status == STATUS.JUMPING)
        {
            animator.SetBool("Jump", true);
            animator.SetBool("Ladder", false);
        }

        else if (status == STATUS.FALLING)
        {
            animator.SetBool("Fall", true);
            animator.SetBool("Ladder", false);
        }

        else if (status == STATUS.AIM)
        {
            animator.SetBool("FrondeCharge", true);
            animator.SetFloat("Speed", 0);
        }

        else if (status == STATUS.LADDER)
        {
            StartCoroutine(TransitionToLadder());
        }

        else if (status == STATUS.CLIMBING)
        {
            animator.SetBool("Ladder", false);
            //Lancement de l'anim de Climb plus bas car besoin de l'appeler qu'à une frame
        }
    }

    public void Moving()
    {
        //Demi-Tour
        if (canMove)
        {
            animator.SetFloat("X", movement.x);

            float rotationValue = transform.rotation.y;
            rb.velocity += moveSpeed * Time.deltaTime * movement;

            if (movement.x > 0)
            {
                goingRight = true;
            }
            else if (movement.x < 0)
            {
                goingRight = false;
            }

                if (goingRight)
                {
                    transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, -90, 0), Quaternion.Euler(0, 90, 0), elapsedTime / timeToLerp);
                    if (elapsedTime < timeToLerp)
                    {
                        isTurning = true;
                        elapsedTime += Time.deltaTime;
                        StartCoroutine(WaitToMoveAfterTurning());
                    }
                    else
                        isTurning = false;
                }
                else
                {
                    transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, -90, 0), Quaternion.Euler(0, 90, 0), elapsedTime / timeToLerp);
                    if (elapsedTime > 0)
                    {
                        isTurning = true;
                        elapsedTime -= Time.deltaTime;
                        StartCoroutine(WaitToMoveAfterTurning());
                    }
                    else
                        isTurning = false;
                }
            
         

            if (movement.x == 0)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
            }

        }

    }

    public void JumpAndCross()
    {
        //Jump
        // D?tection du sol, pour ne pouvoir faire qu'un saut ? la fois et g?re la gravit? en fonction de la hauteur du saut
        if (canDetectGround && status != STATUS.CLIMBING /*&& !uIPrefabBool*/)
        {

            RaycastHit hitGround;
            if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hitGround, .6f))
            {
                if (hitGround.transform.CompareTag("Ground"))
                {
                    //isGrounded = true;
                    status = STATUS.MOVING;
                }             
            }
            else if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hitGround, 4f)) //Sol détécté
            {
                if (hitGround.transform.CompareTag("Ground"))
                {
                    status = STATUS.JUMPING;
                }
            }
            else if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hitGround, 1000f))
            {
                status = STATUS.FALLING;
                //StartCoroutine(WaitBigJump());
            }

            if (isGrounded) //Quand le raycast d?tecte le sol
            {
                if(!monstre.ouaiscamarcheenculedetamerelapute) // Detection de la triggerbox d'activation de l'apparition du monstre
                    canMove = true; // Le joueur est en capacit? de bouger de droite ? gauche

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    if (!ladder_Controller || canJump)
                    {
                        status = STATUS.JUMPING;
                        JumpNow();
                    }
                }
            }
            else
            {
                canMove = false; //Le joueur est dans l'incapacit? de bouger de droite ? gauche
            }
        }
        else if (status == STATUS.LADDER)
        {
            if (obstacle_Controller != null)
            {
                if (status != STATUS.CLIMBING && !obstacle_Controller.canCross)
                {
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
                    {
                        status = STATUS.JUMPING;
                        rb.velocity = ((transform.up / 3f) + (-transform.forward / 3f)) * jumpForce;
                        if (transform.rotation == Quaternion.Euler(0f, 90f, 0f))
                        {
                            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                        }
                        else if (transform.rotation == Quaternion.Euler(0f, -90f, 0f))
                        {
                            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                        }

                        ladder_Controller = null;
                        canDetectGround = true;
                    }
                }
            }
        }

        // CROSS OBSTACLES
        if (obstacle_Controller != null)
        {
            if (status != STATUS.CLIMBING && obstacle_Controller.canCross)
            {
                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space) || status == STATUS.JUMPING || status == STATUS.FALLING)
                {
                    if (status == STATUS.LADDER)
                    {
                        ladder_Controller = null;
                    }
                    toPointAClimb = transform.position;
                    status = STATUS.CLIMBING;
                    elapsedTimeA = 0;
                    animator.SetTrigger("Climb");
                }
            }
        }

    }

    public void JumpNow()
    {
        Debug.Log("SAUTE");
        status = STATUS.JUMPING;
        StartCoroutine(WaitToJump());
        rb.velocity = (transform.up / 3f) * jumpForce;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton1))
        {
            rb.velocity = ((transform.up / 3f) + movement / 2f) * jumpForce;
        }
        else if (!Input.GetKey(KeyCode.RightShift) || !Input.GetKey(KeyCode.JoystickButton1))
        {
            rb.velocity = ((transform.up / 3f) + movement / 3f) * jumpForce;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            rb.velocity = ((transform.up / 3f) + (movement / 3f)) * jumpForce;
        }
    }

    #region [OLD Shoot Script]
    /* public void AimAndShoot()
     {

         //  AIM

         //animator.SetFloat("Blend", pressionAim); // Capting the trigger pression of the aim trigger

         if (pressionAim > 0 || pressionAim < 0)
         {
             status = STATUS.AIM;
         }
         else
         {
             return;
         }


         //  SHOOT

         aimForce = pressionAim;

     }

    public void OnShoot()
    {
        GetComponentInChildren<Bullet_Controller>().Shoot();
    }*/

    #endregion

    #region [The Waits Coroutine]
    IEnumerator WaitToScanManually()
    {
        canScanWithInput = false;
        yield return new WaitForSeconds(.4f);
        canScanWithInput = true;
    }

    public IEnumerator WaitToMove()
    {
        canMove = false;

        yield return new WaitForSeconds(5f);
        canMove = true;
    }
    public IEnumerator WaitToMoveAfterTurning()
    {
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(.1f);
        rb.velocity += movement * moveSpeed * Time.deltaTime;
    }

    public IEnumerator WaitToJump()
    {
        canJump = false;
        yield return new WaitForSeconds(1.5f);
        canJump = true;
    }

    IEnumerator WaitBigJump()
    {
        yield return new WaitForSeconds(1f);
        status = STATUS.FALLING;
    }
    #endregion

    //Echelle
    public IEnumerator TransitionToLadder()
    {
        animator.SetBool("Ladder", true);
        animator.SetBool("Jump", false);
        animator.SetBool("Fall", false);

        if (animator.GetBool("Ladder"))
        {
            if (isFalling)
            {
                yield return new WaitForSeconds(1f);
                isFalling = false;
            }
            else
            {
                yield return new WaitForSeconds(.6f);
            }

            animator.SetFloat("Speed", 0);
            animator.speed = Input.GetAxis("Vertical");
        }
    }

    #region [Animation Events]
    public void AnimCanMoveOnPos()
    {
        obstacle_Controller.canCross = false;
        debutClimb = false;
        status = STATUS.MOVING;
        canMove = true;
        canJump = true;
        obstacle_Controller = null;
        percentageCompleteA = 0;
        isFalling = false;
    }

    public void EnableRootMotion()
    {
        animator.applyRootMotion = true;
    }

    public void DisableRootMotion()
    {
        animator.applyRootMotion = false;
    }

    
    public void FreezePlayer()
    {
        isFrozen = true;
        //rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    public void UnFreezePlayer()
    {
        isFrozen = false;
        //rb.constraints &= ~RigidbodyConstraints.FreezePositionX | ~RigidbodyConstraints.FreezePositionY;
    }
    #endregion

    //Transition fluide quand on appuie sur la touche de croutch
    public IEnumerator CroutchFloatTrans(bool transition)
    {
        if (transition && !isCroutch)
        {
            isCroutch = true;

            for (float i = 0; i < 1f; i += 0.05f)
            {
                animator.SetFloat("CroutchFloat", i);
                yield return 0;
            }

            animator.SetFloat("CroutchFloat", 1f);
        }
        else if (!transition && isCroutch)
        {
            isCroutch = false;

            for (float i = 1; i > 0f; i -= 0.05f)
            {
                animator.SetFloat("CroutchFloat", i);
                yield return 0;
            }
            animator.SetFloat("CroutchFloat", 0f);
        }
    }
}
