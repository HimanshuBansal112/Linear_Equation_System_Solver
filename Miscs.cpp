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

#include "Miscs.h"

std::vector<int> GetUncommonElements(std::vector<int> list1, std::vector<int> list2)
{
	std::vector<int> result;

	for (const int& element : list2)
	{
		if ((std::find(list1.begin(), list1.end(), element) == list1.end()) && (std::find(result.begin(), result.end(), element) == result.end()))
		{
			result.push_back(element);
		}
	}

	return result;
}