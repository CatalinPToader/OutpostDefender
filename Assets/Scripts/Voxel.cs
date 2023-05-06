using UnityEngine;
using System.Collections.Generic;

public class Voxel : MonoBehaviour
{
	public Material material;
	private int[][] faces = {
		new int[]{ // front
			0, 2, 1,
			1, 2, 3
		},

		new int[]{ // back
			4, 5, 6,
			5, 7, 6
		},

		new int[]{ // top
			8, 10, 9,
			9, 10 ,11
		},
		new int[]{ // bottom
			12, 13, 14,
			13, 15, 14
		},
		new int[]{ // left
			16, 18, 17,
			17, 18, 19
		},
		new int[]{ // right
			20, 22, 21,
			21, 22, 23
		}
	};

	private int[] triangles;
	private Mesh mesh = null;
    public MeshFilter meshFilter = null;
	private Vector3[] vertices = {
        // Front Face
        new Vector3(0, 0, 0), // Front bottom left
        new Vector3(1, 0, 0), // Front bottom right
        new Vector3(0, 1, 0), // Front top left
        new Vector3(1, 1, 0), // Front top right

        // Back face
        new Vector3(0, 0, 1), // Back bottom left
        new Vector3(1, 0, 1), // Back bottom right
        new Vector3(0, 1, 1), // Back top left
        new Vector3(1, 1, 1), // Back top right

        // Top face
        new Vector3(0, 1, 0), // Front top left
        new Vector3(1, 1, 0), // Front top right
        new Vector3(0, 1, 1), // Back top left
        new Vector3(1, 1, 1), // Back top right

        // Bottom face
        new Vector3(0, 0, 0), // Front bottom left
        new Vector3(1, 0, 0), // Front bottom right
        new Vector3(0, 0, 1), // Back bottom left
        new Vector3(1, 0, 1),  // Back bottom right

        // Left face
        new Vector3(0, 0, 0), // Front bottom left
        new Vector3(0, 1, 0), // Front top left
        new Vector3(0, 0, 1), // Back bottom left
        new Vector3(0, 1, 1), // Back top left

        // Right face
        new Vector3(1, 1, 0), // Front top right
        new Vector3(1, 0, 0), // Front bottom right
        new Vector3(1, 1, 1), // Back top right
        new Vector3(1, 0, 1), // Back bottom right
		};

	public int disabledFaces = 0;

	public void Start()
	{
        mesh = new Mesh();

		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshRenderer.material = material;

		createTriangles();
		redoMesh();
		meshFilter.mesh = mesh;
	}

	private void createTriangles()
	{
		List<int> trianglesList = new List<int>();
		for (int i = 0; i < 6; i++)
		{
			int mask = 1 << i;
			if ((mask & disabledFaces) == 0)
				trianglesList.AddRange(faces[i]);
		}
		triangles = trianglesList.ToArray();
	}

	public void disableFace(string face)
	{
		switch (face)
		{
			case "front":
				disabledFaces |= 1 << 0;
				break;
			case "back":
				disabledFaces |= 1 << 1;
				break;
			case "top":
				disabledFaces |= 1 << 2;
				break;
			case "bottom":
				disabledFaces |= 1 << 3;
				break;
			case "left":
				disabledFaces |= 1 << 4;
				break;
			case "right":
				disabledFaces |= 1 << 5;
				break;
			default:
				return;
		}

		createTriangles();
        if (mesh != null)
		    redoMesh();
	}

	public void enableFace(string face)
	{
		switch (face)
		{
			case "front":
				disabledFaces &= ~(1 << 0);
				break;
			case "back":
				disabledFaces &= ~(1 << 1);
				break;
			case "top":
				disabledFaces &= ~(1 << 2);
				break;
			case "bottom":
				disabledFaces &= ~(1 << 3);
				break;
			case "left":
				disabledFaces &= ~(1 << 4);
				break;
			case "right":
				disabledFaces &= ~(1 << 5);
				break;
			default:
				return;
		}

		createTriangles();
        if (mesh != null)
		    redoMesh();
	}


	public void redoMesh()
	{
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.Optimize();
		mesh.RecalculateNormals();
	}
}
