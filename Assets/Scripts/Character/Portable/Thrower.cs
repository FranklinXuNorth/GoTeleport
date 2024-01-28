using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Thrower : Portable
{

    float time;

    bool canFire;

    // GameObjects
    GameObject playerObject;
    public GameObject projectile;

    bool isEntered;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        playerObject = GameObject.FindGameObjectWithTag("Player");

        canFire = false;
        isEntered = false;

        time = Time.time;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Time.time - time > config.throwerFiringTime)
        {            
            canFire = true;
        }

        if (canFire && isEntered)
        {
            time = Time.time;
            canFire = false;            

            Vector3 generatePos = new Vector3(transform.position.x,
                transform.position.y + bxcoll.size.y / 2 + projectile.GetComponent<SphereCollider>().radius / 2 + 1f,
                transform.position.z);

            GameObject projectile1 = Instantiate<GameObject>(projectile, generatePos, Quaternion.identity);
            Projectile projectileScript = projectile1.GetComponent<Projectile>();

            Vector3 launchingDir = getLaunchDirection(playerObject.transform.position, generatePos);

            projectileScript.onLaunch.Invoke(launchingDir);

        }
    }

    // if this is a thrower, then it float
    public override void transportObject(Vector3 playerPosition, Vector3 incomingForce, Vector3 rigidbodyVelocity)
    {
        this.transform.position = playerPosition;
    }

    public Vector3 getLaunchDirection(Vector3 end, Vector3 start)
    {
        // equation throwing a projectile and do free-falling 
        Vector3 dist = (end - start);

        float distXZ = Mathf.Abs(new Vector3(dist.x, 0f, dist.z).magnitude);
        float distY = Mathf.Abs(dist.y);

        Vector3 launchXZ = new Vector3(dist.x, 0f, dist.z);
        launchXZ.Normalize();
        Vector3 launchY = Vector3.up;

        float para;


        if (end.y > start.y)
        {
            para = Mathf.Abs(Mathf.Pow(((625 * distXZ * distXZ) / (50 * Mathf.Abs(distXZ - distY))), 0.5f));
        }
        else if (end.y < start.y)
        {
            para = Mathf.Abs(Mathf.Pow(((625 * distXZ * distXZ) / (50 * Mathf.Abs(distXZ + distY))), 0.5f));
        }
        else
            para = Mathf.Abs(Mathf.Pow(((625 * distXZ * distXZ) / (50 * Mathf.Abs(distXZ - distY))), 0.5f));


        Vector3 launchForce = (launchXZ + launchY) * para;

        return launchForce;
    }

    public void changeEnter()
    {
        isEntered = true;
    }

    public void changeExit()
    {
        isEntered = false;
    }

    
   
}
