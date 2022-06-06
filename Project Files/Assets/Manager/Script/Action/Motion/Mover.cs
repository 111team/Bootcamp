using UnityEngine;

public class Mover : Transformer
{
    [Space(10f)]
    public AudioSource moveSound;
    public GameObject movableObject;
    public Vector3 start;
    public Vector3 end;

    protected Vector3 m_InitialPosition;
    protected Quaternion m_InitialRotation;

    protected virtual void Start()
    {
        m_InitialPosition = movableObject.transform.position;
        m_InitialRotation = movableObject.transform.parent.rotation;
    }

    public override void SetTransform(float position)
    {

        /* var curvePosition = transformCurve.Evaluate(position);
         var pos = transform.TransformPoint(Vector3.Lerp(start, end, curvePosition));
         Vector3 deltaPosition = pos - rigidbody.position;

         rigidbody.MovePosition(pos);

        if (m_Platform != null)
             m_Platform.MoveCharacterController(rigidbody.velocity * Time.deltaTime);*/

        var curvePosition = transformCurve.Evaluate(position);
        var pos = m_InitialPosition + m_InitialRotation * Vector3.Lerp(start, end, curvePosition);

        Vector3 deltaPosition = pos - movableObject.transform.position;

        movableObject.transform.position = pos;

        if (m_Platform != null)
            m_Platform.MoveCharacterController(deltaPosition);


    }

    public void Activate()
    {
        activate = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.localPosition + start, transform.localPosition + end);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.localPosition + start, 0.3f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.localPosition + end, 0.3f);
    }
}
