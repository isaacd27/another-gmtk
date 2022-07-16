using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : PoolObject
{
    public UnityEvent OnHit;
    public UnityEvent OnKill;

    public BulletSO script;

    [SerializeField]
    private float DelayBeforeDestroy = 2f;

    [SerializeField]
    CircleCollider2D collider2d;
    
    private int life;
    Vector3 originScale;
    public int Life
    {
        get { return life; }
        set
        {
            life = value;
            if(life <= 0)
            {
                StartCoroutine(DelayBeforeKill());
            }
        }
    }

    private void Awake()
    {
        Life = script.Life;
        originScale = transform.localScale;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Life = script.Life;
        GetComponent<Renderer>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;
    }

    public void SetRadius(float value)
    {
        transform.localScale = originScale * value;
        //collider2d.radius = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger && collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && Life > 0)
        {
            Life--;
            collision.gameObject.GetComponent<Enemy>().applyDmg(script.Dammage);
            OnHit?.Invoke(); 
            OnKill?.Invoke();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") && Life > 0)
        {
            Life = 0;
            Vector3 location = collision.ClosestPoint(transform.position);
            Vector3 dir = (location - transform.position);
            transform.eulerAngles = Vector3.RotateTowards(transform.eulerAngles, dir, 360, 360);
            OnHit?.Invoke();
        }
    }


    IEnumerator DelayBeforeKill()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(DelayBeforeDestroy);
		IsActive = false;
    }
}
