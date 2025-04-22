using UnityEngine;

public class CattleDeathCheck : MonoBehaviour
{
    public Cattle owner;

    void OnTriggerEnter(Collider Col)
    {
        if (Col.gameObject.tag == "Death")
        {
            owner.InvokeDeath();
        }
    }

    void OnTriggerStay(Collider Col)
    {
        if (Col.gameObject.tag == "Death")
        {
            owner.InvokeDeath();
        }
    }
}
