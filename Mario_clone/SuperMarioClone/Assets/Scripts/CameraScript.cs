using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cameraStyle
{
    ZeldaStyle, smoothFollow, Standby
}

public class CameraScript : MonoBehaviour
{
    private  bool Transitioning = false;

    public cameraStyle CameraBehaviour;

    [SerializeField]
    private GameObject target;
    [SerializeField]
    private float speed;

    private Camera cam;
    private Vector3 desiredPos;
    private Vector3 startPos;
    private GameMaster gmaster;

    private float lerpConstant = 0;

    void Start()
    {
        gmaster = FindObjectOfType<GameMaster>();
        cam = GetComponent<Camera>();
        desiredPos = transform.position;
    }

    void LateUpdate()
    {
        if (target && CameraBehaviour == cameraStyle.ZeldaStyle)
            ZeldaMove();

        else if(target && CameraBehaviour == cameraStyle.smoothFollow)
        {
        //   transform.position = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);

            Vector3 targetPos = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);
            lerpConstant += speed * Time.deltaTime;

            Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, lerpConstant);
            transform.position = smoothPos;
        }
    }


    private void ZeldaMoveCamera(Vector3 targetPos, Vector3 startTarget)
    {
        lerpConstant += speed * Time.deltaTime;

        Vector3 smoothPos = Vector3.Lerp(startTarget, targetPos, lerpConstant);
        transform.position = smoothPos;

        if (transform.position == targetPos)
        {
            GameMaster.UnFreeze();
            Transitioning = false;
            GameMaster.resumeProjectileMovement();
        }
    }
    private void ZeldaMove()
    {
        float Height = 2f * cam.orthographicSize;
        float Width = Height * cam.aspect;

        Vector3 Tpos = target.GetComponent<Transform>().position;

        if (Transitioning)
            ZeldaMoveCamera(desiredPos, startPos);


        else if (Tpos.x >= GetComponent<Transform>().position.x + (Width / 2))
        {
            desiredPos = new Vector3(desiredPos.x + Width, desiredPos.y, desiredPos.z);
            transitioning();
        }

        else if (Tpos.x <= GetComponent<Transform>().position.x - (Width / 2))
        {
            desiredPos = new Vector3(desiredPos.x - Width, desiredPos.y, desiredPos.z);
            transitioning();
        }

        else if (Tpos.y >= GetComponent<Transform>().position.y + (Height / 2))
        {
            desiredPos = new Vector3(desiredPos.x, desiredPos.y + Height, desiredPos.z);
            transitioning();
        }

        else if (Tpos.y <= GetComponent<Transform>().position.y - (Height / 2))
        {
            desiredPos = new Vector3(desiredPos.x, desiredPos.y - Height, desiredPos.z);
            transitioning();
        }


    }
    private void transitioning()
    {
        startPos = transform.position;
        lerpConstant = 0;
        GameMaster.Freeze();
        Transitioning = true;
        GameMaster.pauseProjectileMovement();
    }
}


