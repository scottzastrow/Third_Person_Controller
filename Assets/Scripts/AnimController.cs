/*
 * 
 * Copyright (c) 2018 All Rights Reserved, VERGOSOFT LLC
 * Title: Third Person Controller
 * Author: Scott Zastrow
 * Developed with Unity 2017.3.0f3
 * 
 * NOTE: No Keyboard or Mouse Control. Controlled by Xbox Controller.
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour {

    public Animator Anim { get; set; }

    private float horizontalL;
    private float verticalL;
    private float horizontalR;
    private float verticalR;

    private float speedL;
    private float speedR;
    private bool idle;
    private bool turnLeft;
    private bool turnRight;

    //Empty Constructor
    public AnimController()
    { }

    //Custom Construtor
    public AnimController(float horizontalL, float verticalL, float horizontalR, float verticalR, float speedL, float speedR, bool idle, bool turnLeft, bool turnRight)
    {
        this.HorizontalL = horizontalL;
        this.VerticalL = verticalL;
        this.HorizontalR = horizontalR;
        this.VerticalR = verticalR;

        this.SpeedL = speedL;
        this.SpeedR = speedR;
        this.Idle = idle;
        this.TurnLeft = turnLeft;
        this.TurnRight = turnRight;
    }

    //Public Properties
    public float HorizontalL
    {
        get
        {
            Anim.SetFloat("HorizontalL", horizontalL);
            return horizontalL;
        }
        set { horizontalL = value; }
    }
    public float VerticalL
    {
        get
        {
            Anim.SetFloat("VerticalL", verticalL);
            return verticalL;
        }
        set { verticalL = value; }
    }
    public float HorizontalR
    {
        get
        {
            Anim.SetFloat("HorizontalR", horizontalR);
            return horizontalR;
        }
        set { horizontalR = value; }
    }
    public float VerticalR
    {
        get
        {
            Anim.SetFloat("VerticalR", verticalR);
            return verticalR;
        }
        set { verticalR = value; }
    }

    public float SpeedL
    {
        get { Anim.SetFloat("SpeedL", speedL); return speedL; }
        set { speedL = value; }
    }

    public float SpeedR
    {
        get { Anim.SetFloat("SpeedR", speedR); return speedR; }
        set { speedR = value; }
    }

    public bool Idle
    {
        get { Anim.SetBool("Idle", idle); return idle; }
        set { idle = value; }
    }

    public bool TurnLeft
    {
        get { Anim.SetBool("TurnLeft", turnLeft); return turnLeft; }
        set { turnLeft = value; }
    }

    public bool TurnRight
    {
        get { Anim.SetBool("TurnRight", turnRight); return turnRight; }
        set { turnRight = value; }
    }
}
