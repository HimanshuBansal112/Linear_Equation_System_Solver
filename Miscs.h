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