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
        public GameObject graph;

        public static Petridish Instance { get; private set; }
        public int DishRadius { get; private set; }
        public int DishDiameter { get; private set; }
        public bool[,] CellLocations { get; private set; }
        public Random RNG { get; private set; }

        private List<Colony> colonyList;
        private float dishNormailzer;

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
            DishDiameter = DishRadius * 2;
            RNG = new Random();
            CellLocations = new bool[DishDiameter, DishDiameter];
            dishNormailzer = (DishDiameter) / 10;
            
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
            float adjustedX = temperature.value - 1.5f;
            double temperatureEstimate = (2.783e-16 * Math.Pow(adjustedX, 11)) - (4.0496e-9 * Math.Pow(adjustedX, 7)) + (0.00000517429 * Math.Pow(adjustedX, 5)) + (0.00150295 * Math.Pow(adjustedX, 3));
            double timePerDivision = 70 / (temperatureEstimate / 100);
            int simulationLength = (int) Math.Ceiling((trialTime.value * 60) / timePerDivision);

            if (uvLight.AnyTogglesOn())
            {
                foreach (Toggle t in uvLight.ActiveToggles())
                    switch (t.name)
                    {
                        case "High UV Light":
                            simulationLength /= 8;
                            break;
                        case "Mid UV Light":
                            simulationLength /= 4;
                            break;
                        case "Low UV Light":
                            simulationLength /= 2;
                            break;
                    }
            }

            if (bleach.isOn)
            {
                if (20 < temperature.value && temperature.value < 30)
                {
                    simulationLength /= 6;
                } else
                {
                    simulationLength = 0;
                }
                    
            } 

            foreach (Transform child in petriDish.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            logs.text = "";
            logs.text += "Simulation starting...\n";


            SpreadCells((int) numOfCells.value);
            Progress<List<Cell>> progress = new Progress<List<Cell>>();
            progress.ProgressChanged += ReportProgress;

            for (int i = 0; i < simulationLength; i++)
            {
                await Grow(progress);
            }

            await Task.Delay(1);
            logs.text += "...Simulation Complete.\n";

            //log data into json here.
        }

        /*
         * Grows each colony asyncroniously concurrently. The report function will allow the cells to grow
         * on the plate dynamically and the cancelationtoken allows the user to stop the growth entirely.
         * @param progress used to monitor the progress of the async task (like a callback)
         * @param cancellationToken used to cancel the growth if desired (like a callback)
         * @return Task required for non-event type async functions (means nothing)
         */
        private async Task Grow(IProgress<List<Cell>> progress)
        {
            var tasks = colonyList.Select(async colony =>
            {
                List<Cell> newCells = await colony.GrowParallelAsync();
                progress.Report(newCells);
            });
            await Task.WhenAll(tasks);
        }

        /*
         * Sets the intial locations of the bacteria randomly.
         */
        private void SpreadCells(int startingCells)
        {
            for (int i = 0; i < startingCells; i++)
            {
                int x = RNG.Next(0, DishDiameter);
                int y = RNG.Next(0, DishDiameter);
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
