using System;
using UnityEngine;

public class Graph : MonoBehaviour
{
    // Constants
    private const float ErrorMargin = float.Epsilon;

    // Axis
    [Header("Axis Configuration")] [Range(0.0f, 15.0f)]
    public float xLength;

    [Range(0.0f, 7.0f)] public float yLength;
    [Range(0.01f, 0.3f)] public float lineWidth;
    public Material materialLine;
    public Gradient colorForLine;
    private LineRenderer _lineX;
    private LineRenderer _lineY;

    // Arrow head
    public bool enableArrowheadX = true;
    public bool enableArrowheadY = true;
    private LineRenderer _headX;
    private LineRenderer _headY;

    // Misc
    private Vector3 _posOfOrigin;
    private float _changedWidth;
    private float _changedX;
    private float _changedY;
    private bool _changedEnableArrowheadX;
    private bool _changedEnableArrowheadY;


    private void Start()
    {
        this._posOfOrigin = transform.position;

        // Inits the Lines
        _lineX = InitLine();
        _lineY = InitLine();

        // Make arrow head
        _headX = InitHead();
        _headX.transform.Rotate(new Vector3(0, 0, -90));
        _headY = InitHead();

        // Init first lines and values
        ChangeVal();
        DrawAxis();
    }

    private void Update()
    {
        if (ChangeVal())
        {
            DrawAxis();
        }
    }

    // Inits a usable Linerenderer for this project
    private LineRenderer InitLine()
    {
        LineRenderer l = new GameObject().AddComponent<LineRenderer>();
        l.gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        l.material = materialLine;
        l.colorGradient = colorForLine;
        l.startWidth = lineWidth;
        l.endWidth = lineWidth;
        return l;
    }

    // Inits a usable Arrowhead for this project
    private LineRenderer InitHead()
    {
        LineRenderer l = InitLine();
        l.positionCount = 3;
        l.useWorldSpace = false;

        l.SetPosition(0, new Vector3(-0.25f, 0f));
        l.SetPosition(1, new Vector3(0f, 0.25f));
        l.SetPosition(2, new Vector3(0.25f, 0));
        l.transform.position = Vector3.zero;

        return l;
    }

    // Draws the axis with it's correct length
    private void DrawAxis()
    {
        _lineX.endWidth = lineWidth;
        _lineX.startWidth = lineWidth;
        _lineX.SetPosition(0, this._posOfOrigin);
        _lineX.SetPosition(1, this._posOfOrigin + new Vector3(this.xLength, 0));

        _lineY.endWidth = lineWidth;
        _lineY.startWidth = lineWidth;
        _lineY.SetPosition(0, this._posOfOrigin - new Vector3(0, lineWidth / 2));
        _lineY.SetPosition(1, this._posOfOrigin + new Vector3(0, this.yLength));

        _headX.endWidth = lineWidth;
        _headX.startWidth = lineWidth;

        _headY.endWidth = lineWidth;
        _headY.startWidth = lineWidth;

        MoveHead();
    }

    // Moves the arrowhead to the end of the axis
    private void MoveHead()
    {
        _headX.enabled = enableArrowheadX;
        _headY.enabled = enableArrowheadY;

        if (enableArrowheadX)
        {
            _headX.transform.position = this._posOfOrigin + new Vector3(xLength - 0.25f, 0);
        }

        if (enableArrowheadY)
        {
            _headY.transform.position = this._posOfOrigin + new Vector3(0, yLength - 0.25f);
        }
    }

    // Checks if the values of the axis was changed
    private bool ChangeVal()
    {
        if (Math.Abs(this._changedX - this.xLength) < ErrorMargin &&
            Math.Abs(this._changedY - this.yLength) < ErrorMargin &&
            Math.Abs(this._changedWidth - this.lineWidth) < ErrorMargin &&
            enableArrowheadX == _changedEnableArrowheadX &&
            enableArrowheadY == _changedEnableArrowheadY) return false;

        Debug.Log("\t\txLength: " + xLength + "\t_changedX: " + _changedX + "\n" +
                  "\t\tyLength: " + yLength + "\t_changedY: " + _changedY + "\n" +
                  "\t\tlineWidth: " + lineWidth + "\t_changedWidth: " + _changedWidth + "\n" +
                  "\t\tenableArrowheadX: " + enableArrowheadX + "\t_changedEnableArrowheadX: " +
                  _changedEnableArrowheadX + "\n" +
                  "\t\tenableArrowheadY: " + enableArrowheadY + "\t_changedEnableArrowheadY: " +
                  _changedEnableArrowheadY);

        this._changedX = this.xLength;
        this._changedY = this.yLength;
        this._changedWidth = this.lineWidth;
        this._changedEnableArrowheadX = this.enableArrowheadX;
        this._changedEnableArrowheadY = this.enableArrowheadY;
        return true;
    }
}