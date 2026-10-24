using UnityEngine;

public class PlayerSockets : MonoBehaviour
{
    public static PlayerSockets Instance { get; private set; }

    [field: SerializeField] public Transform ItemDropPoint { get; private set; }

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this.gameObject);
	}
}
