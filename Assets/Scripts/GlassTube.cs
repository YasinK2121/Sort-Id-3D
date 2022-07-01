using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassTube : MonoBehaviour
{
    public List<GameObject> ballsInsideTheTube;
    public List<GameObject> ballPoints;
    public GameObject outBallPoint;
    public GameManager GameManager;
    public bool ıAmCorrect;
    private int Count;

    private void Start()
    {
        Count = 0;
        ıAmCorrect = false;
        GameManager = GameObject.FindWithTag("MainCamera").GetComponent<GameManager>();
    }

    public void BallColorControl(bool Check)
    {
        if (Check)
        {
            for (int a = 0; a < GameManager.Colors.Count; a++)
            {
                Count = 0;
                for (int b = 0; b < 4; b++)
                {
                    if (ballsInsideTheTube.Count == 4 && ballsInsideTheTube[b].GetComponent<Renderer>().material.color == GameManager.Colors[a].color)
                    {
                        Count++;
                    }
                }
                if (Count == 4)
                {
                    ıAmCorrect = true;
                    Check = false;
                }
            }
            Check = false;
        }
    }
}
