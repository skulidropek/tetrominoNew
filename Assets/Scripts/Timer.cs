using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private int _timer;
    private int _time;

    public int Time { get => _timer; set => _timer = value; }

    public Timer(int time) 
    {
        _time = time;
    }

    private void Start()
    {
        StartCoroutine(Timer1());
    }

    private IEnumerator Timer1()
    {
        while (true)
        {
            _timer++;
            yield return new WaitForSeconds(_time);
        }
    }
}
