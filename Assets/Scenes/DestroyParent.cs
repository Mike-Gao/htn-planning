using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour
{
    public void OnDestroy()
    {
    	Destroy(gameObject.transform.parent.gameObject);
    }
}