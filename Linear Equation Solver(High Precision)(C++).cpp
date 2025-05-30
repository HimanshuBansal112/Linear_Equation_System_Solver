/*
 * Copyright 2024-2025 Himanshu Bansal
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
#include <unordered_map>
#include <limits>

#include "main.h"

// Main function
int main() {
    GaussEliminationClass test;

    auto array = TakeInputMatrix(test);

    std::unordered_map<int, Fraction> answer = Solve(test, array);

    for (auto& [Key, Value] : answer) {
        std::cout << "x" << Key << " = " << Value.ToString() << std::endl;
    }

    std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
    std::cin.get();

    return 0;
}