using System;
using System.Collections.Generic;

namespace Bacteria
{
    public class Colony
    {
        //Variables//
        private readonly Cell origin;
        private readonly List<Cell> activeCells;
        private List<Cell> cells;

        /*
         * Constrcutor for the colony. Sets the origin cell and creates our list of active cells.
         * @param origin the first cell in our colony
         */
        public Colony(Cell origin)
        {
            this.origin = origin;
            activeCells = new List<Cell>
            {
                this.origin
            };
            cells = new List<Cell>
            {
                this.origin
            };
        }

        //Getters
        public List<Cell> GetCells()
        {
            return cells;
        }

        public Cell GetOrigin()
        {
            return origin;
        }

        //Public Methods//
        /*
         * Each time cycle this is called. Emulates the growth of the colony.
         * @return List<Cell> returns a list of newly grown cells
         */
        public Tuple<List<Cell>, int> GrowColony(double death)
        {
            List<Cell> cellsToGrow = new List<Cell>();
            int dead = 0;

            foreach (Cell cell in activeCells.ToArray())
            {
                if (Petridish.Instance.RNG.NextDouble() < death)
                {
                    activeCells.Remove(cell);
                    dead++;
                }
                else
                {
                    if (cell.AvailableSpace.Count < 1)
                    {
                        activeCells.Remove(cell);
                        dead++;
                    }
                    else
                    {
                        cellsToGrow.Add(cell);
                    }
                }
            }

            List<Cell> newCells = new List<Cell>();

            foreach (Cell c in cellsToGrow)
            {
                newCells.Add(c.Grow());
            }

            cells.AddRange(newCells);
            activeCells.AddRange(newCells);
            return new Tuple<List<Cell>, int>(newCells, dead);
        }
    }
}
