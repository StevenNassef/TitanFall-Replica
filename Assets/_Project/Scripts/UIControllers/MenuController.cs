using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class MenuController : MonoBehaviour
{
	public Image primaryWeaponSelectionImage;
	public Image heavyWeaponSelectionImage;
	public Image titanSelectionImage;

	public Weapon [] primaryWeaponData;
	public Weapon [] heavyWeaponData;
	public OneTitan [] titanData;
	public GameObject infoWindow;
	public Text selectionName;
	public Image selectionIcon;
	public Text selectionData;

	private int primaryWeaponPointer = 0;
	private int heavyWeaponPointer = 0;
	private int titanPointer = 0;

	private float musicVolume = 100;
	private float sfxVolume = 100;
	private float masterVolume = 100;
	private bool isLevelParkour;


	
    private int m_OpenParameterId;
	private Animator m_Open;
    private GameObject m_PreviouslySelected;


    const string k_OpenTransitionName = "Open";
	const string k_ClosedStateName = "Closed";

    public void OnClickToWar() {
        GameInitializations.PrimaryWeapon = primaryWeaponData[primaryWeaponPointer];
		GameInitializations.HeavyWeapon = heavyWeaponData[heavyWeaponPointer];
		GameInitializations.Titan = titanData[titanPointer]; 
		GameInitializations.MasterVolume = masterVolume;
		GameInitializations.MusicVolume = musicVolume;
		GameInitializations.SfxVolume = sfxVolume;
		GameInitializations.FirstLevelIsParkour = isLevelParkour;
		if(isLevelParkour) {
			UnityEngine.SceneManagement.SceneManager.LoadScene("Prototyping_ParkourScene");
		} else {
			UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
		}
    }

    public void OnClickQuit() {
		Application.Quit();
    }

	public void onMasterVolumeChanged(Slider slider) {
		masterVolume = slider.value;
	}

	public void onSfxVolumeChanged(Slider slider) {
		sfxVolume = slider.value;
	}

	public void onMusicVolumeChanged(Slider slider) {
		musicVolume = slider.value;
	}

	public void GetNextPrimaryWeapon(bool forward) {
		int step = -1;
		if(forward) {
			step = 1;
		}
		primaryWeaponPointer = modulus(primaryWeaponPointer + step, primaryWeaponData.Length);
		primaryWeaponSelectionImage.sprite = primaryWeaponData[primaryWeaponPointer].WeaponIcon;
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void GetNextHeavyWeapon(bool forward) {
		int step = -1;
		if(forward) {
			step = 1;
		}
		heavyWeaponPointer = modulus(heavyWeaponPointer + step, heavyWeaponData.Length);
		heavyWeaponSelectionImage.sprite = heavyWeaponData[heavyWeaponPointer].WeaponIcon;
		EventSystem.current.SetSelectedGameObject(null);
	}

	private static int modulus(int x, int m) {
		return (x%m + m)%m;
	}

	public void GetNextTitan(bool forward) {
		int step = -1;
		if(forward) {
			step = 1;
		}
		titanPointer = modulus(titanPointer + step, titanData.Length);
		titanSelectionImage.sprite = titanData[titanPointer].TitanIcon;
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void ShowInfo(int typeIndex) {
		Weapon weaponDataTmp;
		switch(typeIndex) {
			case 0:
				weaponDataTmp = primaryWeaponData[primaryWeaponPointer]; 
				selectionName.text = weaponDataTmp.WeaponName;
				selectionIcon.sprite = weaponDataTmp.WeaponIcon;
				selectionData.text = 
					"A Primary Weapon\n" +
					"Fire-mode		" + weaponDataTmp.FireMode + "\n" +
					"Shooting-type	" + weaponDataTmp.ShootingType + "\n" +
					"Damage			" + weaponDataTmp.Damage + "\n" + 
					"Fire-rate		" + weaponDataTmp.FireRate + "\n" +
					"Ammo-Count		" + weaponDataTmp.AmmoCount + "\n" +
					"Range			" + weaponDataTmp.Range;
				break;
			case 1:
				weaponDataTmp = heavyWeaponData[heavyWeaponPointer];
				WeaponProjectile projectileTmp = weaponDataTmp.Projectile; 
				selectionName.text = weaponDataTmp.WeaponName;
				selectionIcon.sprite = weaponDataTmp.WeaponIcon;
				selectionData.text = 
					"A Heavy Weapon\n" +
					"Fire-mode		" + weaponDataTmp.FireMode + "\n" +
					"Shooting-type	" + weaponDataTmp.ShootingType + "\n" +
					"Fire-rate		" + weaponDataTmp.FireRate + "\n" +
					"Ammo-Count		" + weaponDataTmp.AmmoCount + "\n" +
					"-Projectile-\n" +
					"Flying			" + projectileTmp.FlyingSpeed + 
					"(" + projectileTmp.MaxFlyingDistance + "meters)" + "\n" +
					"Damage			" + projectileTmp.Damage +
					"(" + projectileTmp.DamangeRange + ")" + "\n" + 
					"Explosive-Force" + projectileTmp.ExplosiveForce + "\n";
				break;
			case 2:
				OneTitan titanDataTmp = titanData[titanPointer];
				selectionName.text = titanDataTmp.TitanType.ToString();
				selectionIcon.sprite = titanDataTmp.TitanIcon;
				selectionData.text =
					"A Titan\n" +
					"\nPrimary-Weapon\n" +
					titanDataTmp.PrimaryWeaponDescription + "\n" +
					"\nDefensive-Ability\n" +
					titanDataTmp.DefensiveAbilityDescription + "\n" + 
					"\nCore-Ability\n" +
					titanDataTmp.CoreAbilityDescription + "\n";
				break;

		}				
		
		infoWindow.SetActive(true);
	}

	public void UnShowInfo() {
		infoWindow.SetActive(false);
	}

	public void OnParkourLevelSwitch(bool val) {
		isLevelParkour = val;
	}
    public void OpenPanel (Animator anim)
	{
		m_OpenParameterId = Animator.StringToHash (k_OpenTransitionName);

		if (m_Open == anim)
			return;

		anim.gameObject.SetActive(true);
		var newPreviouslySelected = EventSystem.current.currentSelectedGameObject;

		anim.transform.SetAsLastSibling();

		CloseCurrent();

		m_PreviouslySelected = newPreviouslySelected;

		m_Open = anim;
		m_Open.SetBool(m_OpenParameterId, true);

		// GameObject go = FindFirstEnabledSelectable(anim.gameObject);

		// SetSelected(go);
	}

	static GameObject FindFirstEnabledSelectable (GameObject gameObject)
	{
		GameObject go = null;
		var selectables = gameObject.GetComponentsInChildren<Selectable> (true);
		foreach (var selectable in selectables) {
			if (selectable.IsActive () && selectable.IsInteractable ()) {
				go = selectable.gameObject;
				break;
			}
		}
		return go;
	}


	public void CloseCurrent()
	{
		if (m_Open == null)
			return;

		m_Open.SetBool(m_OpenParameterId, false);
		SetSelected(m_PreviouslySelected);
		StartCoroutine(DisablePanelDeleyed(m_Open));
		m_Open = null;
	}

	
	IEnumerator DisablePanelDeleyed(Animator anim)
	{
		bool closedStateReached = false;
		bool wantToClose = true;
		while (!closedStateReached && wantToClose)
		{
			if (!anim.IsInTransition(0))
				closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(k_ClosedStateName);

			wantToClose = !anim.GetBool(m_OpenParameterId);

			yield return new WaitForEndOfFrame();
		}

		if (wantToClose)
			anim.gameObject.SetActive(false);
	}

	private void SetSelected(GameObject go)
	{
		EventSystem.current.SetSelectedGameObject(go);
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
