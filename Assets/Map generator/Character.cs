using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float speed;
    private Transform[] path;
	private int path_size=0;

    private int pathIndex;

    private List<List<Transform>> pathTransform;

	private Graph graph;
    // Start is called before the first frame update
    void Start()
    {
       GameObject graphObject = GameObject.Find("graph");

		graph = graphObject.GetComponent<Graph>();

		if (graph != null)
		{
			pathTransform = graph.path_transform;

			path_size = pathTransform[0].Count;

			path = new Transform[path_size];

			for (int i = 0; i < path_size - 1; i++)
			{
				path[i] = pathTransform[0][i];
			}
		}
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, path[pathIndex].position, speed*Time.deltaTime);

        if(Vector2.Distance(transform.position, path[pathIndex].position)<0.1f){
            pathIndex++;
        }

        if(pathIndex == path_size-1){
            Destroy(gameObject);
        }
    }
}
