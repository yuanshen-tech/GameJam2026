using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAbout : MonoBehaviour
{
    public Light deng;
    private float time;
    private Collider[] colliders;
    public float r=7;
    public AudioSource audioSource_light;
    public GameObject player;
    private bool isOpen;
    public AudioSource audioSource_bk;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time+= Time.deltaTime;
        if(time>1.5f)
        {
            deng.intensity = Random.Range(1, 1.8f);
            time = 0;
        }
        colliders = Physics.OverlapSphere(transform.position, r,1<<3);
        //Debug.Log(Vector3.Distance(player.transform.position, transform.position));
        if(colliders.Length>0)
        {
            if (!isOpen)
            {
                audioSource_light.Play();
                audioSource_bk.volume = audioSource_bk.volume / 2;
                isOpen=true;
            }
        }
        else
        {
            if(Vector3.Distance(player.transform.position,transform.position)<r)
            {
                audioSource_light.Pause();
                isOpen = false;
            }
        }
    }
}
