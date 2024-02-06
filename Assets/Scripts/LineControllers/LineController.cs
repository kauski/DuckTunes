using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineController))]
public class LineController : MonoBehaviour
{
    [SerializeField] List<Transform> nodes;
    LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = nodes.Count;
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPositions(nodes.ConvertAll(n => n.position - new Vector3(0, 0, 5)).ToArray());
    }
    public Vector3[] GetPositions()
    {
        Vector3[] positions = new Vector3[lr.positionCount];
        lr.GetPositions(positions);
        return positions;
    }
    
}

