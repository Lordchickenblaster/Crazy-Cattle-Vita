using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CattleManager : MonoBehaviour
{
    public int CattleAmount = 54;
    public int ccam;
    public Cattle playerCattle;
    public List<Cattle> NPCCattles;
    public string ncpcattlepath;
    public float surfaceArea = 1f;
    public Vector3 Origin;
    public string lsheepdied = "";
    public float startY = 0f;
    public Text Remain;
    public Text elim;

    public void UpdateSheep()
    {
        elim.enabled = true;
        elim.text = "Sheep No. "+lsheepdied+" has been eliminated.";
        Remain.text = ccam.ToString()+" remain.";
    }

    void Awake()
    {
        ccam = CattleAmount;
        Remain.text = ccam.ToString()+" remain.";
        NPCCattles = new List<Cattle>();
        for (int i = 0; i < CattleAmount; i++)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized*surfaceArea;
            randomDirection *= Random.Range(-1f, 1f); // add a little radial variation
            Vector3 pos = new Vector3(randomDirection.x, startY, randomDirection.y);

            GameObject expl = Instantiate(Resources.Load<GameObject>(ncpcattlepath));
            expl.transform.position = pos+Origin;
            expl.GetComponent<Cattle>().CattleNumber = i+1;
        }
    }
}
