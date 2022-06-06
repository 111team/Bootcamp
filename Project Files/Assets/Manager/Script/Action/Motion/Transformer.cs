using UnityEngine;

public abstract class Transformer : MonoBehaviour
{
    public enum LoopType
    {
        Once,
        PingPong,
        Repeat
    }

    public LoopType loopType;
    public bool activate = false;
    public float duration = 1;
    public AnimationCurve transformCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Range(0, 1)]
    public float previewPosition;
    float time = 0f;
    float position = 0f;
    float direction = 1f;

    protected Platform m_Platform;

    
    protected  void Awake()
    {
        m_Platform = GetComponentInChildren<Platform>();
    }


    public void Update()
    {
        if (activate)
        {
            time = time + (direction * Time.deltaTime / duration);
            switch (loopType)
            {
                case LoopType.Once:
                    LoopOnce();
                    break;
                case LoopType.PingPong:
                    LoopPingPong();
                    break;
                case LoopType.Repeat:
                    LoopRepeat();
                    break;
            }
            SetTransform(position);
        }
    }

    public virtual void SetTransform(float position)
    {

    }

    void LoopPingPong()
    {
        position = Mathf.PingPong(time, 1f);
    }

    void LoopRepeat()
    {
        position = Mathf.Repeat(time, 1f);
    }

    void LoopOnce()
    {
        position = Mathf.Clamp01(time);
        if (position >= 1)
        {
            enabled = false;
            direction *= -1;
        }
    }

}
