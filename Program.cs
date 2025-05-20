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

static Fraction[,] TakeInputMatrix(GaussEliminationClass test)
{
    return test.TakeInput();
}

static Fraction[,] PerformGaussianElimination(GaussEliminationClass test, Fraction[,] array)
{
    if (array.GetLength(0) == 0 || array.GetLength(1) == 0)
        throw new Exception("Input matrix is empty or malformed.");
    return test.EndResult(array);
}

static (Fraction[,], List<int>, List<int>) CleanColumns(GaussEliminationClass test, Fraction[,] array)
{
    return test.clean_coloumn(array);
}

static Dictionary<int, int> InitializeColumnTracking(int columnCount)
{
    var keepTrackColumn = new Dictionary<int, int>();
    for (int j = 0; j < columnCount; j++)
        keepTrackColumn[j] = j;
    return keepTrackColumn;
}

static void HandleUnderDeterminedSystem(ref Fraction[,] array, Dictionary<int, Fraction> answer, Dictionary<int, int> keepTrackColumn, out Fraction[,] array2)
{
    var localArray = array;
    var non_zero_index = Enumerable.Range(0, localArray.GetLength(1) - 1)
        .Where(j => localArray[localArray.GetLength(0) - 1, j] != 0)
        .ToList();
    int rowCount = localArray.GetLength(0);
    int colCount = localArray.GetLength(1);
    var non_zero_in_last_row_and_non_zero_only_once_in_its_column = non_zero_index
        .Where(j => Enumerable.Range(0, rowCount - 1).All(i => localArray[i, j] == 0))
        .ToList();
    Fraction change_in_value = new Fraction(0, 1);
    for (int j = 0; j < non_zero_in_last_row_and_non_zero_only_once_in_its_column.Count - 1; j++)
    {
        change_in_value -= localArray[rowCount - 1, non_zero_in_last_row_and_non_zero_only_once_in_its_column[j]];
        answer[keepTrackColumn[non_zero_in_last_row_and_non_zero_only_once_in_its_column[j]]] = new Fraction(1, 1);
    }
    var possibly_no_solution_but_can_be_valid = Miscs.GetUncommonElements(non_zero_in_last_row_and_non_zero_only_once_in_its_column, non_zero_index);
    Fraction constant_rhs = change_in_value - localArray[rowCount - 1, colCount - 1];
    if (possibly_no_solution_but_can_be_valid.Any())
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
    if (non_zero_in_last_row_and_non_zero_only_once_in_its_column.Any())
    {
        Fraction answer_temp = constant_rhs / localArray[rowCount - 1, non_zero_in_last_row_and_non_zero_only_once_in_its_column[^1]];
        answer[keepTrackColumn[non_zero_in_last_row_and_non_zero_only_once_in_its_column[^1]]] = answer_temp;
    }
    if (possibly_no_solution_but_can_be_valid.Count > 0)
        non_zero_in_last_row_and_non_zero_only_once_in_its_column.Add(possibly_no_solution_but_can_be_valid[0]);
    array2 = new Fraction[rowCount, colCount - non_zero_in_last_row_and_non_zero_only_once_in_its_column.Count];
    for (int i = 0; i < rowCount; i++)
    {
        int k = 0;
        for (int j = 0; j < colCount; j++)
        {
            if (!non_zero_in_last_row_and_non_zero_only_once_in_its_column.Contains(j))
                array2[i, k] = localArray[i, j];
            else
                k -= 1;
            k++;
        }
    }
    foreach (var idx in non_zero_in_last_row_and_non_zero_only_once_in_its_column)
        Miscs.ModifyKey(keepTrackColumn, idx, -1);
    foreach (var idx in non_zero_in_last_row_and_non_zero_only_once_in_its_column)
    {
        var modifiedkeepTrackColumn = keepTrackColumn.ToDictionary(item => item.Key > idx ? item.Key - 1 : item.Key, item => item.Value);
        foreach (var kv in modifiedkeepTrackColumn)
            keepTrackColumn[kv.Key] = kv.Value;
    }
    array = localArray;
}

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

static void HandleUnsolvedVariables(Fraction[,] array, Dictionary<int, int> keepTrackColumn, Dictionary<int, Fraction> answer)
{
    var unflagged_keys = keepTrackColumn.Keys.Where(k => k != -1).ToList();
    if (unflagged_keys.Count > 1)
        throw new InvalidOperationException("More than one variable unsolved at end");
    if (unflagged_keys.Count == 1 && array[0, 0] != 0)
        answer[keepTrackColumn[unflagged_keys[0]]] = -1 * array[0, 1] / array[0, 0];
}

static void AdjustAnswersForPreservedColumns(List<int> preserved_columns, List<int> zero_columns, Dictionary<int, Fraction> answer)
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

static Dictionary<int, Fraction> Solve(GaussEliminationClass test, Fraction[,] array)
{
    array = PerformGaussianElimination(test, array);
    if (array.GetLength(0) == 0)
        throw new Exception("Every variable is allowed to have any value");
    var (cleaned_array, preserved_columns, zero_columns) = CleanColumns(test, array);
    var keepTrackColumn = InitializeColumnTracking(cleaned_array.GetLength(1) - 1);
    var answer = new Dictionary<int, Fraction>();
    ProcessMatrix(test, ref cleaned_array, answer, keepTrackColumn);
    HandleUnsolvedVariables(cleaned_array, keepTrackColumn, answer);
    AdjustAnswersForPreservedColumns(preserved_columns, zero_columns, answer);
    return answer;
}

GaussEliminationClass test = new GaussEliminationClass();
Fraction[,] array = TakeInputMatrix(test);
try
{
    Solve(test, array);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
Console.ReadLine();