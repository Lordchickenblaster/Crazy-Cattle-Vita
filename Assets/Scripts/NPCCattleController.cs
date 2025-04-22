using UnityEngine;
using System.Collections;

public class NPCCattleController : MonoBehaviour
{
    public Cattle cat;
    private CattleManager CM;
    public TextMesh m1;
    public TextMesh m2;

    public float deathCooldown = 0.5f; // ⏱ Time before sheep can die
    public float minimumRange = 0.2f;  // 🔁 Minimum movement strength

    private bool hasExploded = false;
    private bool canDie = false;

    void Start()
    {
        CM = GameObject.Find("CattleManager").GetComponent<CattleManager>();

        // 🔁 Reroll horizontal/vertical if too low
        do
        {
            cat.horizontal = Random.Range(-1f, 1f);
        } while (Mathf.Abs(cat.horizontal) < minimumRange);

        do
        {
            cat.vertical = Random.Range(-1f, 1f);
        } while (Mathf.Abs(cat.vertical) < minimumRange);

        m1.text = cat.CattleNumber.ToString();
        m2.text = cat.CattleNumber.ToString();
        cat.OnDeath += OnDeath;

        cat.rb.isKinematic = true;

        StartCoroutine(ExplosionTimerLoop());
        StartCoroutine(DeathCooldownTimer());
    }

    void OnDeath()
    {
        if (!canDie || hasExploded) return;
        CM.ccam--;
        CM.lsheepdied = cat.CattleNumber.ToString();
        hasExploded = true;
        CM.UpdateSheep();
        cat.Explode();
    }

    IEnumerator ExplosionTimerLoop()
    {
        while (!hasExploded)
        {
            float waitTime = Random.Range(3f, 15f);
            yield return new WaitForSeconds(waitTime);

            if (!hasExploded)
            {
                cat.dothesheep();
            }
        }
    }

    IEnumerator DeathCooldownTimer()
    {
        yield return new WaitForSeconds(deathCooldown);
        canDie = true;
        cat.rb.isKinematic = false;
    }
}