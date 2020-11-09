using Bacteria;
using UnityEngine;
using UnityEngine.UI;

public class TrialButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { Petridish.Instance.PopulateDish(int.Parse(this.name.Substring(0, 1))); });
    }
}
