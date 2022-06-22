using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticalController : MonoBehaviour
{
    
  
    public float AttackTime;
    float attackTimeCounter;

    public GameObject fork;
    // Start is called before the first frame update
    void Start()
    {
        attackTimeCounter = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
            Debug.Log("KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK");
        if ((Time.time - attackTimeCounter) > AttackTime)
        {
            Instantiate(fork, transform.position, Quaternion.identity);
            attackTimeCounter = Time.time;
        }
    }
  
}
