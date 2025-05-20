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
#include <limits>

#include "GaussElimination.h"
#include "Miscs.h"
#include "main.h"

// Function to take input matrix
std::vector<std::vector<Fraction>> TakeInputMatrix(GaussEliminationClass& test) {
    return test.TakeInput();
}

// Function to perform Gaussian elimination
std::vector<std::vector<Fraction>> PerformGaussianElimination(GaussEliminationClass& test, std::vector<std::vector<Fraction>>& array) {
    if (array.empty() || array[0].empty()) {
        throw std::out_of_range("Input matrix is empty or malformed.");
    }
    return test.EndResult(array);
}

// Function to clean columns and retrieve preserved and zero columns
std::tuple<std::vector<std::vector<Fraction>>, std::vector<int>, std::vector<int>> CleanColumns(GaussEliminationClass& test, std::vector<std::vector<Fraction>>& array) {
    return test.clean_coloumn(array);
}

// Function to initialize column tracking
std::unordered_map<int, int> InitializeColumnTracking(int columnCount) {
    std::unordered_map<int, int> keepTrackColumn;
    for (int j = 0; j < columnCount; j++) {
        keepTrackColumn[j] = j;
    }
    return keepTrackColumn;
}

// Function to handle under-determined systems
void HandleUnderDeterminedSystem(std::vector<std::vector<Fraction>>& array,
    std::unordered_map<int, Fraction>& answer,
    std::unordered_map<int, int>& keepTrackColumn,
    std::vector<std::vector<Fraction>>& array2) {
    if (array.empty() || array[0].empty()) {
        throw std::out_of_range("Input matrix is empty or malformed.");
    }
    std::vector<int> non_zero_in_last_row_and_non_zero_only_once_in_its_column;
    std::vector<int> non_zero_index;
    int non_zero_size = 0;

    // Identify non-zero elements in the last row
    for (int j = 0; j < array[0].size() - 1; j++) {
        if (array[array.size() - 1][j] != 0) {
            non_zero_size++;
            non_zero_index.push_back(j);
        }
    }

    // Find columns that are non-zero in the last row and appear only once in their respective columns
    for (int j = 0; j < non_zero_size; j++) {
        bool applicable = true;
        for (int i = 0; i < array.size() - 1; i++) {
            if (array[i][non_zero_index[j]] != 0) {
                applicable = false;
                break;
            }
        }
        if (applicable) {
            non_zero_in_last_row_and_non_zero_only_once_in_its_column.push_back(non_zero_index[j]);
        }
    }

    Fraction change_in_value = Fraction(0, 1);
    int size1 = non_zero_in_last_row_and_non_zero_only_once_in_its_column.size();
    size1 -= 1;

    // Assign arbitrary values to variables and update the answer map
    for (int j = 0; j < size1; j++) {
        change_in_value = change_in_value - array[array.size() - 1][non_zero_in_last_row_and_non_zero_only_once_in_its_column[j]];
        answer[keepTrackColumn[non_zero_in_last_row_and_non_zero_only_once_in_its_column[j]]] = Fraction(1, 1);
    }

    std::vector<int> possibly_no_solution_but_can_be_valid;
    possibly_no_solution_but_can_be_valid = GetUncommonElements(non_zero_in_last_row_and_non_zero_only_once_in_its_column, non_zero_index);
    Fraction constant_rhs = change_in_value - array[array.size() - 1][array[0].size() - 1];

    if (!possibly_no_solution_but_can_be_valid.empty()) {
        change_in_value = change_in_value - array[array.size() - 1][possibly_no_solution_but_can_be_valid[0]];
        constant_rhs = change_in_value - array[array.size() - 1][array[0].size() - 1];
        answer[keepTrackColumn[possibly_no_solution_but_can_be_valid[0]]] = Fraction(1, 1);

        for (int i = 0; i < array.size(); i++) {
            array[i][array[0].size() - 1] = array[i][array[0].size() - 1] + array[i][possibly_no_solution_but_can_be_valid[0]];
            array[i][possibly_no_solution_but_can_be_valid[0]] = Fraction(0, 1);
        }
    }

    if (non_zero_in_last_row_and_non_zero_only_once_in_its_column.size() > 0) {
        Fraction denom = array[array.size() - 1][non_zero_in_last_row_and_non_zero_only_once_in_its_column[non_zero_in_last_row_and_non_zero_only_once_in_its_column.size() - 1]];
        if (denom == 0) {
            throw std::domain_error("Division by zero in underdetermined system.");
        }
        Fraction answer_temp = constant_rhs / denom;
        answer[keepTrackColumn[non_zero_in_last_row_and_non_zero_only_once_in_its_column[non_zero_in_last_row_and_non_zero_only_once_in_its_column.size() - 1]]] = answer_temp;
    }

    if (possibly_no_solution_but_can_be_valid.size() > 0) {
        non_zero_in_last_row_and_non_zero_only_once_in_its_column.push_back(possibly_no_solution_but_can_be_valid[0]);
    }

    // Update the matrix by removing solved columns
    array2.assign(array.size(), std::vector<Fraction>(array[0].size() - non_zero_in_last_row_and_non_zero_only_once_in_its_column.size(), Fraction(0, 1)));
    for (int i = 0; i < array.size(); i++) {
        int k = 0;
        for (int j = 0; j < array[0].size(); j++) {
            int condition1 = std::count(non_zero_in_last_row_and_non_zero_only_once_in_its_column.begin(), non_zero_in_last_row_and_non_zero_only_once_in_its_column.end(), j);
            if (condition1 <= 0) {
                array2[i][k] = array[i][j];
            }
            else {
                k -= 1;
            }
            k++;
        }
    }

    // Update column tracking
    for (int j = 0; j < non_zero_in_last_row_and_non_zero_only_once_in_its_column.size(); j++) {
        int newKey = -1;
        while (keepTrackColumn.find(newKey) != keepTrackColumn.end()) {
            newKey--;
        }
        ModifyKey(keepTrackColumn, non_zero_in_last_row_and_non_zero_only_once_in_its_column[j], newKey);
    }

    for (int j = 0; j < non_zero_in_last_row_and_non_zero_only_once_in_its_column.size(); j++) {
        std::unordered_map<int, int> modifiedkeepTrackColumn;
        for (auto& [Key, Value] : keepTrackColumn) {
            if (Key > non_zero_in_last_row_and_non_zero_only_once_in_its_column[j]) {
                modifiedkeepTrackColumn[Key - 1] = Value;
            }
            else {
                modifiedkeepTrackColumn[Key] = Value;
            }
        }
        keepTrackColumn = modifiedkeepTrackColumn;
    }
}

// Function to process the matrix and solve equations
void ProcessMatrix(GaussEliminationClass& test, std::vector<std::vector<Fraction>>& array,
    std::unordered_map<int, Fraction>& answer, std::unordered_map<int, int>& keepTrackColumn) {
    if (array.empty() || array[0].empty()) {
        throw std::out_of_range("Input matrix is empty or malformed.");
    }
    std::vector<std::vector<Fraction>> array2;

    do {
        if (array[0].size() <= 2) {
            break;
        }

        if (test.invalid_check(array) == "invalid") {
            throw std::runtime_error("Not solvable");
        }

        array2.assign(array.size() - 1, std::vector<Fraction>(array[0].size() - 1, Fraction(0, 1)));

        bool available_fixed_value = false;
        int non_zero_index_main = -1;
        int non_zero_size = 0;
        std::vector<int> non_zero_index;

        for (int j = 0; j < array[0].size() - 1; j++) {
            if (array[array.size() - 1][j] != 0) {
                non_zero_size++;
                non_zero_index.push_back(j);
            }
        }

        if (non_zero_size == 1) {
            available_fixed_value = true;
            non_zero_index_main = non_zero_index[0];
        }

        if (available_fixed_value) {
            Fraction denom = array[array.size() - 1][non_zero_index_main];
            if (denom == 0) {
                throw std::domain_error("Division by zero in ProcessMatrix.");
            }
            Fraction base1 = -1 * array[array.size() - 1][array[0].size() - 1] / denom;
            answer[keepTrackColumn[non_zero_index_main]] = base1;

            int newKey = -1;
            while (keepTrackColumn.find(newKey) != keepTrackColumn.end()) {
                newKey--;
            }
            ModifyKey(keepTrackColumn, non_zero_index_main, newKey);

            std::unordered_map<int, int> modifiedkeepTrackColumn;
            for (auto& [Key, Value] : keepTrackColumn) {
                if (Key > non_zero_index_main) {
                    modifiedkeepTrackColumn[Key - 1] = Value;
                }
                else {
                    modifiedkeepTrackColumn[Key] = Value;
                }
            }
            keepTrackColumn = modifiedkeepTrackColumn;

            for (int i = 0; i < array.size() - 1; i++) {
                for (int j = 0; j < array[0].size() - 1; j++) {
                    if (j == non_zero_index_main) {
                        array2[i][array[0].size() - 2] = array[i][array[0].size() - 1] + base1 * array[i][j];
                    }
                    else if (j > non_zero_index_main) {
                        array2[i][j] = array[i][j + 1];
                    }
                    else {
                        array2[i][j] = array[i][j];
                    }
                }
            }
        }
        else if (array[0].size() - 1 > array.size()) {
            HandleUnderDeterminedSystem(array, answer, keepTrackColumn, array2);
        }
        else if (array[0].size() - 1 < array.size()) {
            throw std::logic_error("Over determined equations");
        }
        else {
            if (non_zero_size == 0 && array[array.size() - 1][array[0].size() - 1] != 0) {
                throw std::runtime_error("Not solvable");
            }
        }

        array = array2;

    } while (array2[0].size() > 2);
}

// Function to handle unsolved variables
void HandleUnsolvedVariables(std::vector<std::vector<Fraction>>& array,
    std::unordered_map<int, int>& keepTrackColumn,
    std::unordered_map<int, Fraction>& answer) {
    std::vector<int> unflagged_keys;
    for (auto& [Key, Value] : keepTrackColumn) {
        if (Key >= 0) {
            unflagged_keys.push_back(Key);
        }
    }

    if (unflagged_keys.size() > 1) {
        throw std::logic_error("More than one variable unsolved at end");
    }

    if (unflagged_keys.size() == 1 && array[0][0] != 0) {
        answer[keepTrackColumn[unflagged_keys[0]]] = -1 * array[0][1] / array[0][0];
    }
}

// Function to adjust answers for preserved columns
void AdjustAnswersForPreservedColumns(const std::vector<int>& preserved_columns,
    const std::vector<int>& zero_columns,
    std::unordered_map<int, int>& keepTrackColumn,
    std::unordered_map<int, Fraction>& answer) {
    std::vector<int> unknown_answers;
    for (int i = preserved_columns.size() - 2; i >= 0; i--) {
        if (i != preserved_columns[i]) {
            ModifyKey(answer, i, preserved_columns[i]);
        }

        if (std::find(preserved_columns.begin(), preserved_columns.end(), i) == preserved_columns.end() &&
            std::find(zero_columns.begin(), zero_columns.end(), i) == zero_columns.end()) {
            unknown_answers.push_back(i);
        }
    }

    for (auto i : zero_columns) {
        std::cout << "x" << i << " is allowed to have any value" << std::endl;
    }

    for (auto i : unknown_answers) {
        std::cout << "x" << i << " is not solvable" << std::endl;
    }
}

std::unordered_map<int, Fraction> Solve(GaussEliminationClass& test, std::vector<std::vector<Fraction>>& array) {
    array = PerformGaussianElimination(test, array);

    if (array.empty()) {
        throw std::logic_error("Every variable is allowed to have any value");
    }

    auto [cleaned_array, preserved_columns, zero_columns] = CleanColumns(test, array);

    auto keepTrackColumn = InitializeColumnTracking(cleaned_array[0].size() - 1);

    std::unordered_map<int, Fraction> answer;

    ProcessMatrix(test, cleaned_array, answer, keepTrackColumn);

    HandleUnsolvedVariables(cleaned_array, keepTrackColumn, answer);

    AdjustAnswersForPreservedColumns(preserved_columns, zero_columns, keepTrackColumn, answer);

    return answer;
}