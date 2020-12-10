using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public static PerlinNoise noise;
    public int width = 256;
    public int height = 256;

    public float offsetX = 100f;
    public float offsetY = 100f;
    public float scale = 20f;
    public int octaves = 2;
    public float persistance = 10f;
    public float lacunarity = 2f;
    Renderer _renderer;

    void Awake(){
        noise = this;
    }

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        offsetX = Random.Range(0f, 99999f);
        offsetY = Random.Range(0f, 99999f);
    }

    Texture2D GenerateTexture(){
        Texture2D texture = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    Color  CalculateColor(int x, int y){
        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;
        float sample = 0;
        for (int i = 0; i < octaves; i++)
        {
            float xCoord = (float)x / width * scale * frequency;
            float yCoord = (float)y / height * scale * frequency;
            sample = Mathf.PerlinNoise(xCoord, yCoord) * 2 -1;
            noiseHeight += sample * amplitude;
            amplitude *= persistance;
            frequency *= lacunarity;
        }

        if(noiseHeight > 0.9f || GameMap.GM.mapMask[x, y] == 1)
            noiseHeight = 0;
        
        return new Color(noiseHeight, noiseHeight, noiseHeight);
    }

    // Update is called once per frame
    public void UpdatePerlin()
    {
        _renderer.material.mainTexture = GenerateTexture();
    }

    public float[,] GeneratePerlin(){
        float[,] perlin = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                perlin[x, y] = CalculateSample(x, y);
            }
        }
        return perlin;
    }

     float  CalculateSample(int x, int y){
        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;
        float sample = 0;
        for (int i = 0; i < octaves; i++)
        {
            float xCoord = (float)x / width * scale * frequency;
            float yCoord = (float)y / height * scale * frequency;
            sample = Mathf.PerlinNoise(xCoord, yCoord) * 2 -1;
            noiseHeight += sample * amplitude;
            amplitude *= persistance;
            frequency *= lacunarity;
        }

        if(GameMap.GM.mapMask[x, y] > 0)
            noiseHeight = 0;
        
        return noiseHeight;
    }
}
