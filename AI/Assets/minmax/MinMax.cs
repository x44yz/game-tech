using System.Collections;
using System.Collections.Generic;

// Pseudocode
// https://www.youtube.com/watch?v=l-hh51ncgDI
//function minmax(position, depth, alpha, beta, maximizingPlayer)
//	if depth == 0 or game over in position
//		return static evaluation of position

//	if maximizingPlayer
//		maxEval = -infinity
//		for each child of postion
//			eval = minimax(child, depth - 1, alpha, beta, false)
//			maxEval = max(maxEval, eval)
//			alpha = max(alpha, eval)
//			if beta <= alpha
//				break
//		return maxEval
//	else
//		minEval = +infinity
//		for each child of position
//			eval = minimax(child, depth - 1, alpha, beta, true)
//			minEval = min(minEval, eval)
//			beta = min(beta, eval)
//			if beta <= alpha
//				break
//		return minEval

namespace AI
{
	public class MinMax
	{
		// return evaluate score
		// public static int Eval()
	}
}

