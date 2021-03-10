using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

  [SerializeField] float rcsThrust = 100f;
  [SerializeField] float mainThrust = 100f;
  [SerializeField] float levelDelay = 1f;

  [SerializeField] AudioClip thrustSoundClip;
  [SerializeField] AudioClip deathSoundClip;
  [SerializeField] AudioClip winSoundClip;

  [SerializeField] ParticleSystem thrustParticle;
  [SerializeField] ParticleSystem deathParticle;
  [SerializeField] ParticleSystem winParticle;



  Rigidbody rigidBody;
  AudioSource engineSound;

  private const string friendlyTag = "friendly";
  private const string fuelTag = "fuel";
  private const string deadTag = "dead";
  private const string finishTag = "finish";

  enum State { alive, dying, transcending };
  State state = State.alive;

  bool debugCollision = false;


  // Start is called before the first frame update
  void Start()
  {
    rigidBody = GetComponent<Rigidbody>();
    engineSound = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update()
  {
    if (Debug.isDebugBuild)
    {
      checkDebug();
    }
    if (state == State.alive)
    {
      ProcessThrust();
      ProcessRotate();
    }
  }

  private void checkDebug()
  {
    if (Input.GetKeyDown(KeyCode.C))
    {
      debugCollision = !debugCollision;
    }
    else if (Input.GetKeyDown(KeyCode.L))
    {
      LoadNextLevel();
    }
  }

  private void OnCollisionEnter(Collision collision)
  {

    if (state != State.alive || debugCollision) { return; }

    switch (collision.gameObject.tag)
    {
      case friendlyTag:
        break;
      case finishTag:
        StartSuccessSequence();
        break;
      case deadTag:
        StartDeathSequence();
        break;
      default:
        break;
    }
  }

  private void StartSuccessSequence()
  {
    state = State.transcending;
    engineSound.Stop();
    engineSound.PlayOneShot(winSoundClip);
    winParticle.Play();
    Invoke("LoadNextLevel", levelDelay);
  }

  private void StartDeathSequence()
  {
    state = State.dying;
    engineSound.Stop();
    deathParticle.Play();
    engineSound.PlayOneShot(deathSoundClip);
    Invoke("LoadFirstLevel", levelDelay);
  }

  private void LoadNextLevel()
  {
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    print(currentSceneIndex);
    int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
    print(nextSceneIndex);
    SceneManager.LoadScene(nextSceneIndex);
  }
  private void LoadFirstLevel()
  {
    SceneManager.LoadScene(0);
  }

  private void ProcessRotate()
  {
    rigidBody.angularVelocity = Vector3.zero;
    float rotationSpeed = rcsThrust * Time.deltaTime;
    if (Input.GetKey(KeyCode.A))
    {
      transform.Rotate(Vector3.forward * rotationSpeed);
    }
    else if (Input.GetKey(KeyCode.D))
    {
      transform.Rotate(-Vector3.forward * rotationSpeed);
    }
  }

  private void ProcessThrust()
  {
    if (Input.GetKey(KeyCode.W))
    {
      applyThrust();
    }
    else
    {
      engineSound.Stop();
      thrustParticle.Stop();
    }
  }

  private void applyThrust()
  {
    float thrustSpeed = mainThrust * Time.deltaTime;
    rigidBody.AddRelativeForce(Vector3.up * thrustSpeed);
    if (!engineSound.isPlaying)
    {
      engineSound.PlayOneShot(thrustSoundClip);
    }
    thrustParticle.Play();
  }

}
