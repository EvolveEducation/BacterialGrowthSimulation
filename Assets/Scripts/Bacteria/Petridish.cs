using System;
using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Petridish : MonoBehaviour
{
//Variables
    public GameObject petriDish; 
    public Button start;
    public ToggleGroup mutagens;

    private int dishRadius;
    private bool[,] cellLocations;
    private int startingCells;
    private int growthRate;
    private List<Colony> colonyList;

//MonoBehaviour//
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        dishRadius = 300;
        cellLocations = new bool[dishRadius * 2, dishRadius * 2];
        startingCells = 10;
        growthRate = 100;
        colonyList = new List<Colony>();
    }

    void Start()
    {
        SpreadCells();
    }


//Getters & Setters//
    public static Petridish Instance
    {
        get; private set;
    }

    public int DishRadius
    {
        get { return dishRadius; }
    }

    public bool[,] CellLocations
    {
        get { return cellLocations; }
    }



//Public Methods//
    /*
     * Determines if a point lies in a circle.
     * @param x,y the point to check.
     * @return true if it lies in the circle.
     */
    public bool InCircle(int x, int y)
    {
        int R = dishRadius;
        int dx = Math.Abs(x - R); //radius is essentially the center
        int dy = Math.Abs(y - R); //radius is essentially the center

        if (dx + dy <= R) return true;
        if (dx > R) return false;
        if (dy > R) return false;
        if (Math.Pow(dx, 2) + Math.Pow(dy, 2) <= Math.Pow(R, 2))
            return true;
        else
            return false;
    }


//Private Methods//
    /*
     * Monitors the growth of the colonies. Writes each cycle to a .json file for later inspection.
     */
    private void Grow()
    {

    }

    /*
     * Sets the intial locations of the bacteria randomly.
     */
    private void SpreadCells()
    {
        Random rng = new Random();
        for (int i = 0; i < startingCells; i++)
        {
            int x = rng.Next(0, dishRadius * 2);
            int y = rng.Next(0, dishRadius * 2);
            if (cellLocations[x, y] == false && InCircle(x, y))
            {
                cellLocations[x, y] = true;
                Colony newColony = new Colony(new Cell(x, y));
                colonyList.Add(newColony);
            } else {
                i--;
            }
        }
    }        
}
