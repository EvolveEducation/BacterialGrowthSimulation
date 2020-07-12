using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrialList : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string json = File.ReadAllText("Assets/Scripts/UI/dataAA.json");
        TrialData td = JsonUtility.FromJson<TrialData>(json);
        StartCoroutine(UpdateTrials(td));
    }

    IEnumerator UpdateTrials(TrialData td)
    {
        UIRepeat uiRepeat = GetComponent<UIRepeat>();
        uiRepeat.SetNumRepetitions(td.trials.Count);

        while (transform.childCount != td.trials.Count)
        {
            yield return new WaitForFixedUpdate();
        }


        for (int i = 0; i < td.trials.Count; i++)
        {
            Transform child = transform.GetChild(i);
            Trial trial = td.trials[i];
            RawImage rawImage = child.GetComponentInChildren<RawImage>();
            rawImage.texture = generateTexture(trial);
        }
    }

    Texture2D generateTexture(Trial trial)
    {
        Texture2D texture = new Texture2D(300, 300);
        foreach (Colony colony in trial.colonies)
        {
            texture.DrawCircle(Color.red, colony.x, colony.y, colony.radius);
        }

        return texture;
    }
}


public static class Tex2DExtension
{
    public static Texture2D DrawCircle(this Texture2D tex, Color col, int cx, int cy, int r)
    {
        int x, y, px, nx, py, ny, d;

        for (x = 0; x <= r; x++)
        {
            d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
            for (y = 0; y <= d; y++)
            {
                px = cx + x;
                nx = cx - x;
                py = cy + y;
                ny = cy - y;

                tex.SetPixel(px, py, col);
                tex.SetPixel(nx, py, col);

                tex.SetPixel(px, ny, col);
                tex.SetPixel(nx, ny, col);

            }
        }
        return tex;
    }
}