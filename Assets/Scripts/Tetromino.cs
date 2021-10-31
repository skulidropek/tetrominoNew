using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    [SerializeField] private Vector3 rotationPoint;
    [SerializeField] private bool _turn;

    private static int _width;
    private static int _height;
    private float _fallTime;
    private int _points;

    private static Transform[,] _grid;
    private float _previusTime;

    private void Start()
    {
        _width = GameManager.instance.Widht;
        _height = GameManager.instance.Height;
        _fallTime = GameManager.instance.FallTime;
        _grid = GameManager.instance.Grid;
        _points = GameManager.instance.Points;
    }

    private void OnDisable()
    {
        GameManager.instance.Points = _points;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            PositionMovement(new Vector3(-1f, 0f, 0f));
        else if (Input.GetKeyDown(KeyCode.D))
            PositionMovement(new Vector3(1f, 0f, 0f));
        else if (Input.GetKeyDown(KeyCode.W))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), _turn ? 90 : 0);
            if(!ValidMove())
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
        }

        if (Time.time - _previusTime > (Input.GetKey(KeyCode.S) ? _fallTime / 10 : _fallTime))
        {
            transform.position += new Vector3(0f, -1f, 0f);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0f, -1f, 0f);

                AddToGrid();
                CheckForLines();

                this.enabled = false;

                if (transform.position.y >= _height - 3) //ѕровер€ет позицию фигуры по y и если она выше нужного то выводит канвас поражени€
                {
                    GameObjectExtension.Find("EndGameCanvas").SetActive(true);
                    GameObjectExtension.Find("MainCanvas").SetActive(false);
                }
                else
                {
                    FindObjectOfType<SpawnerTetromino>().NewTetromino();
                }
            }
            _previusTime = Time.time;
        }
    }

    private void PositionMovement(Vector3 vector3)
    {
        transform.position += vector3;
        if (!ValidMove())
            transform.position -= vector3;
    }

    private void CheckForLines()
    {
        for (int i = _height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                _points++;
                RowDown(i);
            }

        }
    }

    private bool HasLine(int i)
    {
        for (int j = 0; j < _width; j++)
        {
            if (_grid[j, i] == null)
                return false;
        }
        return true;
    }

    private void DeleteLine(int i)
    {
        for (int j = 0; j < _width; j++)
        {
            Destroy(_grid[j, i].gameObject);
        }
    }

    private void RowDown(int i)
    {
        for (int y = i + 1; y < _height; y++)
        {
            for (int j = 0; j < _width; j++)
            {
                if (_grid[j, y] != null)
                {
                    _grid[j, y - 1] = _grid[j, y];
                    _grid[j, y] = null;
                    _grid[j, y - 1].position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    private void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            _grid[roundedX, roundedY] = children;
        }
    }
    public bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= _width || roundedY < 0 || roundedY >= _height)
                return false;

            if (_grid[roundedX, roundedY] != null)
                return false;
        }

        return true;
    }
}
