using System;
using UnityEngine;

public class Graph : MonoBehaviour
{
    // Constants
    private const float ErrorMargin = float.Epsilon;
    private const float WidthDivider = 1.5f;

    // Axis
    [Header("Axis Configuration")] 
    [Range(0.01f, 15.0f)] public float xLength;
    [Range(0.001f, 7.0f)] public float yLength;
    [Range(0.0011f, 0.3f)] public float lineWidth;
    [Range(0.1f, 1f)] public float indexWidth;
    public Material materialLine;
    public Gradient colorForLine;
    private LineRenderer _lineX;
    private LineRenderer _lineY;
    private LineRenderer[] _xList;
    private LineRenderer[] _yList;

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
    private float _changedIndexWidth;


    private void Start()
    {
        this._posOfOrigin = transform.position;

        // Inits the Lines
        _lineX = InitLine();
        _lineX.name = "X Axis";
        _lineY = InitLine();
        _lineY.name = "Y Axis";

        // Make arrow head
        _headX = InitHead();
        _headX.name = "Arrowhead X";
        _headX.transform.Rotate(new Vector3(0, 0, -90));
        _headY = InitHead();
        _headY.name = "Arrowhead Y";

        // Init first lines and values
        ChangeVal();
        DrawAxis();
        MoveHead();
        DrawIndex();
    }

    private void Update()
    {
        if (ChangeVal())
        {
            DrawAxis();
            MoveHead();
            DrawIndex();
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
    }

    // Creates the indices and keeps them updated
    private void DrawIndex()
    {
        if (_xList != null && _yList != null)
        {
            foreach (var lis in _xList)
            {
                Destroy(lis.gameObject);
            }

            foreach (var lis in _yList)
            {
                Destroy(lis.gameObject);
            }
        }

        int xIndex = (int) Mathf.Floor(xLength);
        if (0 == xIndex - xLength && enableArrowheadX) xIndex--;

        int yIndex = (int) Mathf.Floor(yLength);
        if (0 == yIndex - yLength && enableArrowheadY) yIndex--;

        _xList = new LineRenderer[xIndex];
        _yList = new LineRenderer[yIndex];

        IndexSetter(_xList, "X Index", true);
        IndexSetter(_yList, "Y Index", false);
    }

    // Sets the indices to their place
    private void IndexSetter(LineRenderer[] ind, string objectName, bool t)
    {
        var x = 0;
        var y = 0;

        if (t) x = 1;
        else y = 1;

        var widthDivider = lineWidth / WidthDivider;
        for (int i = 0; i < ind.Length; i++)
        {
            ind[i] = InitLine();
            ind[i].name = objectName;
            ind[i].startWidth = widthDivider;
            ind[i].endWidth = widthDivider;
            ind[i].SetPosition(0,
                new Vector3(x * (i + 1) + y * (-indexWidth), y * (i + 1) + x * (-indexWidth)) + _posOfOrigin);
            ind[i].SetPosition(1,
                new Vector3(x * (i + 1) + y * (indexWidth), y * (i + 1) + x * (indexWidth)) + _posOfOrigin);
        }
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
        if (BoolfEquals(_changedX, xLength) &&
            BoolfEquals(_changedY, yLength) &&
            BoolfEquals(_changedWidth, lineWidth) &&
            BoolfEquals(_changedIndexWidth, indexWidth) &&
            enableArrowheadX == _changedEnableArrowheadX &&
            enableArrowheadY == _changedEnableArrowheadY) return false;

        Debug.Log("\t\txLength: " + xLength + "\t_changedX: " + _changedX + "\n" +
                  "\t\tyLength: " + yLength + "\t_changedY: " + _changedY + "\n" +
                  "\t\tlineWidth: " + lineWidth + "\t_changedWidth: " + _changedWidth + "\n" +
                  "\t\tenableArrowheadX: " + enableArrowheadX + "\t_changedEnableArrowheadX: " +
                  _changedEnableArrowheadX + "\n" +
                  "\t\tenableArrowheadY: " + enableArrowheadY + "\t_changedEnableArrowheadY: " +
                  _changedEnableArrowheadY + "\n" +
                  "\t\tindexWidth: " + indexWidth + "\t_changedIndexWidth: " + _changedIndexWidth);

        this._changedX = this.xLength;
        this._changedY = this.yLength;
        this._changedWidth = this.lineWidth;
        this._changedEnableArrowheadX = this.enableArrowheadX;
        this._changedEnableArrowheadY = this.enableArrowheadY;
        this._changedIndexWidth = this.indexWidth;
        return true;
    }

    // Calculates if floats are equal
    private bool BoolfEquals(float a, float b)
    {
        return Math.Abs(a - b) < ErrorMargin;
    }
}