using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterControl : MonoBehaviour
{

    public WalkButtonControl buttonControl;
    private Animator animator;
    private bool trigger;

    public GameObject bulletPrefab;
    private GameObject curBullet;
    private Rigidbody curBulletRB;

    // Start is called before the first frame update
    void Start()
    {
        buttonControl = GameObject.Find("Canvas/ControlsPanel/ShooterPanel/ShootButton").GetComponent<WalkButtonControl>();
        animator = GetComponent<Animator>();
        trigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        CharacterScript tempScript = gameObject.GetComponent<CharacterScript>();
        if (buttonControl.pressed && trigger && tempScript.ammo>0)
        {
            trigger = false;
            animator.SetTrigger("shoot");
            tempScript.ammo = tempScript.ammo - 1;
            StartCoroutine(BulletDrop());
        }

        if (!buttonControl.pressed)
        {
            trigger = true;
        }
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
        if (gameObject.GetComponent<CharacterScript>().skillPower) {
            curBullet.GetComponent<BulletScript>().damage = 15;
        }
        Vector3 velocity = new Vector3(10*Mathf.Sin((transform.eulerAngles.y*Mathf.PI)/180),0,10*Mathf.Cos((transform.eulerAngles.y*Mathf.PI)/180));
        curBulletRB.velocity = velocity;
    }
}
