using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameManager gm;
    public GameObject cam_3p;

    public CharacterController controller;
    public Transform cam;
    public LayerMask groundMask;
    private Animator animator;

    public float speed      = 2f;       // velocidade do jogadpr
    public float gravity    = -9.8f;    // valor da gravidade
    Vector3 velocity;
    bool isGrounded;

    bool canMove = true;
    bool is_spelling = false;
    public Vector3 startposition;
    public GameObject player;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // Audio
    public AudioSource walkSFX;
    bool walkSFX_isPlaying = false;

    void Awake()
    {
        startposition = transform.position;
    }

    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();

        gm = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.gameState != GameManager.GameState.GAME) 
        {
            cam_3p.SetActive(false);
            return;
        } else {
            canMove = true;
            cam_3p.SetActive(true);
        }

        if (Input.GetKey(KeyCode.LeftShift)) {
            speed = 4f;
            animator.SetFloat("correr", speed);
        } else {
            speed = 2f;        
            animator.SetFloat("correr", speed);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && gm.gameState == GameManager.GameState.GAME) {
            gm.ChangeState(GameManager.GameState.PAUSE);
            canMove = false;
            walkSFX_isPlaying = false;
            walkSFX.Stop();
        }

        // Verifica se esta no chão
        isGrounded = Physics.CheckSphere(transform.position, 0.2f, groundMask);

        // Se no chão e descendo, resetar velocidade
        if (isGrounded && velocity.y < 0.0f)
        {
            velocity.y = -1.0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (canMove && !is_spelling) 
        {
            if (x != 0f || z != 0f)
            {
                if (!walkSFX_isPlaying) 
                {
                    walkSFX.Play();
                    walkSFX_isPlaying = true;
                }
            }
            else {
                if (walkSFX_isPlaying)
                {
                    walkSFX.Stop();
                    walkSFX_isPlaying = false;
                }
            }

            animator.SetFloat("virar", x);
            animator.SetFloat("andar", z);

            Vector3 direction = new Vector3(x, 0f, z).normalized;

            if (direction.magnitude >= 0.1f) 
            {   
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);

                // Aplica gravidade no personagem 
                velocity.y += gravity * Time.deltaTime;
                controller.Move(velocity * Time.deltaTime);
            }
        } else {
            controller.Move(new Vector3(0f, 0f, 0f));
            animator.SetFloat("virar", 0f);
            animator.SetFloat("andar", 0f);
        }
    }

    void LateUpdate()
    {
        if (Input.GetKey("o")) 
        {
            Reset();
        }

        if (Input.GetKeyDown("z")) {
            animator.SetTrigger("spell1");
            is_spelling = true;
        }
        if (Input.GetKeyDown("x")) {
            animator.SetTrigger("spell2");
            is_spelling = true;
        }
        if (Input.GetKeyDown("c")) {
            animator.SetTrigger("spell3");
            is_spelling = true;
        }

        RaycastHit hit;
        Debug.DrawRay(cam.position, transform.forward*10.0f, Color.magenta);
        if(Physics.Raycast(cam.position, transform.forward, out hit, 100.0f))
        {
            // Debug.Log(hit.collider.name);
        }
    }

    public void Reset()
    {
        player.transform.position = startposition;

        if(gm.lifes <= 0 && gm.gameState == GameManager.GameState.GAME)
        {
            gm.ChangeState(GameManager.GameState.ENDGAME);
        }        
    }

    void AE_SpellStateSwitch()
    {
        is_spelling = false;
    }
}