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
    float maxDistance = 30f;
   static float  OFFSET = 1.1f;
    private float appearedOnTime = 0f;
    private Camera mainCamera;
    
    void Start(){
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }


    void Update()
    {
        if (appearedOnTime + timeToDisappear < GameSystem.TimeStep)//|| appearedOnTime > GameSystem.TimeStep)
        {
            appearedOnTime = GameSystem.TimeStep;// - (appearedOnTime > GameSystem.TimeStep? timeToDisappear: 0);
            Vector3 newPosition;
            do {
                newPosition = RandomPointOnMesh.GetRandomPointOnMesh(GameSystem.TerrainMesh());
            } while (Vector3.Distance(newPosition, mainCamera.transform.position) > maxDistance);
            cadaver.transform.position = newPosition;
        } else {
            cadaver.transform.position = cadaver.transform.position + new Vector3(0, -2*OFFSET*TimeInterface.deltaTime*(timeToDisappear/GameSystem.SECONDS_PER_YEAR), 0);
        }
    }

    class RandomPointOnMesh : MonoBehaviour
    {
        private static Mesh calculatedMesh;
        private static float[] sizes;
        private static float[]cumulativeSizes;
        private static float total;
        //Extra�do de: https://gist.github.com/v21/5378391
        public static Vector3 GetRandomPointOnMesh(Mesh mesh)
        {
            if (mesh != calculatedMesh){
                calculatedMesh = mesh;
                sizes = GetTriSizes(mesh.triangles, mesh.vertices);
                cumulativeSizes = new float[sizes.Length];
                total = 0;

                for (int i = 0; i < sizes.Length; i++)
                {
                    total += sizes[i];
                    cumulativeSizes[i] = total;
                }
            }
            

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
            pointOnMesh *= 50520f;
            return new Vector3(pointOnMesh.x, pointOnMesh.z + OFFSET, -pointOnMesh.y );

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
