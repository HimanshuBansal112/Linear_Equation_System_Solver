#include <iostream>
#include <vector>
#include <unordered_map>

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