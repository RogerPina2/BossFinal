using UnityEngine;
using UnityEngine.UI;

public class UI_Endgame : MonoBehaviour
{
    public Text message;
    public Text message2;

    GameManager gm;
    private void OnEnable()
    {
        gm = GameManager.GetInstance();

        if(gm.gaia_lifes > 0)
        {
            message.text = "Você Ganhou!!!";
        }
        else
        {
            message.text = "Você Perdeu!!!";
        }
        message2.text = $"Você fez {gm.points} pontos!";
    }

    public void Voltar()
    {
        gm.ChangeState(GameManager.GameState.MENU);
    }
}