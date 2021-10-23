using System;
using UnityEngine;

public class Functions : MonoBehaviour
{
    // private float TIME = 2f;

    public int amountOfPoints;
    public Color graphColor;
    private Graph _graph;
    private LineRenderer _line;
    private float[] _xSteps;
    private float[] _xToPaint;
    private float[] _ySteps;
    private float[] _yToPaint;

    private void Awake()
    {
        _graph = GetComponent<Graph>();
        _line = _graph.InitLine("F1", transform);
    }

    private void Start()
    {
        _xSteps = CreateXSteps(amountOfPoints, _graph.xLength, true);
        _xToPaint = CreateXSteps(amountOfPoints, _graph.xLength, false);

        _ySteps = MathFunction(_xSteps, true);
        _yToPaint = MathFunction(_xSteps, false);

        Debug.Log("\nX Points:\t" + string.Join(",", _xSteps) + "\t length: " + _xSteps.Length +
                  "\nTranslated from:\t" + string.Join(",", _xToPaint) + "\t length: " + _xToPaint.Length);
        Debug.Log("\nY Points:\t" + string.Join(",", _ySteps) + "\t length: " + _ySteps.Length +
                  "\nTranslated from:\t" + string.Join(",", _yToPaint) + "\t length: " + _yToPaint.Length);

        _line.transform.position = Vector3.zero;
        _line.positionCount = _xToPaint.Length;
        _line.sortingOrder = -1;
        _line.startColor = graphColor;
        _line.endColor = graphColor;
        _line.useWorldSpace = false;

        DrawGraph(_line, _xToPaint, _yToPaint);
    }

    private void DrawGraph(LineRenderer line, float[] xP, float[] yP)
    {
        if (xP.Length != yP.Length) throw new FormatException();

        var stat = transform.position;


        for (var i = 0; i < xP.Length; i++)
            if (yP[i] <= _graph.yLength && yP[i] >= 0 - Mathf.Epsilon)
            {
                stat = transform.position + new Vector3(xP[i], yP[i]);
                line.SetPosition(i, stat);
            }
            else
            {
                line.SetPosition(i, stat);
            }
        // StartCoroutine(AnimateDraw(line, xP, yP));
    }

    // TODO not animating
    // IEnumerator AnimateDraw(LineRenderer line, float[] xP, float[] yP)
    // {
    //     Vector3 stat = transform.position;
    //
    //
    //     for (int i = 0; i < xP.Length; i++)
    //     {
    //         for (int j = i; j < xP.Length; j++)
    //         {
    //             if (yP[j] <= _graph.yLength && yP[j] >= 0 - Mathf.Epsilon)
    //             {
    //                 stat = transform.position + new Vector3(xP[i], yP[i]);
    //                 line.SetPosition(j, stat);
    //             }
    //             else
    //             {
    //                 line.SetPosition(j, stat);
    //             }
    //         }
    //
    //         yield return new WaitForSeconds(0.0001f);
    //     }
    // }

    private float[] CreateXSteps(int points, float lengthX, bool scale)
    {
        var xPointGap = lengthX / points;
        points++;
        var length = new float[points];

        float scaler = 1;
        if (scale) scaler = _graph.toMaxX / _graph.xLength;


        for (var i = 0; i < points; i++) length[i] = xPointGap * i * scaler;

        return length;
    }

    private float[] MathFunction(float[] data, bool scale)
    {
        var scaler = _graph.yLength / _graph.toMaxY;
        if (scale) scaler = 1;

        var result = new float[data.Length];

        for (var i = 0; i < data.Length; i++) result[i] = MathFunction(data[i], scaler);

        return result;
    }

    private float MathFunction(float data, float scaler)
    {
        if (data <= 0) data += float.Epsilon;

        var f = Math.Sin(data);
        return (float) f * scaler;
    }
}