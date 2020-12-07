using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Self : MonoBehaviour
{
    public float mov_velocity;
    public float rot_velocity;

    public float gravity;

    public bool move_enabled = true;

    private CharacterController chara_control;
    private Vector3 mov_vector;
    private Vector2 rot_vector;

    public bool shielded { get; private set; }

    public float shield_timer = 10f;

    public Text shieldValue;

    public LevelManager lvlMgr;

    // Start is called before the first frame update
    void Start()
    {
        lvlMgr = FindObjectOfType<LevelManager>();
        chara_control = GetComponent<CharacterController>();
        // TODO: Comment out the following two line when debugging
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (move_enabled) {
            if (chara_control.isGrounded) {
                mov_vector = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
                mov_vector *= mov_velocity;
            }
            mov_vector -= new Vector3(0, gravity * Time.deltaTime, 0);
            chara_control.Move(mov_vector * Time.deltaTime);

            rot_vector.x -= Input.GetAxis("Mouse Y");
            rot_vector.x = Mathf.Clamp(rot_vector.x, -10f, 10f);
            rot_vector.y += Input.GetAxis("Mouse X");
            transform.eulerAngles = new Vector3(0, rot_vector.y * rot_velocity , 0);
            Camera.main.transform.localRotation = Quaternion.Euler(rot_vector.x * rot_velocity, 0, 0);


            if (Input.GetKeyDown("space")) {
                if (shielded) {
                    shielded = !shielded;
                } else {
                    if (shield_timer > 0) {
                        shielded = !shielded;
                    }
                }
            }

            if (shielded) {
                shield_timer -= Time.deltaTime;
                if (shield_timer < 0) {
                    shield_timer = 0;
                    shielded = false;
                }
                shieldValue.text = "Shield Available " + shield_timer.ToString("n2");

            }

        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.collider.tag == "treasure") {
            hit.collider.GetComponent<Renderer>().material.color = Color.yellow;
            // Change state to true;
            Global.ws.treasurePickedUp = true;
        }
    }
    

    void OnTriggerEnter(Collider col)
    {   
        if ((col.gameObject.tag == "crate" || col.gameObject.tag == "rock") && col.gameObject != null)
        {
            if (!shielded)
            {
                Global.ws.hit++;
            }
            lvlMgr.obstacles.Remove(col.gameObject);
            Destroy(col.gameObject);
        }
    }

}
