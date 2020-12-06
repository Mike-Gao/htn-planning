using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{

    public NavMeshAgent agent;
    private HTNPlanner htn;

    private PrimativeTask current;

    private List<PrimativeTask> plan = new List<PrimativeTask>();

    public GameObject currentObject;

    private LevelManager lvlMgr;

    public Text planLabel;

    public Self player;

    public static readonly Cooldown cooldown = new Cooldown();

    // Start is called before the first frame update
    void Start()
    {
        lvlMgr = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<Self>();
        agent = GetComponent<NavMeshAgent>();
        /* Build a HTN */
        var root = new CompoundTask();
        htn = new HTNPlanner(root);
        var ApproachCrate = new ApproachObject("crate");
        var PickUpCrate = new ObstaclePickUp("crate");
        var ApproachPlayer = new ApproachPlayer();
        var ApproachRock = new ApproachObject("rock");
        var PickUpRock = new ObstaclePickUp("rock");
        var ThrowObstacle = new ThrowObstacle();

        var AttackUsingCrate = new CompoundTask();
        AttackUsingCrate.AddMethod(new List<Task>() {ApproachCrate, PickUpCrate, ApproachPlayer, ThrowObstacle});

        var AttackUsingRock = new CompoundTask();
        AttackUsingRock.AddMethod(new List<Task> (){ApproachRock, ApproachPlayer, PickUpRock, ThrowObstacle});

        root.AddMethod(new List<Task> {AttackUsingCrate, AttackUsingRock});



    }

    // Update is called once per frame
    void Update()
    {
        Global.ws.location = transform.position;
        Global.ws.playerInRange = (transform.position - player.transform.position).magnitude < 20;
        Global.ws.playerLocation = player.transform.position;
        Global.ws.crateCount = 0;

        float best = Mathf.Infinity;

        foreach(var elem in lvlMgr.obstacles) {
            if (elem.tag == "crate") Global.ws.crateCount++;
            var curDistSqrt = (transform.position - elem.transform.position).sqrMagnitude;
            if (curDistSqrt < best) {
                best = curDistSqrt;
                Global.ws.nearestObject = elem;
            }
        }

        Action();

    }

    void ShowPlan(List<PrimativeTask> tasks) 
    {
        List<string> taskNames = new List<string>();
        for(int i = tasks.Count - 1; i >= 0; i--) {
            taskNames.Add(tasks[i].name);
        }

        planLabel.text = string.Join("-", taskNames);


    }
    void Action()
    {
        if (current == null && (plan == null || plan.Count == 0)) {
            plan = htn.GetPlan();
            ShowPlan(plan);
            return;
        }

        // if we have a plan, but no current task
        if (current == null) {
            current = plan[plan.Count - 1];
            plan.RemoveAt(plan.Count - 1);
            if (current.Prev(Global.ws)) {
                current.Start(this);
            } else {
                plan = null;
                current = cooldown;
                current.Start(this);
            }
            return;
        }


        if (current.Terminates(this)) {
            if (current == cooldown) {
                current = null;
            } else {
                current = cooldown;
                current.Start(this);
            }
        }

    }


}
