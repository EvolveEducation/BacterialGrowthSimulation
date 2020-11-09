using Bacteria;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class JSONBacteriaModel : ScriptableObject
{
    [SerializeField] 
    private JSONPetriDish petriDish;

    /*
     * Used to create a new instance of a petridish trial
     * @param trialNumber n>0 that represents the current trial running
     */
    public void InstantiateJSON(int trialNumber, bool b, string uvType, float temp, float time, int cells)
    {
        petriDish = new JSONPetriDish
        {
            trialID = trialNumber,
            dishName = "Trial " + trialNumber,
            bleach = b,
            UV = uvType,
            temp = temp,
            time = time,
            startingCells = cells
        };
    }

    /*
     * Updates the JSON fields with informaiton about the colony locations.
     * Called at each growth cycle.
     * @param count the current number of cells
     * @param time the current trial time
     * @param colonies a list of the colonies on the dish
     */
    public void UpdateJSON(int count, double time, List<Colony> colonies)
    {
        JSONPetriDishAtTimeN currDish = new JSONPetriDishAtTimeN
        {
            totalCells = count,
            time = time
        };

        foreach (Colony col in colonies)
        {
            JSONColony colony = new JSONColony()
            {
                x = col.GetOrigin().X,
                y = col.GetOrigin().Y
            };
            double radius = 1;
            foreach (Cell cell in col.GetCells())
            {
                colony.cells.Add(new JSONCell{ x = cell.X, y = cell.Y });
                double temp = Math.Sqrt(Math.Pow(cell.X - colony.x, 2) + Math.Pow(cell.Y - colony.y, 2));
                radius = temp > radius ? temp : radius;
            }
            colony.radius = radius;
            currDish.colonies.Add(colony);
        }

        petriDish.dishTimes.Add(currDish);
    }

    /*
     * Saves the JSON data after the trial has completed.
     */
    public void SaveIntoJson() 
    {
        petriDish.finalCellCount = petriDish.dishTimes[petriDish.dishTimes.Count - 1].totalCells;
        string dishJSON = JsonUtility.ToJson(petriDish);
        File.WriteAllText(Application.dataPath + "/Logs/" + petriDish.dishName + ".json", dishJSON);
    }

    /*
     * Parse the JSON file associated to the trial number and returns the JSONPetriDish object associated with it\
     * @reutrn the petridish object associated with their JSON
     */
    public JSONPetriDish ParseJSON(int trialNumber)
    {
        if (!File.Exists(Application.dataPath + "/Logs/" + "Trial " + trialNumber + ".json"))
        {
            Debug.LogError("No file " + Application.dataPath + "/Logs/" + "Trial " + trialNumber + ".json");
            return null;
        }
        return JsonUtility.FromJson<JSONPetriDish>(File.ReadAllText(Application.dataPath + "/Logs/" + "Trial " + trialNumber + ".json"));
    }
}

[Serializable]
public class JSONPetriDish
{
    public int startingCells;
    public float time;
    public float temp;
    public bool bleach;
    public string UV;
    public int trialID;
    public string dishName;
    public int finalCellCount;
    public List<JSONPetriDishAtTimeN> dishTimes = new List<JSONPetriDishAtTimeN>();
}

[Serializable]
public class JSONPetriDishAtTimeN
{
    public int totalCells;
    public double time;
    public List<JSONColony> colonies = new List<JSONColony>();
}

[Serializable]
public class JSONColony
{
    public int x;
    public int y;
    public double radius;
    public List<JSONCell> cells = new List<JSONCell>();
}

[Serializable]
public class JSONCell
{
    public int x;
    public int y;
}