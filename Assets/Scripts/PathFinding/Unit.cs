using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Transform _target;
    private float _speed = 5f;
    private Vector3[] _path;
    private int _targetIndex;

    public Transform Target
    {
        get { return _target; }
        set
        {
            _target = value;
            PathRequestManager.Instance.RequestPath(transform.position, _target.position, OnPathFound);
        }
    }

    public void OnPathFound(Vector3[] path, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            _path = path;
            _targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    private IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = _path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                _targetIndex++;
                if (_targetIndex >= _path.Length)
                {
                    yield break;
                }

                currentWaypoint = _path[_targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, _speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (_path != null)
        {
            for (int i = _targetIndex; i < _path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(_path[i], Vector3.one);
                
                if (i == _targetIndex)
                    Gizmos.DrawLine(transform.position, _path[i]);
                else
                    Gizmos.DrawLine(_path[i-1], _path[i]);
            }
        }
    }
}
