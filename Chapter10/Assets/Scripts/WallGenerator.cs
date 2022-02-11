using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MeshGenerator
{

    public GameObject wallCube;

    public override void GenerateMesh(int[,] map, float squareSize) {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        for (int c = 0; c < width; c++) {
            for (int r = 0; r < height; r++) {
                if (map[c, r] == 1) {
                    GameObject obj = Instantiate(wallCube, new Vector3(c * squareSize, 0, r * squareSize), Quaternion.identity);
                    obj.transform.parent = transform;
                }
            }
        }
        transform.position = new Vector3(-width / 2.0f, 0, -height / 2.0f);
        MergeCubes();
    }

    private void MergeCubes() {
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true);
        transform.gameObject.SetActive(true);
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }
    }

}
