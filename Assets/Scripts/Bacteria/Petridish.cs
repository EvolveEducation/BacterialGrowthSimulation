using ChartAndGraph;
using System;
using System.Collections.Generic;
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
        public Text logs;
        public Toggle bleach;
        public Slider uvLight;
        public Slider numOfCells;
        public Slider temperature;
        public Slider trialTime;
        public GraphChart graph;
        public TrialList trialList;

        public static Petridish Instance { get; private set; }
        public int DishRadius { get; private set; }
        public int DishDiameter { get; private set; }
        public bool[,] CellLocations { get; private set; }
        public Random RNG { get; private set; }

        private List<Colony> colonyList;
        private float dishNormailzer;
        private double timePerDivision;
        private int timeMultipler;
        private int cellCount;
        private int trialNumber;
        private JSONBacteriaModel jsonDataSet;

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
            trialNumber = 1;
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
        private void SimulationStart()
        {
            colonyList = new List<Colony>();
            jsonDataSet = ScriptableObject.CreateInstance<JSONBacteriaModel>();
            float adjustedX = temperature.value - 1.5f;
            double temperatureEstimate = (2.783e-16 * Math.Pow(adjustedX, 11)) - (4.0496e-9 * Math.Pow(adjustedX, 7)) + (0.00000517429 * Math.Pow(adjustedX, 5)) + (0.00150295 * Math.Pow(adjustedX, 3));
            timePerDivision = 70 / (temperatureEstimate / 100);
            int simulationLength = (int) Math.Floor((trialTime.value * 60) / timePerDivision);

            double e = (6.022 * Math.Pow(10, 23)) * (6.62 * Math.Pow(10, -34)) * (3 * Math.Pow(10, 8));
            double Ea = e / uvLight.value;

            if (bleach.isOn)
            {
                if (20 < temperature.value && temperature.value < 30)
                {
                    simulationLength /= 6;
                } 
                else
                {
                    simulationLength = 0;
                }
            }

            jsonDataSet.InstantiateJSON(trialNumber, bleach.isOn, (int)uvLight.value,
                temperature.value, trialTime.value, (int)numOfCells.value);

            foreach (Transform child in petriDish.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            logs.text = "";
            logs.text += "Simulation starting...\n";

            logs.text += "Spreading " + (int) numOfCells.value + " cell(s) on the petridish.\n";
            SpreadCells((int) numOfCells.value);
            InitializeGraph((int) numOfCells.value);

            for (int i = 1; i <= simulationLength; i++)
            {
                double death = 144.53 * Ea * Math.Log(timePerDivision * 60 * i) - 0.2341;
                ReportProgress(Grow(death));
            }

            logs.text += "...Simulation Complete.\n";
            jsonDataSet.SaveIntoJson();
            trialList.NewTrial(jsonDataSet.ParseJSON(trialNumber), trialNumber);
            trialNumber++;
        }

        /*
         * Populates the Petridish with a generated run that's been stored in JSON.
         * @param trialID is the trial that will be generated
         */
        public void PopulateDish(int trialID)
        {
            foreach (Transform child in petriDish.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            JSONPetriDish jsonDish = jsonDataSet.ParseJSON(trialID);
            JSONPetriDishAtTimeN lastDish = jsonDish.dishTimes[jsonDish.dishTimes.Count - 1];

            logs.text = "";
            logs.text += "Loading " + jsonDish.dishName + "...\n";
            logs.text += "This trial had:\n";
            logs.text += jsonDish.startingCells + " starting colonies,\n";
            logs.text += "a temperature of " + jsonDish.temp.ToString("0.0") + "°C,\n";
            logs.text += "grown under light with a wavelength of " + jsonDish.UV + " nm,\n";
            logs.text += jsonDish.bleach ? "with bleach,\n" : "with no bleach,\n";
            int remainder = (int)Math.Floor(Math.Abs(Math.Floor(jsonDish.time) - jsonDish.time) * 60);
            string rString = (remainder < 10) ? "0" + remainder : remainder.ToString();
            logs.text += "for " + Math.Floor(jsonDish.time).ToString() + " hour(s) and " + rString + " minute(s).\n\n";


            foreach (JSONColony colony in lastDish.colonies)
            {
                foreach (JSONCell cell in colony.cells)
                {
                    cellPrefab.transform.position = new Vector3(cell.x / dishNormailzer, 0, cell.y / dishNormailzer);
                    Instantiate(cellPrefab, petriDish.transform);
                }
            }

            InitializeGraph(jsonDish.dishTimes[0].totalCells);

            for (int i = 1; i < jsonDish.dishTimes.Count; i++)
            {
                PopulateGraph(jsonDish.dishTimes[i].totalCells, (timePerDivision / 60));
            }

            logs.text += "Total number of cells grown: " + jsonDish.finalCellCount;
        }

        /*
         * Grows each colony.
         * @return a list of the newly grown cells
         */
        private Tuple<List<Cell>, int> Grow(double death)
        {
            List<Cell> newCells = new List<Cell>();
            int dead = 0;
            foreach (Colony c in colonyList)
            {
                Tuple<List<Cell>, int> results = c.GrowColony(death);
                newCells.AddRange(results.Item1);
                dead += results.Item2;
            }

            return new Tuple<List<Cell>, int>(newCells, dead);
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
            cellCount = startingCells;
            jsonDataSet.UpdateJSON(this.cellCount, 0, colonyList);
        }

        /*
         * Called whenever a colony grows.
         * Updates the graph, log, and petridish.
         * @param cells is a list a of new cells on the petridish 
         */
        private void ReportProgress(Tuple<List<Cell>, int> cells)
        {
            if (cells.Item1.Count > 0)
            {
                logs.text += cells.Item1.Count + " new cells\n";
                foreach (Cell cell in cells.Item1)
                {
                    cellPrefab.transform.position = new Vector3(cell.X / dishNormailzer, 0, cell.Y / dishNormailzer);
                    Instantiate(cellPrefab, petriDish.transform);
                }
                cellCount += cells.Item1.Count;
                LogData(cellCount, (timePerDivision / 60) * timeMultipler);
                PopulateGraph(cellCount, timePerDivision / 60);
            }
            if (cells.Item2 > 0)
            {
                logs.text += cells.Item2 + " dead or inactive cells\n";
            }
            logs.text += "\n";
        }

        /*
         * Initializes the graph and sets the first point.
         * @param startingCells the number of starting cells spread on the plate
         */
        private void InitializeGraph(int startingCells)
        {
            timeMultipler = 1;
            graph.DataSource.StartBatch();
            graph.DataSource.ClearAndMakeBezierCurve("CellGrowth");
            graph.DataSource.SetCurveInitialPoint("CellGrowth", 0, startingCells);
            graph.DataSource.EndBatch();
        }

        /*
         * Adds a new point to the graph
         * @param cellCount the number of cells (y-axis) for this point
         * @param time the time the divisions happened (x-axis)
         */
        private void PopulateGraph(int cellCount, double time)
        {
            graph.DataSource.StartBatch();
            graph.DataSource.AddLinearCurveToCategory("CellGrowth", new DoubleVector2(time * timeMultipler, cellCount));
            graph.DataSource.MakeCurveCategorySmooth("CellGrowth");
            graph.DataSource.EndBatch();
            timeMultipler++;
        }

        /*
         * Adds a new timestamp of the petridish
         * @param cellCount the number of cells for this point
         * @param time the time the divisions happened
         */
        private void LogData(int cellCount, double time)
        {
            jsonDataSet.UpdateJSON(cellCount, time, colonyList);
        }
    }
}
