using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Node : ScriptableObject
{
    public GameObject tile;

    public Vector2 WorldPosition { get; set; }

    public Point GridPosition {get; private set;}

    public Node top = null;
    public Node bottom = null;
    public Node left = null;
    public Node right = null;

    public bool canRecieveTower = false;
    public bool canRecieveDecoration;
    public bool isPath = false;
    public bool Visited = false;

    public bool isEntry = false;

    public bool isLast = false;


    public Node parent = null; //pai do nó para o melhor caminho
    public int distEntry = 0; //distancia percorrida da entrada ate esse nó
    public double heuristic = 0; //valor da heuristica euclidiana em relação ao nó
    public double funcN = 0; //recebera o resultado da função f(n) = distPercorrida + heuristica


    public int x;
    public int y;

    public Node(int x, int y){
        this.x = x;
        this.y = y;
    }

    public Node(){}


}
