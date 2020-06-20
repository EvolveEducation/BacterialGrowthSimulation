using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;

namespace Ecoli2
{

    //1. center cell
    //2. center cell becomes "colony"
    //3. cells grow around colony
    //3b. update cell knowledge of open spaces (use reference type, not value type)
    //4. merge new cells to colony (append arrays)
    //5. merge touching colonies
    //6. cells grow around colonies (loop steps 3-5)
    //each center cell should be represented by a thread, multiple threading
    //thread safety to prevent error (ex: two colonies growing a cell in the same position)
    //multiple threading will allow for logarithmic growth

    //inactive cells should be kept apart from the active cells. that way, we don't have to keep checking if an inactive cell is active
    //once an inactive cell is determined to be inactive (surrounded by cells) it is no longer checked
    //if inactive, add to colony list. If active, grow

    class cell
    {
        private int[,] location;
        //private bool[,] neighbors = new bool[3, 3];
        private bool active;



        public cell(int[,] loc)
        {
            location = loc;
        }
        //neighbors is being used to track what is next to a cell
        //when all neighbors values become true, cell will go from active to inactive, no longer needs to be checked
        public bool[,] getNeighbors
        {
            get
            {
                return this.neighbors;
            }
        }
        public int[] getLocation
        {
            get
            {
                return this.location;
            }
        }
        public void setNeighbors(int[,] loc, bool[,] cellloc)
        {
            int x;
            int y;
            x = loc[0];
            y = loc[1];

            DoubleLinkedList neighbors = new DoubleLinkedList();
            neighbors.InsertLast(bool there, int [,] loc );

            this.neighbors[0, 0] = cellloc[x - 1, y + 1];
            this.neighbors[0, 1] = cellloc[x, y + 1];
            this.neighbors[0, 2] = cellloc[x + 1, y + 1];
            this.neighbors[1, 0] = cellloc[x - 1, y];
            this.neighbors[1, 1] = cellloc[x, y];//center
            this.neighbors[1, 2] = cellloc[x + 1, y];
            this.neighbors[2, 0] = cellloc[x - 1, y - 1];
            this.neighbors[2, 1] = cellloc[x, y - 1];
            this.neighbors[2, 2] = cellloc[x + 1, y - 1];
        }

        //true=active
        //false=inactive
        public bool IsActive
        {
            get
            {
                return active;
            }
            set
            {
                bool occupied = false;
                int numOcc = 0;
                foreach (bool spot in neighbors)
                {
                    if (spot == true)
                    {
                        occupied = true;
                        numOcc = numOcc + 1;
                    }
                }
                if (numOcc == 8)
                {
                    this.active = false;
                }
                if (numOcc < 8)
                {
                    this.active = true;
                }
            }
        }

        //test isActive before running this
        public void grow()
        {
            int[] loc = this.getLocation;
            foreach (bool spot in this.neighbors)
            {
                if (spot == false)
                {
                    index = this.neighbor.IndexOf(spot);
                    spot = true;
                    switch (index)
                    {
                        case [0, 0]:
                            cellloc[x - 1, y + 1] = true;
                            return cellloc[x - 1, y + 1];
                            break;
                        case [0, 1]:
                            cellloc[x, y + 1] = true;
                            return cellloc[x, y + 1];
                            break;
                        case [0, 2]:
                            cellloc[x + 1, y + 1] = true;
                            return cellloc[x + 1, y + 1];
                            break;
                        case [1, 0]:
                            cellloc[x - 1, y] = true;
                            return cellloc[x - 1, y];
                            break;
                        case [1, 1]:
                            cellloc[x, y] = true; //center
                            return cellloc[x, y];
                            break;
                        case [1, 2]:
                            cellloc[x + 1, y] = true;
                            return cellloc[x + 1, y];
                            break;
                        case [2, 0]:
                            cellloc[x - 1, y - 1] = true;
                            return cellloc[x - 1, y - 1];
                            break;
                        case [2, 1]:
                            cellloc[x, y - 1] = true;
                            return cellloc[x, y - 1];
                            break;
                        case [2, 2]:
                            cellloc[x + 1, y - 1] = true;
                            return cellloc[x + 1, y - 1];
                            break;
                    }
                    break;
                }
            }
        }

    }
}