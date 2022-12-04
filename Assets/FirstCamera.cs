using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCam;
    private WebCamTexture backCam2;

    public Material ActiveMaterial;
    public Material ActiveMaterial2;
    //public AspectRatioFitter fit;

    private void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("oof no cam");
            camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++) 
        {
            Debug.Log("oof " + i);
            Debug.Log(devices[i].name);
            if (i == 0)//!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
            if (i == 1)//!devices[i].isFrontFacing)
            {
                backCam2 = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }

        }

        if(backCam == null)
        {
            Debug.Log("oof null cam");
            return;
        }

        backCam.Play();
        backCam2.Play();
        ActiveMaterial.SetTexture("_MainTex", backCam2);
        ActiveMaterial2.SetTexture("_MainTex", backCam);
        camAvailable = true;
    }

    private void Update()
    {
        /*
        if (!camAvailable)
            return;
        //need to be cast
        float ratio = (float)backCam.width / (float)backCam.height;
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
        */
    }
}
