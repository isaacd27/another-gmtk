using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class TimeEvent : UnityEvent<float> { }

public class Dice : MonoBehaviour
{
    public UnityEvent OnRoll;
    public TimeEvent OnRollCooldownUpdate;
    public UnityEvent OnPlayerEnterRange;
    public UnityEvent OnPlayerExitRange;

    [SerializeField, Range(1,10)]
    private float m_activationRadius = 3;
    [SerializeField, Range(1, 10)]
    private float m_rollCooldownInSeconds = 3f;

    private CircleCollider2D m_collider;
    private Animation m_animationComp;

    private bool m_isPlayerInRange = false;
    private bool m_rollInCooldown = false;
    // Start is called before the first frame update
    void Start()
    {
        // Dice layer
        gameObject.layer = 11;
        gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        m_collider = gameObject.AddComponent<CircleCollider2D>();
        m_collider.radius = m_activationRadius;
        m_collider.isTrigger = true;

        m_animationComp = GetComponent<Animation>();

        m_rollInCooldown = false;
        m_isPlayerInRange = false;
    }
    
    float currentTimeStamp = 0;

    void FixedUpdate()
    {
        if (m_rollInCooldown)
        {
            currentTimeStamp -= Time.fixedDeltaTime;
            OnRollCooldownUpdate?.Invoke(currentTimeStamp);

            if (currentTimeStamp <= 0)
            {
                m_rollInCooldown = false;
            }

            return;
        }

        if (!m_isPlayerInRange)
        {
            return;
        }

        if (Input.GetButtonDown("RollDice"))
        {
            m_animationComp.Play();
            OnRoll?.Invoke();
            m_rollInCooldown = true;
            currentTimeStamp = m_rollCooldownInSeconds;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            OnPlayerEnterRange?.Invoke();
            m_isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!m_isPlayerInRange)
        {
            return;
        }

        if (collision.GetComponent<PlayerController>())
        {
            OnPlayerExitRange?.Invoke();
            m_isPlayerInRange = false;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (m_rollInCooldown)
        {
            Gizmos.color = Color.grey;
        }
        else if (m_isPlayerInRange)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        Gizmos.DrawWireSphere(transform.position, m_activationRadius);        
    }
#endif
}
