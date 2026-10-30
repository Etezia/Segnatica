using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EntityHealth : MonoBehaviour
{
    [SerializeField] private EntityHealthData healthData;

	private float currHealth;
	private float currArmor;

	public EntityType Type => healthData.EntityType;

	public event Action OnDeath;

	public float CurrHealth
	{
		get => currHealth;
		private set => currHealth = Mathf.Clamp(value, 0, healthData.MaxHealth);
	}
	public float CurrArmor
	{
		get => currArmor;
		private set => currArmor = Mathf.Clamp(value, 0, healthData.MaxArmor);
	}

	private void Awake()
	{
		CurrHealth = healthData.MaxHealth;
	}

	public void GetDamage(float value)
	{
		CurrHealth -= value;
		Debug.Log(CurrHealth);
		if (CurrHealth <= 0)
		{
			OnDeath?.Invoke();
			Destroy(gameObject);
		}
	}

	public void HealUp(float value)
	{
		CurrHealth += value;
	}
}
