using UnityEngine;
using UnityEngine.UI;
using MyBox;

[System.Serializable]
[CreateAssetMenu(fileName = "Titan", menuName = "Titan")]
public class OneTitan : ScriptableObject
{
    [Header("Titan Stats")]
    [SerializeField] private Titan titanType;
    [SerializeField] private Sprite titanIcon;
    [SerializeField] private string primaryWeaponDescription;
    [SerializeField] private string defensiveAbilityDescription;
    [SerializeField] private string coreAbilityDescription;  

    public Titan TitanType => titanType;
    public Sprite TitanIcon => titanIcon;
    public string PrimaryWeaponDescription => primaryWeaponDescription;
    public string DefensiveAbilityDescription => defensiveAbilityDescription;
    public string CoreAbilityDescription => coreAbilityDescription;


}
