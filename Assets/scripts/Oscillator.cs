using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

  [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
  [SerializeField] float period = 2f;

  [Range(0, 1)]
  [SerializeField]
  float movementFactor;

  Vector3 startPosition;

  // Start is called before the first frame update
  void Start()
  {
    startPosition = transform.position;
  }

  // Update is called once per frame
  void Update()
  {

    if (period <= Mathf.Epsilon)
    {
      period = 1;
    }

    float cycles = Time.time / period; // Grows continually from 0

    const float tau = Mathf.PI * 2; // about 6.
    float rawSineWave = Mathf.Sin(cycles * tau); // -1 to 1

    movementFactor = rawSineWave / 2f + 0.5f;

    Vector3 offset = movementFactor * movementVector;
    transform.position = startPosition + offset;
  }
}
