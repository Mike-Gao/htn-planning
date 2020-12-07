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

    private LevelManager lvlMgr;

    public Text planLabel;

    public Text actionLabel;

    public Self player;

    public static readonly Cooldown cooldown = new Cooldown();

    // Start is called before the first frame update
    void Start()
    {
        lvlMgr = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<Self>();
        agent = GetComponent<NavMeshAgent>();
        /* Build a HTN */
        // var root = new CompoundTask();
        // htn = new HTNPlanner(root);

        // var ApproachCrate = new ApproachObject("crate");
        // var PickUpCrate = new ObstaclePickUp("crate");
        // var ApproachPlayer = new ApproachPlayer();
        // var ThrowObstacle = new ThrowObstacle();

        // var ApproachRock = new ApproachObject("rock");
        // var PickUpRock = new ObstaclePickUp("rock");
        // var ApproachRandom = new ApproachRandom();

        // var getCrate = new CompoundTask();
        // getCrate.AddMethod(new List<Task>() { ApproachCrate, PickUpCrate});

        // var getRock = new CompoundTask();
        // getRock.AddMethod(new List<Task>() { ApproachRock, PickUpRock });
        
        // root.AddMethod(new List<Task>() { getCrate, ApproachPlayer, ThrowObstacle, ApproachRandom });
        // root.AddMethod(new List<Task>() { getRock, ApproachPlayer, ThrowObstacle, ApproachRandom });

        var root = new CompoundTask();
        htn = new HTNPlanner(root);

        var ApproachCrate = new ApproachObject("crate");
        var PickUpCrate = new ObstaclePickUp("crate");
        var ApproachPlayer = new ApproachPlayer();
        var ThrowObstacle = new ThrowObstacle();

        var ApproachRock = new ApproachObject("rock");
        var PickUpRock = new ObstaclePickUp("rock");

        var ApproachRandom = new ApproachRandom();

        var attackUsingCrate = new CompoundTask();
        attackUsingCrate.AddMethod(new List<Task>() { ApproachCrate, PickUpCrate, ApproachPlayer, ThrowObstacle });
        attackUsingCrate.AddMethod(new List<Task>() { ApproachPlayer, ThrowObstacle });

        var attackUsingRock = new CompoundTask();
        attackUsingRock.AddMethod(new List<Task>() { ApproachRock, PickUpRock, ApproachPlayer, ThrowObstacle});
        attackUsingRock.AddMethod(new List<Task>() { ApproachPlayer, ThrowObstacle });

        var move = new CompoundTask();
        move.AddMethod(new List<Task>() { ApproachRandom });
        move.AddMethod(new List<Task>() { ApproachPlayer });
        
        root.AddMethod(new List<Task>() { attackUsingCrate });
        root.AddMethod(new List<Task>() { attackUsingRock });
        root.AddMethod(new List<Task>() { move });

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
            if (elem.tag == "used") {
                // Cleanup used tag
                lvlMgr.obstacles.Remove(elem);
                Destroy(elem);
            } else if (elem.tag == "crate") {
                Global.ws.crateCount++;
            }
            var curDistSqrt = (transform.position - elem.transform.position).sqrMagnitude;
            if (curDistSqrt < best) {
                best = curDistSqrt;
                Global.ws.nearestObject = elem;
            }
        }

        Action();
        actionLabel.text = current == null? "No Task" : current.name;

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
        //if (plan!=null) Debug.Log(plan.Count);
        if ((current == null || current == cooldown) && (plan == null || plan.Count == 0)) {
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


        if (current.Terminate(this)) {
            if (current == cooldown) {
                current = null;
            } else {
                current = cooldown;
                current.Start(this);
            }
        }

    }


}
