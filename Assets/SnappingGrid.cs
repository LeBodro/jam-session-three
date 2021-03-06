﻿using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SnappingGridData
{
    [SerializeField] public ModuleData[] modules;
    public SnappingGridData(ModuleData[] _modules) => modules = _modules;
}

[RequireComponent(typeof(Grid))]
public class SnappingGrid : MonoBehaviour
{
    public enum PowerState
    {
        OFF,
        ON,
    }

    [SerializeField] BoxCollider2D dropCollider;
    [SerializeField] Grid grid;
    [SerializeField] protected PowerState connectionState = PowerState.OFF;
    [SerializeField] protected Vector2Int gridSize;
    [SerializeField] protected ModuleFactory modules;

    protected Module[] cells;

    // Called from the editor when adding or resetting the component
    void Reset()
    {
        grid = GetComponent<Grid>();
        dropCollider = GetComponent<BoxCollider2D>();
    }

    void Awake()
    {
        cells = new Module[gridSize.x * gridSize.y];
    }

    protected void Clear()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i]?.RePool();
            cells[i] = null;
        }
    }

    public bool Contains(Vector3 droppedPosition)
    {
        return dropCollider.OverlapPoint(droppedPosition);
    }

    public bool TrySnap(Module target)
    {
        Vector3 position = target.transform.position;
        Vector3Int cellCoordinates = grid.WorldToCell(position);
        Vector3 cellCenter = GetCellCenter(cellCoordinates);
        int cellIndex = ToIndex(cellCoordinates);
        if (cells[cellIndex] != null)
        {
            target.CancelMovement();
            return false;
        }

        target.transform.position = new Vector3(cellCenter.x, cellCenter.y);
        cells[cellIndex] = target;
        target.OnRemoved += Unsnap;
        ProcessConnection(target, cellCoordinates);
        OnSnap(target, cellCoordinates);

        return true;
    }

    protected virtual void OnSnap(Module snapped, Vector3Int cellCoordinates) { }

    int ToIndex(Vector3Int coordinates) => ToIndex(coordinates.x, coordinates.y);
    protected int ToIndex(int x, int y) => x + y * gridSize.x;
    protected Vector3Int FromIndex(int index) => new Vector3Int(index % gridSize.x, index / gridSize.x, 0);

    void Unsnap(Module unsnapped)
    {
        int index = 0;
        for (int i = 0; i < cells.Length; i++)
            if (cells[i] == unsnapped)
            {
                index = i;
                break;
            }
        cells[index] = null;
        OnUnsnap(unsnapped, index);
        unsnapped.OnRemoved -= Unsnap;
    }

    protected virtual void OnUnsnap(Module unsnapped, int index) { }

    public Module GetModuleAt(int x, int y)
    {
        Vector3 cellCenter = GetCellCenter(x, y);
        Collider2D candidate = Physics2D.OverlapPoint(cellCenter, LayerMask.GetMask("Draggable"));
        return candidate == null ? null : candidate.GetComponent<Module>();
    }

    public Vector3 GetCellCenter(int x, int y)
    {
        return GetCellCenter(new Vector3Int(x, y, 0));
    }

    public Vector3 GetCellCenter(Vector3Int cellCoordinates)
    {
        return grid.CellToWorld(cellCoordinates) + grid.cellSize * 0.5f;
    }

    void ProcessConnection(Module target, Vector3Int cellCoordinates)
    {
        target.ConfirmMovement();
        if (connectionState == PowerState.OFF)
            target.PowerOff();
        else
            target.PowerOn();
    }

    public SnappingGridData Serialize()
    {
        List<ModuleData> modules = new List<ModuleData>();
        for (int i = 0; i < cells.Length; i++)
            if (cells[i] != null)
                modules.Add(cells[i].Serialize(i));
        return new SnappingGridData(modules.ToArray());
    }

    public virtual void Deserialize(SnappingGridData data)
    {
        foreach (var mData in data.modules)
        {
            Module module = modules.Get(mData.prefab);
            module.Deserialize(mData);
            module.transform.position = GetCellCenter(mData.index % gridSize.x, mData.index / gridSize.x);
            TrySnap(module);
        }
    }

    public void OnDrawGizmos()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (cells != null && cells.Length > 0)
                    Gizmos.color = cells[ToIndex(x, y)] == null ? Color.green : Color.red;
                Gizmos.DrawSphere(GetCellCenter(x, y), 0.25f);
            }
        }
    }
}
