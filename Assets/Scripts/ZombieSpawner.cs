using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    GameManager gm;
    public GameObject Zombie;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
        GameManager.changeStateDelegate += Spawnar;
        Spawnar();
    }

    // Update is called once per frame
    void Spawnar()
    {
        if (gm.gameState == GameManager.GameState.GAME)
        {
            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }
            for(int j = 0; j < 5; j++){
                float x = Random.value * 8f;
                float y = Random.value * 4f;
    
                Vector3 posicao = new Vector3(90 + y, 0.2f, -4f + x);
                Instantiate(Zombie, posicao, Quaternion.identity, transform);
            }
            
            for(int j = 0; j < 5; j++){
                float x = Random.value * 6f;
                float y = Random.value * 3f;
    
                Vector3 posicao = new Vector3(-90 + y, 0.2f, -4f + x);
                Instantiate(Zombie, posicao, Quaternion.identity, transform);
            }
            
            for(int j = 0; j < 5; j++){
                float x = Random.value * 6f;
                float y = Random.value * 3f;
    
                Vector3 posicao = new Vector3(-4f + x, 0.2f, -90 + y);
                Instantiate(Zombie, posicao, Quaternion.identity, transform);
            }

            for(int j = 0; j < 5; j++) {
                float x = Random.value * 6f;
                float y = Random.value * 3f;
    
                Vector3 posicao = new Vector3(-4f + x, 0.2f, 90 + y);
                Instantiate(Zombie, posicao, Quaternion.identity, transform);
            }
        }
    }

    void Update()
    {
        if (transform.childCount <= 0 && gm.gameState == GameManager.GameState.GAME)
        {
            gm.ChangeState(GameManager.GameState.ENDGAME);
        }
    }
}
