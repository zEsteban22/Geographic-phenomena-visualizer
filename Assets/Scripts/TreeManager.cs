using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> trees= new List<GameObject>();
    [SerializeField]
    private List<float> origins= new List<float>();
    private List<Tuple<GameObject,float>> deadTrees= new List<Tuple<GameObject, float>>();
    private RaycastHit hit;
    private 
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        
        for (int i = 0; i < trees.Count; i++)
        {
            if (!trees[i].activeSelf) continue;
            trees[i].transform.localScale = Vector3.one * (GameSystem.timeStep - origins[i]) / GameSystem.LAST_STEP;
            Physics.Raycast(trees[i].transform.position, Vector3.down, out hit);
            if (hit.distance > 0.1)
            {
                trees[i].SetActive(false);
                deadTrees.Add(new Tuple<GameObject, float>(trees[i], GameSystem.timeStep));
            }
        }
        for (int i = 0; i < deadTrees.Count; i++)
        {
            var tuple = deadTrees[i];
            if (GameSystem.timeStep<tuple.Item2)
            {
                tuple.Item1.SetActive(true);
                deadTrees.Remove(tuple);
            }
        }
        
    }
}
