using UnityEngine;
using System.Collections.Generic;

//Grid manager class handles all the grid properties
public class GridManager : MonoBehaviour {
    // staticInstance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static GridManager staticInstance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static GridManager instance {
        get {
            if (staticInstance == null) {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first GridManager object in the scene.
                staticInstance = FindObjectOfType(typeof(GridManager)) as GridManager;
                if (staticInstance == null)
                    Debug.Log("Could not locate an GridManager object. \n You have to have exactly one GridManager in the scene.");
            }
            return staticInstance;
        }
    }

    // Ensure that the instance is destroyed when the game is stopped in the editor.
    void OnApplicationQuit() {
        staticInstance = null;
    }

    public int numOfRows;
    public int numOfColumns;
    public float gridCellSize;
    public float obstacleEpsilon = 0.2f;
    public bool showGrid = true;
    public bool showObstacleBlocks = true;

    public Node[,] nodes { get; set; }

    //Origin of the grid manager
    public Vector3 Origin {
        get { return transform.position; }
    }

    public float StepCost {
        get { return gridCellSize; }
    }

    //Initialise the grid manager
    void Awake() {
        ComputeGrid();
    }

    /// <summary>
    /// Calculate which cells in the grids are mark as obstacles
    /// </summary>
    void ComputeGrid() {
        //Initialise the nodes
        nodes = new Node[numOfColumns, numOfRows];

        for (int i = 0; i < numOfColumns; i++) {
            for (int j = 0; j < numOfRows; j++) {
                Vector3 cellPos = GetGridCellCenter(i,j);
                Node node = new(cellPos);


                var collisions = Physics.OverlapSphere(cellPos, gridCellSize / 2 - obstacleEpsilon, 1 << LayerMask.NameToLayer("Obstacles"));
                if (collisions.Length != 0) {
                    node.MarkAsObstacle();
                }
                nodes[i, j] = node;
            }
        }
    }

    /// <summary>
    /// Returns position of the grid cell in world coordinates
    /// </summary>
    public Vector3 GetGridCellCenter(int col, int row) {
        Vector3 cellPosition = GetGridCellPosition(col, row);
        cellPosition.x += gridCellSize / 2.0f;
        cellPosition.z += gridCellSize / 2.0f;

        return cellPosition;
    }

    /// <summary>
    /// Returns position of the grid cell in a given index
    /// </summary>
    public Vector3 GetGridCellPosition(int col, int row) {
        float xPosInGrid = col * gridCellSize;
        float zPosInGrid = row * gridCellSize;

        return Origin + new Vector3(xPosInGrid, 0.0f, zPosInGrid);
    }

    /// <summary>
    /// Get the grid cell index in the Astar grids with the position given
    /// </summary>
    public (int,int) GetGridCoordinates(Vector3 pos) {
        if (!IsInBounds(pos)) {
            return (-1,-1);
        }

        int col = (int)Mathf.Floor((pos.x-Origin.x) / gridCellSize);
        int row = (int)Mathf.Floor((pos.z-Origin.z) / gridCellSize);

        return (col, row);
    }

    /// <summary>
    /// Check whether the current position is inside the grid or not
    /// </summary>
    public bool IsInBounds(Vector3 pos) {
        float width = numOfColumns * gridCellSize;
        float height = numOfRows * gridCellSize;

        return (pos.x >= Origin.x && pos.x <= Origin.x + width && pos.x <= Origin.z + height && pos.z >= Origin.z);
    }

    public bool IsTraversable(int col, int row) {
        return col >= 0 && row >= 0 && col < numOfColumns && row < numOfRows && !nodes[col, row].isObstacle;
    }


    /// <summary>
    /// Get the neighour nodes in 4 different directions
    /// </summary>
    public List<Node> GetNeighbours(Node node) {
        List<Node> result = new();
        var (column, row) = GetGridCoordinates(node.position);

        if (IsTraversable(column - 1, row)) {
            result.Add(nodes[column - 1, row]);
        }
        if (IsTraversable(column + 1, row)) {
            result.Add(nodes[column + 1, row]);
        }
        if (IsTraversable(column, row - 1)) {
            result.Add(nodes[column, row - 1]);
        }
        if (IsTraversable(column, row + 1)) {
            result.Add(nodes[column, row + 1]);
        }
        return result;
    }

    /// <summary>
    /// Show Debug Grids and obstacles inside the editor
    /// </summary>
    void OnDrawGizmos() {
        //Draw Grid
        if (showGrid) {
            DebugDrawGrid(Color.blue);
        }

        //Grid Start Position
        Gizmos.DrawSphere(Origin, 0.5f);

        if (nodes == null) return;

        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(nodes[3,10].position, 0.5f);

        //Gizmos.color = Color.cyan;
        //var neighbors = GetNeighbours(nodes[3, 10]);
        //foreach (var n in neighbors) {
        //    Gizmos.DrawSphere(n.position, 0.5f);
        //}

        //Draw Obstacle obstruction
        if (showObstacleBlocks) {
            Vector3 cellSize = new Vector3(gridCellSize, 1.0f, gridCellSize);
            Gizmos.color = Color.red;
            for (int i = 0; i < numOfColumns; i++) {
                for (int j = 0; j < numOfRows; j++) {
                    if (nodes != null && nodes[i, j].isObstacle) {
                        Gizmos.DrawCube(GetGridCellCenter(i,j), cellSize);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Draw the debug grid lines in the rows and columns order
    /// </summary>
    public void DebugDrawGrid(Color color) {
        float width = (numOfColumns * gridCellSize);
        float height = (numOfRows * gridCellSize);

        // Draw the horizontal grid lines
        for (int i = 0; i < numOfRows + 1; i++) {
            Vector3 startPos = Origin + i * gridCellSize * new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 endPos = startPos + width * new Vector3(1.0f, 0.0f, 0.0f);
            Debug.DrawLine(startPos, endPos, color);
        }

        // Draw the vertial grid lines
        for (int i = 0; i < numOfColumns + 1; i++) {
            Vector3 startPos = Origin + i * gridCellSize * new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 endPos = startPos + height * new Vector3(0.0f, 0.0f, 1.0f);
            Debug.DrawLine(startPos, endPos, color);
        }
    }

}
