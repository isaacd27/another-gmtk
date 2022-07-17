using UnityEngine;
using UnityEngine.Events;

public class Enemy : IPoolableObject
{
    public UnityEvent OnDeath;


    [SerializeField]
    private float DelayBeforeDestroy = 3f;
    
    // [SerializeField]
    // Dung Prefab;

    [SerializeField]
    private int life;
    public int Life
    {
        get
        {
            return life;
        }
        set
        {
            life = value;
            if(life <= 0)
            {
                Death();
            }
        }
    }

    void Death()
    {
        BoidsManager.Instance.Boids.Remove(gameObject);
        Vector3 newLoc = transform.position;
        newLoc.z = 0;
        // Instantiate(Prefab, newLoc, Quaternion.identity);
        OnDeath?.Invoke();
        Spawner gao = FindObjectOfType<Spawner>();
        if(gao)
        {
            gao.SpawnAt(gameObject.transform);
        }
        // AudioManager.Instance.PlayHumanDyingRandomAudio(newLoc);
        //StartCoroutine(DelayBeforeKill_Coroutine());
        Destroy(this.gameObject);
    }

    public void applyDmg(int dmg)
    {
        Life -= dmg;
    }

    public void Kill()
    {
        Life = 0;
    }

    //IEnumerator DelayBeforeKill_Coroutine()
    //{
    //    GetComponent<Rigidbody2D>().simulated = false;
    //    GetComponentInChildren<Renderer>().enabled = false;
    //    yield return new WaitForSeconds(DelayBeforeDestroy);
    //    Destroy(this.gameObject);
    //}
}
