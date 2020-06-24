using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Colony
{
//Variables//
    private Cell origin;
    private LinkedList<Cell> activeCells;

    /*
     * Constrcutor for the colony. Sets the origin cell and creates our list of active cells.
     * @param origin the first cell in our colony
     */
    public Colony(Cell origin)
    {
        this.origin = origin;
        activeCells = new LinkedList<Cell>();
        activeCells.AddFirst(this.origin);
    }


//Public Methods//
    /*
     * Each time cycle this is called. Emulates the growth of the colony.
     */
    public Grow()
    {
        foreach (Cell cell in activeCells)
        {
            if (cell.AvailableSpace.Count < 0)
            {
                cell.Active = false;
            } else {
                StartCoroutine(cell.Grow(cell.Neighbours,
                (cell) => {
                    activeCells.AddLast(cell);
                }));
            }
        }
    }
}
