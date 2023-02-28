using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    PlayerMovement moveScript;
    public Animator anim;

    public float dashSpeed;
    public float dashTime;


    void Start()
    {
        moveScript = GetComponent<PlayerMovement>();
    }

    
    void Update()
    {
        if(Input.GetButtonDown("Dash"))
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            moveScript.controller.Move(moveScript.moveVector * dashSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
