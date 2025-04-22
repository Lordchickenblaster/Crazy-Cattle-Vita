using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextPulseEffect : MonoBehaviour
{
    public float pulseSpeed = 2f;          // Speed of the pulse
    public float minAlpha = 0.3f;          // Minimum transparency
    public float maxAlpha = 1f;            // Maximum transparency

    private Text legacyText;

    void Start()
    {
        legacyText = GetComponent<Text>();
    }

    void Update()
    {
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);

        Color c = legacyText.color;
        c.a = alpha;
        legacyText.color = c;
    }
}

