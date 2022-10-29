using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// sticks onto a tilemap gameobject, call GetPath() to get a path from start to end
/// path given is a list of vector2s, with first node to go to at 0, final pos is last pos in list
/// </summary>
public class PathCreator : MonoBehaviour
{
    Tilemap tilemap;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }


    public List<Vector2Int> GetPath(Vector2 startPosGiven, Vector2 endPosGiven)
    {
        Vector2Int startPos = new Vector2Int(Mathf.FloorToInt(startPosGiven.x), Mathf.FloorToInt(startPosGiven.y));
        Vector2Int endPos = new Vector2Int(Mathf.FloorToInt(endPosGiven.x), Mathf.FloorToInt(endPosGiven.y));
        List<Vector2Int> path = new List<Vector2Int>();

        //if start and end are the same then stop
        if (startPos == endPos) return path;

        Dictionary<Vector2Int, Node> openList = new Dictionary<Vector2Int, Node>();
        Dictionary<Vector2Int, Node> closedList = new Dictionary<Vector2Int, Node>();

        //create start node
        Node startNode = new Node
        {
            g = 0,
            h = GetHCost(startPos, endPos),
            f = GetHCost(startPos, endPos),
            pos = startPos,
        };

        //add starting node to open list
        openList.Add(startPos, startNode);

        int numLoops = 100;
        //while open list is not empty
        while (openList.Keys.Count != 0 && numLoops > 0)
        {
            numLoops--;

            //store current least f node and its f val
            Node currentNode = null;
            //go through nodes on open list
            foreach (Node node in openList.Values)
            {
                if (currentNode == null) currentNode = node;

                //compare with current lowest f value
                if (currentNode.f > node.f)
                {
                    //set new lowest node and f value if is lower than current
                    currentNode = node;
                }
            }


            //remove currentNode from open list usingpos
            openList.Remove(currentNode.pos);


            //go through the surrounding node positions of currentNode
            Vector2Int[] surroundingNodePositions = GetSurroundingNodePositions(currentNode.pos);
            for (int i = 0; i < surroundingNodePositions.Length; i++)
            {
                Vector2Int surroundingNodePos = surroundingNodePositions[i];

                //check if this node is the goal node
                if (surroundingNodePos == endPos)
                {
                    //if so then add to closed list and break search (current loop)
                    Node endNode = new Node { pos = endPos, parentPos = currentNode.pos };
                    //add current node to closed list
                    closedList.Add(currentNode.pos, currentNode);
                    currentNode = endNode;
                    closedList.Add(endPos, endNode);
                    break;
                }

                //calculate g, h, and f for surrounding node (g = parent.g + cost to go from parent to child, h = dist from goal to node, f = g + h)
                //g is based on wether nearby node is diagnal or orthognal
                Node surroundingNode = new Node
                {
                    g = currentNode.g + 1,
                    h = GetHCost(surroundingNodePos, endPos),
                    pos = surroundingNodePos,
                    parentPos = currentNode.pos
                };
                surroundingNode.f = surroundingNode.g + surroundingNode.h;

                //check if there is another node on open list with same pos as current node
                if (closedList.ContainsKey(surroundingNodePos))
                {
                    //if current node's f value is lower, then replace one on open list with current node, otherwise skip current node
                    if (closedList[surroundingNodePos].f > surroundingNode.f)
                    {
                        closedList[surroundingNodePos] = surroundingNode;
                    }
                }

                //check open list
                else
                {
                    //check if it's already on it
                    if (openList.ContainsKey(surroundingNodePos))
                    {
                        //if node already on has a higher f cost, then replace it
                        if (openList[surroundingNodePos].f > surroundingNode.f)
                            openList[surroundingNodePos] = surroundingNode;
                    }

                    //if not already on it then add it to open list
                    else
                        openList.Add(surroundingNodePos, surroundingNode);
                }
            }

            //if current node is end node, path has been found so break
            if (currentNode.pos == endPos) break;

            //put current node on closed list
            closedList.Add(currentNode.pos, currentNode);
        }


        //check if endNode is on closed list (there is a path)
        if (closedList.ContainsKey(endPos))
        {
            //store current node (rn is endNode)
            Node currentPathNode = closedList[endPos];

            //while true, will be broken when start pos is located
            while (true)
            {
                //add current node pos to path
                path.Add(currentPathNode.pos);

                //if is start node, then break loop
                if (currentPathNode.pos == startPos)
                    break;

                //set current node to parent of the current currentNode
                currentPathNode = closedList[currentPathNode.parentPos];
            }

            path.Reverse();
        }

        return path;
    }
    


    float GetHCost(Vector2Int startPos, Vector2Int endPos)
    {
        return Vector2.Distance(startPos, endPos);
    }

    Vector2Int[] GetSurroundingNodePositions(Vector2Int centerNode)
    {
        List<Vector2Int> returnPositions = new List<Vector2Int>();

        //the orthagnals
        Vector2Int eastPos = centerNode + new Vector2Int(1, 0);
        if (NodePassable(eastPos)) returnPositions.Add(eastPos);
        Vector2Int westPos = centerNode + new Vector2Int(-1, 0);
        if (NodePassable(westPos)) returnPositions.Add(westPos);
        Vector2Int northPos = centerNode + new Vector2Int(0, 1);
        if(NodePassable(northPos)) returnPositions.Add(northPos);
        Vector2Int southPos = centerNode + new Vector2Int(0, -1);
        if(NodePassable(southPos)) returnPositions.Add(southPos);

        //the diagnals
        Vector2Int northEastPos = centerNode + new Vector2Int(1, 1);
        if(returnPositions.Contains(northPos) && returnPositions.Contains(eastPos) && NodePassable(northEastPos)) returnPositions.Add(northEastPos);
        Vector2Int southEastPos = centerNode + new Vector2Int(1, -1);
        if (returnPositions.Contains(southPos) && returnPositions.Contains(eastPos) && NodePassable(southEastPos)) returnPositions.Add(southEastPos);
        Vector2Int southWestPos = centerNode + new Vector2Int(-1, -1);
        if (returnPositions.Contains(southPos) && returnPositions.Contains(westPos) && NodePassable(southWestPos)) returnPositions.Add(southWestPos);
        Vector2Int northWestPos = centerNode + new Vector2Int(1, -1);
        if (returnPositions.Contains(northPos) && returnPositions.Contains(westPos) && NodePassable(northWestPos)) returnPositions.Add(northWestPos);

        return returnPositions.ToArray();
    }

    bool NodePassable(Vector2Int nodePos)
    {
        TileBase nodeTile = tilemap.GetTile(new Vector3Int(nodePos.x, nodePos.y, 0));
        if (nodeTile == null) return true;
        else return false;
    }

    class Node
    {
        public float f, g, h;
        public Vector2Int pos, parentPos;
    }
}
