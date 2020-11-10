using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TrialList : MonoBehaviour
{
    public GameObject trialPrefab;

    public void NewTrial(JSONPetriDish trialData, int trialID)
    {
        RawImage rawImage = trialPrefab.GetComponentInChildren<RawImage>();
        rawImage.texture = GenerateTexture(trialData.dishTimes[trialData.dishTimes.Count - 1]);
        trialPrefab.name = "" + trialID;
        trialPrefab.GetComponentInChildren<Text>().text = trialData.dishName + " : " + trialData.finalCellCount;
        Instantiate(trialPrefab, this.transform);
    }

    private Texture2D GenerateTexture(JSONPetriDishAtTimeN trial)
    {
        Texture2D texture = new Texture2D(300, 300);
        texture.LoadImage(File.ReadAllBytes(Application.dataPath + "/Materials & Models/baseTrial.png"));

        foreach (JSONColony colony in trial.colonies)
        {
            texture.DrawCircle(new Color(1, 0.8941177f, 0.7411765f), colony.x/2, colony.y/2, colony.radius/2);
        }


        double i, angle, x1, y1;
        double r = 149;
        for (double thick = 145; thick <= r; thick++)
        {
            for (i = 0; i < 360; i += 0.1)
            {
                angle = i;
                x1 = thick * Math.Cos(angle * Math.PI / 180);
                y1 = thick * Math.Sin(angle * Math.PI / 180);
                texture.SetPixel((int)(150 + x1), (int)(150 + y1), Color.black);
            }
        }

        for (int x = 0; x < 300; x++)
        {
            for (int y = 0; y < 300; y++)
            {
                int R = 149;
                int dx = Math.Abs(x - R);
                int dy = Math.Abs(y - R);
                if (dx > R || dy > R)
                {
                }
                if (!(Math.Pow(dx, 2) + Math.Pow(dy, 2) <= Math.Pow(R, 2)))
                {
                    texture.SetPixel(x, y, Color.clear);
                }
            }
        }
        texture.Apply();

        return texture;
    }
}


public static class Tex2DExtension
{
    public static Texture2D DrawCircle(this Texture2D tex, Color col, int cx, int cy, double r)
    {
        double i, angle, x1, y1;

        for (double fill = 0; fill <= r; fill+=0.1)
        {
            for (i = 0; i < 360; i += 0.1)
            {
                angle = i;
                x1 = fill * Math.Cos(angle * Math.PI / 180);
                y1 = fill * Math.Sin(angle * Math.PI / 180);
                tex.SetPixel((int)(cx + x1), (int)(cy + y1), col);
            }
        }       

        tex.Apply();
        return tex;
    }
}