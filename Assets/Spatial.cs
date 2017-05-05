using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Circle
{
    public Vector2 m_pos;
    public float m_radius;
}
public class Cell
{
    public List<Circle> circles3 = new List<Circle>();
}
public class Spatial : MonoBehaviour {
    const int size = 1;
    const int cellsize = 1024 / size;
    Cell[,] grid = new Cell[size, size];
    public int count = 10;
    

    void Start () {
        for (int y = 0; y < size; ++y)
            for (int x = 0; x < size; ++x)
                grid[x, y] = new Cell();

        Random.InitState(0);

        for(int i=0;i<count;++i)
        {
            Circle c = new Circle();
            c.m_pos = new Vector2(Random.Range(0, 1023), Random.Range(0, 1023));
            c.m_radius = Random.Range(1, 100);

            int top = (int)((c.m_pos.y - c.m_radius)/ cellsize);
            int bottom = (int)((c.m_pos.y + c.m_radius) / cellsize);
            int left = (int)((c.m_pos.x - c.m_radius) / cellsize);
            int right = (int)((c.m_pos.x + c.m_radius) / cellsize);

            if (top < 0)
                top = 0;
            if (left < 0)
                left = 0;
            if (bottom > size-1)
                bottom = size-1;
            if (right > size-1)
                right = size-1;

            for (int y = top; y <= bottom; ++y)
                for (int x = left; x <= right; ++x)
                    grid[x,y].circles3.Add(c);
        }

        int size1 = 1024;
        int size2 = 1024;

        Color32[] colours = new Color32[size1*size2];

        Texture2D tex = new Texture2D(size1, size2);
        tex = GetComponent<SpriteRenderer>().sprite.texture;
        float t = Time.realtimeSinceStartup;
        for (int y = 0; y < size1; ++y)
        {
            for (int x = 0; x < size2; ++x)
            {
                float value = 0;

                Cell cell = grid[(x / cellsize), (y / cellsize)];
                for (int i = 0; i < cell.circles3.Count; ++i)
                {
                    float d = (new Vector2((float)x, (float)y) - cell.circles3[i].m_pos).magnitude;
                    if (d < (cell.circles3[i].m_radius))
                    {
                        value = value + (1.0f - d / cell.circles3[i].m_radius);
                        value = Mathf.Clamp(value, 0.0f, 1.0f);
                    }

                }
                colours[x + y * size1].r = (byte)(value * 255);
                colours[x + y * size1].g = (byte)(value * 255);
                colours[x + y * size1].b = (byte)(value * 255);
                colours[x + y * size1].a = 255;
            }
        }
        Debug.Log("Time taken = " + (Time.realtimeSinceStartup - t));
        tex.filterMode = FilterMode.Point;
        tex.SetPixels32(colours);
        tex.Apply();
    }

    void Update () {
    }
}
