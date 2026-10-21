using System;
using UnityEngine;

public class NaturalNeedsSystem : MonoBehaviour
{
    [SerializeField] private NaturalNeedsConfig needsConfig;
	[SerializeField] private float currSatiety;
	[SerializeField] private float currHydration;

	public float CurrSatiety { 
		get => currSatiety; 
		private set => currSatiety = Mathf.Clamp(value, 0f, needsConfig.MaxSatiety); 
	}
	public float CurrHydration { 
		get => currHydration; 
		private set => currHydration = Mathf.Clamp(value, 0f, needsConfig.MaxHydration); 
	}

	public event Action OnSatietyValueChange;
	public event Action OnHydrationValueChange;

	private void Awake()
	{
		CurrSatiety = needsConfig.MaxSatiety;
		CurrHydration = needsConfig.MaxHydration;
	}

	private void Update()
	{
		CurrSatiety -= needsConfig.HungerIncreasingCoeff * Time.deltaTime;
		CurrHydration -= needsConfig.ThurstIncreasingCoeff * Time.deltaTime;

		OnSatietyValueChange?.Invoke();
		OnHydrationValueChange?.Invoke();
	}

	public void IncreaseSatiety(float value) => CurrSatiety += value;

	public void IncreaseHydration(float value) => CurrHydration += value;
}
