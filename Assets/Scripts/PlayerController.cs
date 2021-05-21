using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public LayerMask groundMask;
    private Animator animator;

    public float speed      = 6f;       // velocidade do jogadpr
    public float gravity    = -9.8f;    // valor da gravidade
    Vector3 velocity;
    bool isGrounded;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Verifica se esta no chão
        isGrounded = Physics.CheckSphere(transform.position, 0.2f, groundMask);

        // Se no chão e descendo, resetar velocidade
        if (isGrounded && velocity.y < 0.0f)
        {
            velocity.y = -1.0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        animator.SetFloat("virar", x);
        animator.SetFloat("correr", z);

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
    }

    void LateUpdate()
    {
        RaycastHit hit;
        Debug.DrawRay(cam.position, transform.forward*10.0f, Color.magenta);
        if(Physics.Raycast(cam.position, transform.forward, out hit, 100.0f))
        {
            Debug.Log(hit.collider.name);
        }
    }
}