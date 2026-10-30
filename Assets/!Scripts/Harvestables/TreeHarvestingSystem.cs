using UnityEngine;

public class TreeHarvestingSystem : MonoBehaviour
{
    [SerializeField] private Terrain terrain;
	private TreeInstance[] terrainTreeInstances;

	private void Awake()
	{
		terrainTreeInstances = terrain.terrainData.treeInstances;

		foreach (var treeInstance in terrainTreeInstances)
		{
			
		}
	}
}
