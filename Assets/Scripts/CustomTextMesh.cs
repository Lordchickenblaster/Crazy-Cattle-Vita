using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CustomTextMesh : MonoBehaviour
{
    public string text = "HELLO";
    public Font font;
    public int fontSize = 64;
    public Color color = Color.white;
    public float radius = 10f;
    public float animationSpeed = 1f;
    public Material toUse;

    private Mesh mesh;
    private Vector3[] baseVertices;
    private Vector3[] animatedVertices;
    private float[] charOffsets;
    private int charsPerQuad = 4;

    void Start()
    {
        font.RequestCharactersInTexture(text, fontSize);
        mesh = new Mesh();

        int charCount = text.Length;
        Vector3[] vertices = new Vector3[charCount * charsPerQuad];
        Vector2[] uvs = new Vector2[charCount * charsPerQuad];
        int[] triangles = new int[charCount * 6];
        Color[] colors = new Color[vertices.Length];
        charOffsets = new float[charCount];

        float x = 0;

        for (int i = 0; i < charCount; i++)
        {
            CharacterInfo ch;
            if (!font.GetCharacterInfo(text[i], out ch, fontSize)) continue;

            int v = i * charsPerQuad;
            int t = i * 6;

            Vector3 bl = new Vector3(x + ch.minX, ch.minY);
            Vector3 tl = new Vector3(x + ch.minX, ch.maxY);
            Vector3 tr = new Vector3(x + ch.maxX, ch.maxY);
            Vector3 br = new Vector3(x + ch.maxX, ch.minY);

            vertices[v + 0] = bl;
            vertices[v + 1] = tl;
            vertices[v + 2] = tr;
            vertices[v + 3] = br;

            uvs[v + 0] = ch.uvBottomLeft;
            uvs[v + 1] = ch.uvTopLeft;
            uvs[v + 2] = ch.uvTopRight;
            uvs[v + 3] = ch.uvBottomRight;

            for (int j = 0; j < 4; j++)
                colors[v + j] = color;

            triangles[t + 0] = v + 0;
            triangles[t + 1] = v + 1;
            triangles[t + 2] = v + 2;
            triangles[t + 3] = v + 2;
            triangles[t + 4] = v + 3;
            triangles[t + 5] = v + 0;

            charOffsets[i] = ((charCount - i) / (float)charCount) * Mathf.PI * 2f; // Delay each char
            x += ch.advance;
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
        if (toUse != null)
        {
            //toUse.mainTexture = font.material.mainTexture;
            GetComponent<MeshRenderer>().material = toUse;
        }
        else
        {
            GetComponent<MeshRenderer>().material = font.material;
        }

        baseVertices = mesh.vertices;
        animatedVertices = new Vector3[baseVertices.Length];
    }

    void Update()
    {
        float time = Time.time * animationSpeed;

        for (int i = 0; i < text.Length; i++)
        {
            float angle = time + charOffsets[i];
            float xOffset = Mathf.Cos(angle) * radius;
            float yOffset = Mathf.Sin(angle) * radius;

            if (yOffset < -radius * 0.25f) yOffset = -radius * 0.25f;

            for (int j = 0; j < charsPerQuad; j++)
            {
                int index = i * charsPerQuad + j;
                animatedVertices[index] = baseVertices[index] + new Vector3(xOffset, yOffset, 0f);
            }
        }

        mesh.vertices = animatedVertices;
        mesh.RecalculateBounds();
    }
}
