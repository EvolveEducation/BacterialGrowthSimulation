using System;
using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

namespace Bacteria
{
    public class Petridish : MonoBehaviour
    {
        //Variables
        public GameObject petriDish;
        public Button start;
        public ToggleGroup mutagens;

        public static Petridish Instance { get; private set; }
        public int DishRadius { get; private set; }
        public bool[,] CellLocations { get; private set; }

        private int startingCells;
        private int growthRate;
        private List<Colony> colonyList;



        //MonoBehaviour//
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            DishRadius = 300;
            CellLocations = new bool[DishRadius * 2, DishRadius * 2];
            startingCells = 10;
            growthRate = 100;
            colonyList = new List<Colony>();
        }

        void Start()
        {
            SpreadCells();
        }


        //Public Methods//
        /*
         * Determines if a point lies in a circle.
         * @param x,y the point to check.
         * @return true if it lies in the circle.
         */
        public bool InCircle(int x, int y)
        {
            int R = DishRadius;
            int dx = Math.Abs(x - R); //radius is essentially the center
            int dy = Math.Abs(y - R); //radius is essentially the center

            if (dx + dy <= R) return true;
            if (dx > R) return false;
            if (dy > R) return false;
            if (Math.Pow(dx, 2) + Math.Pow(dy, 2) <= Math.Pow(R, 2))
                return true;
            else
                return false;
        }


        //Private Methods//
        /*
         * Grows each colony async in parallel. The report function will allow the cells to grow
         * on the plate dynamically and the cancelationtoken allows the user to stop the growth entirely.
         * @param progress used to monitor the progress of the async task (like a callback)
         * @param cancellationToken used to cancel the growth if desired (like a callback)
         * @return Task required for non-event type async functions (means nothing)
         */
        private async Task Grow(IProgress<List<Cell>> progress, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach<Colony>(colonyList, async (colony) =>
                {
                    List<Cell> newCells = await colony.GrowParallelAsync();
                    cancellationToken.ThrowIfCancellationRequested();
                    progress.Report(newCells);
                });
            });
        }

        /*
         * Sets the intial locations of the bacteria randomly.
         */
        private void SpreadCells()
        {
            Random rng = new Random();
            for (int i = 0; i < startingCells; i++)
            {
                int x = rng.Next(0, DishRadius * 2);
                int y = rng.Next(0, DishRadius * 2);
                if (CellLocations[x, y] == false && InCircle(x, y))
                {
                    CellLocations[x, y] = true;
                    Colony newColony = new Colony(new Cell(x, y));
                    colonyList.Add(newColony);
                }
                else
                {
                    i--;
                }
            }
        }
    }
}
