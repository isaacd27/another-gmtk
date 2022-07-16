using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BasePlayerController2D : MonoBehaviour
{
    protected Rigidbody2D m_rbody = null;

    public virtual void ResetEntity(Vector2 dest)
    {
        m_rbody.position = dest;
    }

    protected virtual void Awake()
    {
        m_rbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (PlayerControllerManager.Instance.IsInputLock)
        {
            return;
        }

        ControllerUpdate();
    }

    private void FixedUpdate()
    {
        if (PlayerControllerManager.Instance.IsInputLock)
        {
            return;
        }

        ControllerFixedUpdate();
    }

    protected abstract void ControllerUpdate();
    protected abstract void ControllerFixedUpdate();
}
