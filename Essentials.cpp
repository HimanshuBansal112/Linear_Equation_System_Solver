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
#include<string>
#include <vector>
#include <sstream>

#include "Essentials.h"

std::string replaceWord(std::string str, const std::string& substring, const std::string& substring_2)
{
    std::string::size_type start = 0;
    while ((start = str.find(substring, start)) != std::string::npos)
    {
        str.replace(start, substring.length(), substring_2);
        start += substring_2.length();
    }
    return str;
}


std::vector<std::string> split(const std::string& str, const std::string& substring)
{
    std::vector<std::string> result;
    std::string::size_type start = 0, end = str.find(substring);
    while (end != std::string::npos)
    {
        std::string temp = str.substr(start, end - start);
        if (!temp.empty())
            result.push_back(temp);
        start = end + substring.length();
        end = str.find(substring, start);
    }
    std::string temp = str.substr(start);
    if (!temp.empty())
        result.push_back(temp);
    return result;
}

bool tryParseInt(const std::string& str, int& result) {
    std::istringstream iss(str);
    iss >> result;

    // Tells if the conversion succeeded
    return !iss.fail() && iss.eof();
}

bool tryParseDouble(const std::string& str, double& result) {
    std::istringstream iss(str);
    iss >> result;

    // Tells if the conversion succeeded
    return !iss.fail() && iss.eof();
}
