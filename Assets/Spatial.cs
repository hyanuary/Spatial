using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Circle
{
    public Vector2 m_pos;
    public float m_radius;
}

public class Spatial : MonoBehaviour {
    Texture2D tex;
    List<Circle> circles = new List<Circle>();
    public int count = 10;

	void Start () {
        Random.InitState(0);
        tex = GetComponent<SpriteRenderer>().sprite.texture;
        for(int i=0;i<count;++i)
        {
            Circle c = new Circle();
            c.m_pos = new Vector2(Random.Range(0, 1023), Random.Range(0, 1023));
            c.m_radius = Random.Range(1, 49);
            circles.Add(c);
        }
        Color32[] colours = new Color32[1024*1024];
        float t = Time.realtimeSinceStartup;
        for (int y = 0; y < 1024; ++y)
        {
            for (int x = 0; x < 1024; ++x)
            {
                float value = 0;
                for (int i = 0; i < circles.Count; ++i)
                {
                    float d = (new Vector2((float)x, (float)y) - circles[i].m_pos).magnitude;
                    if (d < circles[i].m_radius)
                    {
                        value = value + (1.0f - d / circles[i].m_radius);
                       value =  Mathf.Clamp(value, 0.0f, 1.0f);
                    }

                }
                colours[x + y * 1024].r = (byte)(value * 255);
                colours[x + y * 1024].g = (byte)(value * 255);
                colours[x + y * 1024].b = (byte)(value * 255);
                colours[x + y * 1024].a = 255;
            }
        }
        Debug.Log("Time taken = " + (Time.realtimeSinceStartup - t));
        tex.SetPixels32(colours);
        tex.Apply();
    }

    void Update () {
    }
}
