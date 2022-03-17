// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
	private static BattleSystem instance;
	public BattleState state;
	public Text dialogueText;
	public GameObject boardObject;
	BoardManager boardManager;

	public static BattleSystem Instance
	{
		get
		{
			if (instance == null)
			{
				// _instance = GameObject.FindObjectOfType<GameManager> ();
				Debug.LogError("GameManager is null");
			}
			return instance;
		}
	}

	private void Awake()
	{
		instance = this; boardManager = boardObject.GetComponent<BoardManager>();
		Setup();
	}

	void Setup()
	{
		boardManager.SetupBoard();
	}

	void Start()
	{
		state = BattleState.START;
		StartCoroutine(BattleStart());
	}

	IEnumerator BattleStart()
	{
		dialogueText.text = "A wild appeared!";
		yield return new WaitForSeconds(2f);
		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	public void OnSkipButton()
	{
		if (state == BattleState.PLAYERTURN)
		{
			dialogueText.text = "Enemy Turn";
			state = BattleState.ENEMYTURN;
			StartCoroutine(EnemyTurn());
		}
	}

	IEnumerator EnemyTurn()
	{
		dialogueText.text = "Enemy Turn";
		yield return new WaitForSeconds(2f);
		boardManager.MoveUnit(Random.Range(0, boardManager.BoardSize - 1),
		 Random.Range(0, boardManager.BoardSize - 1),
		 1,
		 Random.Range(0, boardManager.EnemyUnits.Count - 1));
		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	void PlayerTurn()
	{
		dialogueText.text = "Player Turn";
	}

	public void PlayerTurnEnded()
	{
		state = BattleState.ENEMYTURN;
		StartCoroutine(EnemyTurn());
	}
}
