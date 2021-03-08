using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntAndKillMazeAlgorithm
{
    MazeCell[,] mazeCells;
    int mazeRows, mazeColumns;

    private int currentRow = 0;
    private int currentColumn = 0;

    private bool courseComplete = false;

    public HuntAndKillMazeAlgorithm(MazeCell[,] mazeCells)
    {
        this.mazeCells = mazeCells;
        mazeRows = mazeCells.GetLength(0);
        mazeColumns = mazeCells.GetLength(1);
    }

    public void HuntAndKill()
    {
        mazeCells [currentRow, currentColumn].visited = true;

        while (courseComplete == false)
        {
            Kill(); // Will run until it hits a dead end.
            Hunt(); // Finds the next unvisited cell with an adjacent visited cell. If it can't fin any, it sets courseComplete to true.
        }
    }

    private void Kill()
    {
        while (RouteStillAvailable(currentRow, currentColumn))
        {
            int direction = Random.Range(1, 5);

            if (direction == 1 && CellIsAvailable(currentRow - 1, currentColumn))
            {
                // North
                DestroyWallIfItExist(mazeCells[currentRow, currentColumn].northWall);
                DestroyWallIfItExist(mazeCells[currentRow - 1, currentColumn].southWall);
                currentRow--;
            }
            else if (direction == 2 && CellIsAvailable(currentRow + 1, currentColumn))
            {
                // South
                DestroyWallIfItExist(mazeCells[currentRow, currentColumn].southWall);
                DestroyWallIfItExist(mazeCells[currentRow + 1, currentColumn].northWall);
                currentRow++;
            }
            else if (direction == 3 && CellIsAvailable(currentRow, currentColumn + 1))
            {
                // East
                DestroyWallIfItExist(mazeCells[currentRow, currentColumn].eastWall);
                DestroyWallIfItExist(mazeCells[currentRow, currentColumn + 1].westWall);
                currentColumn++;
            }
            else if (direction == 4 && CellIsAvailable(currentRow, currentColumn - 1))
            {
                // West
                DestroyWallIfItExist(mazeCells[currentRow, currentColumn].westWall);
                DestroyWallIfItExist(mazeCells[currentRow, currentColumn - 1].eastWall);
                currentColumn--;
            }
            mazeCells [currentRow, currentColumn].visited = true;
        }
    }

    private void Hunt()
    {
        courseComplete = true; // Set it to true, and see if we can prove otherwise below!
        for (int r = 0; r < mazeRows; r++)
        {
            for (int c = 0; c < mazeColumns; c++)
            {
                if (!mazeCells[r,c].visited && CellHasAnAdjacentVisitedCell(r,c))
                {
                    courseComplete = false; // Yep, we found something so definitely do another Kill cycle.
                    currentRow = r;
                    currentColumn = c;
                    DestroyAdjacentWall(currentRow, currentColumn);
                    mazeCells[currentRow, currentColumn].visited = true;
                    return; // Exit the function
                }
            }
        }
    }

    private bool RouteStillAvailable(int row, int column)
    {
        int availableRoutes = 0;

        // North
        if (row > 0 && !mazeCells[row - 1, column].visited)
        {
            availableRoutes++;
        }
        // South
        if (row < mazeRows - 1 && !mazeCells[row + 1, column].visited)
        {
            availableRoutes++;
        }
        // West
        if (column > 0 && !mazeCells[row, column - 1].visited)
        {
            availableRoutes++;
        }
        // East
        if (column < mazeColumns - 1 && !mazeCells[row, column + 1].visited)
        {
            availableRoutes++;
        }
        return availableRoutes > 0;
    }

    private bool CellIsAvailable(int row, int column)
    {
        if (row >= 0 && row < mazeRows && column >= 0 && column < mazeColumns && !mazeCells[row, column].visited)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DestroyWallIfItExist(GameObject wall)
    {
        if (wall != null)
        {
            GameObject.Destroy(wall);
        }
    }

    private bool CellHasAnAdjacentVisitedCell(int row, int column)
    {
        int visitedCells = 0;

        // North
        if (row > 0 && mazeCells[row - 1, column].visited)
        {
            visitedCells++;
        }
        // South
        if (row < mazeRows - 2 && mazeCells[row + 1, column].visited)
        {
            visitedCells++;
        }
        // West
        if (column > 0 && mazeCells[row, column - 1].visited)
        {
            visitedCells++;
        }
        // East
        if (column < mazeColumns - 2 && mazeCells[row, column + 1].visited)
        {
            visitedCells++;
        }
        return visitedCells > 0;
    }

    private void DestroyAdjacentWall(int row, int column)
    {
        bool wallDestroyed = false;

        while (wallDestroyed == false)
        {
            int direction = Random.Range(1, 5);

            // North
            if (direction == 1 && row > 0 && mazeCells[row - 1, column].visited)
            {
                DestroyWallIfItExist(mazeCells[row, column].northWall);
                DestroyWallIfItExist(mazeCells[row - 1, column].southWall);
                wallDestroyed = true;
            }
            // South
            else if (direction == 2 && row < mazeRows - 2 && mazeCells[row + 1, column].visited)
            {
                DestroyWallIfItExist(mazeCells[row, column].southWall);
                DestroyWallIfItExist(mazeCells[row + 1, column].northWall);
                wallDestroyed = true;
            }
            // West
            else if (direction == 3 && column > 0 && mazeCells[row, column - 1].visited)
            {
                DestroyWallIfItExist(mazeCells[row, column].westWall);
                DestroyWallIfItExist(mazeCells[row, column - 1].eastWall);
                wallDestroyed = true;
            }
            // East
            else if (direction == 4 && column < mazeColumns - 2 && mazeCells[row, column + 1].visited)
            {
                DestroyWallIfItExist(mazeCells[row, column].eastWall);
                DestroyWallIfItExist(mazeCells[row, column + 1].westWall);
                wallDestroyed = true;
            }
        }
    }
}