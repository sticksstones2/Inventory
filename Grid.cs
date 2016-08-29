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
    upThenLeft,
}

public class Grid : MonoBehaviour
{
    // __________________________________________________________________________________________________________________________________________________________
    public Origin gridOrigin;
    public int numberWide = 10;
    public int numberHigh = 10;

    public LayoutDirection layoutDirection;

    public Origin squareOrigin;
    public float squareWidth = 1f;
    public float squareHeight = 1f;

    public Color color = new Color(255f, 0, 255f, .5f);


    // PositionToRowsCols _______________________________________________________________________________________________________________________________________


    // PositionToRowsCols _______________________________________________________________________________________________________________________________________

    // Return column and row number via a highjacked vector2, taking an XY position vector.
    /* public Vector2 PositionToRowsCols(Vector2 position)
     {
         Vector2 offset = Offset();
         Vector2 flip = Flip();
         return new Vector2(Mathf.Floor(position.x / squareWidth - offset.x / squareWidth) * flip.x,  Mathf.Floor(position.y / squareHeight - offset.y / squareHeight) * flip.y);

         // ?!?!?!?!?!?!
     }*/

    // NearestToPosition ________________________________________________________________________________________________________________________________________
    /*
    public Vector2 NearestToPosition(Vector2 position)
    {
        Vector2 pos = PositionToRowsCols(position);
        return GridSnap((int)pos.x, (int)pos.y);
    }
    */

    // GridSnap _________________________________________________________________________________________________________________________________________________

    // Return a grid-snapped position vector, taking an XY position vector. Origin bottom-left.
    /*public Vector2 GridSnap(Vector2 position)
    {
        Vector2 pos = PositionToRowsCols(position);
        return GridSnap((int)pos.x, (int)pos.y);
    }
    // Return a grid-snapped position vector, taking column and row number. Origin bottom-left.
    public Vector2 GridSnap(int column, int row)
    {
        Vector2 offset = Offset();
        offset.x += column * squareWidth;
        offset.y += row * squareHeight;
        return offset;
    }
    // Return a grid-snapped position vector, taking an XY position vector. Origin centered.
    public Vector2 GridSnap(Vector2 position, bool centered)
    {
        Vector2 gridSnap = GridSnap(position);
        if (centered)
        {
            gridSnap.x += .5f * squareWidth;
            gridSnap.y += .5f * squareWidth;
        }
        return gridSnap;
    }
    // Return a grid-snapped position vector, taking an XY position vector. Origin centered.
    public Vector2 GridSnap(int column, int row, bool centered)
    {
        Vector2 gridSnap = GridSnap(column, row);
        if (centered)
        {
            gridSnap.x += .5f * squareWidth;
            gridSnap.y += .5f * squareWidth;
        }
        return gridSnap;
    }

    // PRIVATE ==================================================================================================================================================

    // PositionScaledToGrid _____________________________________________________________________________________________________________________________________

    // Calculate the grid offset from the preset values
    private Vector2 PositionScaledToGrid(Vector2 position)
    {
        position.x *= squareWidth;
        position.y *= squareHeight;
        return position;
    }

    // Offset ___________________________________________________________________________________________________________________________________________________

    // Calculate the grid offset from the preset values
    private Vector2 Offset()
    {
        Vector2 offset = new Vector2(offsetX, offsetY);
        if (centerX) offset.x -= squareWidth * numberWide * .5f;
        if (centerY) offset.y -= squareHeight * numberHigh * .5f;
        if (!LeftToRight) offset.x -= squareWidth;
        if (TopToBottom) offset.y += squareHeight;
        return offset;
    }

    // Flip ___________________________________________________________________________________________________________________________________________________

    // Calculate the grid offset from the preset values
    private Vector2 Flip()
    {
        Vector2 flip = new Vector2(1,1);
        if (!LeftToRight) flip.x = -1;
        if (TopToBottom) flip.x = -1;
        return flip;
    }

    // OnDrawGizmos _____________________________________________________________________________________________________________________________________________

    void OnDrawGizmos()
	{
        //Vector3 pos = transform.position; //Camera.current.transform.position;
        Gizmos.color = this.color;

        float posX = offsetX + transform.position.x;
        float posY = offsetY + transform.position.y;

        float gridWidth = squareWidth * numberWide;
        float gridHeight = squareHeight * numberHigh;

        if (centerX)
        {
            posX -= gridWidth * .5f;
        }

        if (centerY)
        {
            posY -= gridHeight * .5f;
        }

        for (int rows = 0; rows <= numberHigh; rows++)
        {
            Gizmos.DrawLine(new Vector3(posX, posY + (squareHeight * rows), 0),
                            new Vector3(posX + gridWidth, posY + (squareHeight * rows), 0));
        }

        for (int cols = 0; cols <= numberWide; cols++)
        {
            Gizmos.DrawLine(new Vector3(posX + (squareWidth * cols), posY, 0),
                            new Vector3(posX + (squareWidth * cols), posY + gridHeight, 0));
        }
    }*/

    public Vector3 PositionAtIndex(int index)
    {
        int row = index / numberWide;
        int column = index % numberWide;

        float x = squareWidth * column;
        float y = squareHeight * row;

        Vector3 gridSpacePos = new Vector3(x, y, 0);

        Vector3 worldSpacePos = GridToWorldSpace(gridSpacePos);
        return worldSpacePos;

        //gridSpacePos += transform.position;
        //return gridSpacePos;

    }


    public float GridWidth()
    {
        return squareWidth * numberWide;
    }

    public float GridHeight()
    {
        return squareHeight * numberHigh;
    }

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


    #region internal functions 

    private Vector3 GridToWorldSpace(Vector3 point)
    {

        #region adjust for gridOrigin

        #region vertical adjustment

        // Top origin is standard

        // Bottom origin
        if ((gridOrigin == Origin.bottomLeft) || (gridOrigin == Origin.bottomCenter) || (gridOrigin == Origin.bottomRight))
        {
            point.y = -point.y;
        }

        // Vertically centered origin
        if ((gridOrigin == Origin.center) || (gridOrigin == Origin.centerLeft) || (gridOrigin == Origin.centerRight))
        {
            point.y = -point.y - GridHeight() * .5f;
        }

        #endregion

        #region horizontal adjustment

        // Left origin is standard

        // Right origin
        if ((gridOrigin == Origin.topRight) || (gridOrigin == Origin.centerRight) || (gridOrigin == Origin.bottomRight))
        {
            point.x = -point.x;
        }

        // Horizontally centered origin
        if ((gridOrigin == Origin.center) || (gridOrigin == Origin.topCenter) || (gridOrigin == Origin.bottomCenter))
        {
            point.x = point.x - GridWidth() * .5f;
        }

        #endregion

        #endregion

        // finally, add in the transform.position
        return point + transform.position;
    }

    private Vector3 WorldToGridSpace(Vector3 point)
    {

        #region adjust for gridOrigin

        #region vertical adjustment

        // Top origin is standard

        // Bottom origin
        if ((gridOrigin == Origin.bottomLeft) || (gridOrigin == Origin.bottomCenter) || (gridOrigin == Origin.bottomRight))
        {
            point.y = +point.y;
        }

        // Vertically centered origin
        if ((gridOrigin == Origin.center) || (gridOrigin == Origin.centerLeft) || (gridOrigin == Origin.centerRight))
        {
            point.y = -point.y - GridHeight() * .5f;
        }

        #endregion

        #region horizontal adjustment

        // Left origin is standard

        // Right origin
        if ((gridOrigin == Origin.topRight) || (gridOrigin == Origin.centerRight) || (gridOrigin == Origin.bottomRight))
        {
            point.x = -point.x;
        }

        // Horizontally centered origin
        if ((gridOrigin == Origin.center) || (gridOrigin == Origin.topCenter) || (gridOrigin == Origin.bottomCenter))
        {
            point.x = point.x - GridWidth() * .5f;
        }

        #endregion

        #endregion

        // finally, add in the transform.position
        return point + transform.position;
    }

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


    #endregion

    #region OnDrawGizmos

    void OnDrawGizmos()
    {
        Gizmos.color = this.color;

        Vector3 start;
        Vector3 end;

        for (int rows = 0; rows <= numberHigh; rows++)
        {
            // start and end points
            start = GridToWorldSpace(LocalRowStart(rows));
            end = GridToWorldSpace(LocalRowEnd(rows));

            Gizmos.DrawLine(start, end);
        }

        for (int cols = 0; cols <= numberWide; cols++)
        {
            // find the unmodded start and end points
            start = LocalColumnStart(cols);
            end = LocalColumnEnd(cols);

            /*
            #region adjust for gridOrigin

            #region vertical adjustment

            // Top origin is standard

            // Bottom origin
            if ((gridOrigin == Origin.bottomLeft) || (gridOrigin == Origin.bottomCenter) || (gridOrigin == Origin.bottomRight))
            {
                start.y = -start.y;
                end.y = -end.y;
            }

            // Vertically centered origin
            if ((gridOrigin == Origin.center) || (gridOrigin == Origin.centerLeft) || (gridOrigin == Origin.centerRight))
            {
                start.y = -start.y - GridHeight() * .5f;
                end.y = -end.y - GridHeight() * .5f;
            }

            #endregion

            #region horizontal adjustment

            // Left origin is standard

            // Right origin
            if ((gridOrigin == Origin.topRight) || (gridOrigin == Origin.centerRight) || (gridOrigin == Origin.bottomRight))
            {
                start.x = -start.x;
                end.x = -end.x;
            }

            // Horizontally centered origin
            if ((gridOrigin == Origin.center) || (gridOrigin == Origin.topCenter) || (gridOrigin == Origin.bottomCenter))
            {
                start.x = start.x - GridWidth() * .5f;
                end.x = end.x - GridWidth() * .5f;
            }

            #endregion

            #endregion

            // finally, add in the transform.position
            start += transform.position;
            end += transform.position;

            Gizmos.DrawLine(start, end);
        }
    }

    #endregion

}