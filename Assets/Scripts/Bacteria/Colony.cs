using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colony
{
//Variables//
    private Cell origin;
    private List<Cell> activeCells;

    /*
     * Constrcutor for the colony. Sets the origin cell and creates our list of active cells.
     * @param origin the first cell in our colony
     */
    public Colony(Cell origin)
    {
        this.origin = origin;
        activeCells = new List<Cell>();
        activeCells.Add(this.origin);
    }


//Public Methods//
    /*
     * Each time cycle this is called. Emulates the growth of the colony.
     */
    public void Grow()
    {
        foreach (Cell cell in activeCells)
        {
            if (cell.AvailableSpace.Count < 0)
            {
                activeCells.Remove(cell);
            } else {
                StartCoroutine(cell.Grow((c) => {
                    activeCells.Add(c);
                }));
            }
        }
    }
}
