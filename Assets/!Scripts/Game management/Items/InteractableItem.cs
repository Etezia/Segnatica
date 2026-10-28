using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    private MeshRenderer renderer;
	private List<Material> initialMaterialsSet;

	private void Awake()
	{
		renderer = GetComponent<MeshRenderer>();
		initialMaterialsSet = new List<Material>();
		renderer.GetSharedMaterials(initialMaterialsSet);
	}

	public void AddMaterial(Material material)
	{
		var materials = renderer.sharedMaterials.ToList();
		materials.Add(material);
		renderer.SetSharedMaterials(materials);
	}

	public void ResetMaterials()
	{
		renderer.SetSharedMaterials(initialMaterialsSet);
	}
}
