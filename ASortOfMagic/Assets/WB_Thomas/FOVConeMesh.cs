using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVConeMesh : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    Mesh mesh;
    RaycastHit2D hit;
    [SerializeField] float meshRes = 2;
    [HideInInspector] public Vector3[] vertices;
    [HideInInspector] public int[] triangles;
    [HideInInspector] public int stepCount;
    public LayerMask obstacleMask, playerMask;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        enemy = GetComponentInParent<Enemy>();
        
        transform.rotation = enemy.transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        MakeMesh();
    }

    void MakeMesh()
    {
        stepCount = Mathf.RoundToInt(enemy.angle * meshRes);
        float stepAngle = enemy.angle / stepCount;

        List<Vector3> viewVertex = new List<Vector3>();

        hit = new RaycastHit2D();
        for (int i = 0; i < stepCount; i++)
        {
            float angle = enemy.transform.eulerAngles.y - enemy.angle / 2 + stepAngle * i;
            Vector3 dir = enemy.DirFromAngle(angle, false);
            hit = Physics2D.Raycast(enemy.transform.position, dir, enemy.radius, obstacleMask);
            if (hit.collider == null)
            {
                viewVertex.Add(transform.position + dir.normalized * enemy.radius);
            }
            else
            {
                viewVertex.Add(transform.position + dir.normalized * hit.distance);
            }
        }
        int vertexCount = viewVertex.Count + 1;
        vertices = new Vector3[vertexCount];
        triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewVertex[i]);
            if (i < vertexCount - 2)
            {

                triangles[i * 3 + 2] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3] = i + 2;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

    }
}
