﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    public float lifeDuration = 2f;

    private float lifeTimer;

    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = lifeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        // Make the bullet move
        transform.position += transform.forward * speed * Time.deltaTime;

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
        }
    }
}
