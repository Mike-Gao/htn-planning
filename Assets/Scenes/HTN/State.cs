using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public bool treasurePickedUp = false;
    public bool crateInHand = false;

    public int crateCount = 0;
    public bool playerInRange = false;

    public bool crateOnGround = false;

    public GameObject nearestObject;

    public GameObject target;
    
    public Vector3 location;

    public Vector3 playerLocation;



    public State Clone() {
        return new State {
            treasurePickedUp = treasurePickedUp,
            crateCount = crateCount,
            crateInHand = crateInHand,
            playerInRange = playerInRange,
            crateOnGround = crateOnGround,
            nearestObject = nearestObject,
            target = target,
            location = location,
            playerLocation = playerLocation,
        };
    }
}