using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{

    // Inspired by https://www.red3d.com/cwr/papers/1999/gdc99steer.pdf
    // And https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-seek--gamedev-849
    // And https://natureofcode.com/book/chapter-6-autonomous-agents/

    // Start is called before the first frame update

    private LevelManager lvlmgr;

    Vector3 dir;

    public float speed;

    public Vector3 velocity;

    public float lastwander;

    void Start()
    {
        lvlmgr = FindObjectOfType<LevelManager>();
        velocity = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        velocity = velocity.normalized * speed;
        lastwander = Random.Range(-0.25f, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 avoidF = GetAvoidForce();
        Vector3 wanderF = GetWanderForce();
        Vector3 separateF = GetSeperationForce();

        if (avoidF == new Vector3(0,0,0))
        {
            avoidF = wanderF;
        }

        if (separateF == new Vector3(0,0,0))
        {
            separateF = avoidF * 0.5f + wanderF * 0.2f;
        }

        velocity += (wanderF * 0.2f + avoidF * 1.8f + separateF) / 3 * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, speed);
        velocity.y = 0;
        var disp = Vector3.ClampMagnitude(velocity * Time.deltaTime, speed * Time.deltaTime);
        transform.position += disp;
        Debug.DrawLine(transform.position, transform.position + avoidF, Color.red, 0.05f);
        Debug.DrawLine(transform.position, transform.position + wanderF, Color.green, 0.05f);
        Debug.DrawLine(transform.position, transform.position + separateF, Color.blue, 0.05f);
        Debug.DrawLine(transform.position, transform.position + velocity, Color.cyan, 0.05f);
    }

    Vector3 GetWanderForce()
    {
        lastwander += Random.Range(-0.05f, 0.05f);
        lastwander = Mathf.Clamp(lastwander, -0.5f, 0.5f);
        var deltaF = Vector2.Perpendicular(new Vector2(velocity.x, velocity.z));
        deltaF *= lastwander;

        var wander = new Vector3(deltaF.x + velocity.x, 0, deltaF.y + velocity.z);
        wander = wander.normalized * speed;

        return wander - velocity;
    }


    Vector3 GetAvoidForce()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, velocity, 5f);
        if (hits.Length > 0)
        {
            Vector3 ttl = new Vector3(0,0,0);
            foreach(var h in hits)
            {
                var n = h.normal;
                n.y = 0;
                ttl += n.normalized / h.distance;
            }
            return ttl.normalized * speed;
        } else
        {
            return new Vector3(0,0,0);
        }
    }
    Vector3 GetSeperationForce() 
    {
        List<Vector3> nearbyMouseDir = new List<Vector3>();
        for(int i = 0; i < lvlmgr.mice.Count; i++)
        {
            if (lvlmgr.mice[i] == this) continue;
            dir = transform.position - lvlmgr.mice[i].transform.position;
            if (dir.sqrMagnitude < 20)
            {
                dir.y = 0;
                nearbyMouseDir.Add(dir.normalized / dir.magnitude);
            }

        }
        dir = transform.position - lvlmgr.player.transform.position;
        if (dir.sqrMagnitude < 20)
        {
            dir.y = 0;
            nearbyMouseDir.Add(dir.normalized / dir.magnitude);
        }
        dir = transform.position - Global.ws.location;
        if (dir.sqrMagnitude < 20)
        {
            dir.y = 0;
            nearbyMouseDir.Add(dir.normalized / dir.magnitude);
        }
        var desired = new Vector3(0,0,0);
        for(int i = 0; i < nearbyMouseDir.Count; i++)
        {
            desired += nearbyMouseDir[i];
        }
        return desired.normalized * speed - velocity;

    }
    void OnDestroy()
    {
        lvlmgr.mice.Remove(this);
    }

    void OnTriggerEnter(Collider col)
    {
        // Mouse hit by projectile or cave monster should be removed
        if (col.gameObject.tag == "monster"){
            Destroy(this);
        }
        if (col.gameObject.tag == "crate" || col.gameObject.tag == "rock") {
            // if its a projectile, destroy
            if (col.gameObject.transform.position.y > 2) Destroy(this);
        }

    }
}
