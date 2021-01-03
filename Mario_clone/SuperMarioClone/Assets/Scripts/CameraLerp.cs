using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    public static bool Transitioning = false;

    public GameObject target;
    public float speed;

    private Camera cam;
    Vector3 desiredPos;

    private float t = 0;

    void Start()
    {
        cam = GetComponent<Camera>();

        desiredPos = transform.position;
    }

    void Update()
    {
        if (target)
        {
            float Height = 2f * cam.orthographicSize;
            float Width = Height * cam.aspect;

            Vector3 Tpos = target.GetComponent<Transform>().position;

            if (Tpos.x >= GetComponent<Transform>().position.x + (Width / 2))
            {
                desiredPos = new Vector3(desiredPos.x + Width, desiredPos.y, desiredPos.z);
                Move();
            }
        }
    }

    void Move()
    {
        t += speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, desiredPos, t);
    }
}
