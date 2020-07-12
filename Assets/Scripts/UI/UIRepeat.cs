using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class UIRepeat : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToRepeat;
    [Range(1, 30)]
    [SerializeField]
    private int numRepetitions = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateChildren());
    }

    void OnValidate()
    {
        StartCoroutine(UpdateChildren());
    }

    IEnumerator UpdateChildren()
    {
        yield return new WaitForEndOfFrame();
        Transform[] children = GetComponentsInChildren<Transform>();
        List<Transform> clones = new List<Transform>();
        int targetClones = numRepetitions - 1;
        foreach (Transform child in children)
        {
            if (child.name.Equals($"{objectToRepeat.name}(Clone)"))
            {
                clones.Add(child);
            }
        }
        if (clones.Count > targetClones)
        {
            int numToDelete = clones.Count - targetClones;
            for (int i = 0; i < numToDelete; i++)
            {
                DestroyImmediate(clones[i].gameObject);
            }
        }
        else if (clones.Count < targetClones)
        {
            int numToAdd = targetClones - clones.Count;
            for (int i = 0; i < numToAdd; i++)
            {
                Instantiate(objectToRepeat, Vector3.zero, Quaternion.identity, gameObject.transform);
            }
        }
    }

    public void SetNumRepetitions(int num)
    {
        numRepetitions = num;
        StartCoroutine(UpdateChildren());
    }
}
