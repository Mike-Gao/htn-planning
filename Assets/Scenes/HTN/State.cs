﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public bool treasurePickedUp = false;
    public GameObject ObjectInHand = null;

    public int crateCount = 0;
    public bool playerInRange = false;

    public GameObject nearestObject;
    
    public Vector3 location;

    public float random;

    public Vector3 randPos;


    public int hit;
    public Vector3 playerLocation;


    public State Clone() {
        return new State {
            treasurePickedUp = treasurePickedUp,
            crateCount = crateCount,
            ObjectInHand = ObjectInHand,
            playerInRange = playerInRange,
            nearestObject = nearestObject,
            location = location,
            random = random,
            randPos = randPos,
            hit = hit,
            playerLocation = playerLocation,
        };
    }
}