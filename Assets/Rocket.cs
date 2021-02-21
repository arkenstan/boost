using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    Rigidbody rigidBody;
    AudioSource engineSound;

    private const string friendlyTag = "friendly";
    private const string fuelTag = "fuel";
    private const string deadTag = "dead";

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

    private void OnCollisionEnter(Collision collision) {
        print("Collided");
        switch(collision.gameObject.tag){
            case friendlyTag:{
                print("OK");
                break;
            }
            case fuelTag:{
                print("FUEL");
                break;
            }case deadTag:{
                print("DEAD");
                break;
            }
            default:
                break;
        }
    }

    private void ProcessRotate(){
        rigidBody.freezeRotation = true;
        float rotationSpeed = rcsThrust * Time.deltaTime;
        if( Input.GetKey(KeyCode.A) ){
            transform.Rotate(Vector3.forward * rotationSpeed);
        }else if( Input.GetKey(KeyCode.D) ){
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
        rigidBody.freezeRotation = false;
    }

    private void ProcessThrust(){
        float thrustSpeed = mainThrust * Time.deltaTime;
        if(Input.GetKey(KeyCode.Space)){
            rigidBody.AddRelativeForce(Vector3.up * thrustSpeed);
            if(!engineSound.isPlaying){
                engineSound.Play();
            }
        }else{
            engineSound.Stop();
        }
    }

    private void ProcessInput(){
        ProcessThrust();
        ProcessRotate();
    }

}
