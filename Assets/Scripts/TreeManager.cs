using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    private List<Tuple<GameObject,float>> treesAndOrigins = new List<Tuple<GameObject, float>>();
    private List<Tuple<GameObject,float>> deadTrees= new List<Tuple<GameObject, float>>();
    [SerializeField]
    private float treesMaxAge = 12f;
    private RaycastHit hit;
    private
    void Start()
    {
        foreach (var tree in GameObject.FindGameObjectsWithTag("Tree"))
            treesAndOrigins.Add(new Tuple<GameObject, float>(tree, tree.transform.position.magnitude * 100 % treesMaxAge - treesMaxAge));
    }

    void Update()
    {   
        foreach (var tree in treesAndOrigins) 
        {
            if (!tree.Item1.activeSelf) continue;
            tree.Item1.transform.localScale = Vector3.one * ((GameSystem.TimeStep - tree.Item2) % GameSystem.LAST_STEP) / GameSystem.LAST_STEP;
            
            if (Physics.Raycast(tree.Item1.transform.position, Vector3.down, out hit) && hit.collider.tag == "Terrain" &&
                hit.distance > 0.1)
            {
                /*
                Debug.Log(hit.collider.gameObject.name);
                GameObject o = new GameObject("aaaaaaaaaaaa");
                o.transform.position = trees[i].transform.position;
                o = new GameObject("bbbbbbbbbbb");
                o.transform.position = hit.collider.transform.position;*/
                tree.Item1.SetActive(false);
                deadTrees.Add(new Tuple<GameObject, float>(tree.Item1, GameSystem.TimeStep));
            }
        }
        for (int i = 0; i < deadTrees.Count; i++)
        {
            var tuple = deadTrees[i];
            if (GameSystem.TimeStep<tuple.Item2)
            {
                tuple.Item1.SetActive(true);
                deadTrees.Remove(tuple);
            }
        }
        
    }
}
