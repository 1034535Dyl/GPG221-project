using UnityEngine;

public partial class UnitControl : MonoBehaviour
{
	public GameObject backpackRef;
	private bool _hasCargo;

	public bool HasCargo
	{
		get { return _hasCargo; }
		set 
		{
			_hasCargo = value;
			backpackRef.SetActive(_hasCargo);
		}
	}
}

