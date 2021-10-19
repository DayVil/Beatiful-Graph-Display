using System;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [Range(0.0f, 10.0f)] public float xLength = 3f;
    [Range(0.0f, 10.0f)] public float yLength = 3f;

    [Range(0.1f, 5f)] public float lineWidth;
    public Material materialLine;
    public Gradient colorForLine;
    public LineRenderer _lineX;
    public LineRenderer _lineY;

    private float _changedWidth;
    private float _changedX;
    private float _changedY;

    // Start is called before the first frame update
    private void Start()
    {
        // Scaff
        _lineX = new GameObject().AddComponent<LineRenderer>();
        _lineX.gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        _lineX.material = materialLine;
        _lineX.colorGradient = colorForLine;
        _lineX.SetPosition(0, new Vector3(2, 2));
        _lineX.SetPosition(1, new Vector3(-10, -10));
        
        ChangeVal();
        DrawGraph();
    }

    // Update is called once per frame
    // TODO GRAPH LOS!
    private void Update()
    {
        if (ChangeVal())
        {
            Debug.Log("Changed!");
        }
    }

    private void DrawGraph()
    {
        DrawLine(new Vector2(this.xLength, 0f));
        DrawLine(new Vector2(0f, this.yLength));
    }

    // TODO to implement!
    private void DrawLine(Vector2 p0)
    {
        // throw new NotImplementedException();
    }

    private bool ChangeVal()
    {
        if (!(Math.Abs(this._changedX - this.xLength) < float.Epsilon &&
              Math.Abs(this._changedY - this.yLength) < float.Epsilon &&
              Math.Abs(this._changedWidth - this.lineWidth) < float.Epsilon))
        {
            this._changedX = this.xLength;
            this._changedY = this.yLength;
            this._changedWidth = this.lineWidth;

            return true;
        }

        return false;
    }
}