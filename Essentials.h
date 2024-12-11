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

#include <vector>
#include <string>

std::string replaceWord(std::string str, const std::string& substring, const std::string& substring_2);
std::vector<std::string> split(const std::string& str, const std::string& substring);
bool tryParseInt(const std::string& str, int& result);
bool tryParseDouble(const std::string& str, double& result);
#endif