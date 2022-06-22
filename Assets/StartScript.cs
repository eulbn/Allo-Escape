using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour
{

    public GameObject manager;
    public GameObject player;
    public GameObject cameraa;

    public GameObject playButton;

    public GameObject boost;
    public GameObject jump;
    public GameObject reload;


    public void start()
    {
        if(manager)
        {
            manager.SetActive(true);
        }
        if (player)
        {
            player.SetActive(true);
        }
        if(cameraa)
        {
            if(cameraa.GetComponent<CameraFollow>())
            {
                cameraa.GetComponent<CameraFollow>().enabled = true;
            }

        }
        if(playButton)
            playButton.SetActive(false);
        if(boost)
            boost.SetActive(true);
        if(jump)
            jump.SetActive(true);

        if (reload)
        {
            reload.SetActive(true);
        }
    }
}
