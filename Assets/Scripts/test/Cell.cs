using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Cell
{
    //Variables//
    private bool active;
    private int x;
    private int y;
    private HashSet<Cell> neighbours;

    /*
     * Constructor for the cell class.
     * @param x,y relate to the location of the cell on the petridish
     */
    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
        active = true;
        neighbours = new HashSet<Cell>();
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

    public bool Active
    {
        get { return active; }
        set { active = value; }
    }

    public HashSet<Cell> AvailableSpace
    {
        get { return neighbours; }
    }


//Public Methods//
    /*
     * Creates a new neighbouring cell and sends that info back to the colony.
     * If no cell can be made the cell is listed as inactive and nothing is sent back.
     * @param available a 2d array of locations that are available for growth
     */
    public IEnumerator Grow(int[,] available, System.Action<Cell> callback)
    {
        RandomNumberGenerator rng = new RandomNumberGenerator.create();
        int x = rng.GetInt32(available.Length);
        int y = rng.GetInt32(available.Length);
        Cell newCell = new Cell(x, y);
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
        
    }
}
