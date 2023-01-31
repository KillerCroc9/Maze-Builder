using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
public class MazeGenrator : MonoBehaviour
{
    public GameObject Maze;

    public GameObject cube;
    int size = 100;
   
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("MazeCaller", 1f, 0f);
        
    }

    void MazeCaller()
    {
        MyMaze maze = new MyMaze(size, size);
        maze.UpdateGrid();
        for (int i = 0; i < size - 1; i++)
        {
            for (int j = 0; j < size - 1; j++)
            {
                if (maze.mazeGrid[i, j] == 'X')
                {
                    Instantiate(cube, new Vector3((j*10)-450, 5, (i*10)- 450), Quaternion.identity, Maze.transform);
                }

            }
        }
        //Invoke("cleaner", 4.5f);
    }
    void cleaner()
    {
        foreach (Transform child in Maze.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Creates a random, perfect (without cycles) maze
    /// </summary>
    public class MyMaze
    {
        // The code of the class is an adaptation, with the original commentary, of the answer given
        // by user DoubleMx2 on August 25'13 to a question posted by user nazar_art at stackoverflow.com:

        private int dimensionX, dimensionY; // dimension of maze
        public int gridDimensionX, gridDimensionY; // dimension of output grid
        public char[,] mazeGrid; // output grid
        private Cell[,] cells; // 2d array of Cells
        private System.Random random = new System.Random(); // The random object

        // constructor
        public MyMaze(int xDimension, int yDimension)
        {
            dimensionX = xDimension;              // dimension of maze
            dimensionY = yDimension;
            gridDimensionX = xDimension * 2 + 1;  // dimension of output grid
            gridDimensionY = yDimension * 2 + 1;
            mazeGrid = new char[gridDimensionX, gridDimensionY];
            Init();
            GenerateMaze();
        }

        private void Init()
        {
            // create cells
            cells = new Cell[dimensionX, dimensionY];
            for (int x = 0; x < dimensionX; x++)
                for (int y = 0; y < dimensionY; y++)
                    cells[x, y] = new Cell(x, y, false); // create cell (see Cell constructor)
        }

        // inner class to represent a cell
        public class Cell
        {
            public int x, y; // coordinates
                             // cells this cell is connected to
            ArrayList neighbors = new ArrayList();
            // impassable cell
            public bool wall = true;
            // if true, has yet to be used in generation
            public bool open = true;
            // construct Cell at x, y
            public Cell(int x, int y)
            {
                this.x = x;
                this.y = y;
                wall = true;
            }
            // construct Cell at x, y and with whether it isWall
            public Cell(int x, int y, bool isWall)
            {
                this.x = x;
                this.y = y;
                wall = isWall;
            }
            // add a neighbor to this cell, and this cell as a neighbor to the other
            public void AddNeighbor(Cell other)
            {
                if (!this.neighbors.Contains(other))
                    // avoid duplicates
                    this.neighbors.Add(other);
                if (!other.neighbors.Contains(this))
                    // avoid duplicates
                    other.neighbors.Add(this);
            }
            // used in updateGrid()
            public bool IsCellBelowNeighbor()
            {
                return this.neighbors.Contains(new Cell(this.x, this.y + 1));
            }
            // used in updateGrid()
            public bool IsCellRightNeighbor()
            {
                return this.neighbors.Contains(new Cell(this.x + 1, this.y));
            }

            // useful Cell equivalence
            public override bool Equals(System.Object other)
            {
                //if (!(other instanceof Cell)) return false;
                if (other.GetType() != typeof(Cell))
                    return false;
                Cell otherCell = (Cell)other;
                return (x == otherCell.x) && (y == otherCell.y);
            }

            // should be overridden with equals
            public override int GetHashCode()
            {
                // random hash code method designed to be usually unique
                return x + y * 256;
            }

        }
        // generate from upper left (In computing the y increases down often)
        private void GenerateMaze()
        {
            GenerateMaze(0, 0);
        }
        // generate the maze from coordinates x, y
        private void GenerateMaze(int x, int y)
        {
            GenerateMaze(GetCell(x, y)); // generate from Cell
        }
        private void GenerateMaze(Cell startAt)
        {
            // don't generate from cell not there
            if (startAt == null) return;
            startAt.open = false; // indicate cell closed for generation
            var cellsList = new ArrayList { startAt };

            while (cellsList.Count > 0)
            {
                Cell cell;
                // this is to reduce but not completely eliminate the number
                // of long twisting halls with short easy to detect branches
                // which results in easy mazes
                if (random.Next(10) == 0)
                {
                    cell = (Cell)cellsList[random.Next(cellsList.Count)];
                    cellsList.RemoveAt(random.Next(cellsList.Count));
                }

                else
                {
                    cell = (Cell)cellsList[cellsList.Count - 1];
                    cellsList.RemoveAt(cellsList.Count - 1);
                }
                // for collection
                ArrayList neighbors = new ArrayList();
                // cells that could potentially be neighbors
                Cell[] potentialNeighbors = new Cell[]{
                        GetCell(cell.x + 1, cell.y),
                        GetCell(cell.x, cell.y + 1),
                        GetCell(cell.x - 1, cell.y),
                        GetCell(cell.x, cell.y - 1)
                    };
                foreach (Cell other in potentialNeighbors)
                {
                    // skip if outside, is a wall or is not opened
                    if (other == null || other.wall || !other.open)
                        continue;
                    neighbors.Add(other);
                }
                if (neighbors.Count == 0) continue;
                // get random cell
                Cell selected = (Cell)neighbors[random.Next(neighbors.Count)];
                // add as neighbor
                selected.open = false; // indicate cell closed for generation
                cell.AddNeighbor(selected);
                cellsList.Add(cell);
                cellsList.Add(selected);
            }
            UpdateGrid();
        }
        // used to get a Cell at x, y; returns null out of bounds
        public Cell GetCell(int x, int y)
        {
            try
            {
                return cells[x, y];
            }
            catch (IndexOutOfRangeException)
            { // catch out of bounds
                return null;
            }
        }
        // draw the maze
        public void UpdateGrid()
        {
            char backChar = ' ', wallChar = 'X', cellChar = ' ';
            // fill background
            for (int x = 0; x < gridDimensionX; x++)
                for (int y = 0; y < gridDimensionY; y++)
                    mazeGrid[x, y] = backChar;
            // build walls
            for (int x = 0; x < gridDimensionX; x++)
                for (int y = 0; y < gridDimensionY; y++)
                    if (x % 2 == 0 || y % 2 == 0)
                        mazeGrid[x, y] = wallChar;
            // make meaningful representation
            for (int x = 0; x < dimensionX; x++)
                for (int y = 0; y < dimensionY; y++)
                {
                    Cell current = GetCell(x, y);
                    int gridX = x * 2 + 1, gridY = y * 2 + 1;
                    mazeGrid[gridX, gridY] = cellChar;
                    if (current.IsCellBelowNeighbor())
                        mazeGrid[gridX, gridY + 1] = cellChar;
                    if (current.IsCellRightNeighbor())
                        mazeGrid[gridX + 1, gridY] = cellChar;
                }
        }
    } // end nested class MyMaze
}
