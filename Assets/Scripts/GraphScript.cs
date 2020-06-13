using System.Collections;
using System.Collections.Generic;
using ChartAndGraph;
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
            {
                //add 30 random points , each with a category and an x,y value
                graph.DataSource.AddPointToCategory("Player 1", Random.value * 10f, Random.value * 10f);
                graph.DataSource.AddPointToCategory("Player 2", Random.value * 10f, Random.value * 10f);
            }
            graph.DataSource.EndBatch(); // end the update batch . this call will render the graph
        }
    }
}
