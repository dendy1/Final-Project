﻿using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const float MinimumPathUpdateTime = .2f;
    private const float PathUpdateMoveThreshhold = 0.5f;
    
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float turnDistance = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float stoppingDistance = 5f;
    [SerializeField] private Transform target;
    
    private MyPath _path;
    
    private Vector3[] _waypoints;
    private int _index;

    private void Start()
    {
        StartCoroutine("UpdatePath");
    }

    public void OnPathFound(Vector3[] path, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            _waypoints = path;
            _path = new MyPath(path, transform.position, turnDistance, stoppingDistance);
            StopCoroutine("FollowPath1");
            StartCoroutine("FollowPath1");
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        StopCoroutine("UpdatePath");
        StartCoroutine("UpdatePath");
    }

    private IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < 0.2f)
            yield return new WaitForSeconds(0.2f);
        
        PathRequestManager.Instance.RequestPath(transform.position,target.position, OnPathFound);
        
        float sqrMoveThreshHold = PathUpdateMoveThreshhold * PathUpdateMoveThreshhold;
        Vector3 lastTargetPosition = target.position;
        
        while (true)
        {
            yield return new WaitForSeconds(MinimumPathUpdateTime);
            if ((target.position - lastTargetPosition).sqrMagnitude > sqrMoveThreshHold)
            {
                _index = 0;
                PathRequestManager.Instance.RequestPath(transform.position,target.position, OnPathFound);
                lastTargetPosition = target.position;
            }
        }
    }

    private IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(_path.LookPoints[0]);
        
        while (followingPath)
        {
            Vector2 position2D = new Vector2(transform.position.x, transform.position.z);
            
            if (_path.TurnBoundaries[pathIndex].HasCrossedLine(position2D))
                if (pathIndex == _path.FinishLineIndex)
                {
                    followingPath = false;
                    break;
                }
            else
                pathIndex++;

            if (followingPath)
            {
                Quaternion targetRotation = Quaternion.LookRotation (_path.LookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                var tmp = transform.position.y;
                transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed * 1, Space.Self);
                transform.position = new Vector3(transform.position.x, tmp, transform.position.z);
            }
            
            yield return null;
        }
    }
    
    private IEnumerator FollowPath1()
    {
        Vector3 currentWaypoint = _waypoints[0];
        while (true) {
            if (transform.position == currentWaypoint) {
                _index ++;
                if (_index >= _waypoints.Length) {
                    
                    yield break;
                }
                currentWaypoint = _waypoints[_index];
            }

            transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,movementSpeed * Time.deltaTime);
            yield return null;

        }
    }

    private void OnDrawGizmos()
    {
        if (_path != null)
        {
            foreach (var w in _waypoints)
            {
                Gizmos.DrawCube(w, Vector3.one);
            }
            _path.DrawWithGizmos();
        }
    }
}
