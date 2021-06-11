using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameManager gm;
    public GameObject cam_default;
    GameObject cam_3p;

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

    public GameObject projectile;

    void Awake()
    {
        startposition = transform.position;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        cam_3p = cam_default;
        gm = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.gameState != GameManager.GameState.GAME) 
        {
            Cursor.lockState = CursorLockMode.Confined;
            cam_3p.SetActive(false);
            return;
        } else {
            canMove = true;
            cam_3p.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.Locked;

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
            if (z != 0) {
                animator.SetBool("andando", true);
            } else {
                animator.SetBool("andando", false);
            }
            

            Vector3 direction = new Vector3(x, 0f, z).normalized;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            if (z >= 0f) {
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            } else {
                transform.rotation = Quaternion.Euler(0f, targetAngle + 180f, 0f);
            }

            if (direction.magnitude >= 0.1f) 
            {       
                
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
        if (gm.gameState != GameManager.GameState.GAME) 
            return;

        if (Input.GetMouseButtonDown(0)) {            
            animator.SetTrigger("spell1");
            is_spelling = true;
        }

        if (Input.GetMouseButtonDown(1)) {
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

    public void TakeDamage()
    {
        gm.lifes--;
        Reset();
    }

    public void Reset()
    {
        if (gm.lifes <= 0 && gm.gameState == GameManager.GameState.GAME)
        {       
            player.transform.position = startposition;
            gm.ChangeState(GameManager.GameState.ENDGAME);
        }   
    }

    void AE_SpellStateSwitch()
    {
        is_spelling = false;
    }

    void AE_Spelling_1()
    {
        Vector3 spawn = new Vector3(transform.position.x, 1.7f, transform.position.z + 1);
        
        GameObject bulletObject = Instantiate(projectile);
        bulletObject.transform.position = spawn + transform.forward;
        bulletObject.transform.forward = transform.forward;

        
        // Rigidbody rb = Instantiate(projectile, spawn, Quaternion.identity).GetComponent<Rigidbody>();
        // rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        // rb.AddForce(transform.up * 8f, ForceMode.Impulse);
    }

    void AE_Spelling_2()
    {
        // int qtd = 3;

        // Vector3 _spawn = new Vector3(transform.position.x, 1.7f, transform.position.z);
        // transform.rotation = Quaternion.Euler(0f, targetAngle + 180f, 0f);

        float angle = transform.rotation.eulerAngles.y;

        float _angle = 0f;
        if (angle >= 90f && angle < 180f) {
            _angle = 180f - angle;
        }
        else if (angle >= 180f && angle < 270f) {
            _angle = angle - 180f;                
        } 
        else if (angle >= 270f && angle < 360f) {
            _angle = 360f - angle;
        }
        
        float a = Mathf.Cos(_angle * Mathf.Deg2Rad); 
        float b = Mathf.Sin(_angle * Mathf.Deg2Rad);
        

        Vector3 _spawn = new Vector3(transform.position.x, 1.7f, transform.position.z);
        GameObject bulletObject = Instantiate(projectile);
        bulletObject.transform.position = _spawn + transform.forward;
        bulletObject.transform.forward = transform.forward;

        Vector3 _spawn1 = new Vector3(transform.position.x - a, 1.7f, transform.position.z + b);
        GameObject bulletObject1 = Instantiate(projectile);
        bulletObject1.transform.position = _spawn1 + transform.forward;
        bulletObject1.transform.forward = transform.forward;

        Vector3 _spawn2 = new Vector3(transform.position.x + a, 1.7f, transform.position.z - b);
        GameObject bulletObject2 = Instantiate(projectile);
        bulletObject2.transform.position = _spawn2 + transform.forward;
        bulletObject2.transform.forward = transform.forward;

        
        // for (int i = 0; i < qtd; i++) {
            

        //     Vector3 _spawn = new Vector3(transform.position.x, 1.7f, transform.position.z);

        //     GameObject bulletObject = Instantiate(projectile);
        //     bulletObject.transform.position = _spawn + transform.forward;
        //     bulletObject.transform.forward = transform.forward;

        // }
    }

}