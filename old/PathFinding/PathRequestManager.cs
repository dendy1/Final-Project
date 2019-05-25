using System;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PathRequestManager : MonoBehaviour
{
    private struct PathRequest
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
    
    public static PathRequestManager Instance { get; set; }

    private Pathfinding _pathfinding;
    private Queue<PathRequest> _pathRequestsQueue;
    private PathRequest _currentPathRequest;

    private bool _isProcessingPath;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one PathRequestManager on the scene!");
            return;
        }

        Instance = this;
        _pathfinding = GetComponent<Pathfinding>();
        _pathRequestsQueue = new Queue<PathRequest>();
    }

    public void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        _pathRequestsQueue.Enqueue(newRequest);
        TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!_isProcessingPath && _pathRequestsQueue.Count > 0)
        {
            _currentPathRequest = _pathRequestsQueue.Dequeue();
            _isProcessingPath = true;
            _pathfinding.StartFindPath(_currentPathRequest.PathStart, _currentPathRequest.PathEnd);
        }
    }

    public void OnFinishedProcessingPath(Vector3[] pathPoints, bool success)
    {
        _currentPathRequest.Callback(pathPoints, success);
        _isProcessingPath = false;
        TryProcessNext();
    }
}    
