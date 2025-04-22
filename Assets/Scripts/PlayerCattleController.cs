using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCattleController : MonoBehaviour
{
    public Cattle cat;
    public PostGameHandler PGH;
    public Transform cam;
    public GameObject lose;
    public GameObject win;
    bool won = false;
    private CattleManager CM;
    public AudioSource winas;

    void Start()
    {
        CM = GameObject.Find("CattleManager").GetComponent<CattleManager>();
        cat.OnDeath += OnDeath;
    }

    void FixedUpdate()
    {
        if (won) return;
        cat.horizontal = UnityEngine.N3DS.GamePad.CirclePad.x;
        cat.vertical = -UnityEngine.N3DS.GamePad.CirclePad.y;
        /*cat.horizontal = Input.GetAxis("Horizontal");
        cat.vertical = -Input.GetAxis("Vertical");*/
    }

    void setTorque()
    {
        UnityEngine.N3DS.Keyboard.Show();
        if (UnityEngine.N3DS.Keyboard.GetResult() == (int)N3dsKeyboardResult.Okay)
        {
            string te = UnityEngine.N3DS.Keyboard.GetText();
            float t = float.Parse(te);
            cat.turnTorque = t;
            cat.maximumTorque = t;
        }
    }

    void Update()
    {
        Shader.SetGlobalVector("_Global_PlayerWorldPosition", cat.cattle.position);
        if (won) return;
        if (Input.GetKeyDown(KeyCode.R))
        {
            cat.dothesheep();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            cat.bahh.Stop();
        }

        /*if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            setTorque();
        }*/

        if (CM.ccam == 0)
        {
            win.SetActive(true);
            won = true;
            winas.Play();
            PGH.Win();
        }
    }

    void OnDeath()
    {
        cam.SetParent(null);
        PGH.Lose();
        lose.SetActive(true);
        cat.Explode();
    }
}
