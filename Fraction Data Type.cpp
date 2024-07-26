#include <iostream>
#include <string>

#include "Essentials.h"
#include "Fraction Data Type.h"
#include <vector>

Fraction::Fraction(int numerator, int denominator)
{
	null = false;
	if (denominator == 0)
	{
		throw std::invalid_argument("Denominator cannot be zero.");
	}

	Numerator = numerator;
	Denominator = denominator;
	Simplify();
}

void Fraction::Simplify()
{
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
	return std::to_string(Numerator)+"/"+std::to_string(Denominator);
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
		throw std::invalid_argument("Cannot divide by zero.");
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

int Fraction::TryParse(std::string input, Fraction& result)
{
	result.null = true;

	if (replaceWord(input, " ", "") == "")
	{
		return false;
	}

	input = replaceWord(input, "\\", "/");

	int count = 0;
	for (char ch : input) {
		if (ch == '/') {
			count++;
		}
	}

	if (count > 1) {
		return false;
	}

	if (input[input.size() - 1] == '/' || input[0]=='/') {
		return false;
	}

	std::vector<std::string> parts = split(input, "/");

	int numerator;
	int denominator;
	if (parts.size() == 1)
	{
		double numerator1 = 1;
		double denominator1 = 1;
		bool success = tryParseDouble(parts[0], numerator1);

		if (success) {
			while (std::round(numerator1 / 10) == numerator1 / 10 && std::round(denominator1 / 10) == denominator1 / 10) {
				numerator1 /= 10;
				denominator1 /= 10;
			}

			if (numerator1 > 2147483640 || denominator1 > 2147483640)
			{
				return -1;
			}
			while (floor(numerator1) != numerator1 && numerator1 < 2147483640 && denominator1 < 2147483640)
			{
				numerator1 *= 10;
				denominator1 *= 10;
			}

			if (numerator1 > 2147483640 || denominator1 > 2147483640)
			{
				numerator1 /= 10;
				denominator1 /= 10;
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
		}
		return success;
	}
	else if (parts.size() == 2)
	{
		if (!tryParseInt(parts[0], numerator) || !tryParseInt(parts[1], denominator)) {
			double numerator1 = 1;
			double denominator1 = 1;
			if (!tryParseDouble(parts[0], numerator1) || !tryParseDouble(parts[1], denominator1))
			{
				return false;
			}
			
			while (std::round(numerator1 / 10) == numerator1 / 10 && std::round(denominator1 / 10) == denominator1 / 10) {
				numerator1 /= 10;
				denominator1 /= 10;
			}

			if (numerator1 > 2147483640 || denominator1 > 2147483640)
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
	return true;
}