using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WebWork;

public class ControllerInput : MonoBehaviour
{

    private Controls controls = null;
    public GameObject CameraOffsetObject = null;
    public GameObject wheel;

    private float AdjustMoveSpeed = 1;
    private float AdjustRotSpeed = 25;

    private int[] ControlOutput = new int[3];

    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
        controls.Player.Enable();

        WebSocket.Connect("ws://192.168.0.52:8000/ws");
    }

    private int debugi = 0;

    // Update is called once per frame
    void Update()
    {
        /*debugi++;
        if(debugi == 60)
        {
            WebSocket.SendMessage("1-1-1");
            debugi = 0;
        }
        return;*/

        float LThumbXpos = controls.Player.LThumbX.ReadValue<float>();
        float LThumbYpos = controls.Player.LThumbY.ReadValue<float>();
        float RThumbXpos = controls.Player.RThumbX.ReadValue<float>();
        float RThumbYpos = controls.Player.RThumbY.ReadValue<float>();

        bool LTriggerPressed = controls.Player.LTrigger.IsPressed();
        bool RTriggerPressed = controls.Player.RTrigger.IsPressed();
        bool AButtonPressed = controls.Player.AButton.IsPressed();
        bool BButtonPressed = controls.Player.BButton.IsPressed();

        //Debug Log
        /*
        Debug.Log("Left thumb: " + LThumbXpos + " " + LThumbYpos);
        Debug.Log("Right thumb: " + RThumbXpos + " " + RThumbYpos);
        Debug.Log("Left Trigger: " + LTriggerPressed);
        Debug.Log("Right Trigger: " + RTriggerPressed);
        Debug.Log("A Button: " + AButtonPressed);
        Debug.Log("B Button: " + BButtonPressed);
        */

        //Functionality
        
        //Adjusting Player position
        if(LTriggerPressed && RTriggerPressed)
        {
            //Movement
            Vector3 movevec;
            movevec = CameraOffsetObject.transform.forward * LThumbYpos;
            movevec += CameraOffsetObject.transform.right * LThumbXpos;

            if (AButtonPressed)
                movevec += new Vector3(0, -0.5f, 0);
            if (BButtonPressed)
                movevec += new Vector3(0, 0.5f, 0);

            CameraOffsetObject.transform.position += movevec * AdjustMoveSpeed * Time.deltaTime;

            //Rotation
            CameraOffsetObject.transform.Rotate(0.0f, RThumbXpos * AdjustRotSpeed * Time.deltaTime, 0.0f, Space.Self);
        }

        //Car controls
        else
        {
            int speed = 0;
            int direction = 1;
            int angle = 0;

            //Speed
            speed = (int)Math.Round(Math.Abs(RThumbYpos) * 9);

            //Direction
            direction = (Math.Sign(RThumbYpos) + 1) / 2;

            //Turning
            if(LThumbXpos == 0)
                angle = 0;
            else if (LThumbYpos < 0)
                angle = 30 * Math.Sign(LThumbXpos);
            else
            {
                double radsangle = Math.Atan(LThumbXpos / LThumbYpos) / 3;
                radsangle *= 180.0d / Math.PI;
                angle = (int)Math.Round(radsangle);
            }

            Debug.Log("Speed: " + speed);
            Debug.Log("Direction: " + direction);
            Debug.Log("Angle: " + angle);

            if(ControlOutput[0] != speed || ControlOutput[1] != direction || ControlOutput[2] != angle)
            {
                ControlOutput[0] = speed;
                ControlOutput[1] = direction;
                ControlOutput[2] = angle;

                string message = "" + speed + "-" + direction + "-" + angle;
                Debug.Log("Message: " + message);

                if (String.IsNullOrWhiteSpace(message) || message.Length < 5)
                {
                    Debug.Log("Last invalid: " + Time.time);
                }
                else
                {
                    WebSocket.SendMessage(message);
                }
            }
        }
        wheel.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -60*LThumbXpos));
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
