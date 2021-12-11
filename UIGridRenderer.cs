using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UIGridRenderer : MaskableGraphic
{
    [Space]
    [SerializeField] public Vector2Int cellCount;
    [SerializeField] private Vector2 lineThickness;
    [SerializeField] private bool dynamicThickness;

    private Vector2 rectSize = new Vector2();
    private Vector2 cellSize = new Vector2();
    private Vector2 offsetPos = new Vector2();

    private Vector2 lineSize;

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        SetAllDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vertHelper)
    {
        vertHelper.Clear();

        rectSize = rectTransform.rect.size;
        cellSize = rectSize / (Vector2)cellCount;
        offsetPos = rectSize * rectTransform.pivot;

        Vector2 thickness = lineThickness;
        if (dynamicThickness)
            thickness = thickness / transform.lossyScale;

        Vector2 sizeSqr = (thickness * thickness) * 0.5f;
        lineSize = new Vector2(Mathf.Sqrt(sizeSqr.x), Mathf.Sqrt(sizeSqr.y));

        int indexCount = 0;

        if (cellCount.x > 0)
            for (float x = 0; x <= cellCount.x; x++)
            {
                int edge = 1;

                if (x == 0)
                    edge = 0;

                if (x == cellCount.x)
                    edge = 2;

                DrawLineVertically(x, indexCount, edge, vertHelper);
                indexCount++;
            }

        if (cellCount.y > 0)
            for (float y = 0; y <= cellCount.y; y++)
            {
                int edge = 1;

                if (y == 0)
                    edge = 0;

                if (y == cellCount.y)
                    edge = 2;

                DrawLineHorizontally(y, indexCount, edge, vertHelper);
                indexCount++;
            }
    }

    private void DrawLineVertically(float x, int index, int edge, VertexHelper vertHelper)
    {
        float xOffset;
        if (edge == 0)
            xOffset = 0f;
        else
            xOffset = lineSize.x;

        float size;
        if (edge == 1)
            size = lineSize.x * 2;
        else
            size = lineSize.x;

        Vector2 cellPos = new Vector2((cellSize.x * x) - offsetPos.x - xOffset, -offsetPos.y);

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = new Vector3(cellPos.x, cellPos.y);
        vertHelper.AddVert(vertex);

        vertex.position = new Vector3(cellPos.x, cellPos.y + rectSize.y);
        vertHelper.AddVert(vertex);

        vertex.position = new Vector3(cellPos.x + size, cellPos.y + rectSize.y);
        vertHelper.AddVert(vertex);

        vertex.position = new Vector3(cellPos.x + size, cellPos.y);
        vertHelper.AddVert(vertex);

        int offset = index * 4;

        vertHelper.AddTriangle(offset + 0, offset + 1, offset + 2);
        vertHelper.AddTriangle(offset + 2, offset + 3, offset + 0);
    }

    private void DrawLineHorizontally(float y, int index, int edge, VertexHelper vertHelper)
    {
        float yOffset;
        if (edge == 0)
            yOffset = 0f;
        else
            yOffset = lineSize.y;

        float size;
        if (edge == 1)
            size = lineSize.y * 2;
        else
            size = lineSize.y;

        Vector2 cellPos = new Vector2(-offsetPos.x, (cellSize.y * y) - offsetPos.y - yOffset);

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = new Vector3(cellPos.x, cellPos.y);
        vertHelper.AddVert(vertex);

        vertex.position = new Vector3(cellPos.x + rectSize.x, cellPos.y);
        vertHelper.AddVert(vertex);

        vertex.position = new Vector3(cellPos.x + rectSize.x, cellPos.y + size);
        vertHelper.AddVert(vertex);

        vertex.position = new Vector3(cellPos.x, cellPos.y + size);
        vertHelper.AddVert(vertex);

        int offset = index * 4;

        vertHelper.AddTriangle(offset + 0, offset + 1, offset + 2);
        vertHelper.AddTriangle(offset + 2, offset + 3, offset + 0);
    }
}
