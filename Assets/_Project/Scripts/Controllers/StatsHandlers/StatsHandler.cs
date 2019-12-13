using UnityEngine;
using MyBox;
public class StatsHandler : MonoBehaviour
{
    [ReadOnly] [SerializeField] protected float currentHealthPoints;
    [SerializeField] protected ObjectType type;
    [SerializeField] protected float maxHealthPoints;

    public delegate void OnStatsHandlerEventWithArgs(float damage, ObjectType type);
    public event OnStatsHandlerEventWithArgs OnDamageTakenArgs;

    public delegate void OnStatsHandlerEvent();

    public event OnStatsHandlerEvent OnObjectDestroyed;
    public event OnStatsHandlerEvent OnDamageTaken;



    void Start()
    {
        InitializeHandler();
    }

    protected virtual void Update()
    {

    }

    protected virtual void InitializeHandler()
    {
        currentHealthPoints = maxHealthPoints;
    }
    public virtual bool TakeDamage(float damage, out ObjectType type)
    {   
        type = this.type;
        return TakeDamage(damage);
    }
    public virtual bool TakeDamage(RayCastDamage castDamage, out ObjectType type)
    {
        return TakeDamage(castDamage.Damage, out type);
    }

    public virtual bool TakeDamage(float damage)
    {
        
        float finalDamage = CalculateDamage(damage, type);

        //Subtract the final damage from the currentHealthPoints
        currentHealthPoints -= finalDamage;
        if(currentHealthPoints < 0)
            currentHealthPoints = 0;

        //Call the OnDamageTaken Event
        if (OnDamageTakenArgs != null)
        {
            OnDamageTakenArgs.Invoke(finalDamage, type);
        }
        if (OnDamageTaken != null)
        {
            Debug.Log("Damage Taken Invoked");
            OnDamageTaken.Invoke();
        }
        HitEffect(finalDamage);
        if (currentHealthPoints <= 0)
        {
            //TODO : try to find a better way of handling this

            //Call the OnObjectDestroyed Event
            if (OnObjectDestroyed != null)
            {
                OnObjectDestroyed.Invoke();
            }
            DestroyObject();
            return true;
        }
        return false;
    }
    [ButtonMethod]
    protected void SimulateDamage()
    {
        ObjectType type;
        TakeDamage(10, out type);
    }
    [ButtonMethod]
    protected void PauseGame()
    {
        Time.timeScale = 0;
    }
    [ButtonMethod]
    protected void ResumeGame()
    {
        Time.timeScale = 1;
    }
    protected virtual float CalculateDamage(float damage, ObjectType type)
    {
        // Debug.Log(damage);
        return damage;
    }
    protected virtual void HitEffect(float damage)
    {
        //TODO : make an effect according to the value of the damage
    }
    protected virtual void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
public enum ObjectType
{
    Pilot, Titan, Env, Sheild
}

public struct RayCastDamage
{
    public RayCastDamage(float damage, float range)
    {
        this.damage = damage;
        this.range = range;
    }
    float damage;
    float range;

    public float Damage => damage;
    public float Range => range;
}