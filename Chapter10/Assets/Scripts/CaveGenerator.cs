using UnityEngine;
using System;

public class CaveGenerator : MonoBehaviour {

    [SerializeField]
    private int width;

    [SerializeField]
    public int height;

    [SerializeField]
    private int seed;

    [SerializeField]
    private bool useRandomSeed;

    private int[,] map;

    void Start() {
        InitializeRandomGrid();
        DrawCaveMesh();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            CellularAutomata(false);
            DrawCaveMesh();
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            CellularAutomata(true);
            DrawCaveMesh();
        } else if (Input.GetKeyDown(KeyCode.N)) {
            InitializeRandomGrid();
            DrawCaveMesh();
        }
    }

    void InitializeRandomGrid() {
        map = new int[width, height];
        if (useRandomSeed) {
            seed = (int)DateTime.Now.Ticks;
        }

        System.Random randomGen = new System.Random(seed.GetHashCode());

        int mapMiddle = (height / 2);

        for (int c = 0; c < width; c++) {
            for (int r = 0; r < height; r++) {
                if (c == 0 || c == width - 1 || r == 0 || r == height - 1) {
                    map[c, r] = 1;
                } else {
                    if (c == mapMiddle) {
                        map[c, r] = 0;
                    } else {
                        map[c, r] = (randomGen.Next(0, 100) < 50) ? 1 : 0;
                    }
                }
            }
        }
    }

    void DrawCaveMesh() {
        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(map, 1);
    }

    void CellularAutomata(bool clean = false) {
        int[,] newmap = new int[width, height];
        for (int c = 0; c < width; c ++) {
            for (int r = 0; r < height; r ++) {
                int numWalls = GetSurroundingWallCount(c, r, 1);
                int numWalls2 = GetSurroundingWallCount(c, r, 2);

                if (isWall(c,r)) {
                    if (numWalls > 3) {
                        newmap[c, r] = 1;
                    } else {
                        newmap[c, r] = 0;
                    }
                } else {
                    if (!clean) {
                        if (numWalls >= 5 || numWalls2 <= 2) {
                            newmap[c, r] = 1;
                        } else {
                            newmap[c, r] = 0;
                        }
                    } else {
                        if (numWalls >= 5) {
                            newmap[c, r] = 1;
                        } else {
                            newmap[c, r] = 0;
                        }
                    }
                }
            }
        }
        map = newmap;
    }

    int GetSurroundingWallCount(int c, int r, int size) {
        int wallCount = 0;
        for (int iX = c - size; iX <= c + size; iX ++) {
            for (int iY = r - size; iY <= r + size; iY ++) {
                if (iX != c || iY != r) {
                    wallCount += isWall(iX, iY) ? 1 : 0;
                }
            }
        }

        return wallCount;
    }

    bool isWall(int c, int r) {
        if (c < 0 || r < 0) {
            return true;
        }
        if (c > width - 1 || r > height - 1) {
            return true;
        }
        return map[c, r] == 1;
    }

}