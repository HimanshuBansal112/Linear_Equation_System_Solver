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
#pragma once

#include <iostream>
#include <vector>
#include <string>

#include "Essentials.h"
#include "Fraction Data Type.h"

#ifndef GAUSS_ELIMINATION_H
#define GAUSS_ELIMINATION_H

class GaussEliminationClass {
public:
	std::vector<std::vector<Fraction>> matrix_sort(std::vector<std::vector<Fraction>> matrix);
	std::vector<std::vector<Fraction>> Convert_to_one(std::vector<std::vector<Fraction>> matrix, int row_no, int column_count, std::vector<int> index);
	std::vector<std::vector<Fraction>> Convert_to_zero(std::vector<std::vector<Fraction>> matrix, int row_no, int column_no, int column_count, int ones_row_no);
	static bool AreMatricesEqual(std::vector<std::vector<Fraction>> matrix1, std::vector<std::vector<Fraction>> matrix2);
	std::vector<std::vector<Fraction>> GaussElimination(std::vector<std::vector<Fraction>> matrix);
	bool underdetermined_check(std::vector<std::vector<Fraction>> matrix);
	std::string invalid_check(std::vector<std::vector<Fraction>> matrix);
	void print_matrix(std::vector<std::vector<Fraction>> array);
	std::vector<std::vector<Fraction>> clean(std::vector<std::vector<Fraction>> array);
	std::tuple<std::vector<std::vector<Fraction>>, std::vector<int>, std::vector<int>> clean_coloumn(std::vector<std::vector<Fraction>> array);
	std::vector<std::vector<Fraction>> EndResult(std::vector<std::vector<Fraction>> matrix);
	std::vector<std::vector<Fraction>> TakeInput();
	std::vector<std::vector<Fraction>> RandomInput();

};

#endif