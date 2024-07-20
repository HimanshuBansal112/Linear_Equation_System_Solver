#pragma once

#include <vector>
#include <string>

#ifndef ESSENTIALS_H
#define ESSENTIALS_H

std::string replaceWord(std::string str, const std::string& substring, const std::string& substring_2);
std::vector<std::string> split(const std::string& str, const std::string& substring);
bool tryParseInt(const std::string& str, int& result);
bool tryParseDouble(const std::string& str, double& result);
#endif