using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Animator))]
public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private LayerMask damageableLayers;

	[SerializeField] private Animation attackAnimation;
	private Animator animator;

	[SerializeField] private float basicDamage;
	[SerializeField] private DamageModifiersData damageModifiersData;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		
	}

	private void OnCollisionEnter(Collision collision)
	{
		
	}
}
