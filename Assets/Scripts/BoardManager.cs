using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	public GameObject TilePrefab;
	public Camera MainCamera;
	public GameObject UnitPrefab;
	public int BoardSize;
	public List<GameObject> PlayerUnits = new List<GameObject>();
	public List<GameObject> EnemyUnits = new List<GameObject>();
	public int? selection;
	private GameObject[,] _tiles;

	public void SetupBoard()
	{
		_tiles = new GameObject[BoardSize, BoardSize];
		for (int i = 0; i < BoardSize; i++)
		{
			for (int j = 0; j < BoardSize; j++)
			{
				GameObject tile = Instantiate(TilePrefab, new Vector3(i, j, 0), Quaternion.identity);
				tile.transform.parent = transform;
				tile.GetComponent<TileState>().initializeTile(i, j, this);
				// Debug.Log(_tiles);
				_tiles[i, j] = tile;
			}
		}
		MainCamera.transform.position = new Vector3(BoardSize / 2 - 0.5f, BoardSize / 2 - 0.5f, -10);

		// Spawning player
		SpawnUnit(0, new Vector2(0, 0));
		SpawnUnit(1, new Vector2(BoardSize - 1, BoardSize - 1));
	}

	void SpawnUnit(int type, Vector2 position)
	{
		GameObject unit = Instantiate(UnitPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
		int index = type == 0 ? PlayerUnits.Count : EnemyUnits.Count;
		unit.GetComponent<UnitState>().InitializeUnit(index, position, type, this);
		_tiles[(int)position.x, (int)position.y].GetComponent<TileState>().SetState(TileStates.Filled);
		if (type == 0)
		{
			PlayerUnits.Add(unit);
		}
		else
		{
			EnemyUnits.Add(unit);
		}
	}

	public void onPlayerClickTile(int x, int y)
	{
		UpdatePlayerPosition(x, y);
		PlayerUnits[(int)selection].GetComponent<UnitState>().Deselect();
		BattleSystem.Instance.PlayerTurnEnded();
	}

	void UpdatePlayerPosition(int x, int y)
	{
		if (selection != null)
		{
			MoveUnit(x, y, 0, (int)selection);
		}
	}

	public bool IsValidTile(int x, int y)
	{
		return x >= 0 && x < BoardSize && y >= 0 && y < BoardSize && _tiles[x, y].GetComponent<TileState>().state == TileStates.Empty;
	}

	public void MoveUnit(int x, int y, int type, int index)
	{
		if (type == 0)
		{
			Vector2 pos = PlayerUnits[index].GetComponent<UnitState>().position;
			_tiles[(int)pos.x, (int)pos.y].GetComponent<TileState>().SetState(TileStates.Empty);
			PlayerUnits[index].GetComponent<UnitState>().UpdatePosition(new Vector2(x, y));
			_tiles[x, y].GetComponent<TileState>().SetState(TileStates.Filled);
		}
		else if (type == 1)
		{
			Vector2 pos = EnemyUnits[index].GetComponent<UnitState>().position;
			_tiles[(int)pos.x, (int)pos.y].GetComponent<TileState>().SetState(TileStates.Empty);
			EnemyUnits[index].GetComponent<UnitState>().UpdatePosition(new Vector2(x, y));
			_tiles[x, y].GetComponent<TileState>().SetState(TileStates.Filled);
		}
	}

	public void SelectUnit(int index)
	{
		selection = index;
	}

	public void ClearSelection()
	{
		selection = null;
	}
}
