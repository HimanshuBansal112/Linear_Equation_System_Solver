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
#include <unordered_map>

#ifndef MISCS_H
#define MISCS_H

template<typename TKey, typename TValue>
void ModifyKey(std::unordered_map<TKey, TValue>& dictionary, TKey oldKey, TKey newKey)
{
    auto it = dictionary.find(oldKey);
    if (it == dictionary.end()) {
        throw std::invalid_argument("Old key is not in the dictionary.");
    }
    if (dictionary.find(newKey) != dictionary.end()) {
        throw std::invalid_argument("New key is already present in the dictionary.");
    }

    dictionary[newKey] = std::move(it->second);
    dictionary.erase(it);
}

std::vector<int> GetUncommonElements(std::vector<int> list1, std::vector<int> list2);

#endif