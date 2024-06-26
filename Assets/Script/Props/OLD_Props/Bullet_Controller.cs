using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Bullet_Controller : MonoBehaviour
{
    public GameObject projectile;
    public Cursor_Controller cursorController;
    public PlayerController playerController;
    public bool haveShot;
    PointLightScanner pointLightScanner;
    // Start is called before the first frame update

    GameObject bullet;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        cursorController = FindObjectOfType<Cursor_Controller>();
    }
    void Start()
    {

    }

    public void Shoot()
    {
        bullet = Instantiate(projectile, transform.position, Quaternion.identity);
        var dir = new Vector2(cursorController.transform.position.x, cursorController.transform.position.y) - new Vector2(transform.position.x, transform.position.y);


        if (playerController.transform.rotation == Quaternion.Euler(0, 90, 0))
            bullet.GetComponent<Rigidbody>() .velocity = dir.normalized * ((Input.GetAxis("AimV") + Mathf.Abs(Input.GetAxis("AimH"))) * 15);
        if (playerController.transform.rotation == Quaternion.Euler(0, -90, 0))
            bullet.GetComponent<Rigidbody>() .velocity = dir.normalized * ((-Input.GetAxis("AimV") + Mathf.Abs(Input.GetAxis("AimH"))) * 15);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (playerController.canShoot)
    //    {
    //        if (Input.GetAxis("Fire2") == 1 && !haveShot)
    //        {
    //            playerController.animator.SetTrigger("TirFronde");
    //            playerController.canShoot = false;
    //            haveShot = true;
    //        }
    //        if(haveShot)
    //        {
    //            if (Input.GetAxis("Fire2") < 1)
    //            {
    //                haveShot = false;
    //            }
    //            return;
    //        }
    //    }
    //}


}
