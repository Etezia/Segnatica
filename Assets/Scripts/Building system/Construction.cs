using UnityEngine;

[RequireComponent(typeof(Collider), typeof(EntityHealth))]
public class Construction : MonoBehaviour
{
    [SerializeField] private string constructionName;
}
