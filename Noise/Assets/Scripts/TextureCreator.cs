using UnityEngine;


public class TextureCreator : MonoBehaviour
{
    public int resolution = 256;

    [Tooltip("If the frequency is doubled the pattern changes twice as fast.")]
    public float frequency = 1f;

    [Range(1, 3)]
    public int dimensions = 3;

    private Texture2D texture;

    private void OnEnable()
    {
        if(texture == null)
        {
            texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
            texture.name = "Procedural Texture";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Trilinear;
            texture.anisoLevel = 9; //Anisotropic Filtering
            GetComponent<MeshRenderer>().material.mainTexture = texture;
        }

        FillTexture();
    }

    private void Update()
    {
        if(transform.hasChanged)
        {
            transform.hasChanged = false;
            FillTexture();
        }
    }

    public void FillTexture()
    {
        if(texture.width != resolution)
        {
            texture.Resize(resolution, resolution);
        }

        /*
         * - Visualizing the coordinates of the textures space
         * 
         * Its centered on its own origin being (-0.5f, 0.5f), (0.5, 0.5), (-0.5f, 0.5f), (0.5, 0.5)
         */

        Vector3 point00 = transform.TransformPoint(new Vector3 (-0.5f, -0.5f));
        Vector3 point10 = transform.TransformPoint(new Vector3 (0.5f, -0.5f));
        Vector3 point01 = transform.TransformPoint(new Vector3 (-0.5f, 0.5f));
        Vector3 point11 = transform.TransformPoint(new Vector3 (0.5f, 0.5f));

        NoiseMethod method = Noise.valueMethods[dimensions - 1];
        float stepSize = 1f / resolution;
        Random.seed = 42;

        for (int y = 0; y < resolution; y++)
        {
            //Interpolate between the bottom left and top left corner based on y giving us a point on the left side

            Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
            Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);

            for (int x = 0; x < resolution; x++)
            {
                Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                //We can repeat this pattern by using modulus on stepSize and then to keep it visible do times 10
                texture.SetPixel(x, y, Color.white * method(point, frequency));
            }
        }

        texture.Apply();
    }
}
