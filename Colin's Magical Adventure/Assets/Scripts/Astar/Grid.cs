﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform Ennemy; //source du pathfinding
    public LayerMask WallMask; 
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float Distance;

    Node[,] grid;
    public List<Node> FinalPath;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for(int y =0; y< gridSizeY; y++)
        {
            for(int x = 0; x<gridSizeX; x++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool Wall = true;

                if(Physics.CheckSphere(worldPoint, nodeRadius, WallMask))
                {
                    Wall = false;
                }

                try
                {
                    grid[x, y] = new Node(Wall, worldPoint, x, y);
                }
                catch (System.Exception s)
                {
                    Debug.Log(s);
                    throw new System.Exception("Erreur");
                }
                
            }
        }
    }

    public Node NodeFromWorldPosition(Vector3 a_WorldPosition)
    {
        float xpoint = ((a_WorldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float ypoint = ((a_WorldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y);

        xpoint = Mathf.Clamp01(xpoint);
        ypoint = Mathf.Clamp01(ypoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xpoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * ypoint);

        return grid[x, y];
    }

    public List<Node> GetNeighborNodes(Node a_Node)
    {
        List<Node> NeighboringNodes = new List<Node>();
        int xCheck;
        int yCheck;

        //Right Side
        xCheck = a_Node.gridX + 1;
        yCheck = a_Node.gridY;
        if(xCheck >= 0 && xCheck < gridSizeX)
        {
            if(yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Left Side
        xCheck = a_Node.gridX - 1;
        yCheck = a_Node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Top Side
        xCheck = a_Node.gridX;
        yCheck = a_Node.gridY +1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Bottom Side
        xCheck = a_Node.gridX;
        yCheck = a_Node.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        return NeighboringNodes;
    }

    // Fonction dessinant le chemin et les murs
    // PAS NECESSAIRE
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if(grid != null)
        {
            foreach(Node n in grid) // pour chaque noeud de la grill
            {
                if (n.IsWall) // si le noeud est un mur
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.yellow;
                }

                if(FinalPath != null)
                {
                    if (FinalPath.Contains(n)) // si le noeud courant est dans le chemin final
                    {
                        Gizmos.color = Color.red;
                    }
                }

                Gizmos.DrawCube(n.Position, Vector3.one * (nodeDiameter - Distance)); //dessine le noeud
            }
        }
    }
}
