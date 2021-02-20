﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource engineSound;
    bool engineSoundToggle;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        engineSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput(){
        if(Input.GetKey(KeyCode.Space)){
            rigidBody.AddRelativeForce(Vector3.up);
            if(!engineSound.isPlaying){
                engineSound.Play();
            }
        }else{
            engineSound.Stop();
        }


        if( Input.GetKey(KeyCode.A) ){
            transform.Rotate(Vector3.forward);
        }else if( Input.GetKey(KeyCode.D) ){
            transform.Rotate(-Vector3.forward);
        }

    }
}
