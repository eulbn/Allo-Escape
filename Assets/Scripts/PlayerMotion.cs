using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerMotion : MonoBehaviour
{
    Vector3 RandomDirection;//Direction for the player
    public float Speed;//movement speed

    public float ChangeDirectionDistance;
    public GameObject Manager;
    public float LerpTime;
    DrawMechnics drawMechincs;

    public float boostInterpolationSpeed = 6.0f;
    public float maxSpeed;

    public bool isJumping = false;
    public bool isBoosting = false;

    public float jumpForce = 10f;

    public float jumpTime = 2.0f;
    float jumpTimeCounter = 0;



    public float hitTime = 3.0f;
    float hitTimeCounter = Mathf.Infinity;


    public GameObject[] smallPotato;
    public GameObject Text;

    public GameObject manager;

    Vector2 ReflectedVector;
    Vector2 OrginalVector;

    Animator animator;
    void Start()
    {

        drawMechincs = Manager.GetComponent<DrawMechnics>();
      
        
        RandomDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        

        Vector3 RotateToward = transform.position + RandomDirection;

        transform.LookAt(RotateToward, Vector3.up);

        LerpTimeCounter = 1;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (GameWon)
            return;

        if ((Time.time - hitTimeCounter) > 1 && (Time.time - hitTimeCounter) < hitTime)
        {
            return;
        }
        else if ((Time.time - hitTimeCounter) > hitTime && (Time.time - hitTimeCounter) < hitTime + 1)
        {
            animator.SetTrigger("TillSit");

            foreach(var small in smallPotato)
            {
                if(small)
                {
                    if(small.GetComponent<Animator>())
                    {
                        small.GetComponent<Animator>().SetTrigger("TillSit");
                    }
                }
            }
            hitTimeCounter = Mathf.Infinity;
        }
    

        MovePlayer();
        if (!isBoosting && !isJumping)
            ChangeDirection();
        if (isBoosting)
        {
            Boosting();
        }
        if (isJumping && check)
        {
            Jumping();
        }
        isJumping = false;

        if ((Time.time - jumpTimeCounter) > jumpTime)
            check = true;

    }
    float LerpTimeCounter;
    void MovePlayer()
    {


        if (LerpTimeCounter < 0.9)
        {
            Vector2 temRotation = Vector2.Lerp(OrginalVector, ReflectedVector, LerpTimeCounter);

            
           
            RandomDirection = new Vector3(temRotation.x, 0, temRotation.y);

        
            Vector3 RotateToward = transform.position + RandomDirection;
            transform.LookAt(RotateToward, Vector3.up);

            //transform.LookAt(transform.forward);

            LerpTimeCounter += LerpTime * Time.deltaTime;

        }


        Debug.DrawLine(transform.position, RandomDirection, Color.red);

        Vector3 NormalizedDirection = RandomDirection.normalized;
        transform.Translate(NormalizedDirection * Speed * Time.deltaTime, Space.World);





    }

    void ChangeDirection()
    {
        Vector2 temPlayerPostion = new Vector2(transform.position.x, transform.position.z);
        for (int i = 0; i < drawMechincs.linesPoints.Count; i++)
        {
            if (!drawMechincs.touchedLine.Contains(i))
                for (int j = 0; j < drawMechincs.linesPoints[i].singleLinePoints.Count - 1; j++)
                    if (drawMechincs.linesPoints[i].singleLinePoints.Count > 1)
                    {
                        Vector2 temPointPostion1 = new Vector2(drawMechincs.linesPoints[i].singleLinePoints[j].x, drawMechincs.linesPoints[i].singleLinePoints[j].z);
                        Vector2 temPointPostion2 = new Vector2(drawMechincs.linesPoints[i].singleLinePoints[j + 1].x, drawMechincs.linesPoints[i].singleLinePoints[j + 1].z);

                        //float Distance = HandleUtility.DistancePointLine(temPlayerPostion, temPointPostion1, temPlayerPostion);
                        float Distance = DistancePointLine(temPlayerPostion, temPointPostion1, temPointPostion2);

                        if (Distance < ChangeDirectionDistance)
                        {
                            Vector2 Differnce = temPointPostion1 - temPointPostion2;
                            Vector2 NormalOfDiffernce = new Vector2(Differnce.y, -Differnce.x);
                            ReflectedVector = Vector2.Reflect(new Vector2(RandomDirection.x, RandomDirection.z).normalized, NormalOfDiffernce.normalized);
                            OrginalVector = RandomDirection;
                            LerpTimeCounter = 0;

                            /*Vector3 tem = new Vector3(ReflectedVector.x, 0f, ReflectedVector.y);
                            Debug.DrawLine(RandomDirection, RandomDirection * 1000, Color.red);
                            Debug.DrawLine(tem, tem * 1000, Color.green);
                            EditorApplication.isPaused = true; ;*/
                            //RandomDirection = new Vector3(ReflectedVector.x, 0f, ReflectedVector.y);
                            //drawMechincs.linesPoints[i].singleLinePoints.Clear();
                            drawMechincs.touchedLine.Add(i);
                            return;
                        }
                    }

        }

    }


    public static float DistancePointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        return Vector3.Magnitude(ProjectPointLine(point, lineStart, lineEnd) - point);
    }
    public static Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 rhs = point - lineStart;
        Vector3 vector2 = lineEnd - lineStart;
        float magnitude = vector2.magnitude;
        Vector3 lhs = vector2;
        if (magnitude > 1E-06f)
        {
            lhs = (Vector3)(lhs / magnitude);
        }
        float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
        return (lineStart + ((Vector3)(lhs * num2)));
    }


    bool check = true;
    public void Jump()
    {
        if (!isJumping && check)
        {
            jumpTimeCounter = Time.time;
            
            isJumping = true;
        }
    }
    void Jumping()
    {
        GetComponent<Rigidbody>().AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        if (animator)
        {
            animator.SetTrigger("Jump");
            foreach (var small in smallPotato)
            {
                if (small)
                {
                    if (small.GetComponent<Animator>())
                    {
                        small.GetComponent<Animator>().SetTrigger("Jump");
                    }
                }
            }
        }
        check = false;
    }
    float originalSpeed;
    bool isMaxSpeed = false;

    public void Boost()
    {
        if (!isBoosting)
        {
            isBoosting = true;
            originalSpeed = Speed;
        }
    }
    float t = 0;
    
    void Boosting()
    {
        float temSpeed = Mathf.Lerp(Speed, maxSpeed, t);
        if (t > 0.9)
        {
            isMaxSpeed = true;
        }
        if (isMaxSpeed && t < 0.1)
        {
            Debug.Log("End");
            isBoosting = false;
            Speed = originalSpeed;
            isMaxSpeed = false;
        }
        else
        {
            Speed = originalSpeed + temSpeed;
           
            if(isMaxSpeed)
            {
                t -= Time.deltaTime * boostInterpolationSpeed;
            }
            else
            {
                t += Time.deltaTime * boostInterpolationSpeed;
            }

        }

        /* Debug.Log(t);
         if (t < 0.9)
         {
             transform.position = Vector3.Lerp(transform.position, (transform.position + RandomDirection).normalized * 3, t);
             t += Time.deltaTime * maxSpeed;

         }
         else
         {
             t = 0;
             isBoosting = false;
         }*/






    }
    int child = 0;
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "obstical")
        {
            hitTimeCounter = Time.time;
            animator.SetTrigger("Hit");

            foreach (var small in smallPotato)
            {
                if (small)
                {
                    if (small.GetComponent<Animator>())
                    {
                        small.GetComponent<Animator>().SetTrigger("Hit");
                    }
                }
            }

            Debug.Log("Game over");
        }
        if (other.gameObject.tag == "Small")
        {
            Debug.Log("Hi");


            if(Text)
            {
                if(Text.GetComponent<Text>())
                {
                    child++;
                    Text.GetComponent<Text>().text = "Rescued " + child + "/3";
                }
            }


            if(other.gameObject.GetComponent<FollowScript>())
            {
                other.gameObject.GetComponent<FollowScript>().enabled = true;
            }
            if (other.gameObject.GetComponent<Animator>())
            {
                other.gameObject.GetComponent<Animator>().enabled = true;
            }
            if (other.gameObject.GetComponent<BoxCollider>())
            {
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
            }

            for (int i = 0; i < other.transform.childCount; i++)
            { 
                if(other.transform.GetChild(i).GetComponent<ParticleSystem>())
                {
                    other.transform.GetChild(i).GetComponent<ParticleSystem>().Stop();
                }

            }

        }
    }
    bool GameWon = false;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Table")
        {
            animator.SetTrigger("Victory");


            if (manager)
                manager.SetActive(false);

            for(int i = 0; i< transform.childCount; i++)
            {
                if(transform.GetChild(i).GetComponent<Camera>())
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }



            Speed = 0;
            GameWon = true;


            foreach(var sp in smallPotato)
            {
                if (sp)
                {
                    if(sp.GetComponent<Animator>())
                        sp.GetComponent<Animator>().SetTrigger("Victory");

                    if (sp.GetComponent<FollowScript>())
                        sp.GetComponent<FollowScript>().enabled = false; ;
                }
            }

            if (manager)
                manager.SetActive(false);

        }
    }

    public void Reload()
    { 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }





}
