using UnityEngine;
using MyBox;
public class DamageHandler : MonoBehaviour
{
    [ReadOnly] [SerializeField] protected float currentHealthPoints;
    [SerializeField] protected ObjectType type;
    [SerializeField] protected float maxHealthPoints;

    public delegate void OnDamageHandlerEventWithArgs(float damage, ObjectType type);
    public event OnDamageHandlerEventWithArgs OnDamageTakenArgs;

    public delegate void OnDamageHandlerEvent();

    public event OnDamageHandlerEvent OnObjectDestroyed;
    public event OnDamageHandlerEvent OnDamageTaken;



    void Start()
    {
        InitializeHandler();
    }

    void Update()
    {

    }

    protected virtual void InitializeHandler()
    {
        currentHealthPoints = maxHealthPoints;
    }

    public bool TakeDamage(float damage, out ObjectType type)
    {
        type = this.type;
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