using UnityEngine;
using System.Collections;

public class GridF: MonoBehaviour
{
    public int horizontalCells = 5;
    public int verticalCells = 5;


    private void OnDrawGizmos()
    {
        Vector3 instancePosition = gameObject.transform.position;
        Vector3 topRightPositionCorner = new(
            x: (instancePosition.x > 0 ? instancePosition.x + gameObject.transform.lossyScale.x / 2:
            instancePosition.x - gameObject.transform.lossyScale.x / 2) * 10,

            z: (instancePosition.z >= 0? instancePosition.z + gameObject.transform.lossyScale.z/2 :
            instancePosition.z - gameObject.transform.lossyScale.z / 2) * 10,

            y: gameObject.transform.position.y);

        Vector3 sizeGrid = new(
            x: gameObject.transform.lossyScale.x/horizontalCells * 10,
            y: 0.5f,
            z: gameObject.transform.lossyScale.z/verticalCells * 10
            );;

        for(int horizontalIndex = 0; horizontalIndex < horizontalCells; horizontalIndex ++)
        {
            for(int verticalIndex = 0; verticalIndex < verticalCells; verticalIndex ++)
            {
                if((horizontalIndex + verticalIndex) % 2 == 0) Gizmos.color = new Color(r: 84, g: 130, b: 130);
                else Gizmos.color = new Color(r: 0, g: 130, b: 130);

                Vector3 positionGridItem = new(
                    x: (topRightPositionCorner.x + sizeGrid.x / 2) + horizontalIndex * sizeGrid.x,
                    y: topRightPositionCorner.y,
                    z: (topRightPositionCorner.z - sizeGrid.z / 2) - verticalIndex * sizeGrid.z);

                Gizmos.DrawCube(positionGridItem, sizeGrid);

            }
        }
        
    }

}

