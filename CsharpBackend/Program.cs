using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace EcoliPlay
{
    class Program
    {
        static void Main(string[] args)
        {
            //logic changed so it uses a 2D array rather than 2 arrays

            //IDictionary<int, string> xlocs = new Dictionary<int, string>();
            //IDictionary<int, string> ylocs = new Dictionary<int, string>();

            bool[,] cellloc = new bool[600, 600];
            

            //needs error checking to address for square vs circular grid
            static int[,] newLoc(bool[,] cellloc)
            {
                //This chunk is code I got rid of (was used to fill the 2 arrays)
                //    int n = 0;
                //    int m = 0;
                //    while (n < 600)
                //    {
                //        //for each loop repeat, add a dict entry where key is n (location) and value is cell presence (empty or false is starting value)
                //        n = n + 1;
                //    }
                //    while (m < 600)
                //    {
                //        //for each loop repeat, add a dict entry where key is n (location) and value is cell presence (empty or false is starting value)
                //        m = m + 1;
                //    }

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
                return newcell;
                //return int[xloc, yloc];

                //this function is for placing a brand new cell, isolated from others
                //determines if a cell is present in the locs arrays
                //if cell not present, returns an array with the determined location of the new cell
                //this logic needs to be changed to use dictionaries
            }



            void cell(int[,] location)
            {
                Console.WriteLine("cell drawn");
                //returns nothin
                //draws a cell
                //this has to work with unity functionality to make a circular cell appear
            }


            //needs error checking to address for circular vs square grid
            void grow(int[,] xandy, bool[,] celloc)
            {
                int x = xandy[0, 0];
                int y = xandy[0, 1];
                int tryXindex = 0;
                int tryYindex = 0;
                var rand = new Random();
                int xOption = rand.Next(0, 2);
                int yOption = rand.Next(0, 2);
                switch (xOption)
                {
                    case 0:
                        //option 1 for x, assign to x for the new (grown) cell
                        tryXindex = x-1;
                        break;
                    case 1:
                        //option 2 for x, assign to x for the new (grown) cell      
                        tryXindex = x;
                        break;
                    case 2:
                        //option 3 for x, assign to x for the new (grown) cell
                        tryXindex = x + 1;
                        break;
                    default:
                        //some error has ocurred, cases 0-2 should result from rand
                        break;
                }
                switch (yOption)
                {
                    case 0:
                        //option 1 for y, assign to y for the new (grown) cell
                        tryYindex = y - 1;
                        break;
                    case 1:
                        //option 2 for y, assign to y for the new (grown) cell      
                        tryYindex = y;
                        break;
                    case 2:
                        //option 3 for y, assign to y for the new (grown) cell
                        tryYindex = y + 1;
                        break;
                    default:
                        //some error has ocurred, cases 0-2 should result from rand
                        break;
                }
                //now that x and y location of grown cell have been generated, that location needs is tested to see if it already contains a cell

                int[,] newcell = new int[tryXindex, tryYindex];
                if (cellloc[tryXindex, tryYindex] == true)
                {
                    grow(xandy, cellloc);
                }
                if (cellloc[tryXindex, tryYindex] == false)
                {
                    cell(newcell);
                }

                //This function is for growing new cells from preexising cells
                //ifs were changed to switch for greater efficiency/readability
            }

            void cellList()
            {
                //this function checks the locs arrays for cells
                //constructs an array with the locations of the existing cells
                //was neeed nor an earlier version of the code, could probably be deleted now
            }

            //this will be changed to recieve info from gui
            int numStartCells;
            Console.Write("How many cells would you like the petri dish to start with?");
            numStartCells = int.Parse(Console.ReadLine());

            //e coli under normal conditions have a logarithmic growth rate, log phase until 300 minutes
            //this will be changed to recieve info from gui
            int time;
            Console.Write("Enter digit for hundred minutes' growth. (ex. 1=100 minutes, 2=200 minutes): ");
            time = int.Parse(Console.ReadLine());


        }
    }
}
