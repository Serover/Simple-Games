using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulationButton : BaseButton
{
    [Header("Object Manipulation")]

    [SerializeField]
    private List<GameObject> Objs = new List<GameObject>();

    [Header("Rotations")]

    [SerializeField]
    private Vector3 Rotate;
    [SerializeField]
    private float RotateOverTime;


    [Header("Movements")]
    [SerializeField]
    private Vector3 Move;
    [SerializeField]
    private float MoveOverTime;

    private Vector3 ogPos;
    private Quaternion ogRot;

    private void Start()
    {
        ogPos = GetComponent<Transform>().position;
        ogRot = GetComponent<Transform>().rotation;
    }

    private void Update()
    {

    }

    // remember DoStuff and UndoStuff only run once, it's not DOINGstuff or UNDOING stuff.
    // So in this case since we have "OverTime", we simply have to START a rotation sequence.
    protected override void DoStuff()
    {
        // rotate move etc
    }

    protected override void UndoStuff()
    {
        // rotate back move back etc
    }

}
