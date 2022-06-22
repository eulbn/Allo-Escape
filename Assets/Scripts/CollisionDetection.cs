using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionDetection : MonoBehaviour
{ 

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var x in collision)
            Debug.Log(collision.gameObject.tag);

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Game over");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


}
