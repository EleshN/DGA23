using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Player2D : MonoBehaviour
{
    
    public float moveSpeed = 3f;

    public Animator anim;

    public Rigidbody2D body;


    void Start()
    {
        
    }

    private void Awake()
    {

    }


    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontal, vertical, 0f).normalized;
        transform.position += movement * moveSpeed;
        body.velocity = movement * moveSpeed;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
    }

    
}

