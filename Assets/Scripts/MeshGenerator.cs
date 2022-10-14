using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    [SerializeField] private Vector3[] _vertices;
    [SerializeField] private int[] _triangles;
    [SerializeField] private float _terrainHeight;
    [SerializeField] private float _mapScale;
    [SerializeField] private int _mapRadius;
    [SerializeField] private Gradient _gradient;
    private int _mapRadiusX;
    private int _mapRadiusZ;
    private Color[] colors;
    float minTerrainHeight;
    float maxTerrainHeight;


    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        _mapScale = 1f;
        SetMinMaxMapSize();

        GetShape2();
        DrawShape();
    }

    private void Update()
    {
        SetMinMaxMapSize();
        GetShape2();
        DrawShape();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }

    }

    private void GetShape2()
    {
        //_vetices
        _vertices = new Vector3[(_mapRadiusZ + 1) * (_mapRadiusX + 1)];
        Debug.Log(_vertices.Length);

        for (int counter = 0, z = 0; z <= _mapRadiusZ; z++)
        {
            for (int x = 0; x <= _mapRadiusX; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f,z * .3f) * _terrainHeight;
                _vertices[counter++] = new Vector3(x, y, z);

                if(minTerrainHeight > y)
                    minTerrainHeight = y;
                if(maxTerrainHeight < y)
                    maxTerrainHeight = y;

            }
        }

        //triangles
        _triangles = new int[_mapRadiusZ * _mapRadiusX * 6];

        int verticeNumber = 0;
        int tableCounter = 0;
        for(int x = 0; x < _mapRadiusX; x++)
        {
            for (int z = 0; z < _mapRadiusZ; z++)
            {
                _triangles[tableCounter++] = verticeNumber;
                _triangles[tableCounter++] = verticeNumber + _mapRadiusZ + 1;
                _triangles[tableCounter++] = verticeNumber + 1;

                _triangles[tableCounter++] = verticeNumber + 1;
                _triangles[tableCounter++] = verticeNumber + _mapRadiusZ + 1;
                _triangles[tableCounter++] = verticeNumber + _mapRadiusZ + 2;

                verticeNumber++;

            }
            verticeNumber++;
        }

        colors = new Color[_vertices.Length];
        for (int counter = 0, z = 0; z <= _mapRadiusZ; z++)
        {
            for (int x = 0; x <= _mapRadiusX; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, _vertices[counter].y);
                colors[counter++] = _gradient.Evaluate(height);
            }
        }

        transform.transform.localScale = new Vector3(1, 1, 1)*_mapScale;

    }


    private void GetShape()
    {
        //these are dots which creates shape
        _vertices = new Vector3[]
        {
            new Vector3(0,0,0), //0
            new Vector3(0,0,1), //1
            new Vector3(1,0,0), //2
            new Vector3(1,0,1)  //3
        };

        //dots has to be ordered clockwise
        _triangles = new int[]
        {
            0,1,2,
            1,3,2
        };
    }

    private void DrawShape()
    {
        mesh.Clear();
        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    private void SetMinMaxMapSize()
    {
        if (_mapRadius < 1)
        {
            _mapRadius = 1;
            _mapRadiusX = 1;
            _mapRadiusZ = 1;
        }
        if(_mapRadius > 50)
        {
            _mapRadius = 50;
            _mapRadiusX = 50;
            _mapRadiusZ = 50;
        }
        else
        {
            _mapRadiusX = _mapRadius;
            _mapRadiusZ = _mapRadius;
        }

        if(_terrainHeight < 1)
        {
            _terrainHeight = 1;
        }
        if(_terrainHeight > 15)
        {
            _terrainHeight = 15;
        }

    }

    /*private void OnDrawGizmos()
    {
        if (_vertices == null)
            return;
        
        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.DrawSphere(_vertices[i], .1f);
        }
    }*/
}
