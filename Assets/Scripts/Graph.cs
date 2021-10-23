using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    // Constants
    private const float ErrorMargin = float.Epsilon;
    private const float WidthDivider = 1.5f;
    private const float MaxX = 15f;
    private const float MaxY = 7f;

    public GameObject canvasGameObj;

    // Axis
    [Header("Axis Configuration")] [Range(1.3f, MaxX)]
    public float xLength;

    [Range(1.3f, MaxY)] public float yLength;
    [Range(0.0011f, 0.3f)] public float lineWidth;

    public Material materialLine;
    public Color colorForLine = Color.black;

    // Index
    [Header("Index Configuration")] [Range(0.1f, 1f)]
    public float indexWidth;

    public int toMaxX;
    public int toMaxY;

    // Arrow head
    [Header("Arrowhead Configuration")] public bool enableArrowheadX = true;
    public bool enableArrowheadY = true;
    private bool _changedEnableArrowheadX;
    private bool _changedEnableArrowheadY;
    private float _changedIndexWidth;
    private int _changedMaxX;
    private int _changedMaxY;
    private float _changedWidth;
    private float _changedX;
    private float _changedY;
    private LineRenderer _headX;
    private LineRenderer _headY;
    private LineRenderer _lineX;
    private LineRenderer _lineY;

    // Misc
    private Vector3 _posOfOrigin;
    private LineRenderer[] _xBarList;
    private int _xIndex;
    private Text[] _xNumList;
    private LineRenderer[] _yBarList;
    private int _yIndex;
    private Text[] _yNumList;

    // Inits all variables
    private void Awake()
    {
        this._posOfOrigin = transform.position;

        // Inits the Lines
        _lineX = InitLine("Axis X");
        _lineY = InitLine("Axis Y");

        // Inits Index
        _xIndex = (int) Mathf.Floor(MaxX);
        _yIndex = (int) Mathf.Floor(MaxY);
        InitIndexLists();
        InitIndexNum();

        // Make arrow head
        _headX = InitHead("Arrowhead X");
        _headX.transform.Rotate(new Vector3(0, 0, -90));
        _headY = InitHead("Arrowhead Y");
    }

    // Starts first update cycle
    private void Start()
    {
        // Init first refresh
        UpdatedComponents();
    }

    // Checks if values were changed and then updates it
    private void Update()
    {
        if (ChangeVal())
        {
            UpdatedComponents();
        }
    }

    // Updates all components
    private void UpdatedComponents()
    {
        DrawAxis();
        MoveHead();
        DrawIndex();
        DrawIndexNumber();
    }

    // Inits a usable Linerenderer for this project
    private LineRenderer InitLine(string lineName)
    {
        var l = new GameObject(lineName).AddComponent<LineRenderer>();
        l.gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        l.material = materialLine;
        l.startColor = colorForLine;
        l.endColor = colorForLine;
        l.startWidth = lineWidth;
        l.endWidth = lineWidth;
        return l;
    }

    // Inits the array line for all possible index lines
    private void InitIndexLists()
    {
        _xBarList = new LineRenderer[_xIndex];
        _yBarList = new LineRenderer[_yIndex];
        IndexSetter(_xBarList, "Index X", true);
        IndexSetter(_yBarList, "Index Y", false);

        _xNumList = new Text[_xIndex];
        _yNumList = new Text[_yIndex];
    }

    // Inits the array for all possible numbers
    private void InitIndexNum()
    {
        _xNumList = new Text[_xIndex];
        _yNumList = new Text[_yIndex];

        for (int i = 0; i < _xNumList.Length; i++)
        {
            _xNumList[i] = InitNum("Number X");
            _xNumList[i].fontStyle = FontStyle.Bold;
            _xNumList[i].transform.position = new Vector3(-7, -3.7f) + new Vector3(i + 1, 0);
        }

        for (int i = 0; i < _yNumList.Length; i++)
        {
            _yNumList[i] = InitNum("Number Y");
            _yNumList[i].fontStyle = FontStyle.Bold;
            _yNumList[i].transform.position = new Vector3(-7.5f, -3.27f) + new Vector3(0, i + 1);
        }
    }

    // Inits a single number
    private Text InitNum(string nameObject)
    {
        Text txt = new GameObject(nameObject).AddComponent<Text>();
        txt.transform.position = _posOfOrigin;
        txt.alignment = TextAnchor.UpperCenter;
        txt.transform.SetParent(canvasGameObj.transform, false);
        txt.color = colorForLine;
        txt.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        txt.fontSize = 40;

        return txt;
    }

    // Inits a usable Arrowhead for this project
    private LineRenderer InitHead(string lineName)
    {
        LineRenderer l = InitLine(lineName);
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
        for (var i = 0; i < _xBarList.Length; i++)
        {
            _xBarList[i].enabled = CheckIfAppears(i, enableArrowheadX, xLength);
        }

        for (var i = 0; i < _yBarList.Length; i++)
        {
            _yBarList[i].enabled = CheckIfAppears(i, enableArrowheadY, yLength);
        }
    }

    private void DrawIndexNumber()
    {
        float tmpX = toMaxX / xLength;
        for (int i = 0; i < _xNumList.Length; i++)
        {
            if (CheckIfAppears(i, enableArrowheadX, xLength))
            {
                float calc = Mathf.Round(tmpX * (i + 1));
                _xNumList[i].text = calc.ToString(CultureInfo.CurrentCulture);
                _xNumList[i].enabled = true;
            }
            else _xNumList[i].enabled = false;
        }

        float tmpY = toMaxY / yLength;
        for (int i = 0; i < _yNumList.Length; i++)
        {
            if (CheckIfAppears(i, enableArrowheadY, yLength))
            {
                float calc = Mathf.Round(tmpY * (i + 1));
                _yNumList[i].text = calc.ToString(CultureInfo.CurrentCulture);
                _yNumList[i].enabled = true;
            }
            else _yNumList[i].enabled = false;
        }
    }

    private bool CheckIfAppears(int cycle, bool arrowHead, float len)
    {
        return cycle + 1 < len || (!arrowHead && cycle + 1 <= len);
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
            ind[i] = InitLine(objectName);
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
            toMaxX == _changedMaxX &&
            toMaxY == _changedMaxY &&
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
        this._changedMaxX = toMaxX;
        this._changedMaxY = toMaxY;
        return true;
    }

    // Calculates if floats are equal
    private bool BoolfEquals(float a, float b)
    {
        return Math.Abs(a - b) < ErrorMargin;
    }
}