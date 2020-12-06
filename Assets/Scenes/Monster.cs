using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{

    public NavMeshAgent agent;
    private HTNPlanner htn;

    private PrimativeTask current;

    public GameObject currentObject;

    private LevelManager lvlMgr;

    private Self player;

    // Start is called before the first frame update
    void Start()
    {
        lvlMgr = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<Self>();
        agent = GetComponent<NavMeshAgent>();
        /* Build a HTN */
        var root = new CompoundTask();
        htn = new HTNPlanner(root);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
