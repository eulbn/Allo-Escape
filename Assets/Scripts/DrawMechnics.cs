using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class DrawMechnics : MonoBehaviour
{
    public float yaxisArrangement;

    public GameObject linePrefab;
    public float pointDistance = 0.1f;
    List<GameObject> createdLine;
    LineRenderer lineRenderer;

    public class LinePoints { public List<Vector3> singleLinePoints; public LinePoints() { singleLinePoints = new List<Vector3>(); } }

    public List<LinePoints> linesPoints;

    public List<int> touchedLine;
    int currentLine = -1;
    bool selectedDestoyed = false;
    private void Start()
    {
        linesPoints = new List<LinePoints>();
        createdLine = new List<GameObject>();
        touchedLine = new List<int>();
        
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            selectedDestoyed = true;
            CreateLine();
        }
        
        if (Input.GetMouseButton(0))
        {
            if(!touchedLine.Contains(currentLine) && selectedDestoyed)
                UpdateLine();
        }

        if(linesPoints.Count >= 2)
        {
            DestroyLine(0, false, 0);
        }

        for(int i = 0; i < touchedLine.Count; i++)
        {
            DestroyLine(touchedLine[i], true, i);
        }
        
    }
    void DestroyLine(int lineToBeDestroyed, bool touched, int touhcedIndex)
    {
        if (linesPoints[lineToBeDestroyed].singleLinePoints.Count != 0)
        {
            linesPoints[lineToBeDestroyed].singleLinePoints.RemoveAt(0);
            if (createdLine[lineToBeDestroyed].GetComponent<LineRenderer>())
            {
                Vector3[] tem = linesPoints[lineToBeDestroyed].singleLinePoints.ToArray();
                createdLine[lineToBeDestroyed].GetComponent<LineRenderer>().SetPositions(tem);
            }
        }
        else
        {
            linesPoints.RemoveAt(lineToBeDestroyed);
            Destroy(createdLine[lineToBeDestroyed]);
            createdLine.RemoveAt(lineToBeDestroyed);
            if (touched)
            {
                touchedLine.RemoveAt(touhcedIndex);
                selectedDestoyed = false;
            }
            
            currentLine--;
        }

        
    }
    void CreateLine()
    {
        GetMousePostion();
        linesPoints.Add(new LinePoints());
        currentLine += 1;
        linesPoints[currentLine].singleLinePoints.Add(GetMousePostion());
        createdLine.Add(Instantiate(linePrefab, Vector3.zero, Quaternion.identity));
        lineRenderer = createdLine[currentLine].GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, linesPoints[currentLine].singleLinePoints[linesPoints[currentLine].singleLinePoints.Count - 1]);

    }

    void UpdateLine()
    {
        Vector3 temTouchPostion = GetMousePostion();
        if ((linesPoints[currentLine].singleLinePoints[linesPoints[currentLine].singleLinePoints.Count - 1] - temTouchPostion).magnitude > pointDistance)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, temTouchPostion);
            linesPoints[currentLine].singleLinePoints.Add(temTouchPostion);

        }
    }

    RaycastHit hit;
    Vector3 GetMousePostion()
    {
        Vector3 tem = hit.point;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
        if (Physics.Raycast(ray.origin, ray.direction * 1000, out hit, Mathf.Infinity))
        {
            
            tem = new Vector3(tem.x, tem.y + yaxisArrangement, tem.z);
            
        }
        
        return tem;
            
    }
}