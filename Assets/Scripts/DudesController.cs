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
using InspectorX;

public class DudesController : MonoBehaviour
{

    AnimController DudeController = new AnimController();

    public GameObject dudeBase;
    public GameObject dudeAvatar;
    public GameObject xAxis;
    public GameObject yAxis;

    public float sensitivity;
    public float clampValue;
    public float locoSpeed;
    public Transform transformHeading;

    private float speedOfClamp;
    private float adjustedLocoSpeed;

    private float dudeHorizontalL;
    private float dudeVerticalL;
    private float dudeHorizontalR;
    private float dudeVerticalR;

    private float dudeSpeedL;
    private float dudeSpeedR;
    private float speedClampX;

    private bool idle;

    private decimal getFacingAngleX;
    private float facingAngleX;
    private decimal getFacingAngleY;
    private float facingAngleY;
    private bool facingLeft;
    private bool facingRight;
    private bool turnLeft;
    private bool turnRight;
    private bool iNeedToTurn;


    void Start()
    {
        this.DudeController.Anim = GetComponent<Animator>();
        idle = true;
    }

    void Update()
    {
        ///////////////////////////////////////////////SET///////////////////////////////////////////////

        //Set | Hooks up to project settings / input
        this.DudeController.HorizontalL = Input.GetAxis("HorizontalL");
        this.DudeController.VerticalL = Input.GetAxis("VerticalL");
        this.DudeController.HorizontalR = Input.GetAxis("HorizontalR");
        this.DudeController.VerticalR = Input.GetAxis("VerticalR");

        //Set | SpeedL/R from input Returns Pos. Num. regardless of Joystick quadrant
        this.DudeController.SpeedL = Mathf.Abs(Mathf.Pow(dudeHorizontalL, 2) + Mathf.Pow(dudeVerticalL, 2));
        this.DudeController.SpeedR = Mathf.Abs(Mathf.Pow(dudeHorizontalR, 2) + Mathf.Pow(dudeVerticalR, 2));

        //Set | Logic Values
        this.DudeController.Idle = idle;
        this.DudeController.TurnLeft = turnLeft;
        this.DudeController.TurnRight = turnRight;

        //Set | Joystick Angles
        //radianL = Mathf.Atan2(dudeHorizontalL, dudeVerticalL);
        //this.DudeController.DegreeL = radianL * (180 / Mathf.PI);

        ///////////////////////////////////////////////GET///////////////////////////////////////////////

        //Get | Hooks up to Animation Controller
        dudeHorizontalL = DudeController.HorizontalL;
        dudeVerticalL = DudeController.VerticalL;
        dudeHorizontalR = DudeController.HorizontalR;
        dudeVerticalR = DudeController.VerticalR;

        dudeSpeedL = DudeController.SpeedL;
        dudeSpeedR = DudeController.SpeedR;
        idle = DudeController.Idle;
        turnLeft = DudeController.TurnLeft;
        turnRight = DudeController.TurnRight;

        //////////////////////////////////////////////LOGIC//////////////////////////////////////////////

        //Look Left/Right
        if (dudeHorizontalR > .2)
        {
            yAxis.transform.Rotate(Vector3.up * ((sensitivity * 3 * dudeSpeedR) * Time.deltaTime));
            getFacingAngleY = (decimal)yAxis.transform.eulerAngles.y;
            facingAngleY = AxisR.GetAxisRotation(getFacingAngleY);
        }
        else if (dudeHorizontalR < -.2)
        {
            yAxis.transform.Rotate(Vector3.down * ((sensitivity * 3 * dudeSpeedR) * Time.deltaTime));
            getFacingAngleY = (decimal)yAxis.transform.eulerAngles.y;
            facingAngleY = AxisR.GetAxisRotation(getFacingAngleY);
        }

        if (facingAngleY > 0)
        {
            facingRight = true;
            facingLeft = false;
        }
        else if (facingAngleY < 0)
        {
            facingRight = false;
            facingLeft = true;
        }
        else if (facingAngleY == 0)
        {
            facingRight = false;
            facingLeft = false;
        }
        //print("facingRight: " + facingRight + " facingLeft: " + facingLeft);

        //Get Angle Rotation for X on xAxis
        getFacingAngleX = (decimal)xAxis.transform.eulerAngles.x;
        facingAngleX = AxisR.GetAxisRotation(getFacingAngleX);

        //Slows speed the closer it gets to xClampValue
        speedClampX = SpeedClampX(facingAngleX, clampValue);

        float sensitiveDeltaX = DeltaSensitiveX(sensitivity);

        //Clamp Up/Down
        if (facingAngleX > -(clampValue) & facingAngleX < clampValue)
        {
            //Look Up/Down
            if (dudeVerticalR > .2)
            {
                if (facingAngleX > -(clampValue - 20))
                {
                    xAxis.transform.Rotate(Vector3.left * (speedClampX * sensitiveDeltaX * Time.deltaTime));
                }
            }
            if (dudeVerticalR < -.2)
            {
                if (facingAngleX < clampValue - 5)
                {
                    xAxis.transform.Rotate(Vector3.right * (speedClampX * sensitiveDeltaX * Time.deltaTime));
                }
            }
        }

        //Idle Logic
        if (dudeSpeedL > .2 | dudeSpeedL < -.2)
            idle = false;
        else
            idle = true;

        //Turn Logic
        if (idle == false & facingLeft == true || idle == false & facingRight == true)
            iNeedToTurn = true;
        else
            iNeedToTurn = false;

        if (iNeedToTurn == true)
        {
            if (facingLeft == true)
            {
                turnLeft = true;
                turnRight = false;
            }
            else if (facingRight == true)
            {
                turnLeft = false;
                turnRight = true;
            }
            TurnAvatar();
        }
        else if (iNeedToTurn == false)
        {
            turnLeft = false;
            turnRight = false;
        }
        ///////////////////////////////////////////LOCOMOTION/////////////////////////////////////////////

        LocoSpeed();
        if (dudeSpeedL > .2 & dudeVerticalL > .2)
        {
            dudeBase.transform.position += transform.forward * Time.deltaTime * adjustedLocoSpeed;
        }
        if (dudeSpeedL > .2 & dudeVerticalL < -.2)
        {
            dudeBase.transform.position -= transform.forward * Time.deltaTime * (adjustedLocoSpeed);
        }

    }
    /////////////////////////////////////////////METHODS/////////////////////////////////////////////

    private void LocoSpeed()
    {
        if (dudeSpeedL > .2 & dudeSpeedL < .8)
        {
            //locoSpeed = 1.7f;
            adjustedLocoSpeed = locoSpeed * .17f;
        }
        if (dudeSpeedL >= .8)
        {
            if (dudeVerticalL > .8)
            {
                //locoSpeed = 5f;
                adjustedLocoSpeed = locoSpeed * .5f;
            }
            if (dudeVerticalL < -.8)
            {
                //locoSpeed = 3f;
                adjustedLocoSpeed = locoSpeed * .3f;
            }
        }
    }

    private void TurnAvatar()
    {
        float sensitiveDeltaY = DeltaSensitiveY(sensitivity);
        transformHeading.transform.eulerAngles = new Vector3(0, facingAngleY, 0);
        dudeAvatar.transform.rotation = Quaternion.Lerp(this.transform.rotation, transformHeading.rotation, sensitiveDeltaY * Time.deltaTime);
    }

    //Translates Sensitivity value for use with deltaTime
    private float DeltaSensitiveY(float sensitive)
    {
        sensitive = sensitive / 15;
        if (sensitive > 1)
            return Mathf.RoundToInt(Mathf.Abs(sensitive));
        else
            return sensitive = 1;
    }
    private float DeltaSensitiveX(float sensitive)
    {
        sensitive = sensitive / 6;
        if (sensitive > 3)
            return Mathf.RoundToInt(Mathf.Abs(sensitive));
        else
            return sensitive = 3;
    }


    private float SpeedClampX(float facingAngleX, float clampValue)
    {
        if (facingAngleX < clampValue)
            speedClampX = (1 - Mathf.Abs(facingAngleX / clampValue)) * (.07f * sensitivity);
        else
            speedClampX = 0;

        return speedClampX;
    }
}
