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
using System.Buffers.Text;

GaussEliminationClass test = new GaussEliminationClass();

//Fraction[,] array = {
//			{ new Fraction(2,1), new Fraction(0,1), new Fraction(-2,1), new Fraction(0,1) },
//			{ new Fraction(0,1), new Fraction(2,1), new Fraction(-1,1), new Fraction(0,1) }
//		};

Fraction[,] array = test.TakeInput();


array = test.EndResult(array);

if (array.GetLength(0) == 0)
{
	Console.WriteLine("\nEvery variable is allowed to have any value");
	Environment.Exit(0);
}

var result = test.clean_coloumn(array);
array = result.Item1;
List<int> preserved_columns = result.Item2;
List<int> zero_columns = result.Item3;
//foreach(int i in preserved_columns)
//{
//	Console.WriteLine(i);
//}

//test.print_matrix(array);
//Environment.Exit(0);
Dictionary<int,Fraction> answer = new Dictionary<int,Fraction>();
Dictionary<int, int> keepTrackColumn = new Dictionary<int, int>();

for (int j = 0; j < array.GetLength(1) - 1; j++)
{
	keepTrackColumn.Add(j, j);
}

Fraction[,] array2;
do
{
	if (array.GetLength(1) <= 2)
	{
		break;
	}
	if (test.invalid_check(array)== "invalid")
	{
		throw new Exception("Not solvable");
	}
	array2 = new Fraction[array.GetLength(0) - 1, array.GetLength(1) - 1];
	bool available_fixed_value = false;
	int non_zero_index_main = -1;
	int non_zero_count = 0;
	List<int> non_zero_index = new List<int>();

	//part 1
	{
		for (int j = 0; j < array.GetLength(1) - 1; j++) //exluding constant as it can be zero like x=0
		{
			if (array[array.GetLength(0) - 1, j] != 0)
			{
				non_zero_count++;
				non_zero_index.Add(j);
			}
		}

		if (non_zero_count == 1)
		{
			available_fixed_value = true;
			non_zero_index_main = non_zero_index[0];
		}
	}

	//Fraction[,] array2 = new Fraction[array.GetLength(0) - 1, array.GetLength(1) - 1];

	if (available_fixed_value)
	{
		Fraction base1 = -1 * array[array.GetLength(0) - 1, array.GetLength(1) - 1] / array[array.GetLength(0) - 1, non_zero_index_main];
		answer.Add(keepTrackColumn[non_zero_index_main], base1);

		Miscs.ModifyKey(keepTrackColumn, non_zero_index_main, -1);
		Dictionary<int, int> modifiedkeepTrackColumn = new Dictionary<int, int>();
		foreach (KeyValuePair<int, int> item in keepTrackColumn)
		{
			if (item.Key > non_zero_index_main)
			{
				modifiedkeepTrackColumn.Add(item.Key - 1, item.Value);
			}
			else
			{
				modifiedkeepTrackColumn.Add(item.Key, item.Value);
			}
		}

		keepTrackColumn = modifiedkeepTrackColumn;

		for (int i = 0; i < array.GetLength(0) - 1; i++)
		{
			for (int j = 0; j < array.GetLength(1) - 1; j++)
			{
				if (j == non_zero_index_main)
				{
					array2[i, array.GetLength(1) - 2] = array[i, array.GetLength(1) - 1] + base1 * array[i, j];
				}
				else if (j > non_zero_index_main)
				{
					array2[i, j] = array[i, j + 1];
				}
				else if (j < non_zero_index_main)
				{
					array2[i, j] = array[i, j];
				}
				//array2[i, non_zero_index_main] 
			}
		}
	}
	else if (array.GetLength(1) - 1 > array.GetLength(0))
	{
		List<int> non_zero_in_last_row_and_non_zero_only_once_in_its_column = new List<int>();
		for (int j = 0; j < non_zero_count; j++)
		{
			bool applicable = true;
			for (int i = 0; i < array.GetLength(0) - 1; i++)
			{
				if (array[i, non_zero_index[j]] != 0)
				{
					applicable = false;
				}
			}
			if (applicable)
			{
				non_zero_in_last_row_and_non_zero_only_once_in_its_column.Add(non_zero_index[j]);
			}
		}

		Fraction change_in_value = new Fraction(0, 1);

		for (int j = 0; j < non_zero_in_last_row_and_non_zero_only_once_in_its_column.Count() - 1; j++)
		{
			//modify array matrix by changing constant,add answer
			change_in_value -= array[array.GetLength(0) - 1, non_zero_in_last_row_and_non_zero_only_once_in_its_column[j]];
			answer.Add(keepTrackColumn[non_zero_in_last_row_and_non_zero_only_once_in_its_column[j]], new Fraction(1,1));
		}

		List<int> possibly_no_solution_but_can_be_valid = new List<int>();
		possibly_no_solution_but_can_be_valid = Miscs.GetUncommonElements(non_zero_in_last_row_and_non_zero_only_once_in_its_column, non_zero_index);
		Fraction constant_rhs = change_in_value - array[array.GetLength(0) - 1, array.GetLength(1) - 1];

		for (int j = 0; j < possibly_no_solution_but_can_be_valid.Count(); j++)
		{
			change_in_value -= array[array.GetLength(0) - 1, possibly_no_solution_but_can_be_valid[j]];
			constant_rhs = change_in_value - array[array.GetLength(0) - 1, array.GetLength(1) - 1];
			answer.Add(keepTrackColumn[possibly_no_solution_but_can_be_valid[j]], new Fraction(1,1));

			for (int i = 0; i < array.GetLength(0); i++)
			{
				//array[i, array.GetLength(1) - 1] is constant
				array[i, array.GetLength(1) - 1] += array[i, possibly_no_solution_but_can_be_valid[j]]; //because constant is on lhs for now, we make negative when shifting to rhs
				array[i, possibly_no_solution_but_can_be_valid[j]] = new Fraction(0, 1);
			}
			break;
		}

		if (non_zero_in_last_row_and_non_zero_only_once_in_its_column.Count() > 0)
		{
			Fraction answer_temp = constant_rhs / array[array.GetLength(0) - 1, non_zero_in_last_row_and_non_zero_only_once_in_its_column[non_zero_in_last_row_and_non_zero_only_once_in_its_column.Count() - 1]];

			answer.Add(keepTrackColumn[non_zero_in_last_row_and_non_zero_only_once_in_its_column[non_zero_in_last_row_and_non_zero_only_once_in_its_column.Count() - 1]], answer_temp);
		}

		//adding for deletion, though not come in non_zero_in_last_row_and_non_zero_only_once_in_its_column class
		if (possibly_no_solution_but_can_be_valid.Count() > 0)
		{
			non_zero_in_last_row_and_non_zero_only_once_in_its_column.Add(possibly_no_solution_but_can_be_valid[0]);
		}
		//remove used column
		array2 = new Fraction[array.GetLength(0), array.GetLength(1) - non_zero_in_last_row_and_non_zero_only_once_in_its_column.Count()];
		for (int i = 0; i < array.GetLength(0); i++)
		{
			int k = 0;
			for (int j = 0; j < array.GetLength(1); j++)
			{
				if (!non_zero_in_last_row_and_non_zero_only_once_in_its_column.Contains(j))
				{
					array2[i, k] = array[i, j];
				}
				else
				{
					k -= 1;
				}
				k++;
			}
		}
		//update key indexes
		for (int j = 0; j < non_zero_in_last_row_and_non_zero_only_once_in_its_column.Count(); j++)
		{
			Miscs.ModifyKey(keepTrackColumn, non_zero_in_last_row_and_non_zero_only_once_in_its_column[j], -1);
		}

		for (int j = 0; j < non_zero_in_last_row_and_non_zero_only_once_in_its_column.Count(); j++)
		{
			Dictionary<int, int> modifiedkeepTrackColumn = new Dictionary<int, int>();
			foreach (KeyValuePair<int, int> item in keepTrackColumn)
			{
				if (item.Key > non_zero_in_last_row_and_non_zero_only_once_in_its_column[j])
				{
					modifiedkeepTrackColumn.Add(item.Key - 1, item.Value);
				}
				else
				{
					modifiedkeepTrackColumn.Add(item.Key, item.Value);
				}
			}

			keepTrackColumn = modifiedkeepTrackColumn;
		}

	}
	else if (array.GetLength(1) - 1 < array.GetLength(0))
	{
		throw new Exception("Over determined equations");
	}
	else
	{
		if (non_zero_count == 0 && array[array.GetLength(0) - 1, array.GetLength(1) - 1] != 0)
		{
			throw new Exception("Not solvable");
		}
	}
	array = array2;
} while (array2.GetLength(1) > 2);

List<int> unflagged_keys = new List<int>();

foreach (KeyValuePair<int, int> item in keepTrackColumn)
{
	if (item.Key != -1)
	{
		unflagged_keys.Add(item.Key);
	}
}

if (unflagged_keys.Count()>1)
{
	throw new Exception("More than one variable unsolved at end");
}

if (unflagged_keys.Count() == 1 && array[0, 0] != 0)
{
	answer.Add(keepTrackColumn[unflagged_keys[0]],-1 * array[0, 1] / array[0, 0]);
}

List<int> unknown_answers = new List<int>();

for (int i = preserved_columns.Count() - 2; i >= 0; i--)
{
	if (i != preserved_columns[i])
	{
		Miscs.ModifyKey(answer, i, preserved_columns[i]);
	}

	if (!preserved_columns.Contains(i) && !zero_columns.Contains(i))
	{
		unknown_answers.Add(i);
	}
}

foreach (KeyValuePair<int,Fraction> specific_answer in answer)
{
	Console.WriteLine($"x{specific_answer.Key} = {specific_answer.Value}");
}

foreach (int i in  zero_columns)
{
	Console.WriteLine($"x{i} is allowed to have any value");
}

foreach (int i in unknown_answers)
{
	Console.WriteLine($"x{i} is not solvable");
}

//Fraction[,] array2 = new Fraction[array.GetLength(0)-1, array.GetLength(1)-1];

//for (int i = 0; i < array.GetLength(0); i++)
//{
//	for (int j = 0; j < array.GetLength(1)-1; j++)
//	{
//		if (j != array.GetLength(1) - 2)
//		{
//			array2[i, j] = array[i, j];
//		}
//		else
//		{
//			array2[i, j] = array[i, j + 1] - array[i, j];
//		}
//	}
//}

Console.ReadLine();
