using Random = System.Random;
using System.Collections.Generic;

namespace Bacteria
{
    public class Cell
    {
        //Variables//
        public int X { get; }
        public int Y { get; }
        public List<int[]> AvailableSpace { get; }


        /*
         * Constructor for the cell class.
         * @param x,y relate to the location of the cell on the petridish
         */
        public Cell(int x, int y)
        {
            this.X = x;
            this.Y = y;
            AvailableSpace = new List<int[]>();
            FindAvailableSpace();
        }


        //Public Methods//
        /*
         * Creates a new neighbouring cell and sends that info back to the colony.
         * If no cell can be made the cell is listed as inactive and nothing is sent back.
         * @param available a 2d array of locations that are available for growth
         * @return Task<Cell> returns the new Cell 
         */
        public Cell Grow()
        {
            int randomLocation = Petridish.Instance.RNG.Next(0, AvailableSpace.Count);
            int[] location = AvailableSpace[randomLocation];
            Petridish.Instance.CellLocations[location[0], location[1]] = true;
            AvailableSpace.RemoveAt(randomLocation);
            return new Cell(location[0], location[1]);
        }

        //Private Methods//
        /*
         * Finds unactive spaces around the cell that it may grow into. Then
         * populates the neighbours hashset with them.
         */
        private void FindAvailableSpace()
        {
            for (int i = X - 1; i < X + 3; i++)
            {
                for (int j = Y - 1; j < Y + 3; j++)
                {
                    if (i < Petridish.Instance.DishDiameter && j < Petridish.Instance.DishDiameter)
                    {
                        if (!Petridish.Instance.CellLocations[i, j] && Petridish.Instance.InCircle(i, j))
                        {
                            AvailableSpace.Add(new int[] { i, j });
                        }
                    }
                }
            }
        }
    }
}
