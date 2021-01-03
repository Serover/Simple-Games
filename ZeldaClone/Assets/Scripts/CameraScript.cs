using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraScript : MonoBehaviour
{
    public static bool Transitioning = false;

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

    void Update()
    {
        if (target)
            WhentoMove();
    }
    private void MoveCamera(Vector3 targetPos, Vector3 startTarget)
    {
        lerpConstant += speed * Time.deltaTime;

        Vector3 smoothPos = Vector3.Lerp(startTarget, targetPos, lerpConstant);
        transform.position = smoothPos;

        if (transform.position == targetPos)
        {
            Transitioning = false;
            gmaster.resumeProjectileMovement();
        }
    }
    private void WhentoMove()
    {
        float Height = 2f * cam.orthographicSize;
        float Width = Height * cam.aspect;

        Vector3 Tpos = target.GetComponent<Transform>().position;

        if (Transitioning)
            MoveCamera(desiredPos, startPos);


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
        Transitioning = true;
        gmaster.pauseProjectileMovement();
    }
}


