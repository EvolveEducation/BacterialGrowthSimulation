using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petridish : MonoBehaviour
{
//Variables
    private int startingCells;
    private int growthRate;
    private List<Colony> colonyList;
    private bool[,] colonyLocations;
    private int petridishSize;

    //MonoBehaviour
    void Start()
    {
        startingCells = 10;
        growthRate = 100;
        petridishSize = 600;
        colonyList = new List<Colony>();
        colonyLocations = new bool[petridishSize, petridishSize];
        SpreadCells();
    }


//Private Methods
    /*
     * Sets the intial locations of the bacteria randomly.
     */
    public void SpreadCells()
    {
        Random rand = new Random();
        for (int i = 0; i < startingCells; i++)
        {
            int x = rand.Next(0, petridishSize);
            int y = rand.Next(0, petridishSize);
            if (colonyLocations[x, y] == false)
            {
                colonyLocations[x, y] = true;
                Colony newColony = new Colony(new Cell(x, y));
                colonyList.Add(newColony);
            } else {
                i--;
            }
        }
    }

}
