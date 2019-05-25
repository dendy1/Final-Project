using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
    private const float VerticalSlope = 1e5f;
    
    private float _slope;
    private float _perpendicularSlope;

    private float _yInterception;

    private Vector2 _point1, _point2;
    private bool approachSide;

    public Line(Vector2 pointOnLine, Vector2 pointPerpendicularToLine)
    {
        float dx = pointOnLine.x - pointPerpendicularToLine.x;
        float dy = pointOnLine.y - pointPerpendicularToLine.y;

        if (dx == 0)
            _perpendicularSlope = VerticalSlope;
        else
            _perpendicularSlope = dy / dx;

        if (_perpendicularSlope == 0)
            _slope = VerticalSlope;
        else
            _slope = -1 / _perpendicularSlope;

        _yInterception = pointOnLine.y - _slope * pointOnLine.x;

        _point1 = pointOnLine;
        _point2 = pointOnLine + new Vector2(1, _slope);

        approachSide = false;
        approachSide = GetSide(pointPerpendicularToLine);;
    }

    private bool GetSide(Vector2 v)
    {
        return (v.x - _point1.x) * (_point2.y - _point1.y) > (v.y - _point1.y) * (_point2.x - _point1.x);
    }

    public bool HasCrossedLine(Vector2 v)
    {
        return GetSide(v) != approachSide;
    }
    
    public float DistanceFromPoint(Vector2 p) {
        float yInterceptPerpendicular = p.y - _perpendicularSlope * p.x;
        float intersectX = (yInterceptPerpendicular - _yInterception) / (_slope - _perpendicularSlope);
        float intersectY = _slope * intersectX + _yInterception;
        return Vector2.Distance (p, new Vector2 (intersectX, intersectY));
    }
    
    public void DrawWithGizmos(float length) 
    {
        Vector3 lineDir = new Vector3 (1, 0, _slope).normalized;
        Vector3 lineCentre = new Vector3 (_point1.x, 0, _point1.y) + Vector3.up;
        Gizmos.DrawLine(lineCentre - lineDir * length / 2f, lineCentre + lineDir * length / 2f);
    }
}