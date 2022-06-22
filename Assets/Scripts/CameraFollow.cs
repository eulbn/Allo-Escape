using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Target;
    public float followSpeed = 2.0f;
    public float adjustY = 37.0f;

    public float adjustX = 2.0f;
    public float adjustZ = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(Target)
        {
            transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, transform.position.z),
                new Vector3(Target.transform.position.x + adjustX, adjustY, Target.transform.position.z + adjustZ), Time.time * followSpeed);
        }
    }
}
