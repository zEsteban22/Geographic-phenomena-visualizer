using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalsController : MonoBehaviour
{
    [SerializeField]
    GameObject cadaver;
    [SerializeField]
    float timeToDisappear = 1f;
    [SerializeField]
    float minHigh = 8.1522f;
    private float appearedOnTime = 0f;
    
    void Start(){}

    
    void Update()
    {
        if(appearedOnTime + timeToDisappear < GameSystem.TimeStep)
        {
            appearedOnTime = GameSystem.TimeStep;
            Debug.Log(cadaver.transform.position);
            cadaver.transform.position = RandomPointOnMesh.GetRandomPointOnMesh(GameSystem.TerrainMesh());
            Debug.Log(cadaver.transform.position);
        }
    }

    class RandomPointOnMesh : MonoBehaviour
    {
        //Extra�do de: https://gist.github.com/v21/5378391
        public static Vector3 GetRandomPointOnMesh(Mesh mesh)
        {
            //if you're repeatedly doing this on a single mesh, you'll likely want to cache cumulativeSizes and total
            float[] sizes = GetTriSizes(mesh.triangles, mesh.vertices);
            float[] cumulativeSizes = new float[sizes.Length];
            float total = 0;

            for (int i = 0; i < sizes.Length; i++)
            {
                total += sizes[i];
                cumulativeSizes[i] = total;
            }

            //so everything above this point wants to be factored out

            float randomsample = Random.value * total;

            int triIndex = -1;

            for (int i = 0; i < sizes.Length; i++)
            {
                if (randomsample <= cumulativeSizes[i])
                {
                    triIndex = i;
                    break;
                }
            }

            if (triIndex == -1) Debug.LogError("triIndex should never be -1");

            Vector3 a = mesh.vertices[mesh.triangles[triIndex * 3]];
            Vector3 b = mesh.vertices[mesh.triangles[triIndex * 3 + 1]];
            Vector3 c = mesh.vertices[mesh.triangles[triIndex * 3 + 2]];

            //generate random barycentric coordinates

            float r = Random.value;
            float s = Random.value;

            if (r + s >= 1)
            {
                r = 1 - r;
                s = 1 - s;
            }
            //and then turn them back to a Vector3
            Vector3 pointOnMesh = a + r * (b - a) + s * (c - a);
            pointOnMesh *= 50520.01f;
            Vector3 p = new Vector3();
            p.x = pointOnMesh.x;
            p.y = pointOnMesh.z;
            p.z = pointOnMesh.y;
            p.y += 2f;
            return p;

        }
        private static float[] GetTriSizes(int[] tris, Vector3[] verts)
        {
            int triCount = tris.Length / 3;
            float[] sizes = new float[triCount];
            for (int i = 0; i < triCount; i++)
            {
                sizes[i] = .5f * Vector3.Cross(verts[tris[i * 3 + 1]] - verts[tris[i * 3]], verts[tris[i * 3 + 2]] - verts[tris[i * 3]]).magnitude;
            }
            return sizes;
        }
    }
}