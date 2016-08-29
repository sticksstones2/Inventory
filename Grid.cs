using UnityEngine;
using System.Collections;

public enum Origin
{
    center,
    topLeft,
    topCenter,
    topRight,
    centerRight,
    bottomRight,
    bottomCenter,
    bottomLeft,
    centerLeft
}

public enum LayoutDirection
{
    rightThenDown,
    downThenRight,
    rightThenUp,
    upThenRight,
    leftThenDown,
    downThenLeft,
    leftThenUp,
    upThenLeft
}

public class Grid : MonoBehaviour
{
    public Origin gridOrigin;
    public int numberWide = 10;
    public int numberHigh = 10;

    public LayoutDirection layoutDirection;

    public Origin squareOrigin;
    public float squareWidth = 1f;
    public float squareHeight = 1f;

    public Color color = new Color(255f, 0, 255f, .5f);

    public float GridWidth()
    {
        return squareWidth * numberWide;
    }

    public float GridHeight()
    {
        return squareHeight * numberHigh;
    }

    #region Find the Vector3 position for a gridSquare
    
    // All functions that return a gridSquare position should route though this
    public Vector3 PositionAtIndex(int index)
    {
        int column = 0;
        int row = 0;

        switch (layoutDirection)
        {
            case LayoutDirection.rightThenDown:
                column = index % numberWide;
                row = -index / numberWide;
                break;
            case LayoutDirection.downThenRight:
                column = index / numberHigh;
                row = -index % numberHigh;
                break;
            case LayoutDirection.rightThenUp:
                column = index % numberWide;
                row = index / numberWide - numberHigh + 1;
                break;
            case LayoutDirection.upThenRight:
                column = index / numberHigh;
                row = index % numberHigh - numberHigh + 1;
                break;
            case LayoutDirection.leftThenDown:
                column = -index % numberWide + numberWide - 1;
                row = -index / numberWide;
                break;
            case LayoutDirection.downThenLeft:
                column = -index / numberHigh + numberWide - 1;
                row = -index % numberHigh;
                break;
            case LayoutDirection.leftThenUp:
                column = -index % numberWide + numberWide - 1;
                row = index / numberWide - numberHigh + 1;
                break;
            case LayoutDirection.upThenLeft:
                column = -index / numberHigh + numberWide - 1;
                row = index % numberHigh - numberHigh + 1;
                break;
        }

        float x = squareWidth * column;
        float y = squareHeight * row;

        Vector3 gridSpacePos = new Vector3(x, y, 0);
        gridSpacePos += GridOffset();
        gridSpacePos += SquareOffset();
        gridSpacePos += transform.position;

        return gridSpacePos;
    }

    public Vector3? PositionAtColsRows(int column, int row)
    {
        if ((column > numberWide) || (column < 0))
        {
            return null;
        }
        if ((row > numberHigh) || (row < 0))
        {
            return null;
        }
        return PositionAtIndex(column + row * numberWide);
    }

    #endregion

    #region Find the Vector3 position for start and end points, for rows and columns

    // the following doesn't use layout direction. Probably should
    public Vector3 RowStart(int row)
    {
        return LocalRowStart(row) + transform.position;
    }

    public Vector3 RowEnd(int row)
    {
        return LocalRowEnd(row) + transform.position;
    }

    public Vector3 ColumnStart(int column)
    {
        return LocalColumnStart(column) + transform.position;
    }

    public Vector3 ColumnEnd(int column)
    {
        return LocalColumnEnd(column) + transform.position;
    }

    #endregion

    #region Internal

    private Vector3 LocalRowStart(int row)
    {
        return new Vector3(0, -squareHeight * row, 0);
    }

    private Vector3 LocalRowEnd(int row)
    {
        return LocalRowStart(row) + new Vector3(GridWidth(),0,0);
    }

    private Vector3 LocalColumnStart(int column)
    {
        return new Vector3(squareWidth * column, 0, 0);
    }

    private Vector3 LocalColumnEnd(int column)
    {
        return LocalColumnStart(column) + new Vector3(0, -GridHeight(), 0);
    }

    private Vector3 GridOffset()
    {
        Vector3 gridOffset = new Vector3();

        // Left origin is standard

        // Right origin
        if ((gridOrigin == Origin.topRight) || (gridOrigin == Origin.centerRight) || (gridOrigin == Origin.bottomRight))
        {
            gridOffset.x = -GridWidth();
        }

        // Horizontally centered origin
        if ((gridOrigin == Origin.center) || (gridOrigin == Origin.topCenter) || (gridOrigin == Origin.bottomCenter))
        {
            gridOffset.x = -GridWidth() * .5f;
        }

        // Top origin is standard

        // Bottom origin
        if ((gridOrigin == Origin.bottomLeft) || (gridOrigin == Origin.bottomCenter) || (gridOrigin == Origin.bottomRight))
        {
            gridOffset.y = GridHeight();
        }

        // Vertically centered origin
        if ((gridOrigin == Origin.center) || (gridOrigin == Origin.centerLeft) || (gridOrigin == Origin.centerRight))
        {
            gridOffset.y = GridHeight() * .5f;
        }

        return gridOffset;
    }

    private Vector3 SquareOffset()
    {
        Vector3 squareOffset = new Vector3();

        // Left origin is standard

        // Right origin
        if ((squareOrigin == Origin.topRight) || (squareOrigin == Origin.centerRight) || (squareOrigin == Origin.bottomRight))
        {
            squareOffset.x = squareWidth;
        }

        // Horizontally centered origin
        if ((squareOrigin == Origin.center) || (squareOrigin == Origin.topCenter) || (squareOrigin == Origin.bottomCenter))
        {
            squareOffset.x = squareWidth * .5f;
        }

        // Top origin is standard

        // Bottom origin
        if ((squareOrigin == Origin.bottomLeft) || (squareOrigin == Origin.bottomCenter) || (squareOrigin == Origin.bottomRight))
        {
            squareOffset.y = -squareHeight;
        }

        // Vertically centered origin
        if ((squareOrigin == Origin.center) || (squareOrigin == Origin.centerLeft) || (squareOrigin == Origin.centerRight))
        {
            squareOffset.y = -squareHeight * .5f;
        }

        return squareOffset;
    }

    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = this.color;

        Vector3 start;
        Vector3 end;
        Vector3 gridOffset = GridOffset();

        for (int rows = 0; rows <= numberHigh; rows++)
        {
            // start and end points
            start = LocalRowStart(rows);
            end = LocalRowEnd(rows); 

            start += gridOffset;
            end += gridOffset;

            // finally, add in the transform.position
            start += transform.position;
            end += transform.position;

            Gizmos.DrawLine(start, end);
        }

        for (int cols = 0; cols <= numberWide; cols++)
        {
            // find the unmodded start and end points
            start = LocalColumnStart(cols);
            end = LocalColumnEnd(cols);

            start += gridOffset;
            end += gridOffset;

            // finally, add in the transform.position
            start += transform.position;
            end += transform.position;

            Gizmos.DrawLine(start, end);
        }
    }

}