using UnityEngine;
using MyBox;
public class DamageHandler : MonoBehaviour
{
    [ReadOnly] [SerializeField] private float currentHealthPoints;
    [SerializeField] protected ObjectType type;
    [SerializeField] protected float maxHealthPoints;

    public delegate void OnDamageHandlerEventWithArgs(float damage, ObjectType type);
    public event OnDamageHandlerEventWithArgs OnDamageTaken;

    public delegate void OnDamageHandlerEvent();

    public event OnDamageHandlerEvent OnObjectDestroyed;


    void Start()
    {
        currentHealthPoints = maxHealthPoints;
    }

    void Update()
    {

    }

    public bool TakeDamage(float damage, out ObjectType type)
    {
        type = this.type;
        float finalDamage = CalculateDamage(damage, type);

        //Subtract the final damage from the currentHealthPoints
        currentHealthPoints -= finalDamage;
        
        //Call the OnDamageTaken Event
        if(OnDamageTaken != null)
        {
            OnDamageTaken.Invoke(finalDamage, type);
        }
        HitEffect(finalDamage);
        if (currentHealthPoints <= 0)
        {
            //TODO : try to find a better way of handling this

            //Call the OnObjectDestroyed Event
            if(OnObjectDestroyed != null)
            {
                OnObjectDestroyed.Invoke();
            }
            DestroyObject();
            return true;
        }
        return false;
    }

    protected virtual float CalculateDamage(float damage, ObjectType type)
    {
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
    Pilot, Titan, Env
}