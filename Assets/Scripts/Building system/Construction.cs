using UnityEngine;

[RequireComponent(typeof(Collider), typeof(EntityHealthData))]
public class Construction : MonoBehaviour
{
    [SerializeField] private string constructionName;
}
