using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite emptyTile, pathTile, entryTile;
    [SerializeField] public Node tileNode;

    [SerializeField] public GameObject tilePrefab, Instance_point, tower, pointPrefab, Spawner;

    [SerializeField] private GameObject tree, flower, grass, grass2;


    private int radius = 24;

    private MatrixGraph matrix;

    private List<Node> Q1, Q2, Q3, Q4;

    private Node source1, source2, destination1, destination2;

    private List<Node> requiredVertices = new List<Node>();
    private List<List<Node>> path = new List<List<Node>>();

    public List<List<Transform>> path_transform = new List<List<Transform>>();


    // start is called before the first frame update
    void Awake()
    {

        Vector3 novaEscala = new Vector3((float)2, (float)2.5, 0);

        // Atribua a nova escala ao GameObject
        tower.transform.localScale = novaEscala;

        tileNode.tile = emptyTile;

        matrix = new MatrixGraph(radius);

        Q1 = matrix.getQuadrant(1);
        Q2 = matrix.getQuadrant(2);
        Q3 = matrix.getQuadrant(3);
        Q4 = matrix.getQuadrant(4);

        sortSourceAndDestination();
        sortQVertex();



        List<Node> path1 = new List<Node>();
        List<Node> path2 = new List<Node>();
        List<Node> path3 = new List<Node>();
        List<Node> path4 = new List<Node>();

        //Busca do Q1
        //Busca do Q1
        path1.AddRange(BreadthFirstPaths.BFS(source1, requiredVertices, requiredVertices[0]));
        path2.AddRange(BreadthFirstPaths.BFS(source1, requiredVertices, requiredVertices[1]));

        //Busca do Q2
        path3.AddRange(BreadthFirstPaths.BFS(source2, requiredVertices, requiredVertices[2]));
        path4.AddRange(BreadthFirstPaths.BFS(source2, requiredVertices, requiredVertices[3]));

        //Busca do Q3
        path3.AddRange(BreadthFirstPaths.BFS(requiredVertices[2], requiredVertices, requiredVertices[4]));
        path4.AddRange(BreadthFirstPaths.BFS(requiredVertices[3], requiredVertices, requiredVertices[5]));

        //Busca ao Destino 1
        path3.AddRange(BreadthFirstPaths.BFS(requiredVertices[4], requiredVertices, destination1));
        path4.AddRange(BreadthFirstPaths.BFS(requiredVertices[5], requiredVertices, destination1));

        //Busca do Q4
        path1.AddRange(BreadthFirstPaths.BFS(requiredVertices[0], requiredVertices, requiredVertices[6]));
        path2.AddRange(BreadthFirstPaths.BFS(requiredVertices[1], requiredVertices, requiredVertices[7]));

        //Busca ao Destino 2
        path1.AddRange(BreadthFirstPaths.BFS(requiredVertices[6], requiredVertices, destination2));
        path2.AddRange(BreadthFirstPaths.BFS(requiredVertices[7], requiredVertices, destination2));


        List<Node> possibleTowers = findPossibleTowerPlaces();

        printMap();

        // foreach (var item in path)
        // {
        //     printPath(item);
        // }
        printPath(path1, 0);
        printPath(path2, 1);
        printPath(path3, 2);
        printPath(path4, 3);

        PutRandomTowers(possibleTowers);

        Spawner.SetActive(true);
        GameObject spawner = Instantiate(Spawner, new Vector2(source1.x * 2, (source1.y) - 1), Quaternion.identity);
    }

    List<int> GenerateUniqueRandomNumbers(int min, int max, int count)
    {
        if (count > max - min + 1 || max < min)
        {
            // Handle invalid input
            return null;
        }

        List<int> numbers = new List<int>();
        HashSet<int> uniqueNumbers = new HashSet<int>();

        while (uniqueNumbers.Count < count)
        {
            int randomNumber = Random.Range(min, max + 1);

            if (!uniqueNumbers.Contains(randomNumber))
            {
                uniqueNumbers.Add(randomNumber);
                numbers.Add(randomNumber);
            }
        }

        return numbers;
    }

    public void printMap()
    {
        Node tileToPrint = matrix.head.bottom;
        for (int i = 0; i < radius; i++)
        {
            Node firstPosition = tileToPrint;

            for (int j = 0; j < radius; j++)
            {

                if (!tileToPrint.isPath)
                {
                    tilePrefab.GetComponent<SpriteRenderer>().sprite = emptyTile;

                    GameObject tile = Instantiate(tilePrefab, new Vector2(tileToPrint.x * 2, -(tileToPrint.y * 2)), Quaternion.identity);
                    if (!tileToPrint.canRecieveTower)
                    {
                        int x = Random.Range(0, 10);
                        if (x % 2 == 0)
                        {
                            Instantiate(flower, new Vector2(tileToPrint.x * 2, -(tileToPrint.y * 2) + (float)0.1), Quaternion.identity);
                        }
                        else if (x % 3 == 0)
                        {
                            Instantiate(grass, new Vector2(tileToPrint.x * 2, -(tileToPrint.y * 2) + (float)0.1), Quaternion.identity);
                        }
                        else if (x % 5 == 0 || x % 7 == 0)
                        {
                            Instantiate(tree, new Vector2(tileToPrint.x * 2, -(tileToPrint.y * 2) + (float)+1), Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(grass2, new Vector2(tileToPrint.x * 2, -(tileToPrint.y * 2) + (float)0.1), Quaternion.identity);
                        }
                    }

                }



                // GameObject tile = Instantiate(tilePrefab, new Vector2(tileToPrint.x * 2, -(tileToPrint.y * 2)), Quaternion.identity);
                tileToPrint = tileToPrint.right;
            }


            tileToPrint = firstPosition.bottom;
        }

        tilePrefab.GetComponent<SpriteRenderer>().sprite = emptyTile;
        for (int j = 0; j < radius; j++)
        {
            Instantiate(tilePrefab, new Vector2(-2, -(j * 2)), Quaternion.identity);
            Instantiate(tilePrefab, new Vector2(radius * 2, -(j * 2)), Quaternion.identity);
        }
    }

    public void printPath(List<Node> path, int ind)
    {
        // tilePrefab.GetComponent<SpriteRenderer>().sprite = pathTile;
        int index = 0;
        List<Transform> path1 = new List<Transform>();

        foreach (Node n in path)
        {

            if (!n.isEntry)
            {
                tilePrefab.GetComponent<SpriteRenderer>().sprite = pathTile;

                GameObject tile = Instantiate(tilePrefab, new Vector2(n.x * 2, -(n.y * 2)), Quaternion.identity);
                path1.Add(tile.transform);

                // if (ind == 0)
                // {
                //     GameObject newObj = new GameObject("a" + index);

                //     Vector2 position = new Vector2(tile.transform.position.x, tile.transform.position.y);
                //     Instantiate(newObj, position, Quaternion.identity);
                // }


            }
            else
            {
                tilePrefab.GetComponent<SpriteRenderer>().sprite = entryTile;

                GameObject tile = Instantiate(tilePrefab, new Vector2(n.x * 2, -(n.y * 2)), Quaternion.identity);
                path1.Add(tile.transform);

                // if (ind == 0)
                // {
                //     if (n.isLast)
                //     {
                //         GameObject newObj = new GameObject("aEnd");

                //         Vector2 position = new Vector2(tile.transform.position.x, tile.transform.position.y);
                //         Instantiate(newObj, position, Quaternion.identity);
                //     }
                //     else
                //     {
                //         GameObject newObj = new GameObject("a" + index);

                //         Vector2 position = new Vector2(tile.transform.position.x, tile.transform.position.y);
                //         Instantiate(newObj, position, Quaternion.identity);
                //     }
                // }



            }

            index++;


        }

        path_transform.Add(path1);


        //     for (int index = 0; index < masterInstance.path1.Count; index++)
        // {
        //     GameObject newObj = new GameObject("a" + index);

        //     Vector3 position = new Vector3(masterInstance.path1[index].x, 0, masterInstance.path1[index].y);
        //     newObj.transform.position = position;
        // }
    }


    public void sortSourceAndDestination()
    {
        int x;
        x = Random.Range(0, matrix.n);
        source1 = Q1[x];

        x = Random.Range(0, matrix.n);
        source2 = Q2[x];

        x = Random.Range(Q3.Count - matrix.n, Q3.Count - 1);
        destination1 = Q3[x];

        x = Random.Range(Q4.Count - matrix.n, Q4.Count - 1);
        destination2 = Q4[x];

        source1.isPath = source2.isPath = destination1.isPath = destination2.isPath = true;
        source1.isEntry = source2.isEntry = destination1.isEntry = destination2.isEntry = true;
        destination1.isLast = destination2.isLast = true;
    }
    public void sortQVertex()
    {
        int pos;
        int offset;
        int temp;

        //Sorteio dos vértices Q1
        offset = Random.Range(2, 5);
        temp = offset * 12;
        pos = offset + temp;
        this.requiredVertices.Add(Q1[pos]);

        offset = Random.Range(8, 11);
        temp = offset * 12;
        pos = offset + temp;
        if (!requiredVertices.Contains(Q1[pos]))
        {
            requiredVertices.Add(Q1[pos]);
        }
        else
        {
            offset = Random.Range(7, 11);
            temp = offset * 12;
            pos = offset + temp;
            if (!requiredVertices.Contains(Q1[pos]))
            {
                requiredVertices.Add(Q1[pos]);
            }
        }

        //Sorteio dos vértices Q2
        offset = Random.Range(2, 5);
        temp = offset * 12;
        offset = Random.Range(7, 11);
        pos = offset + temp;
        this.requiredVertices.Add(Q2[pos]);

        offset = Random.Range(8, 11);
        temp = offset * 12;
        offset = Random.Range(2, 5);
        pos = offset + temp;
        if (!requiredVertices.Contains(Q2[pos]))
        {
            requiredVertices.Add(Q2[pos]);
        }
        else
        {
            offset = Random.Range(8, 11);
            temp = offset * 12;
            pos = offset + temp;
            if (!requiredVertices.Contains(Q2[pos]))
            {
                requiredVertices.Add(Q2[pos]);
            }
        }

        //Sorteio dos vértices Q3
        offset = Random.Range(2, 5);
        temp = offset * 12;
        pos = offset + temp;
        this.requiredVertices.Add(Q3[pos]);

        offset = Random.Range(8, 10);
        temp = offset * 12;
        pos = offset + temp;
        if (!requiredVertices.Contains(Q3[pos]))
        {
            requiredVertices.Add(Q3[pos]);
        }
        else
        {
            offset = Random.Range(8, 10);
            temp = offset * 12;
            pos = offset + temp;
            if (!requiredVertices.Contains(Q3[pos]))
            {
                requiredVertices.Add(Q3[pos]);
            }
        }

        //Sorteio dos vértices Q4
        offset = Random.Range(2, 5);
        temp = offset * 12;
        offset = Random.Range(7, 10);
        pos = offset + temp;
        this.requiredVertices.Add(Q4[pos]);

        offset = Random.Range(8, 10);
        temp = offset * 12;
        offset = Random.Range(2, 5);
        pos = offset + temp;
        if (!requiredVertices.Contains(Q4[pos]))
        {
            requiredVertices.Add(Q4[pos]);
        }
        else
        {
            offset = Random.Range(8, 10);
            temp = offset * 12;
            pos = offset + temp;
            if (!requiredVertices.Contains(Q4[pos]))
            {
                requiredVertices.Add(Q4[pos]);
            }
        }

        foreach (Node n in requiredVertices)
        {
            n.isPath = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    List<Node> findPossibleTowerPlaces()
    {
        List<Node> possibleTowerPlace = new List<Node>();
        Node tileToPrint = matrix.head.bottom;
        for (int i = 0; i < radius; i++)
        {
            Node firstPosition = tileToPrint;

            for (int j = 0; j < radius; j++)
            {
                if (tileToPrint.isPath == false && tileToPrint.left && tileToPrint.left.isPath)
                {
                    tileToPrint.canRecieveTower = true;
                }
                else if (tileToPrint.isPath == false && tileToPrint.right && tileToPrint.right.isPath)
                {
                    tileToPrint.canRecieveTower = true;
                }
                else if (tileToPrint.isPath == false && tileToPrint.top && tileToPrint.top.isPath)
                {
                    tileToPrint.canRecieveTower = true;
                }
                else if (tileToPrint.isPath == false && tileToPrint.bottom && tileToPrint.bottom.isPath)
                {
                    tileToPrint.canRecieveTower = true;
                }

                if (tileToPrint.canRecieveTower)
                {
                    possibleTowerPlace.Add(tileToPrint);
                }
                tileToPrint = tileToPrint.right;
            }
            tileToPrint = firstPosition.bottom;
        }
        //find all nodes adjecent to paths that are not paths
        return possibleTowerPlace;
    }

    // void ShowScenenary()
    // {
    //     //all nodes that are not paths and cant receive towers will have decorations
    // }

    void PutRandomTowers(List<Node> towerPlacements)
    {
        List<int> randomNumbers = GenerateUniqueRandomNumbers(0, towerPlacements.Count, 12);

        int index =0;

        //List<Node> randomElements = GetRandomElements(towerPlacements, 5);

        foreach (var item in randomNumbers)
        {
           GameObject t =  Instantiate(tower, new Vector3(towerPlacements[item].x * 2, -(towerPlacements[item].y * 2) + 1, 0), Quaternion.identity);
           t.name = "tower" + index;

           index++;
        }
        //put towers randomly in the placements available 

    }

  
}


class MatrixGraph
{

    private int radius;
    public int n;
    private int x;
    public Node head;

    //public Node tail;


    public MatrixGraph(int radius)
    {
        this.radius = radius;

        this.n = (int)radius / 2;
        this.x = (int)n * n;

        //head = head Node 
        head = new Node();
        head.bottom = new Node(0, 0);

        Node columnCurrent = head.bottom, lineCurrent;

        // First line 
        for (int i = 0; i < radius - 1; i++)
        {
            columnCurrent.right = new Node(i + 1, 0);
            columnCurrent.right.left = columnCurrent;
            columnCurrent = columnCurrent.right;
        }

        for (int i = 1; i < radius; i++)
        {
            while (columnCurrent.left != null)
            {
                columnCurrent = columnCurrent.left;
            }

            // next line
            columnCurrent.bottom = new Node(0, i);
            lineCurrent = columnCurrent.bottom;
            lineCurrent.top = columnCurrent;

            // building line`s column
            for (int j = 0; j < radius - 1; j++)
            {
                lineCurrent.right = new Node(j + 1, i);
                lineCurrent.right.left = lineCurrent;

                lineCurrent = lineCurrent.right;
                columnCurrent = columnCurrent.right;

                lineCurrent.top = columnCurrent;
                columnCurrent.bottom = lineCurrent;
            }

            // point to last node in line
            columnCurrent = lineCurrent;
        }

    }

    public Node getQuadrantStart(int q)
    {
        Node ret = head.bottom;
        if (q == 1)
        {
            return ret;
        }
        else if (q == 2)
        {
            while (ret.x != n)
            {
                ret = ret.right;
            }
            return ret;
        }
        else if (q == 3)
        {
            while (ret.y != (n))
            {
                ret = ret.bottom;
            }
            return ret;
        }
        else if (q == 4)
        {
            while (ret.x != n)
            {
                ret = ret.right;
            }
            while (ret.y != (n))
            {
                ret = ret.bottom;
            }
            return ret;
        }

        return null;

    }

    public List<Node> getQuadrant(int q)
    {
        List<Node> list = new List<Node>();
        Node curr = getQuadrantStart(q);
        Node first = curr;

        int count = 0;
        if (curr != null)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    list.Add(curr);
                    curr = curr.right;
                }
                curr = first.bottom;
                first = curr;

            }
        }

        return list;
    }


    // public void fill()
    // {

    // }
}