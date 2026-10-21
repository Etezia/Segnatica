using System;
using UnityEngine;

public class NaturalNeedsSystem : MonoBehaviour
{
	public NaturalNeedsSystem Instance { get; private set; }

    [SerializeField] private NaturalNeedsConfig needsConfig;
	[SerializeField] private float currSatiety;
	[SerializeField] private float currHydration;

	private float satietyRatio => currSatiety / needsConfig.MaxSatiety;
	private float hydrationRatio => currHydration / needsConfig.MaxHydration;

	public float CurrSatiety { 
		get => currSatiety; 
		private set => currSatiety = Mathf.Clamp(value, 0f, needsConfig.MaxSatiety); 
	}
	public float CurrHydration { 
		get => currHydration; 
		private set => currHydration = Mathf.Clamp(value, 0f, needsConfig.MaxHydration); 
	}

	public event Action<float> OnSatietyValueChange;
	public event Action<float> OnHydrationValueChange;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this.gameObject);

			CurrSatiety = needsConfig.MaxSatiety;
		CurrHydration = needsConfig.MaxHydration;
	}

	private void Update()
	{
		CurrSatiety -= needsConfig.HungerIncreasingCoeff * Time.deltaTime;
		CurrHydration -= needsConfig.ThurstIncreasingCoeff * Time.deltaTime;

		OnSatietyValueChange?.Invoke(satietyRatio);
		OnHydrationValueChange?.Invoke(hydrationRatio);
	}

	public void IncreaseSatiety(float value) => CurrSatiety += value;

	public void IncreaseHydration(float value) => CurrHydration += value;
}
