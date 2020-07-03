using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Bacteria
{
    public class Petridish : MonoBehaviour
    {
        //Variables
        public GameObject petriDish;
        public Button start;
        public ToggleGroup mutagens;
        public Slider numOfCells;
        public Text logs;
        // public GraphAnimation graph //not sure how to call charts and graphs stuff
        
        public static Petridish Instance { get; private set; }
        public int DishRadius { get; private set; }
        public bool[,] CellLocations { get; private set; }

        private int startingCells;
        private int simulationLength;
        private int growthRate;
        private List<Colony> colonyList;
        private CancellationTokenSource cts;

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
            simulationLength = 100;
            growthRate = 2;
            colonyList = new List<Colony>();
            cts = new CancellationTokenSource();
        }

        void Start()
        {
            SpreadCells();
            start.onClick.AddListener(SimulationStart);
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
        * Monitors the growth of the colonies and based on the IProgress: generates new cells, 
        * logs data, and populates the graph. Each trial is also saved to a json file.
        */
        private async void SimulationStart()
        {
            logs.text += "Simulation starting...\n";
            //logs.text += mutagens.name + " is being used. Typical growth rate is " + mutagen.growthrate
            Progress<List<Cell>> progress = new Progress<List<Cell>>();
            progress.ProgressChanged += ReportProgress;

            for (int i = 0; i < simulationLength; i++)
            {
                try
                {
                    await Grow(progress, cts.Token);
                }
                catch (OperationCanceledException)
                {
                    logs.text += "...Simulation Cancelled.";
                }
                await Task.Delay(growthRate);
            }
            

            logs.text += "...Simulation Complete.";

            //log data into json here.
        }

        /*
         * Triggers a cancelaiton token to stop the simulation.
         */
        private void CancelSimulation()
        {
            cts.Cancel();
        }


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
                    
                    //create a new instance of a prefab cell here as well
                    
                    colonyList.Add(newColony);
                }
                else
                {
                    i--;
                }
            }
        }

        /*
         * Called whenever the progress has been changed; whenever a colony grows.
         * Updates the graph, log, and petridish itself dynamically on screen. 
         * @param sender the method that called it (unused but needed)
         * @param cells is a list a of new cells on the petridish 
         */
        private void ReportProgress(object sender, List<Cell> cells)
        {
            logs.text += cells.Count + " new cells.\n";
            
            foreach (Cell cell in cells)
            {
                // Change this to a prefab clone for a cell 
                GameObject c = new GameObject();
                c.transform.parent = petriDish.transform;
                c.transform.localPosition = new Vector2(cell.X, cell.Y);
            }

            //log graph data somehow
        }
    }
}
