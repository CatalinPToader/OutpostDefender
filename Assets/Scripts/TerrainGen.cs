using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGen : MonoBehaviour
{
	public int terrainSize = 10;
	public Material matForCube;

	struct Cube
	{
		public GameObject go;
		public Voxel script;
	}
	struct Layers
	{
		public List<Cube> terrain;
		public GameObject combined;
	}

	void Start()
	{
		int[,,] matrix = new int[terrainSize, terrainSize, terrainSize]; // Create a 10x10x10 matrix
		Layers[] terrainLayers = new Layers[terrainSize];
		for (int j = 0; j < terrainSize; j++)
		{
			terrainLayers[j].terrain = new List<Cube>();
			List<MeshFilter> meshFilters = new List<MeshFilter>();
			for (int i = 0; i < terrainSize; i++)
			{
				for (int k = 0; k < terrainSize; k++)
				{
					//float noiseValue = Perlin3D.Noise(i * 0.1f, j * 0.1f + 100f, k * 0.1f + 200f);
					GameObject newVoxel = new GameObject(string.Format("Voxel {0} {1} {2}", i, j, k));
					newVoxel.transform.SetParent(transform);
					newVoxel.transform.position = new Vector3(i, j, k);
					Voxel script = newVoxel.AddComponent<Voxel>();
					script.enabled = false;
					script.material = matForCube;
					MeshFilter meshFilterVoxel = newVoxel.AddComponent<MeshFilter>();
					script.meshFilter = meshFilterVoxel;
					script.Start();
					meshFilters.Add(meshFilterVoxel);

					Cube newCube = new Cube { go = newVoxel, script = script };
					terrainLayers[j].terrain.Add(newCube);

					if (j != 0 && j != terrainSize - 1)
					{
						newCube.script.disableFace("top");
						newCube.script.disableFace("bottom");
					}
					else if (j == 0)
					{
						newCube.script.disableFace("top");
					}
					else if (j == terrainSize - 1)
					{
						newCube.script.disableFace("bottom");
					}

					if (i != 0 && i != terrainSize - 1)
					{
						newCube.script.disableFace("left");
						newCube.script.disableFace("right");
					}
					else if (i == 0)
					{
						newCube.script.disableFace("right");
					}
					else if (i == terrainSize - 1)
					{
						newCube.script.disableFace("left");
					}

					if (k != 0 && k != terrainSize - 1)
					{
						newCube.script.disableFace("front");
						newCube.script.disableFace("back");
					}
					else if (k == 0)
					{
						newCube.script.disableFace("back");
					}
					else if (k == terrainSize - 1)
					{
						newCube.script.disableFace("front");
					}
				}
			}

			CombineInstance[] combine = new CombineInstance[meshFilters.Count];
			for (int i = 0; i < meshFilters.Count; i++)
			{
				combine[i].mesh = meshFilters[i].sharedMesh;
				combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			}

			terrainLayers[j].combined = new GameObject(string.Format("CombinedMeshLayer{0}", j));
			terrainLayers[j].combined.transform.SetParent(transform);
			MeshFilter combinedMeshFilter = terrainLayers[j].combined.AddComponent<MeshFilter>();
			combinedMeshFilter.mesh = new Mesh();
			combinedMeshFilter.mesh.CombineMeshes(combine);

			MeshRenderer meshRenderer = terrainLayers[j].combined.AddComponent<MeshRenderer>();
			meshRenderer.material = matForCube;

			foreach (Cube c in terrainLayers[j].terrain) {
				c.go.GetComponent<MeshRenderer>().enabled = false;
			}
		}
	}
}