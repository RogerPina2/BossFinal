using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaiaController : MonoBehaviour
{
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
    }

    public void TakeDamage()
    {
        gm.gaia_lifes--;
        Reset();
    }

    public void Reset()
    {
        if (gm.gaia_lifes <= 0 && gm.gameState == GameManager.GameState.GAME)
        {       
            gm.ChangeState(GameManager.GameState.ENDGAME);
        }   
    }
}
