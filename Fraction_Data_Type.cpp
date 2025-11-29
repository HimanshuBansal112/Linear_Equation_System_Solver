/*
 * Copyright 2024-2025 Himanshu Bansal
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific
 * language governing permissions and limitations under the License.
 * ==============================================================================
 */
#include <iostream>
#include <string>
#include <cmath>
#include <limits>
#include <cmath>

#include "Essentials.h"
#include "Fraction_Data_Type.h"
#include <vector>

Fraction::Fraction(int numerator, int denominator)
{
	null = false;
	if (denominator == 0)
	{
		throw std::domain_error("Denominator cannot be zero.");
	}

	Numerator = numerator;
	Denominator = denominator;
	Simplify();
}

Fraction::Fraction(double number)
{
	if (!Fraction::TryParse(std::to_string(number), *this)) {
		throw std::invalid_argument("Error happened");
	}
}

Fraction::Fraction(std::string number)
{
	if (!Fraction::TryParse(number, *this)) {
		throw std::invalid_argument("Error happened");
	}
}

void Fraction::Simplify()
{
    if (Denominator == 0) {
        throw std::domain_error("Denominator cannot be zero in Simplify.");
    }
    int gcd = GCD(Numerator, Denominator);
    Numerator /= gcd;
    Denominator /= gcd;

    if (Denominator < 0)
    {
        Numerator = -Numerator;
        Denominator = -Denominator;
    }
}

int Fraction::GCD(int a, int b)
{
	while (b != 0)
	{
		int temp = b;
		b = a % b;
		a = temp;
	}
	return a;
}

std::string Fraction::ToString()
{
	return std::to_string(Numerator) + "/" + std::to_string(Denominator);
}

// Addition
Fraction Fraction::operator+ (const Fraction& b) const
{
	int numerator = Numerator * b.Denominator + b.Numerator * Denominator;
	int denominator = Denominator * b.Denominator;
	return Fraction(numerator, denominator);
}

// Subtraction
Fraction Fraction::operator- (const Fraction& b) const
{
	int numerator = Numerator * b.Denominator - b.Numerator * Denominator;
	int denominator = Denominator * b.Denominator;
	return Fraction(numerator, denominator);
}

// Multiplication
Fraction Fraction::operator* (const Fraction& b) const
{
	int numerator = Numerator * b.Numerator;
	int denominator = Denominator * b.Denominator;
	return Fraction(numerator, denominator);
}

Fraction Fraction::operator* (const int& b) const
{
	int numerator = Numerator * b;
	int denominator = Denominator;
	return Fraction(numerator, denominator);
}

Fraction operator*(int b, const Fraction& frac) {
	int numerator = frac.Numerator * b;
	int denominator = frac.Denominator;
	return Fraction(numerator, denominator);
}
// Division
Fraction Fraction::operator/ (const Fraction& b) const
{
	if (b.Numerator == 0)
	{
		throw std::domain_error("Cannot divide by zero.");
	}
	int numerator = Numerator * b.Denominator;
	int denominator = Denominator * b.Numerator;
	return Fraction(numerator, denominator);
}


//comparison
int Fraction::CompareTo(const int& obj) const
{
	return CompareTo(Fraction(obj, 1)); // Convert int to Fraction for comparison
}

int Fraction::CompareTo(const Fraction& other) const
{
	if (other.null == true) return 1;

	// Compare fractions based on their double values
	double thisValue = (double)Numerator / Denominator;
	double otherValue = (double)other.Numerator / other.Denominator;

	if (thisValue < otherValue) return -1;
	if (thisValue > otherValue) return 1;
	return 0;
}

bool Fraction::operator== (const Fraction& a) const
{
	return CompareTo(a) == 0;
}

bool Fraction::operator== (const int& a) const
{
	return CompareTo(a) == 0;
}

bool Fraction::operator!= (const Fraction& a) const
{
	return CompareTo(a) != 0;
}

bool Fraction::operator!= (const int& a) const
{
	return CompareTo(a) != 0;
}

bool Fraction::operator> (const Fraction& a) const
{
	return CompareTo(a) > 0;
}

bool Fraction::operator> (const int& a) const
{
	return CompareTo(a) > 0;
}

bool Fraction::operator< (const Fraction& a) const
{
	return CompareTo(a) < 0;
}

bool Fraction::operator< (const int& a) const
{
	return CompareTo(a) < 0;
}

bool Fraction::operator>= (const Fraction& a) const
{
	return CompareTo(a) >= 0;
}

bool Fraction::operator>= (const int& a) const
{
	return CompareTo(a) >= 0;
}

bool Fraction::operator<= (const Fraction& a) const
{
	return CompareTo(a) <= 0;
}

bool Fraction::operator<= (const int& a) const
{
	return CompareTo(a) <= 0;
}

std::tuple<long, long> Fraction::DecimalToFraction(double value, double tolerance = 1.0E-10) {
	if (std::isnan(value) || std::isinf(value)) {
        throw std::invalid_argument("Value must be a finite number.");
    }

	long numerator = 1, denominator = 0;
    long prevNumerator = 0, prevDenominator = 1;
    double remainder = value;

    while (true)
    {
        long integralPart = (long)std::floor(remainder);
        long tempNumerator = numerator;
        long tempDenominator = denominator;

        numerator = integralPart * numerator + prevNumerator;
        denominator = integralPart * denominator + prevDenominator;

        prevNumerator = tempNumerator;
        prevDenominator = tempDenominator;

        double approx = (double)numerator / denominator;
        if (std::abs(value - approx) < tolerance)
            break;

        remainder = 1.0 / (remainder - integralPart);
    }

    return std::tuple<long, long>(numerator, denominator);
}

int Fraction::TryParse(std::string input, Fraction& result)
{
    result.null = true;

    if (input.empty() || replaceWord(input, " ", "") == "")
    {
        return 0;
    }

    input = replaceWord(input, "\\", "/");

    int count = 0;
    for (char ch : input) {
        if (ch == '/') {
            count++;
        }
    }

    if (count > 1) {
        return 0;
    }

    if (input[0] == '/' || input[input.size() - 1] == '/') {
        return 0;
    }

    std::vector<std::string> parts = split(input, "/");

    int numerator = 0;
    int denominator = 0;
    if (parts.size() == 1)
    {
        double numerator1 = 1;
        double denominator1 = 1;
        bool success = tryParseDouble(parts[0], numerator1);

        if (success) {
            int decimals = 0;
			std::vector<std::string> decimal_parts = split(parts[0], ".");
            if (decimal_parts.size() == 2) {
                decimals = decimal_parts[1].length();
			}
            if (decimals > 3) {
				double tolerance = std::pow(10, -decimals);
                std::tie(numerator1, denominator1) = DecimalToFraction(numerator1, tolerance);
            }
            else {
                std::tie(numerator1, denominator1) = DecimalToFraction(numerator1);
            }
            if (std::abs(numerator1) > std::numeric_limits<int>::max() || std::abs(denominator1) > std::numeric_limits<int>::max())
            {
                return -1;
            }
            result.null = false;
            int numerator = (int)std::round(numerator1);
            int denominator = (int)std::round(denominator1);
            if (denominator == 0)
            {
                return -2;
            }
            result = Fraction(numerator, denominator);
            return 1;
        }
        return 0;
    }
    else if (parts.size() == 2)
    {
        if (!tryParseInt(parts[0], numerator) || !tryParseInt(parts[1], denominator)) {
            double numerator1 = 1;
            double denominator1 = 1;
            if (!tryParseDouble(parts[0], numerator1) || !tryParseDouble(parts[1], denominator1))
            {
                return 0;
            }

            while (std::round(numerator1 / 10) == numerator1 / 10 && std::round(denominator1 / 10) == denominator1 / 10) {
                numerator1 /= 10;
                denominator1 /= 10;
            }

			if (numerator1 > std::numeric_limits<int>::max() || denominator1 > std::numeric_limits<int>::max())
            {
                return -1;
            }

            numerator = (int)numerator1;
            denominator = (int)denominator1;

            result.null = false;
            if (denominator == 0)
            {
                return -2;
            }

            result.Numerator = numerator;
            result.Denominator = denominator;
            result.Simplify();
            return 1;
        }
        else if (denominator == 0) {
            return -2;
        }
        result.Numerator = numerator;
        result.Denominator = denominator;
        result.Simplify();
        return 1;
    }

    result.null = false;
    if (denominator == 0)
    {
        return -2;
    }

    result.Numerator = numerator;
    result.Denominator = denominator;
    result.Simplify();
    return 1;
}