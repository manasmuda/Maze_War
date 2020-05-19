using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public float speed = 10.0f;
    public Animator anim;
    public Animator animator;
    public Vector3 offset;
    public Rigidbody rb;
    public Vector2 jspos;
    private Vector2 jsposF;
    public Transform js;
    public float jsangle;
    public float jsmag;
    public int basicDir;
    public float deltaTime;
    public int isColliding;
    public float collisionAngle;

    public GameObject bulletPrefab;

    private GameObject curBullet;
    private Rigidbody curBulletRB;

    private bool shootAl = true;

    public WalkButtonControl walkButtonControl;
    public WalkButtonControl shootButtonControl;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        animator = GetComponent<Animator>();
        walkButtonControl = GameObject.Find("Canvas/ControlsPanel/PlayPanel/SprintButton").GetComponent<WalkButtonControl>();
        js = GameObject.Find("Canvas/ControlsPanel/PlayPanel/VariableJoystick/Background/Handle").transform;
        shootButtonControl = GameObject.Find("Canvas/ControlsPanel/ShooterPanel/ShootButton").GetComponent<WalkButtonControl>();
        jsposF = js.position;
        speed = 0.05f;
        jsangle = 0;
        jsmag = 0;
        basicDir = Playerprefs.playerPos;
        isColliding = 1;
        collisionAngle = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   if (walkButtonControl == null)
        {
            walkButtonControl = GameObject.Find("Canvas/ControlsPanel/PlayPanel/SprintButton").GetComponent<WalkButtonControl>();
        }
        if (js == null)
        {
            js = GameObject.Find("Canvas/ControlsPanel/PlayPanel/VariableJoystick/Background/Handle").transform;
        }
        if (shootButtonControl == null)
        {
            shootButtonControl = GameObject.Find("Canvas/ControlsPanel/ShooterPanel/ShootButton").GetComponent<WalkButtonControl>();
        }
        shootAl = !shootButtonControl.pressed;
        deltaTime = Time.deltaTime;
        jspos = jsposF - (Vector2)js.position;

        if (jspos != new Vector2(0, 0) && shootAl)
        {
            jsangle = (Mathf.Atan2(jspos.x, jspos.y) * Mathf.Rad2Deg);
            if (walkButtonControl.pressed)
                jsmag = Mathf.Sqrt(Mathf.Pow(jspos.x, 2) + Mathf.Pow(jspos.y, 2));
            else
                jsmag = 0;
        }
        else
        {
            jsangle = 0;
            jsmag = 0;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Gunplay"))
        {
            jsmag = 0;
        }

        /*if (Input.GetKey("3"))
        {
            jsmag = 1.5f * jsmag;
        }

        if (Input.GetKeyDown("1"))
        {
            if (shootAl)
            {
                jsmag = 0;
                anim.SetTrigger("shoot");
                anim.SetTrigger("plantBomb");
                StartCoroutine(BulletDrop());
                shootAl = false;
            }
        }

        if (Input.GetKey("1"))
        {
            jsmag = 0;
        }

        if (Input.GetKeyUp("1"))
        {
            shootAl = true;
        }*/

        Vector3 vecrot = transform.eulerAngles;
        if (jsangle != 0)
        {
            if (basicDir == -1)
            {
                vecrot.y = jsangle;
            }
            else
            {
                vecrot.y = jsangle + 180;
            }
        }
        else
        {
            if (basicDir == -1)
            {
                vecrot.y = 180;
            }
            else
            {
                vecrot.y = 0;
            }
        }
        transform.eulerAngles = vecrot;

        if (jsmag > 20)
        {
            anim.SetFloat("speed", 1);
        }
        else
        {
            anim.SetFloat("speed", 0);
        }

        if (collisionAngle - jsangle > 10 || collisionAngle - jsangle < -10)
        {
            isColliding = 1;
        }

        Vector3 vec = transform.position;
        vec.z = vec.z + Mathf.Cos((transform.eulerAngles.y * Mathf.PI) / 180) * jsmag * speed * Time.deltaTime * isColliding;
        vec.x = vec.x + Mathf.Sin((transform.eulerAngles.y * Mathf.PI) / 180) * jsmag * speed * Time.deltaTime * isColliding;
        rb.MovePosition(vec);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "wall")
        {
            isColliding = 0;
            collisionAngle = jsangle;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "wall")
            isColliding = 1;
    }


    IEnumerator BulletDrop()
    {
        yield return new WaitForSeconds(0.7f);
        Vector3 bulletPos = transform.position;
        Vector3 bulletAngle = transform.eulerAngles;
        bulletPos.y = 1.3f;
        bulletPos.x = bulletPos.x + 0.5f * Mathf.Sin(Mathf.PI * bulletAngle.y / 180);
        bulletPos.z = bulletPos.z + 0.5f * Mathf.Cos(Mathf.PI * bulletAngle.y / 180);
        curBullet = Instantiate(bulletPrefab, bulletPos, Quaternion.Euler(0, 0, 0));
        curBulletRB = curBullet.GetComponent<Rigidbody>();
        curBulletRB.velocity = new Vector3(0, 0, 0);
    }
}
