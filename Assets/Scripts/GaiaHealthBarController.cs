using UnityEngine;
using UnityEngine.UI;

public class GaiaHealthBarController : MonoBehaviour {

    GameManager gm;
    
    public int healthMax = 10;

    public Image healthBar;

    public int Life
    {
        get
        {
            return gm.gaia_lifes;
        }
        set
        {
            gm.gaia_lifes = Mathf.Clamp(value, 0, healthMax);
        }
    }

    void Start() {
        gm = GameManager.GetInstance();
    }

    void Update() {
        healthBar.fillAmount = gm.gaia_lifes*1.0f / healthMax;
    }
}