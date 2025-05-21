/*
 * Copyright 2024 Himanshu Bansal
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific
 * language governing permissions and limitations under the License.
 * ==============================================================================
 */
using Linear_Equation_Solver_High_Precision_;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Linear_Equation_Solver_High_Precision_
{
	public class Main
	{
		// Function to take input matrix
		public static Fraction[,] TakeInputMatrix(GaussEliminationClass test)
		{
			return test.TakeInput();
		}

		// Function to perform Gaussian elimination
		static Fraction[,] PerformGaussianElimination(GaussEliminationClass test, Fraction[,] array)
		{
			if (array.GetLength(0) == 0 || array.GetLength(1) == 0)
				throw new Exception("Input matrix is empty or malformed.");
			return test.EndResult(array);
		}

		// Function to clean columns and retrieve preserved and non-zero columns
		static (Fraction[,], List<int>, List<int>) CleanColumns(GaussEliminationClass test, Fraction[,] array)
		{
			return test.clean_coloumn(array);
		}

		// Function to initialize column tracking
		static Dictionary<int, int> InitializeColumnTracking(int columnCount)
		{
			var keepTrackColumn = new Dictionary<int, int>();
			for (int j = 0; j < columnCount; j++)
				keepTrackColumn[j] = j;
			return keepTrackColumn;
		}

		// Function to handle under-determined systems
		static void HandleUnderDeterminedSystem(ref Fraction[,] array, Dictionary<int, Fraction> answer, Dictionary<int, int> keepTrackColumn, out Fraction[,] array2)
		{
			var localArray = array;
			int rowCount = localArray.GetLength(0);
			int colCount = localArray.GetLength(1);
			var non_zero_index = new List<int>();

			// Identify non-zero elements in the last row
			for (int j = 0; j < colCount - 1; j++)
			{
				if (localArray[rowCount - 1, j] != 0)
					non_zero_index.Add(j);
			}
			var non_zero_in_last_row_and_non_zero_only_once_in_its_column = new List<int>();

			// Find columns that are non-zero in the last row and appear only once in their respective columns
			foreach (var j in non_zero_index)
			{
				bool applicable = true;
				for (int i = 0; i < rowCount - 1; i++)
				{
					if (localArray[i, j] != 0)
					{
						applicable = false;
						break;
					}
				}
				if (applicable)
					non_zero_in_last_row_and_non_zero_only_once_in_its_column.Add(j);
			}
			Fraction change_in_value = new Fraction(0, 1);
			int size1 = non_zero_in_last_row_and_non_zero_only_once_in_its_column.Count - 1;

			// Assign arbitrary values to variables and update the answer map
			for (int j = 0; j < size1; j++)
			{
				change_in_value -= localArray[rowCount - 1, non_zero_in_last_row_and_non_zero_only_once_in_its_column[j]];
				answer[keepTrackColumn[non_zero_in_last_row_and_non_zero_only_once_in_its_column[j]]] = new Fraction(1, 1);
			}
			var possibly_no_solution_but_can_be_valid = Miscs.GetUncommonElements(non_zero_in_last_row_and_non_zero_only_once_in_its_column, non_zero_index);
			Fraction constant_rhs = change_in_value - localArray[rowCount - 1, colCount - 1];
			if (possibly_no_solution_but_can_be_valid.Count > 0)
			{
				change_in_value -= localArray[rowCount - 1, possibly_no_solution_but_can_be_valid[0]];
				constant_rhs = change_in_value - localArray[rowCount - 1, colCount - 1];
				answer[keepTrackColumn[possibly_no_solution_but_can_be_valid[0]]] = new Fraction(1, 1);
				for (int i = 0; i < rowCount; i++)
				{
					localArray[i, colCount - 1] += localArray[i, possibly_no_solution_but_can_be_valid[0]];
					localArray[i, possibly_no_solution_but_can_be_valid[0]] = new Fraction(0, 1);
				}
			}
			if (non_zero_in_last_row_and_non_zero_only_once_in_its_column.Count > 0)
			{
				int idx = non_zero_in_last_row_and_non_zero_only_once_in_its_column[non_zero_in_last_row_and_non_zero_only_once_in_its_column.Count - 1];
				Fraction denom = localArray[rowCount - 1, idx];
				if (denom == 0)
				{
					throw new DivideByZeroException("Division by zero in underdetermined system.");
				}
				Fraction answer_temp = constant_rhs / denom;
				answer[keepTrackColumn[idx]] = answer_temp;
			}
			if (possibly_no_solution_but_can_be_valid.Count > 0)
				non_zero_in_last_row_and_non_zero_only_once_in_its_column.Add(possibly_no_solution_but_can_be_valid[0]);

			// Update the matrix by removing solved columns
			array2 = new Fraction[rowCount, colCount - non_zero_in_last_row_and_non_zero_only_once_in_its_column.Count];
			for (int i = 0; i < rowCount; i++)
			{
				int k = 0;
				for (int j = 0; j < colCount; j++)
				{
					if (!non_zero_in_last_row_and_non_zero_only_once_in_its_column.Contains(j))
					{
						array2[i, k] = localArray[i, j];
					}
					else
					{
						k--;
					}
					k++;
				}
			}

			// Update column tracking
			foreach (var idx in non_zero_in_last_row_and_non_zero_only_once_in_its_column)
			{
				int newKey = -1;
				while (keepTrackColumn.ContainsKey(newKey))
					newKey--;
				Miscs.ModifyKey(keepTrackColumn, idx, newKey);
			}
			foreach (var idx in non_zero_in_last_row_and_non_zero_only_once_in_its_column)
			{
				var modifiedkeepTrackColumn = new Dictionary<int, int>();
				foreach (var kv in keepTrackColumn)
				{
					if (kv.Key > idx)
						modifiedkeepTrackColumn[kv.Key - 1] = kv.Value;
					else
						modifiedkeepTrackColumn[kv.Key] = kv.Value;
				}
				keepTrackColumn.Clear();
				foreach (var kv in modifiedkeepTrackColumn)
					keepTrackColumn[kv.Key] = kv.Value;
			}
			array = localArray;
		}

		// Function to process the matrix and solve equations
		static void ProcessMatrix(GaussEliminationClass test, ref Fraction[,] array, Dictionary<int, Fraction> answer, Dictionary<int, int> keepTrackColumn)
		{
			Fraction[,] array2 = null;
			do
			{
				if (array.GetLength(1) <= 2)
					break;
				if (test.invalid_check(array) == "invalid")
					throw new InvalidOperationException("Not solvable");
				array2 = new Fraction[array.GetLength(0) - 1, array.GetLength(1) - 1];
				var localArray = array;
				var non_zero_index = Enumerable.Range(0, localArray.GetLength(1) - 1)
					.Where(j => localArray[localArray.GetLength(0) - 1, j] != 0)
					.ToList();
				bool available_fixed_value = non_zero_index.Count == 1;
				int non_zero_index_main = available_fixed_value ? non_zero_index[0] : -1;
				if (available_fixed_value)
				{
					Fraction base1 = -1 * localArray[localArray.GetLength(0) - 1, localArray.GetLength(1) - 1] / localArray[localArray.GetLength(0) - 1, non_zero_index_main];
					answer[keepTrackColumn[non_zero_index_main]] = base1;
					Miscs.ModifyKey(keepTrackColumn, non_zero_index_main, -1);
					var modifiedkeepTrackColumn = keepTrackColumn.ToDictionary(item => item.Key > non_zero_index_main ? item.Key - 1 : item.Key, item => item.Value);
					keepTrackColumn.Clear();
					foreach (var kv in modifiedkeepTrackColumn)
						keepTrackColumn[kv.Key] = kv.Value;
					for (int i = 0; i < localArray.GetLength(0) - 1; i++)
					{
						for (int j = 0; j < localArray.GetLength(1) - 1; j++)
						{
							if (j == non_zero_index_main)
								array2[i, localArray.GetLength(1) - 2] = localArray[i, localArray.GetLength(1) - 1] + base1 * localArray[i, j];
							else if (j > non_zero_index_main)
								array2[i, j] = localArray[i, j + 1];
							else
								array2[i, j] = localArray[i, j];
						}
					}
				}
				else if (array.GetLength(1) - 1 > array.GetLength(0))
				{
					HandleUnderDeterminedSystem(ref array, answer, keepTrackColumn, out array2);
				}
				else if (array.GetLength(1) - 1 < array.GetLength(0))
				{
					throw new InvalidOperationException("Over determined equations");
				}
				else
				{
					if (!non_zero_index.Any() && array[array.GetLength(0) - 1, array.GetLength(1) - 1] != 0)
						throw new InvalidOperationException("Not solvable");
				}
				array = array2;
			} while (array2.GetLength(1) > 2);
		}

		// Function to handle unsolved variables
		static void HandleUnsolvedVariables(Fraction[,] array, Dictionary<int, int> keepTrackColumn, Dictionary<int, Fraction> answer)
		{
			var unflagged_keys = keepTrackColumn.Keys.Where(k => k >= 0).ToList();
			if (unflagged_keys.Count > 1)
				throw new InvalidOperationException("More than one variable unsolved at end");
			if (unflagged_keys.Count == 1 && array[0, 0] != 0)
				answer[keepTrackColumn[unflagged_keys[0]]] = -1 * array[0, 1] / array[0, 0];
		}

		// Function to adjust answers for preserved columns
		public static void AdjustAnswersForPreservedColumns(List<int> preserved_columns, List<int> zero_columns, Dictionary<int, Fraction> answer)
		{
			var unknown_answers = new List<int>();
			for (int i = preserved_columns.Count - 2; i >= 0; i--)
			{
				if (i != preserved_columns[i])
					Miscs.ModifyKey(answer, i, preserved_columns[i]);
				if (!preserved_columns.Contains(i) && !zero_columns.Contains(i))
					unknown_answers.Add(i);
			}
			foreach (var kv in answer)
				Console.WriteLine($"x{kv.Key} = {kv.Value}");
			foreach (var i in zero_columns)
				Console.WriteLine($"x{i} is allowed to have any value");
			foreach (var i in unknown_answers)
				Console.WriteLine($"x{i} is not solvable");
		}

		public static (List<int>, List<int>, Dictionary<int, Fraction>) Solve(GaussEliminationClass test, Fraction[,] array)
		{
			array = PerformGaussianElimination(test, array);
			if (array.GetLength(0) == 0)
				throw new Exception("Every variable is allowed to have any value");
			var (cleaned_array, preserved_columns, zero_columns) = CleanColumns(test, array);
			var keepTrackColumn = InitializeColumnTracking(cleaned_array.GetLength(1) - 1);
			var answer = new Dictionary<int, Fraction>();
			ProcessMatrix(test, ref cleaned_array, answer, keepTrackColumn);
			HandleUnsolvedVariables(cleaned_array, keepTrackColumn, answer);
			return (preserved_columns, zero_columns, answer);
		}
	}
}
