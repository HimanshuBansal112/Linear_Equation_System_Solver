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

class Fraction {
private:
	int Numerator;
	int Denominator;
public:
	bool null;
	Fraction(int numerator, int denominator);
	Fraction() : Numerator(0), Denominator(1) {}
private:
	void Simplify();
	int GCD(int a, int b);

public:
	std::string ToString();
	
	Fraction operator+ (const Fraction& b) const;
	Fraction operator- (const Fraction& b) const;
	Fraction operator* (const Fraction& b) const;
	Fraction operator* (const int& b) const;
	friend Fraction operator*(int b, const Fraction& frac);
	Fraction operator/ (const Fraction& b) const;
	
	int CompareTo(const int& obj) const;
	int CompareTo(const Fraction& other) const;
	
	bool operator== (const Fraction& a) const;
	bool operator== (const int& a) const;
	bool operator!= (const Fraction& a) const;
	bool operator!= (const int& a) const;
	bool operator> (const Fraction& a) const;
	bool operator> (const int& a) const;
	bool operator< (const Fraction& a) const;
	bool operator< (const int& a) const;
	bool operator>= (const Fraction& a) const;
	bool operator>= (const int& a) const;
	bool operator<= (const Fraction& a) const;
	bool operator<= (const int& a) const;

	static int TryParse(std::string input, Fraction& result);
};