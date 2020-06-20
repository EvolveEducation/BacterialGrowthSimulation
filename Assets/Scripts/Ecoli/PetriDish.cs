using System;
using System.Collections.Generic;
using System.Threading;
using Cell;

public class PetriDish
{
    List<Cell> cellList = new List<Cell>();
	bool[,] cellloc = new bool[600, 600];
    public Timer clock;
    public int growTime = 100;



    public PetriDish(int numStart, int time)
    {
        this.growTime = time;
        i = 0;
        while (i <= numStart)
        {
            Random rand = new Random();
            int xloc = rand.Next(0, 600);
            Random random = new Random();
            int yloc = random.Next(0, 600);
            if (cellloc[xloc, yloc] == true)
            {
                newLoc(cellloc);
                //if the randomly generted location is occupied, recursively call the function again to generate a new location
                //this prevents placing a cell in an already occupied location
            }

            if (cellloc[xloc, yloc] == false)
            {
                cellloc[xloc, yloc] = true;
                //the randomly generated location does not already contain a cell
                //change the values that correspond to the x and y location keys so they contain a cell
            }
            //return [xloc, yloc], which is the x and y coordinates of the newly generated cell
            int[,] newcell = new int[xloc, yloc];
            this.cellList.Add(Cell(newcell));
            i++;
        }
    }


    public void TimeToGrow()
    {
        i = 0;
        while (i <= this.growTime)
        {
            Thread.Sleep(20);
            foreach (Cell next in this.cellList)
            {
                if (next.IsActive())
                {
                    this.cellList.Add(Cell(next.grow()));
                }
            }
        }
        Console.WriteLine(cellList);
    }
}
