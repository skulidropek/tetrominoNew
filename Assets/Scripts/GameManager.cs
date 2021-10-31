using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _pointsText;
    [SerializeField] private static int _height = 20;
    [SerializeField] private static int _width = 10;
    [SerializeField] private float _fallTime;

    public static GameManager instance;
    public float FallTime { get => _fallTime; set => _fallTime = value; }
    public Transform[,] Grid { get => _grid; set => _grid = value; }
    public int Height { get => _height; }
    public int Widht { get =>_width; }
    public int Points { get => _points; set => _points = value; }

    private Transform[,] _grid = new Transform[_width, _height];
    private int _points;
    private int _timer;

    private void Start()
    {
        StartCoroutine(Timer());
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    private void Update()
    {
        if(_timer > 60 && 0.1f < _fallTime)
        {
            _timer = 0;
            _fallTime -= 0.05f;
        }

        foreach (TextMeshProUGUI _point in _pointsText)
            _point.text = $"{_points}";
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            _timer++;
            yield return new WaitForSeconds(1);
        }
    }
}
