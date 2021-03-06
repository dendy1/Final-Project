﻿using UnityEngine;

public class MyPath
{
    public readonly Vector3[] LookPoints;
    public readonly Line[] TurnBoundaries;
    public readonly int FinishLineIndex;
    public readonly int StoppingIndex;

    public MyPath(Vector3[] waypoints, Vector3 startPosition, float turnDistance, float stoppingDistance)
    {
        LookPoints = waypoints;
        TurnBoundaries = new Line[LookPoints.Length];
        FinishLineIndex = TurnBoundaries.Length - 1;

        Vector2 previousPoint = V3toV2(startPosition);

        for (int i = 0; i < LookPoints.Length; i++)
        {
            Vector2 currentPoint = V3toV2(LookPoints[i]);
            Vector2 directionToCurrentPoint = (currentPoint - previousPoint).normalized;
            Vector2 turnBoundary = i == FinishLineIndex ? currentPoint : currentPoint - directionToCurrentPoint * turnDistance;

            TurnBoundaries[i] = new Line(turnBoundary, previousPoint - directionToCurrentPoint * turnDistance);
            
            previousPoint = turnBoundary;
        }

        float distanceFromEnd = 0;
        for (int i = LookPoints.Length - 1; i > 0; i--)
        {
            distanceFromEnd += Vector3.Distance(LookPoints[i], LookPoints[i - 1]);

            if (distanceFromEnd > stoppingDistance)
            {
                StoppingIndex = i;
                break;
            }
        }
    }

    private Vector2 V3toV2(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public void DrawWithGizmos()
    {
        Gizmos.color = Color.black;
        foreach (Vector3 p in LookPoints) 
        {
            Gizmos.DrawCube(p + Vector3.up, Vector3.one);
        }

        Gizmos.color = Color.white;
        foreach (Line l in TurnBoundaries) 
        {
            l.DrawWithGizmos(10);
        }
    }
}
