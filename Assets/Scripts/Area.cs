using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] GameObject _area;

    float _xLeftBound;
    public float xLeftBound => _xLeftBound;
    float _xRightBound;
    public float xRightBound => _xRightBound;
    float _yLeftBound;
    public float yLeftBound => _yLeftBound;
    float _yRightBound;
    public float yRightBound => _yRightBound;
    float _zLeftBound;
    public float zLeftBound => _zLeftBound;
    float _zRightBound;
    public float zRightBound => _zRightBound;

    public float width => _area.transform.localScale.x;
    public float height => _area.transform.localScale.y;
    public float depth => _area.transform.localScale.z;

    void Start()
    {
        
    }

    void Update()
    {
        _xLeftBound = _area.transform.position.x - _area.transform.localScale.x / 2f;
        _xRightBound = _area.transform.position.x + _area.transform.localScale.x / 2f;
        _yLeftBound = _area.transform.position.y - _area.transform.localScale.y / 2f;
        _yRightBound = _area.transform.position.y + _area.transform.localScale.y / 2f;
        _zLeftBound = _area.transform.position.z - _area.transform.localScale.z / 2f;
        _zRightBound = _area.transform.position.z + _area.transform.localScale.z / 2f;
    }

    public bool IsInBound(Vector3 position)
    {
        return position.x > _xLeftBound && position.x < _xRightBound &&
                position.y > _yLeftBound && position.y < _yRightBound &&
                position.z > _zLeftBound && position.z < _zRightBound;
    }

    public Vector3 GetInBoundPosition(Vector3 position)
    {
        if (position.x < _xLeftBound) 
        {
            position.x += width;
        }

        if (position.x > _xRightBound)
        {
            position.x -= width;
        }
        
        if (position.y < _yLeftBound)
        {
            position.y += height;
        }
        
        if (position.y > _yRightBound)
        {
            position.y -= height;
        }
        
        if (position.z < _zLeftBound)
        {
            position.z += depth;
        }
        
        if (position.z > _zRightBound)
        {
            position.z -= depth;
        }
        return position;
    }
}