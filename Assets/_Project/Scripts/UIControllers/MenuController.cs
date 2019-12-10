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
	// public ScriptableObject [] titanData;

	private int primaryWeaponPointer = 0;
	private int heavyWeaponPointer = 0;
	private int titanPointer = 0;

	
    private int m_OpenParameterId;
	private Animator m_Open;
    private GameObject m_PreviouslySelected;


    const string k_OpenTransitionName = "Open";
	const string k_ClosedStateName = "Closed";

    public void OnClickToWar() {
        // TODO 
    }

    public void OnClickQuit() {
		Application.Quit();
    }

	public void onMasterVolumeChanged() {
		// TODO
	}

	public void onSfxVolumeChanged() {
		// TODO
	}

	public void onMusicVolumeChanged() {
		// TODO
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
		// int step = -1;
		// if(forward) {
		// 	step = 1;
		// }
		// titanPointer = modulus(titanPointer + step, titanData.Length);
		// titanSelectionImage.sprite = titanData[titanPointer].titanIcon;
		EventSystem.current.SetSelectedGameObject(null);
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
