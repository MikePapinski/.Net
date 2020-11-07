using System;
using System.Collections.Generic;
using System.Globalization;

namespace RPNCalulator {
	public class RPN {
		private Stack<int> _operators;
		Dictionary<string, Func<int, int, int>> _operationFunction;
		Dictionary<string, Func<int, int>> _singleOperationFunction;

		public int EvalRPN(string input) {
			_operationFunction = new Dictionary<string, Func<int, int, int>>
			{
				["+"] = (fst, snd) => (fst + snd),
				["-"] = (fst, snd) => (fst - snd),
				["*"] = (fst, snd) => (fst * snd),
				["/"] = (fst, snd) =>
				{
					if (fst == 0)
					{
						throw new DivideByZeroException();
					}
					else
					{
						return fst / snd;
					}
				},
				["^"] = (fst, snd) => (int)Math.Pow(fst, snd),
			};
			_singleOperationFunction = new Dictionary<string, Func<int, int>>
			{
				["!"] = (fst) => Strong(fst),
				["abs"] = (fst) => (Math.Abs(fst))
			};

			_operators = new Stack<int>();

			var splitInput = input.Split(' ');
			foreach (var op in splitInput)
			{
				if (IsNumber(op))
					_operators.Push(Int32.Parse(op));
				else
				if (IsOperator(op))
				{
					if (_singleOperationFunction.ContainsKey(op))
					{
						var num1 = _operators.Pop();
						_operators.Push(_singleOperationFunction[op](num1));
					}
					else
					{
						var num1 = _operators.Pop();
						var num2 = _operators.Pop();
						_operators.Push(_operationFunction[op](num1, num2));
					}
				}
			}

			var result = _operators.Pop();
			if (_operators.IsEmpty)
			{
				return result;
			}
			throw new InvalidOperationException();
		}

		private bool IsNumber(String input) => Int32.TryParse(input, out _);

		private bool IsOperator(String input) =>
			input.Equals("+") || input.Equals("-") ||
			input.Equals("*") || input.Equals("/") ||
			input.Equals("^") || input.Equals("^") ||
			input.Equals("!") || input.Equals("abs");

		private Func<int, int, int> Operation(String input) =>
			(x, y) =>
			(
				(input.Equals("+") ? x + y :
					(input.Equals("*") ? x * y : int.MinValue)
				)
			);

		

		private int Strong(int input)
		{
			if(input == (1 | 0))
			{
				return 1;
			}
			else
			{
				return input * Strong(input - 1);
			}

		}
	}
}