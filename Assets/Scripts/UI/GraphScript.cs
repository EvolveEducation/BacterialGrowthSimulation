using System.Collections;
using System.Collections.Generic;
using ChartAndGraph;
<<<<<<< HEAD:Assets/Scripts/GraphScript.cs
using UnityEngine;

public class GraphScript : MonoBehaviour
{
    void Start()
    {
        GraphChart graph = GetComponent<GraphChart>();
        if (graph != null)
        {
            graph.DataSource.StartBatch();  // start a new update batch
            graph.DataSource.ClearCategory("Player 1");  // clear the categories we have created in the inspector
            graph.DataSource.ClearCategory("Player 2");

            for (int i = 0; i < 5; i++)
=======
using UnityEngine;

public class GraphScript : MonoBehaviour
{
    void Start()
    {
        GraphChart graph = GetComponent<GraphChart>();
        if (graph != null)
        {
            graph.DataSource.StartBatch();  // start a new update batch
            graph.DataSource.ClearCategory("Player 1");  // clear the categories we have created in the inspector
            graph.DataSource.ClearCategory("Player 2");
            for (int i = 0; i < 30; i++)
>>>>>>> 38a141e843cbce0c7bed4af620c97cb8e5bee3d9:Assets/Scripts/UI/GraphScript.cs
            {
                //add 30 random points , each with a category and an x,y value
                graph.DataSource.AddPointToCategory("Player 1", Random.value * 10f, Random.value * 10f);
                graph.DataSource.AddPointToCategory("Player 2", Random.value * 10f, Random.value * 10f);
            }
            graph.DataSource.EndBatch(); // end the update batch . this call will render the graph
        }
    }
}
