using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Unity.Jobs;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PathRequestManager : MonoBehaviour
{
   
    public static PathRequestManager Instance { get; set; }
    
    private Pathfinding _pathfinding;
    private ConcurrentQueue<PathResult> _pathResults;
    
    private Thread _pathFindingThread;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one PathRequestManager on the scene!");
            return;
        }

        Instance = this;
        _pathfinding = GetComponent<Pathfinding>();
        _pathResults = new ConcurrentQueue<PathResult>();
    }

    private void Update()
    {
        if (!_pathResults.IsEmpty)
        {
            for (int i = 0; i < _pathResults.Count; i++)
            {
                PathResult result;
                if (_pathResults.TryDequeue(out result))
                {
                    result.Callback(result.Path, result.PathSuccessfull);
                }
            }
        }
    }
    
    public void RequestPath(PathRequest request)
    {
        Thread newThread = new Thread(() => _pathfinding.FindPath(request, OnFinishedProcessingPath));
        newThread.Start();
    }

    public void OnFinishedProcessingPath(PathResult result)
    {
        _pathResults.Enqueue(result);
    }
}    

public struct PathRequest
{
    public Vector3 PathStart { get; set; }
    public Vector3 PathEnd { get; set; }
    public Action<Vector3[], bool> Callback { get; set; }

    public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathStart = pathStart;
        PathEnd = pathEnd;
        Callback = callback;
    }
}

public struct PathResult
{
    public Vector3[] Path { get; }
    public bool PathSuccessfull { get; }
    public Action<Vector3[], bool> Callback { get;  }

    public PathResult(Vector3[] path, bool pathSuccessfull, Action<Vector3[], bool> callback)
    {
        Path = path;
        PathSuccessfull = pathSuccessfull;
        Callback = callback;
    }
}