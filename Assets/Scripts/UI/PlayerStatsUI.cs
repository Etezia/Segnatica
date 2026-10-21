using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
	[SerializeField] private NaturalNeedsSystem playerNeedsSystem;

    [SerializeField] private Image healthBar;
	[SerializeField] private Image satietyBar;
	[SerializeField] private Image hydrationBar;
	[SerializeField] private Image armorBar;
	[SerializeField] private Image manaBar;

	private void OnEnable()
	{
		playerNeedsSystem.OnSatietyValueChange += UpdateSatietyValue;
		playerNeedsSystem.OnHydrationValueChange += UpdateHydrationValue;
	}

	private void OnDisable()
	{
		playerNeedsSystem.OnSatietyValueChange -= UpdateSatietyValue;
		playerNeedsSystem.OnHydrationValueChange -= UpdateHydrationValue;
	}

	private void UpdateSatietyValue(float fillAmount) => satietyBar.fillAmount = fillAmount;

	private void UpdateHydrationValue(float fillAmount) => hydrationBar.fillAmount = fillAmount;
}
