using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    GameManager gm;

    public float speed = 12f;
    public float lifeDuration = 2f;

    private float lifeTimer;

     // Audio
    public AudioManager AudioManager;
    public AudioClip zombieDying;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
        lifeTimer = lifeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        // Make the bullet move
        transform.position += transform.forward * speed * Time.deltaTime;
        Vector3 scale = transform.localScale;
        scale.x -= 2 * Time.deltaTime;
        scale.y -= 2 * Time.deltaTime;
        scale.z -= 2 * Time.deltaTime;

        transform.localScale = scale;

        // Check if the bullet should be destroyed;
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f) {
            Destroy (gameObject);
        }
    }

    public void OnCollisionEnter(Collision col) {
        Destroy(gameObject);

        if (col.gameObject.tag == "Zombie") {
            Destroy(col.gameObject);
            AudioManager.PlaySFX(zombieDying);
            gm.points += 10;
        }
    }
}
