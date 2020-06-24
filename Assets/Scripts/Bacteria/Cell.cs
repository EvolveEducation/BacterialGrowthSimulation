using System;
using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Cell
{
//Variables//
    private int x;
    private int y;
    private List<int[]> neighbours;

    /*
     * Constructor for the cell class.
     * @param x,y relate to the location of the cell on the petridish
     */
    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
        neighbours = new List<int[]>();
        FindAvailableSpace();
    }


    //Getters & Setters//
    public int X
    {
        get { return x; }
    }

    public int Y
    {
        get { return y; }
    }

    public List<int[]> AvailableSpace
    {
        get { return neighbours; }
    }


//Public Methods//
    /*
     * Creates a new neighbouring cell and sends that info back to the colony.
     * If no cell can be made the cell is listed as inactive and nothing is sent back.
     * @param available a 2d array of locations that are available for growth
     */
    public IEnumerator Grow(System.Action<Cell> callback)
    {
        Random rng = new Random();
        int randomLocation = rng.Next(0, neighbours.Count);
        int[] location = neighbours[randomLocation];
        Petridish.Instance.CellLocations[location[0], location[1]] = true;
        Cell newCell = new Cell(location[0], location[1]);
        neighbours.RemoveAt(randomLocation);
        yield return newCell;
        callback(newCell);
    }

//Private Methods//
    /*
     * Finds unactive spaces around the cell that it may grow into. Then
     * populates the neighbours hashset with them.
     */
    private void FindAvailableSpace()
    {
        for (int i = x-1; i < 3; i++)
        {
            for (int j = y-1; j < 3; j++)
            {
                if (!Petridish.Instance.CellLocations[i, j] && Petridish.Instance.InCircle(i, j))
                {
                    neighbours.Add(new int[] { i, j });
                }
            }
        }
    }
}
