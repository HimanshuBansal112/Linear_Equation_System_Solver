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
#include <string>

#include "Essentials.h"
#include "Fraction_Data_Type.h"
#include "GaussElimination.h"
#include <random>

std::vector<std::vector<Fraction>> GaussEliminationClass::matrix_sort(std::vector<std::vector<Fraction>> matrix)
{
	int len = matrix.size();
	if (matrix.size() > matrix[0].size())
	{
		len = matrix[0].size();
	}

	for (int i = 0; i < len; i++)
	{
		if (matrix[i][i] == 0)
		{
			for (int i2 = i + 1; i2 < len; i2++)
			{
				if (matrix[i2][i] != 0)
				{
					for (int i3 = 0; i3 < matrix[0].size(); i3++)
					{
						Fraction temp(0, 1);
						temp = matrix[i2][i3];
						matrix[i2][i3] = matrix[i][i3];
						matrix[i][i3] = temp;
					}
					i2 = len;
				}
			}
		}
	}
	return matrix;
}

std::vector<std::vector<Fraction>> GaussEliminationClass::Convert_to_one(std::vector<std::vector<Fraction>> matrix, int row_no, int column_count, std::vector<int> index)
{
	row_no -= 1;
	Fraction base_no = matrix[index[0]][index[1]];

	for (int j = 0; j < column_count; j++)
	{
		matrix[row_no][j] = matrix[row_no][j] / base_no;
	}
	return matrix;
}

std::vector<std::vector<Fraction>> GaussEliminationClass::Convert_to_zero(std::vector<std::vector<Fraction>> matrix, int row_no, int column_no, int column_count, int ones_row_no)
{
	column_no -= 1;
	row_no -= 1;
	Fraction base_no = matrix[ones_row_no][column_no];
	Fraction multiply_base = matrix[row_no][column_no] / base_no;

	for (int j = 0; j < column_count; j++)
	{
		matrix[row_no][j] = matrix[row_no][j] - (multiply_base * matrix[ones_row_no][j]); //column number is row number where value is one
	}
	return matrix;
}

bool GaussEliminationClass::AreMatricesEqual(std::vector<std::vector<Fraction>> matrix1, std::vector<std::vector<Fraction>> matrix2)
{
	if (matrix1.size() != matrix2.size() || matrix1[0].size() != matrix2[0].size())
	{
		// Matrices have different dimensions
		return false;
	}

	for (int i = 0; i < matrix1.size(); i++)
	{
		for (int j = 0; j < matrix1[0].size(); j++)
		{
			// Compare corresponding elements
			if (matrix1[i][j] != matrix2[i][j])
			{
				return false;
			}
		}
	}

	// All corresponding elements are equal
	return true;
}

std::vector<std::vector<Fraction>> GaussEliminationClass::GaussElimination(std::vector<std::vector<Fraction>> matrix)
{
	int column_count = matrix[0].size();

	for (int i = 1; i < matrix.size(); i++)
	{
		if (column_count != matrix[0].size())
		{
			throw std::invalid_argument("Column count doesn't match at every row");
		}
	}

	std::vector<int> index_to_be_one = { 0, 0 };

	int last_row_no = 0;

	for (int j = 0; j < column_count; j++)
	{
		for (int i = 0; i < matrix.size(); i++)
		{
			if (index_to_be_one[0] == i && index_to_be_one[1] == j)
			{
				if (matrix[i][j] == 0)
				{
					index_to_be_one[1]++;
				}
				else if (matrix[i][j] == 1)
				{
					last_row_no = index_to_be_one[0];
					index_to_be_one[0]++;
					index_to_be_one[1]++;
				}
				else
				{
					matrix = Convert_to_one(matrix, i + 1, column_count, index_to_be_one);
					last_row_no = index_to_be_one[0];
					index_to_be_one[0]++;
					index_to_be_one[1]++;
				}
			}
			else if (j < i)
			{
				if (matrix[i][j] != 0)
				{
					matrix = Convert_to_zero(matrix, i + 1, j + 1, matrix[0].size(), last_row_no);
					std::vector<std::vector<Fraction>> matrix_copy(matrix); //to clone
					matrix = matrix_sort(matrix);
					if (!AreMatricesEqual(matrix, matrix_copy))
					{
						matrix = GaussElimination(matrix);
					}
				}
			}
		}
	}

	return matrix;
}

bool GaussEliminationClass::underdetermined_check(std::vector<std::vector<Fraction>> matrix)
{
	int len = matrix.size();

	if (matrix.size() > matrix[0].size())
	{
		len = matrix[0].size();
	}
	for (int i = 0; i < len; i++)
	{
		for (int j = 0; j < len; j++)
		{
			if (matrix[i][j] == 1)
			{
				j = len;
				continue;
			}
			else if (matrix[i][j] != 0)
			{
				return false;
			}
		}
	}
	return true;
}

std::string GaussEliminationClass::invalid_check(std::vector<std::vector<Fraction>> matrix)
{
	for (int i = 0; i < matrix.size(); i++)
	{
		bool is_lhs_zero = true;
		for (int j = 0; j < matrix[0].size(); j++)
		{
			if (matrix[i][j] != 0 && j < matrix[0].size() - 1)
			{
				is_lhs_zero = false;
			}
			if (j == matrix[0].size() - 1 && is_lhs_zero == true)
			{
				if (matrix[i][j] != 0)
				{
					return "invalid";
				}
			}
		}
	}
	return "correct";
}

void GaussEliminationClass::print_matrix(std::vector<std::vector<Fraction>> array)
{
	for (int i = 0; i < array.size(); i++)
	{
		std::cout << "[";
		for (int j = 0; j < array[0].size(); j++)
		{
			if (j != array[0].size() - 1)
			{
				std::cout << array[i][j].ToString() << " ";
			}
			else
			{
				std::cout << array[i][j].ToString();
			}
		}
		std::cout << "]" << std::endl;
	}
}

std::vector<std::vector<Fraction>> GaussEliminationClass::clean(std::vector<std::vector<Fraction>> array)
{
	std::vector<int> non_zero_list;

	for (int i = 0; i < array.size(); i++)
	{
		bool zero_row = true;
		for (int j = 0; j < array[0].size(); j++)
		{
			if (array[i][j] != 0)
			{
				zero_row = false;
				break;
			}
		}
		if (zero_row == false)
		{
			non_zero_list.push_back(i);
		}
	}

	//non zero list is for non zero row
	std::vector<std::vector<Fraction>> temp12(non_zero_list.size(), std::vector<Fraction>(array[0].size(), Fraction(0,1)));

	for (int i = 0; i < non_zero_list.size(); i++)
	{
		for (int j = 0; j < array[0].size(); j++)
		{
			temp12[i][j] = array[non_zero_list[i]][j];
		}
	}

	return temp12;
}

std::tuple<std::vector<std::vector<Fraction>>, std::vector<int>, std::vector<int>> GaussEliminationClass::clean_coloumn(std::vector<std::vector<Fraction>> array)
{
	std::vector<int> non_zero_list;
	std::vector<int> zero_list;

	for (int j = 0; j < array[0].size() - 1; j++)
	{
		bool zero_column = true;

		for (int i = 0; i < array.size(); i++)
		{
			if (array[i][j] != 0)
			{
				zero_column = false;
				break;
			}
		}
		if (zero_column == false)
		{
			non_zero_list.push_back(j);
		}
		else {
			zero_list.push_back(j);
		}
	}

	non_zero_list.push_back(array[0].size() - 1);

	//non zero list is for non zero column
	std::vector<std::vector<Fraction>> temp12(array.size(), std::vector<Fraction>(non_zero_list.size(), Fraction(0, 1)));

	for (int i = 0; i < array.size(); i++)
	{
		for (int j = 0; j < non_zero_list.size(); j++)
		{
			temp12[i][j] = array[i][non_zero_list[j]];
		}
	}

	return std::tuple<std::vector<std::vector<Fraction>>, std::vector<int>, std::vector<int>>(temp12, non_zero_list, zero_list);
}

std::vector<std::vector<Fraction>> GaussEliminationClass::EndResult(std::vector<std::vector<Fraction>> matrix)
{
	matrix = matrix_sort(matrix);
	matrix = GaussElimination(matrix);
	matrix = clean(matrix);
	return matrix;
}

std::vector<std::vector<Fraction>> GaussEliminationClass::TakeInput()
{
	int row = 0, column = 0;
	std::string row1, column1;

	std::cout << "Enter number of equations: ";
	std::cin >> row1;
	if (row1.empty()) {
		row1 = "a";
	}

	while (!tryParseInt(row1, row) || row <= 0)
	{
		std::cout << "Invalid equations count. Re-enter please: ";
		std::cin >> row1;
		if (row1.empty()) {
			row1 = "a";
		}
	}


	std::cout << "Enter number of variables: ";
	std::cin >> column1;
	if (column1.empty()) {
		column1 = "a";
	}

	while (!tryParseInt(column1, column) || column <= 0)
	{
		std::cout << "Invalid variable count. Re-enter please: ";
		std::cin >> column1;
		if (column1.empty()) {
			column1 = "a";
		}
	}

	column = column + 1;

	std::vector<std::vector<Fraction>> array(row, std::vector<Fraction>(column, Fraction(0, 1)));

	std::string temp = "a";

	std::cout << "\n\nFormat is a0*x0 + a1*x1 + .... + c = 0\n" << std::endl;

	for (int i = 0; i < row; i++)
	{
		std::cout << "\nFor equation " << i << ": " << std::endl;
		for (int j = 0; j < column; j++)
		{
			if (j == column - 1)
			{
				std::cout << "Write constant c: ";
			}
			else
			{
				std::cout << "Write coefficient for variable x" << j << ": ";
			}

			std::cin >> temp;
			if (temp.empty()) {
				temp = "a";
			}

			while (Fraction::TryParse(temp, array[i][j])!=true)
			{
				if (Fraction::TryParse(temp, array[i][j]) == -1) {
					std::cout << "Large numbers not supported. Re-enter please: ";
				}
				else if (Fraction::TryParse(temp, array[i][j]) == -2) {
					std::cout << "Denominator should not be 0. Re-enter please: ";
				}
				else {
					std::cout << "Invalid value. Re-enter please: ";
				}
				std::cin >> temp;
				if (temp.empty()) {
					temp = "a";
				}
			}
		}
	}
	return array;
}

std::vector<std::vector<Fraction>> GaussEliminationClass::RandomInput()
{
	int row = 0, column = 0;
	std::string row1, column1;

	std::cout << "This is a random constant generator for testing" << std::endl;

	std::cout << "Enter number of equations: ";
	std::cin >> row1;
	if (row1.empty()) {
		row1 = "a";
	}

	while (!tryParseInt(row1, row) || row <= 0)
	{
		std::cout << "Invalid equations count. Re-enter please: ";
		std::cin >> row1;
		if (row1.empty()) {
			row1 = "a";
		}
	}


	std::cout << "Enter number of variables: ";
	std::cin >> column1;
	if (column1.empty()) {
		column1 = "a";
	}

	while (!tryParseInt(column1, column) || column <= 0)
	{
		std::cout << "Invalid variable count. Re-enter please: ";
		std::cin >> column1;
		if (column1.empty()) {
			column1 = "a";
		}
	}

	column = column + 1;

	std::vector<std::vector<Fraction>> array(row, std::vector<Fraction>(column, Fraction(0, 1)));

	std::uniform_real_distribution<double> unif(0, 10000);

	std::default_random_engine re;

	double a;

	for (int i = 0; i < row; i++)
	{
		for (int j = 0; j < column; j++)
		{
			a = unif(re);
			a = std::round(a*100)/100;
			if (Fraction::TryParse(std::to_string(a), array[i][j])!=true)
			{
				if (Fraction::TryParse(std::to_string(a), array[i][j]) == -1) {
					std::cout << "Large numbers found.";
				}
				else if (Fraction::TryParse(std::to_string(a), array[i][j]) == -2) {
					std::cout << "Denominator is equal to 0.";
				}
				else {
					std::cout << "Invalid number found.";
				}
				std::invalid_argument("Error!");
			}
		}
	}
	return array;
}