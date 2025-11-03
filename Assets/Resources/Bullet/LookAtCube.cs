using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCube : MonoBehaviour
{
    public Transform aimCube;
    public GameObject bulletPrefab;
    public float timer = 4f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(aimCube.position);
        if (Input.GetMouseButtonUp(0))
        {
            BulletShoot();

        }
    }

    void BulletShoot()
    {
        GameObject BulletObject = (GameObject)Instantiate(bulletPrefab);
        BulletObject.transform.position = transform.position;
        BulletObject.transform.rotation = transform.rotation;
       
    }

  
}
