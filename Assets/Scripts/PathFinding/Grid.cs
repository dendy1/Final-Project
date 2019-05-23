using System;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Grid : MonoBehaviour
{
   public bool displayGridGizmos;
   
   public LayerMask unwalkableLayerMask;
   public TerrainType[] walkableRegions;
   public Vector2 gridWorldSize;
   public float nodeRadius;
   public int obstacleProximityPenalty = 10;
   
   private Node[,] _grid;
   private float _nodeDiameter;
   private int _gridSizeX, _gridSizeY;
   private Dictionary<int, int> _walkableRegionsDictionary = new Dictionary<int, int>();
   private LayerMask _walkableMask;

   private int _maxPenalty = Int32.MinValue;
   private int _minPenalty = Int32.MaxValue;
   
   public int MaxSize => _gridSizeX * _gridSizeY;
   
   private void Awake()
   {
      _nodeDiameter = nodeRadius * 2;
      _gridSizeX = Mathf.RoundToInt(gridWorldSize.x / _nodeDiameter);
      _gridSizeY = Mathf.RoundToInt(gridWorldSize.y / _nodeDiameter);

      foreach (var region in walkableRegions)
      {
         _walkableMask.value |= region.TerrainMask.value;
         _walkableRegionsDictionary.Add((int)Mathf.Log(region.TerrainMask.value, 2), region.TerrainPenalty);
      }
      
      CreateNodeGrid();
   }

   private void CreateNodeGrid()
   {
      _grid = new Node[_gridSizeX, _gridSizeY];

      Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 -
                           Vector3.forward * gridWorldSize.y / 2;
      
      for (int x = 0; x < _gridSizeX; x++)
         for (int y = 0; y < _gridSizeY; y++)
         {
            Vector3 worldPosition = bottomLeft + Vector3.right * (x * _nodeDiameter + nodeRadius) +
                               Vector3.forward * (y * _nodeDiameter + nodeRadius);
            bool walkable = !Physics.CheckSphere(worldPosition, nodeRadius, unwalkableLayerMask);

            int movementPenalty = 0;

            if (walkable)
            {
               Ray ray = new Ray(worldPosition + Vector3.up * 50, Vector3.down);
               RaycastHit hit;

               if (Physics.Raycast(ray, out hit, 100, _walkableMask))
               {
                  _walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
               }
            }

            if (!walkable)
               movementPenalty += obstacleProximityPenalty;
            
            _grid[x, y] = new Node(worldPosition, x, y, walkable, movementPenalty);
         }
      
      BlurPenaltyMap(3);
   }

   public Node GetNodeFromWorldPosition(Vector3 worldPosition)
   {
      float posX = ((worldPosition.x - transform.position.x) + gridWorldSize.x * 0.5f) / _nodeDiameter;
      float posY = ((worldPosition.z - transform.position.z) + gridWorldSize.y * 0.5f) / _nodeDiameter;

      posX = Mathf.Clamp(posX, 0, gridWorldSize.x - 1);
      posY = Mathf.Clamp(posY, 0, gridWorldSize.y - 1);

      int x = Mathf.FloorToInt(posX);
      int y = Mathf.FloorToInt(posY);
      
      return _grid[x, y];
   }

   public List<Node> GetNeighbours(Node node)
   {
      List<Node> neightbours = new List<Node>();
      
      for (int x = -1; x <= 1; x++)
         for (int y = -1; y <= 1; y++)
         {
            if (x == 0 && y == 0)
               continue;

            int currentX = node.GridPositionX + x;
            int currentY = node.GridPositionY + y;
            
            if (currentX >= 0 && currentX < _gridSizeX && currentY >= 0 && currentY < _gridSizeY)
               neightbours.Add(_grid[currentX, currentY]);
         }

      return neightbours;
   }

   private void BlurPenaltyMap(int blurSize)
   {
      int kernelSize = blurSize * 2 + 1;
      int kernelExtents = (kernelSize - 1) / 2;
      
      int[,] penaltiesHorizontalPass = new int[_gridSizeX, _gridSizeY];
      int[,] penaltiesVerticalPass = new int[_gridSizeX, _gridSizeY];

      for (int y = 0; y < _gridSizeY; y++)
      {
         for (int x = -kernelExtents; x <= kernelExtents; x++)
         {
            int sampleX = Mathf.Clamp(x, 0, kernelExtents);
            penaltiesHorizontalPass[0, y] += _grid[sampleX, y].MovementPenalty;
         }

         for (int x = 1; x < _gridSizeX; x++)
         {
            int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, _gridSizeX);
            int addIndex = Mathf.Clamp(x + kernelExtents, 0, _gridSizeX - 1);

            penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - _grid[removeIndex, y].MovementPenalty + _grid[addIndex, y].MovementPenalty;
         }
      }
      
      for (int x = 0; x < _gridSizeX; x++)
      {
         for (int y = -kernelExtents; y <= kernelExtents; y++)
         {
            int sampleY = Mathf.Clamp(y, 0, kernelExtents);
            penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
         }

         int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
         _grid[x, 0].MovementPenalty = blurredPenalty;

         for (int y = 1; y < _gridSizeY; y++)
         {
            int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, _gridSizeY);
            int addIndex = Mathf.Clamp(y + kernelExtents, 0, _gridSizeY - 1);
            
            penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
            
            blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
            _grid[x, y].MovementPenalty = blurredPenalty;

            if (blurredPenalty > _maxPenalty)
               _maxPenalty = blurredPenalty;
            if (blurredPenalty < _minPenalty)
               _minPenalty = blurredPenalty;
         }
      }
   }

   private void OnDrawGizmos()
   {
      Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

      if (displayGridGizmos)
         if (_grid != null)
         {
            foreach (var node in _grid)
            {
               Gizmos.color = Color.Lerp(Color.white, Color.black,
                  Mathf.InverseLerp(_minPenalty, _maxPenalty, node.MovementPenalty));
               Gizmos.color = node.Walkable ? Gizmos.color : Color.red;
               Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiameter));
            }
         }
   }
}
