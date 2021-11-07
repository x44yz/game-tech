using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TicTacToe
{
	public class ChessType
	{
		public const int None = -1;
		public const int O = 0;
		public const int X = 1;
	}

	public enum GameStatus
	{
		START,
		RUNNING,
		END,
	}

	public enum GameResult
	{
		WIN,
		LOSE,
		TIE,
	}

	public class TestMinMax : MonoBehaviour
	{
		public const int BOARD_CELLS = 9;
		public const int BOARD_WIDTH = 3;
		public const int PLAYER_NUM = 2;
		public const int REALPLAYER_INDEX = 0;
		public const int AI_THINK_DEPTH = 9;

		public const int REALPLAYER_CHESS_TYPE = ChessType.O;
		public const int AI_CHESS_TYPE = ChessType.X;

		private System.Random rnd = new System.Random();

		public Transform chessParent;
		public Transform[] boardCells;

		public GameObject objO = null;
		public GameObject objX = null;

		public GameObject[] playerHeads = new GameObject[PLAYER_NUM];
		public GameObject panelResult = null;
		public GameObject txtWin = null;
		public GameObject txtLose = null;
		public GameObject txtTie = null;

		private int[,] chesss = new int[BOARD_WIDTH, BOARD_WIDTH];

		private int playerIdx = -1;
		private bool playerThinking = false;

		private GameStatus status = GameStatus.START;

		private MinMax ai = null;

		private void Start()
		{
			for (int i = 0; i < BOARD_WIDTH; ++i)
			{
				for (int j = 0; j < BOARD_WIDTH; ++j)
					chesss[i,j] = ChessType.None;
			}

			ai = new MinMax(FuncGameOver, FuncEvaluate, FuncMoves, FuncBoardgen);

			panelResult.SetActive(false);

			status = GameStatus.RUNNING;
		}

		private void Update()
		{
			if (status != GameStatus.RUNNING)
				return;

			if (playerThinking)
			{
				
			}
			else
			{
				if (playerIdx >= 0 && playerIdx < PLAYER_NUM)
				{
					var curp = playerHeads[playerIdx].transform.localPosition;
					playerHeads[playerIdx].transform.localPosition = new Vector3(curp.x, -54f, curp.z);
				}

				playerIdx += 1;
				playerIdx = playerIdx % PLAYER_NUM;
				Debug.Assert(playerIdx >= 0 && playerIdx < PLAYER_NUM, "CHECK");

				var p = playerHeads[playerIdx].transform.localPosition;
				playerHeads[playerIdx].transform.localPosition = new Vector3(p.x, 0, p.z);

				StartCoroutine(PlayerEnterTurn());
			}
		}

		public void OnClickButtonPlay()
		{
			panelResult.SetActive(false);

			playerIdx = -1;
			playerThinking = false;
			status = GameStatus.START;

			for (int i = chessParent.childCount - 1; i >= 0; --i)
			{
				var child = chessParent.GetChild(i);
				child.SetParent(null, false);
				Destroy(child.gameObject);
			}

			for (int i = 0; i < BOARD_WIDTH; ++i)
			{
				for (int j = 0; j < BOARD_WIDTH; ++j)
					chesss[i,j] = ChessType.None;
			}
			status = GameStatus.RUNNING;
		}

		public void OnClickBoardCell(int cidx)
		{
			// Debug.Log("xx-- OnClickBoardCell > " + cidx);
			Debug.Assert(cidx >= 0 && cidx < BOARD_CELLS, "CHECK");

			if (playerIdx != REALPLAYER_INDEX)
			{
				Debug.Log("xx-- now it's not your turn.");
				return;
			}

			int cx = cidx / BOARD_WIDTH;
			int cy = cidx % BOARD_WIDTH;
			Debug.Log("click on cell > (" + cx + "," + cy + ")");

			Debug.Assert(cx >= 0 && cx < BOARD_WIDTH, "CHECK");
			Debug.Assert(cy >= 0 && cy < BOARD_WIDTH, "CHECK");

			if (chesss[cx, cy] != ChessType.None)
			{
				Debug.Log("xx-- cell is not empty.");
				return;
			}

			Debug.Assert(REALPLAYER_INDEX == ChessType.O, "CHECK");
			GameObject objChess = Instantiate(objO);

			objChess.transform.SetParent(chessParent, false);
			objChess.transform.position = boardCells[cidx].position;
			chesss[cx, cy] = ChessType.O;

			PlayerFinishTurn();
		}

		public void PlayerFinishTurn()
		{
			playerThinking = false;

			// check end
			int winner = GetWinner(chesss);
			Debug.Log("winner is > " + winner);
			if (winner == REALPLAYER_CHESS_TYPE)
				EndGame(GameResult.WIN);
			else if (winner == AI_CHESS_TYPE)
				EndGame(GameResult.LOSE);

			// check ChessType.None
			bool existBlankCell = false;
			for (int i = 0; i < BOARD_WIDTH; ++i)
			{
				for (int j = 0; j < BOARD_WIDTH; ++j)
				{
					if (chesss[i, j] == ChessType.None)
					{
						existBlankCell = true;
						break;
					}
				}
				
				if (existBlankCell)
					break;
			}

			if (!existBlankCell)
			{
				EndGame(GameResult.TIE);
			}
		}

		public IEnumerator PlayerEnterTurn()
		{
			playerThinking = true;
		
			// AI
			if (playerIdx != REALPLAYER_INDEX)
			{
				// Simulate think
				yield return new WaitForSeconds(0.5f);

				Debug.Assert(ai != null, "CHECK");
				var move = ai.Think(chesss, AI_THINK_DEPTH, AI_CHESS_TYPE, REALPLAYER_CHESS_TYPE);
				
				Debug.Assert(move.Length == 2, "CHECK");
				Debug.Assert(move[0] >= 0 && move[0] < BOARD_WIDTH, "CHECK");
				Debug.Assert(move[1] >= 0 && move[1] < BOARD_WIDTH, "CHECK");
				
				int cx = move[0];
				int cy = move[1];
				Debug.Log("ai next move > (" + cx + "," + cy + ")");

				GameObject objChess = Instantiate(objX);

				objChess.transform.SetParent(chessParent, false);
				objChess.transform.position = boardCells[cx * BOARD_WIDTH + cy].position;
				chesss[cx, cy] = ChessType.X;

				PlayerFinishTurn();
			}
		}

		// public bool IsGameOver()
		// {
		// 	if (GetWinner(chesss) != ChessType.None)
		// 		return true;

		// 	// check ChessType.None
		// 	for (int i = 0; i < BOARD_WIDTH; ++i)
		// 	{
		// 		for (int j = 0; j < BOARD_WIDTH; ++j)
		// 			if (chesss[i, j] == ChessType.None)
		// 				return false;
		// 	}
		// 	return true;
		// }

		// AI Callback
		public bool FuncGameOver(int[,] board, int player, int opp)
		{
			if (GetWinner(board) != ChessType.None)
				return true;

			// check ChessType.None
			for (int i = 0; i < BOARD_WIDTH; ++i)
			{
				for (int j = 0; j < BOARD_WIDTH; ++j)
					if (board[i, j] == ChessType.None)
						return false;
			}
			return true;
		}

		public int FuncEvaluate(int[,] board, int player, int opp)
		{
			int winner = GetWinner(board);
			if (winner == player)
				return 10;
			else if (winner == opp)
				return -10;
			return 0;
		}

		public List<int[]> FuncMoves(int[,] board, int player)
		{
			List<int[]> moves = new List<int[]>();

			for (int i = 0; i < BOARD_WIDTH; ++i)
			{
				for (int j = 0; j < BOARD_WIDTH; ++j)
				{
					if (board[i, j] == ChessType.None)
						moves.Add(new int[2]{i, j});
				}
			}

			return moves;
		}

		public int[,] FuncBoardgen(int[,] board, int player, int[] move)
		{
			int[,] newBoard = new int[BOARD_WIDTH, BOARD_WIDTH];
			Array.Copy(board, newBoard, board.Length);
			newBoard[move[0], move[1]] = player;
			return newBoard;
		}

		private int GetWinner(int[,] board)
		{
			// vertical
			for (int j = 0; j < BOARD_WIDTH; ++j)
			{
				for (int i = 0; i < BOARD_WIDTH; ++i)
				{
					if (board[i, j] == ChessType.None ||
						board[i, j] != board[0, j])
						break;

					if (i == BOARD_WIDTH - 1)
						return board[0, j];
				}
			}

			// horizontal
			for (int i = 0; i < BOARD_WIDTH; ++i)
			{
				for (int j = 0; j < BOARD_WIDTH; ++j)
				{
					if (board[i, j] == ChessType.None ||
						board[i, j] != board[i, 0])
						break;

					if (j == BOARD_WIDTH - 1)
						return board[i, 0];
				}
			}

			// top left to bottom right
			// [0,0], [1,1], [2,2]
			for (int i = 0; i < BOARD_WIDTH; ++i)
			{
				if (board[i, i] == ChessType.None ||
					board[i, i] != board[0, 0])
					break;

				if (i == BOARD_WIDTH - 1)
					return board[0, 0];
			}

			// bottom left to top right
			// [2,0],[1,1],[0,2]
			for (int i = BOARD_WIDTH - 1, j = 0; i >= 0; --i, ++j)
			{
				if (board[i, j] == ChessType.None ||
					board[i, j] != board[BOARD_WIDTH - 1, 0])
					break;

				if (i == 0)
					return board[BOARD_WIDTH - 1, 0];
			}

			return ChessType.None;
		}
	
		private void EndGame(GameResult ret)
		{
			status = GameStatus.END;

			panelResult.SetActive(true);
			txtWin.SetActive(ret == GameResult.WIN);
			txtLose.SetActive(ret == GameResult.LOSE);
			txtTie.SetActive(ret == GameResult.TIE);
		}
	}
}

