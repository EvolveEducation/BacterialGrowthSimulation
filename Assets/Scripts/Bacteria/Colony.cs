﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bacteria
{
    public class Colony
    {
        //Variables//
        private readonly Cell origin;
        private readonly List<Cell> activeCells;

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
        }


        //Public Methods//
        /*
         * Each time cycle this is called. Emulates the growth of the colony.
         * @return Task<List<Cell>> returns a list of newly grown cells
         */
        public async Task<List<Cell>> GrowParallelAsync()
        {
            List<Task<Cell>> cellsToGrow = new List<Task<Cell>>();
            
            //for loop
            foreach (Cell cell in activeCells.ToArray())
            {
                if (cell.AvailableSpace.Count < 1)
                {
                    activeCells.Remove(cell);
                }
                else
                {
                    cellsToGrow.Add(Task.Run(() => cell.Grow()));
                }
            }
            var cells = await Task.WhenAll(cellsToGrow);
            activeCells.AddRange(cells);

            return new List<Cell>(cells);
        }
    }
}
