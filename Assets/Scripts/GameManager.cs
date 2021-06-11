using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager _instance;
    public static GameManager GetInstance()
    {
        if(_instance == null)
        {
            _instance = new GameManager();
        }

        return _instance;
    }

    public delegate void ChangeStateDelegate();
    public static ChangeStateDelegate changeStateDelegate;

    public void ChangeState(GameState nextState)
    {
        if (nextState == GameState.GAME) Reset();
        gameState = nextState;
        changeStateDelegate();
    }

    public enum GameState { MENU, GAME, PAUSE, ENDGAME };

    public GameState gameState { get; private set; }
    public int lifes;
    public int points;
    
    public int gaia_lifes;

    private GameManager()
    {
        gaia_lifes = 10;
        lifes = 300;
        points = 0;
        gameState = GameState.MENU;
    }

    private void Reset()
    {
        gaia_lifes = 10;
        lifes = 300;
        points = 0;
    }
}
