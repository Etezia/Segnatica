using UnityEngine;

[CreateAssetMenu(fileName = "Character Movement Data", menuName = "Scriptable Objects/Character Movement Data")]
public class CharacterMovementData : ScriptableObject
{
	[field: SerializeField] public float BasicSpeed { get; private set; }
	[field: SerializeField] public float RunSpeed { get; private set; }
	[field: SerializeField] public float CrouchSpeed { get; private set; }
	[field: SerializeField] public float SideSpeedCoeff { get; private set; }
	[field: SerializeField] public float BackwardsSpeedCoeff { get; private set; }
	[field: SerializeField] public float InAirControlCoeff { get; private set; }
	[field: SerializeField] public float JumpForce { get; private set; }
}
