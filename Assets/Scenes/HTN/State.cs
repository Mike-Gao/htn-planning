using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public bool treasurePickedUp = false;
    public GameObject ObjectInHand = null;

    public int crateCount = 0;
    public bool playerInRange = false;

    public GameObject nearestObject;

    public GameObject target;
    
    public Vector3 location;

    public float random;

    public int hit;
    public Vector3 playerLocation;



    public State Clone() {
        return new State {
            treasurePickedUp = treasurePickedUp,
            crateCount = crateCount,
            ObjectInHand = ObjectInHand,
            playerInRange = playerInRange,
            nearestObject = nearestObject,
            target = target,
            location = location,
            hit = hit,
            random = random,
            playerLocation = playerLocation,
        };
    }
}