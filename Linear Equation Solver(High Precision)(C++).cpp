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
#include <iostream>
#include <vector>
#include <unordered_map>
#include <algorithm>

#include "GaussElimination.h"
#include "Miscs.h"

int main()
{
    GaussEliminationClass test;
    std::vector<std::vector<Fraction>> array = test.TakeInput();

    array = test.EndResult(array);

	if (array.size() == 0) {
		std::cout << "\nEvery variable is allowed to have any value" << std::endl;
		return 0;
	}

    auto result = test.clean_coloumn(array);
    array = std::get<0>(result);
    std::vector<int> preserved_columns = std::get<1>(result);
    std::vector<int> zero_columns = std::get<2>(result);

    //test.print_matrix(array);

	std::unordered_map<int, Fraction> answer;
	std::unordered_map<int, int> keepTrackColumn;

	for (int j = 0; j < array[0].size() - 1; j++)
	{
		keepTrackColumn[j] = j;
	}

	std::vector<std::vector<Fraction>> array2;
	do
	{
		if (array[0].size() <= 2) {
			break;
		}
		if (test.invalid_check(array) == "invalid")
		{
			throw std::invalid_argument("Not solvable");
		}
		array2.assign(array.size() - 1, std::vector<Fraction>(array[0].size() - 1, Fraction(0, 1)));
		bool available_fixed_value = false;
		int non_zero_index_main = -1;
		int non_zero_size = 0;
		std::vector<int> non_zero_index;

		//part 1
		{
			for (int j = 0; j < array[0].size() - 1; j++) //exluding constant as it can be zero like x=0
			{
				if (array[array.size() - 1][j] != 0)
				{
					non_zero_size++;
					non_zero_index.push_back(j);
				}
			}

			if (non_zero_size == 1)
			{
				available_fixed_value = true;
				non_zero_index_main = non_zero_index[0];
			}
		}

		if (available_fixed_value)
		{
			Fraction base1 = -1 * array[array.size() - 1][array[0].size() - 1] / array[array.size() - 1][non_zero_index_main];
			answer[keepTrackColumn[non_zero_index_main]] = base1;

			int newKey = -1;
			while (keepTrackColumn.find(newKey) != keepTrackColumn.end()) {
				newKey--;
			}
			ModifyKey(keepTrackColumn, non_zero_index_main, newKey);

			std::unordered_map<int, int> modifiedkeepTrackColumn;
			for(auto& [Key, Value] : keepTrackColumn)
			{
				if (Key > non_zero_index_main)
				{
					modifiedkeepTrackColumn[Key - 1] = Value;
				}
				else
				{
					modifiedkeepTrackColumn[Key] = Value;
				}
			}

			keepTrackColumn = modifiedkeepTrackColumn;

			for (int i = 0; i < array.size() - 1; i++)
			{
				for (int j = 0; j < array[0].size() - 1; j++)
				{
					if (j == non_zero_index_main)
					{
						array2[i][array[0].size() - 2] = array[i][array[0].size() - 1] + base1 * array[i][j];
					}
					else if (j > non_zero_index_main)
					{
						array2[i][j] = array[i][j + 1];
					}
					else if (j < non_zero_index_main)
					{
						array2[i][j] = array[i][j];
					}
				}
			}
		}
		else if (array[0].size() - 1 > array.size())
		{
			std::vector<int> non_zero_in_last_row_and_non_zero_only_once_in_its_column;

			for (int j = 0; j < non_zero_size; j++)
			{
				bool applicable = true;
				for (int i = 0; i < array.size() - 1; i++)
				{
					if (array[i][non_zero_index[j]] != 0)
					{
						applicable = false;
					}
				}
				if (applicable)
				{
					non_zero_in_last_row_and_non_zero_only_once_in_its_column.push_back(non_zero_index[j]);
				}
			}

			Fraction change_in_value = Fraction(0, 1);

			int size1 = non_zero_in_last_row_and_non_zero_only_once_in_its_column.size();
			size1 -= 1;
			for (int j = 0; j < size1; j++)
			{
				//modify array matrix by changing constant,push_back answer
				change_in_value = change_in_value - array[array.size() - 1][non_zero_in_last_row_and_non_zero_only_once_in_its_column[j]];
				answer[keepTrackColumn[non_zero_in_last_row_and_non_zero_only_once_in_its_column[j]]] = Fraction(1, 1);
			}

			std::vector<int> possibly_no_solution_but_can_be_valid;
			possibly_no_solution_but_can_be_valid = GetUncommonElements(non_zero_in_last_row_and_non_zero_only_once_in_its_column, non_zero_index);
			Fraction constant_rhs = change_in_value - array[array.size() - 1][array[0].size() - 1];

			for (int j = 0; j < possibly_no_solution_but_can_be_valid.size(); j++)
			{
				change_in_value = change_in_value - array[array.size() - 1][possibly_no_solution_but_can_be_valid[j]];
				constant_rhs = change_in_value - array[array.size() - 1][array[0].size() - 1];
				answer[keepTrackColumn[possibly_no_solution_but_can_be_valid[j]]] = Fraction(1, 1);

				for (int i = 0; i < array.size(); i++)
				{
					//array[i][array[0].size() - 1] is constant
					array[i][array[0].size() - 1] = array[i][array[0].size() - 1] + array[i][possibly_no_solution_but_can_be_valid[j]]; //because constant is on lhs for now, we make negative when shifting to rhs
					array[i][possibly_no_solution_but_can_be_valid[j]] = Fraction(0, 1);
				}
				break;
			}

			if (non_zero_in_last_row_and_non_zero_only_once_in_its_column.size() > 0) {
				Fraction answer_temp = constant_rhs / array[array.size() - 1][non_zero_in_last_row_and_non_zero_only_once_in_its_column[non_zero_in_last_row_and_non_zero_only_once_in_its_column.size() - 1]];

				answer[keepTrackColumn[non_zero_in_last_row_and_non_zero_only_once_in_its_column[non_zero_in_last_row_and_non_zero_only_once_in_its_column.size() - 1]]] = answer_temp;
			}
			//push_backing for deletion, though not come in non_zero_in_last_row_and_non_zero_only_once_in_its_column class
			if (possibly_no_solution_but_can_be_valid.size() > 0) {
				non_zero_in_last_row_and_non_zero_only_once_in_its_column.push_back(possibly_no_solution_but_can_be_valid[0]);
			}

			//remove used column
			array2.assign(array.size(), std::vector<Fraction>(array[0].size() - non_zero_in_last_row_and_non_zero_only_once_in_its_column.size(), Fraction(0, 1)));
			for (int i = 0; i < array.size(); i++)
			{
				int k = 0;
				for (int j = 0; j < array[0].size(); j++)
				{
					int condition1 = count(non_zero_in_last_row_and_non_zero_only_once_in_its_column.begin(), non_zero_in_last_row_and_non_zero_only_once_in_its_column.end(), j);
					if (condition1<=0)
					{
						array2[i][k] = array[i][j];
					}
					else
					{
						k -= 1;
					}
					k++;
				}
			}
			//update key indexes
			for (int j = 0; j < non_zero_in_last_row_and_non_zero_only_once_in_its_column.size(); j++)
			{
				int newKey = -1;
				while (keepTrackColumn.find(newKey) != keepTrackColumn.end()) {
					newKey--;
				}
				ModifyKey(keepTrackColumn, non_zero_in_last_row_and_non_zero_only_once_in_its_column[j], newKey);
			}

			for (int j = 0; j < non_zero_in_last_row_and_non_zero_only_once_in_its_column.size(); j++)
			{
				std::unordered_map<int, int> modifiedkeepTrackColumn;
				for(auto& [Key, Value] : keepTrackColumn)
				{
					if (Key > non_zero_in_last_row_and_non_zero_only_once_in_its_column[j])
					{
						modifiedkeepTrackColumn[Key - 1] = Value;
					}
					else
					{
						modifiedkeepTrackColumn[Key] = Value;
					}
				}

				keepTrackColumn = modifiedkeepTrackColumn;
			}

		}
		else if (array[0].size() - 1 < array.size())
		{
			throw std::invalid_argument("Over determined equations");
		}
		else
		{
			if (non_zero_size == 0 && array[array.size() - 1][array[0].size() - 1] != 0)
			{
				throw std::invalid_argument("Not solvable");
			}
		}
		array = array2;
	} while (array2[0].size() > 2);

	std::vector<int> unflagged_keys;


	for(auto& [Key, Value] : keepTrackColumn)
	{
		if (Key >= 0)
		{
			unflagged_keys.push_back(Key);
		}
	}

	if (unflagged_keys.size() != 1)
	{
		throw std::invalid_argument("More than one variable unsolved at end");
	}
	
	if (array[0][0] != 0)
	{
		answer[keepTrackColumn[unflagged_keys[0]]] = -1 * array[0][1] / array[0][0];
	}

	std::vector<int> unknown_answers;

	for (int i = preserved_columns.size() - 2; i >= 0; i--) {
		if (i != preserved_columns[i]) {
			ModifyKey(answer, i, preserved_columns[i]);
		}

		if (std::find(preserved_columns.begin(), preserved_columns.end(), i) == preserved_columns.end() && std::find(zero_columns.begin(), zero_columns.end(), i) == zero_columns.end()) {
			unknown_answers.push_back(i);
		}
	}

	for(auto& [Key, Value] : answer)
	{
		std::cout << "x" << Key << "=" << Value.ToString() << std::endl;
	}

	for (auto i : zero_columns) {
		std::cout << "x" << i << " is allowed to have any value" << std::endl;
	}

	for (auto i : unknown_answers) {
		std::cout << "x" << i << " is not solvable" << std::endl;
	}
	
	std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
	std::cin.get();
	return 0;
}
