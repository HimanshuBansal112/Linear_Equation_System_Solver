#pragma once

#ifndef FRACTION_DATA_TYPES_H
#define FRACTION_DATA_TYPES_H

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

#endif