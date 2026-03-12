using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathArrowGuide : MonoBehaviour
{
    public Transform player;
    public Transform target;

    public GameObject arrowPrefab;

    public float arrowSpacing = 1.5f;
    public float updateRate = 0.5f;

    private NavMeshPath path;
    private List<GameObject> arrows = new List<GameObject>();
    private float timer;

    public bool isActive = true;
    void Start()
    {
        path = new NavMeshPath();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= updateRate)
        {
            timer = 0;
            GeneratePath();
        }
    }

    void GeneratePath()
    {
        if (!NavMesh.CalculatePath(player.position, target.position, NavMesh.AllAreas, path))
           return;
        Debug.Log("Generate!!!");
        ClearArrows();
        if(!isActive) return;
        if (path.corners.Length < 2)
            return;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Vector3 start = path.corners[i];
            Vector3 end = path.corners[i + 1];

            float distance = Vector3.Distance(start, end);
            int count = Mathf.FloorToInt(distance / arrowSpacing);

            for (int j = 0; j < count; j++)
            {
                float t = j / (float)count;

                Vector3 pos = Vector3.Lerp(start, end, t);
                pos.y += 0.05f;

                Vector3 dir = (end - start).normalized;

                GameObject arrow = Instantiate(
                    arrowPrefab,
                    pos,
                    Quaternion.LookRotation(dir)
                );

                arrows.Add(arrow);
            }
        }
    }

    public void ClearArrows()
    {
        
        foreach (var a in arrows)
        {
            if (a)
                Destroy(a);
        }

        arrows.Clear();
    }
}