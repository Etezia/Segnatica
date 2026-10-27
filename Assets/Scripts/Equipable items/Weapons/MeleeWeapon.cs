using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider), typeof(Animator))]
public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private LayerMask damageableLayers;
	[SerializeField] private BoxCollider collider;
	private Collider[] overlappedColliders = new Collider[1];

	[SerializeField] private string attackStageName;
	private Animator animator;

	[SerializeField] private float basicDamage;
	[SerializeField] private DamageModifiersData damageModifiersData;
	private bool isWeaponReady = true;

	[SerializeField] private InputActionAsset inputSystem;
	private InputAction attackAction;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		collider = GetComponent<BoxCollider>();

		isWeaponReady = true;

		attackAction = inputSystem.FindAction("Attack");
	}

	private void OnEnable()
	{
		attackAction.performed += PlayAttackAnimation;
	}

	private void OnDisable()
	{
		attackAction.performed -= PlayAttackAnimation;
	}

	private void PlayAttackAnimation(InputAction.CallbackContext context)
	{
		if (isWeaponReady)
			animator.Play(attackStageName);
	}

	public void StartAttack()
	{
		isWeaponReady = false;
		StartCoroutine(TryHitUntilAttackEnd(5));
	}

	private IEnumerator TryHitUntilAttackEnd(int attackFrameCount)
	{
		for (var i = 0; i < attackFrameCount; i++)
		{
			var collidersCount =
				Physics.OverlapBoxNonAlloc(transform.TransformPoint(collider.center), Vector3.Scale(transform.lossyScale, collider.size) * 0.5f,
				overlappedColliders, transform.rotation, damageableLayers, QueryTriggerInteraction.Ignore);

			for (var j = 0; j < collidersCount; j++)
			{
				if (overlappedColliders[j].TryGetComponent<EntityHealth>(out var entityHealth))
				{
					entityHealth.GetDamage(basicDamage);
				}
			}

			if (collidersCount > 0)
				break;
			else
				yield return new WaitForEndOfFrame();
		}

		yield break;
	}

	public void OnAttackEnded() => isWeaponReady = true;
}
//это по большей части затычка для реализации механики рубки деревьев. системы вооружения в игре пока что нет :(