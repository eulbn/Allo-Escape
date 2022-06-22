using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticalMotion : MonoBehaviour
{
    public Transform player;
    public float xMarginOfError, zMarginOfError;
    public float originalY;
    public float hoverTime = 2;
    public float hoverDistance;
    public float originalSpeedY;
    public float variationSpeedXZ;
    public float finalSpeedY;
    float hoverTimeCoutner;

    public float stayTime = 3.0f;
    float stayTimeCountr;

    public float changeDirectionTime = 0.5f;
    float changeDirectionTimeCounter;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x + xMarginOfError, originalY, transform.position.y);
        hoverTimeCoutner = Time.time;

        temX = Random.Range(xMarginOfError, -xMarginOfError);
        temZ = Random.Range(zMarginOfError, -zMarginOfError);


        player = GameObject.Find("Potato").transform;
    }


    float temX;
    float temZ;
    // Update is called once per frame
    bool check = true;
    void Update()
    {
        if (player.position.y + 1 < transform.position.y && check)
        {

            if ((Time.time - hoverTimeCoutner) > hoverTime)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, player.position.y, transform.position.z), Time.deltaTime * finalSpeedY);


            }
            else
            {

                if (transform.position.y > hoverDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, hoverDistance, player.position.z), Time.deltaTime * originalSpeedY);
                    changeDirectionTimeCounter = Time.time;
                }
                else
                {

                    float moveX = Mathf.Lerp(transform.position.x, player.position.x + temX, Time.deltaTime * variationSpeedXZ);
                    float moveZ = Mathf.Lerp(transform.position.z, player.position.z + temZ, Time.deltaTime * variationSpeedXZ);


                    transform.position = new Vector3(moveX, transform.position.y, moveZ);

                    if ((Time.time - changeDirectionTimeCounter) > changeDirectionTime)
                    {
                        temX = Random.Range(xMarginOfError, -xMarginOfError);
                        temZ = Random.Range(zMarginOfError, -zMarginOfError);
                        changeDirectionTimeCounter = Time.time;
                    }


                    //transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x + temX, transform.position.y, player.transform.position.z + temZ), Time.deltaTime * variationSpeedXZ);
                }


            }
        }
        else if (player.position.y + 1 > transform.position.y && check)
        {
            stayTimeCountr = Time.time;
            check = false;
        }


        if((Time.time - stayTimeCountr) > stayTime && !check)
        {
            Debug.Log("hiii");
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, originalY, transform.position.z ), Time.deltaTime * finalSpeedY);

            if (Mathf.Abs(transform.position.y - originalY) < 0.2)
                Destroy(gameObject);
        }

    }
}
