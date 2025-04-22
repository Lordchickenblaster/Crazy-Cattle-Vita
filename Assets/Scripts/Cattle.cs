using UnityEngine;
using System;

public class Cattle : MonoBehaviour
{
    public int CattleNumber;
    public float doamovespeed = 100f;
    public float turnAngle = 15f;         // Visual lean angle when turning
    public float leanSmoothing = 5f;
    public float turnTorque = 10f;        // Actual physical torque for turning
    public float maximumTorque = 50f;     // Maximum allowed torque
    public float slowdownMultiplier = 0.5f; // NEW: Multiplier when changing direction or braking
    public AudioSource bahh;

    public Transform cattle;              // Visual model
    public Transform orientation;         // Orientation helper
    public Rigidbody rb;
    public float horizontal;
    public float vertical;
    public event Action OnDeath;

    private float currentLean = 0f;

    public void Explode()
    {
        GameObject expl = Instantiate(Resources.Load<GameObject>("DeathEffect"));

        expl.transform.position = cattle.position;
        expl.transform.rotation = Quaternion.Euler(0f,cattle.rotation.eulerAngles.y, 0f);
        Destroy(expl, 1.3f);
        Destroy(gameObject);
    }

    public void dothesheep()
    {
        bahh.pitch = UnityEngine.Random.Range(0.7f,1.3f);
        bahh.Play();
    }

    public void InvokeDeath()
    {
        if (OnDeath != null)
            {
                OnDeath.Invoke();
            }
    }

    Vector3 GetForward()
    {
        Vector3 f = -cattle.up;
        f.y = 0f;
        return f;
    }

    void FixedUpdate()
    {
        // === Movement ===
        Vector3 forward = GetForward();
        float currentSpeed = Vector3.Dot(rb.velocity, forward);
        float adjustedSpeed = vertical;

        // If input is opposite to current movement direction, apply slowdown
        if (Mathf.Sign(currentSpeed) != Mathf.Sign(vertical) && Mathf.Abs(currentSpeed) > 0.1f)
        {
            adjustedSpeed *= slowdownMultiplier;
        }

        float engineForce = adjustedSpeed * doamovespeed;
        rb.AddForce(forward * engineForce, ForceMode.Acceleration);

        // === Physical Turning ===
        float torqueY = (horizontal * vertical) * turnTorque;
        torqueY = Mathf.Clamp(torqueY, -maximumTorque, maximumTorque);
        rb.AddTorque(Vector3.up * -torqueY, ForceMode.Acceleration);

        // === Visual Leaning ===
        float targetLean = 0f;
        if (Mathf.Abs(horizontal) > 0.1f && Mathf.Abs(vertical) > 0.1f)
        {
            targetLean = -horizontal * turnAngle;
        }

        currentLean = Mathf.Lerp(currentLean, targetLean, Time.fixedDeltaTime * leanSmoothing);

        Vector3 euler = cattle.localEulerAngles;
        euler.y = currentLean;
        cattle.localEulerAngles = euler;
    }
}