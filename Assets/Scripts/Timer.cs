using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private Stopwatch stopwatch;
    public TimeSpan timeElapsed { get; private set; }

    private void Start() {
        stopwatch = new Stopwatch();
        stopwatch.Start();
    }

    private void Update() {
        timeElapsed = stopwatch.Elapsed;
        UnityEngine.Debug.Log(timeElapsed);
    }

    public void StopTimer() {
        stopwatch.Stop();
    }
}
