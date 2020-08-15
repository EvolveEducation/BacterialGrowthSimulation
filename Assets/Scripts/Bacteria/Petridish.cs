using System;
using System.Collections.Generic;
using System.Linq;
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
        public GameObject cellPrefab;
        public GameObject petriDish;
        public Button start;
        public ToggleGroup uvLight;
        public Slider numOfCells;
        public Text logs;
        public Toggle bleach;
        public Toggle colorBlindness;
        public Slider temperature;
        public Slider trialTime;
        // public GraphAnimation graph //not sure how to call charts and graphs stuff

        public static Petridish Instance { get; private set; }
        public int DishRadius { get; private set; }
        public bool[,] CellLocations { get; private set; }

        private int startingCells;
        private int simulationLength;
        private List<Colony> colonyList;
        private float dishNormailzer;
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
            dishNormailzer = (DishRadius * 2) / 10;
            
        }

        void Start()
        {
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
            int dy = Math.Abs(y - R);

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
            colonyList = new List<Colony>();
            cts = new CancellationTokenSource();
            simulationLength = (int)(50 * (trialTime.value / 12));

            if (uvLight.AnyTogglesOn())
            {
                foreach (Toggle t in uvLight.ActiveToggles())
                    switch (t.name)
                    {
                        case "High UV Light":
                            simulationLength /= 2;
                            break;
                        case "Mid UV Light":
                            simulationLength /= 4;
                            break;
                        case "Low UV Light":
                            simulationLength /= 8;
                            break;
                    }
            }
            Debug.Log(simulationLength);

            int ij = (int) Math.Abs(temperature.value - 70);

            Debug.Log(ij);

            if (bleach.isOn)
            {
                simulationLength /= 2;
            }

            Debug.Log(simulationLength);

            foreach (Transform child in petriDish.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            startingCells = (int) numOfCells.value;
            logs.text = "";
            logs.text += "Simulation starting...\n";


            SpreadCells();
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
                    logs.text += "...Simulation Cancelled.\n";
                }
            }
            

            logs.text += "...Simulation Complete.\n";

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
         * Grows each colony asyncroniously concurrently. The report function will allow the cells to grow
         * on the plate dynamically and the cancelationtoken allows the user to stop the growth entirely.
         * @param progress used to monitor the progress of the async task (like a callback)
         * @param cancellationToken used to cancel the growth if desired (like a callback)
         * @return Task required for non-event type async functions (means nothing)
         */
        private async Task Grow(IProgress<List<Cell>> progress, CancellationToken cancellationToken)
        {
            var tasks = colonyList.Select(async colony =>
            {
                List<Cell> newCells = await colony.GrowParallelAsync();
                cancellationToken.ThrowIfCancellationRequested();
                progress.Report(newCells);
            });
            await Task.WhenAll(tasks);
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

                    cellPrefab.transform.position = new Vector3(x / dishNormailzer, 0, y / dishNormailzer);
                    Instantiate(cellPrefab, petriDish.transform);
                    
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
                cellPrefab.transform.position = new Vector3(cell.X / dishNormailzer, 0, cell.Y / dishNormailzer);
                Instantiate(cellPrefab, petriDish.transform);
            }

            //log graph data somehow
        }
    }
}
