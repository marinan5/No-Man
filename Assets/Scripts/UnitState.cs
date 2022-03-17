using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SelectionState
{
	Selected,
	Default
}

public enum UnitType
{
	Player,
	Enemy
}

public class UnitState : MonoBehaviour, IPointerClickHandler
{
	public SelectionState selectionState;
	public int index;
	public BoardManager manager;
	public Vector2 position;
	public UnitType type;

	private void Awake()
	{
		selectionState = SelectionState.Default;
	}

	public void InitializeUnit(int index, Vector2 position, int type, BoardManager manager)
	{
		this.index = index;
		this.manager = manager;
		this.position = position;
		this.type = type == 0 ? UnitType.Player : UnitType.Enemy;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (selectionState == SelectionState.Default && BattleSystem.Instance.state == BattleState.PLAYERTURN && type == UnitType.Player)
		{
			selectionState = SelectionState.Selected;
			manager.SelectUnit(index);
		}
		else
		{
			selectionState = SelectionState.Default;
			manager.ClearSelection();
		}
	}

	public void UpdatePosition(Vector2 newPosition)
	{
		position = newPosition;
		transform.position = newPosition;
	}

	public void Select()
	{
		selectionState = SelectionState.Selected;
		manager.SelectUnit(index);
	}

	public void Deselect()
	{
		selectionState = SelectionState.Default;
		manager.ClearSelection();
	}

}
