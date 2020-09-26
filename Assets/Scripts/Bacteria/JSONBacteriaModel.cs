using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONBacteriaModel : MonoBehaviour
{
 [SerializeField] private JSONPetriDish _Dish = new JSONPetriDish();

    public void SaveIntoJson() {
        string dish = JsonUtility.ToJson(_Dish);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/PotionData.json", dish);
    }
}

[System.Serializable]
public class JSONPetriDish {
    public string dishName;
    public int totalCells;
    public List<JSONColony> colonies = new List<JSONColony>();
}

[System.Serializable]
public class JSONColony {
    public int x;
    public int y;
    public List<JSONCell> colonies = new List<JSONCell>();
}

[System.Serializable]
public class JSONCell {
    public int x;
    public int y;
}