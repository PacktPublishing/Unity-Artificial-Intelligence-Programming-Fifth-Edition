using UnityEngine;
using System.Collections.Generic;

public abstract class MeshGenerator : MonoBehaviour {
    abstract public void GenerateMesh(int[,] map, float squareSize);
}