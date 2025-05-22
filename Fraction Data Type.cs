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
using System;

public class Fraction : IComparable, IComparable<Fraction>
{
	public int Numerator { get; private set; }
	public int Denominator { get; private set; }

	public Fraction(int numerator, int denominator)
	{
		if (denominator == 0)
		{
			throw new ArgumentException("Denominator cannot be zero.");
		}

		Numerator = numerator;
		Denominator = denominator;
		Simplify();
	}

    // Constructor using double type
    public Fraction(double number)
    {
        // Use TryParse to convert double to fraction
        if (TryParse(number.ToString(System.Globalization.CultureInfo.InvariantCulture), out var result) == 1)
        {
            Numerator = result.Numerator;
            Denominator = result.Denominator;
        }
        else
        {
            throw new ArgumentException("Invalid double value for Fraction.");
        }
    }

    // Constructor using string type
    public Fraction(string number)
    {
        if (TryParse(number, out var result) == 1)
        {
            Numerator = result.Numerator;
            Denominator = result.Denominator;
        }
        else
        {
            throw new ArgumentException("Invalid string value for Fraction.");
        }
    }

	private void Simplify()
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

	private int GCD(int a, int b)
	{
		while (b != 0)
		{
			int temp = b;
			b = a % b;
			a = temp;
		}
		return a;
	}

	public override string ToString()
	{
		return $"{Numerator}/{Denominator}";
	}

	public override bool Equals(object? obj)
	{
		if (obj == null)
		{
			return false;
		}
	
		if (obj is int numerator)
		{
			return Numerator == numerator && Denominator == 1;
		}

		if (obj is Fraction otherFraction)
		{
			return Numerator == otherFraction.Numerator && Denominator == otherFraction.Denominator;
		}
		return false;
	}

	public override int GetHashCode()
	{
		unchecked
		{
			int hash = 17;
			hash = hash * 23 + Numerator.GetHashCode();
			hash = hash * 23 + Denominator.GetHashCode();
			return hash;
		}
	}

	// Addition
	public static Fraction operator +(Fraction a, Fraction b)
	{
		int numerator = a.Numerator * b.Denominator + b.Numerator * a.Denominator;
		int denominator = a.Denominator * b.Denominator;
		return new Fraction(numerator, denominator);
	}

	// Subtraction
	public static Fraction operator -(Fraction a, Fraction b)
	{
		int numerator = a.Numerator * b.Denominator - b.Numerator * a.Denominator;
		int denominator = a.Denominator * b.Denominator;
		return new Fraction(numerator, denominator);
	}

	// Multiplication
	public static Fraction operator *(Fraction a, Fraction b)
	{
		int numerator = a.Numerator * b.Numerator;
		int denominator = a.Denominator * b.Denominator;
		return new Fraction(numerator, denominator);
	}

	public static Fraction operator *(int a, Fraction b)
	{
		int numerator = a * b.Numerator;
		int denominator = b.Denominator;
		return new Fraction(numerator, denominator);
	}

	// Division
	public static Fraction operator /(Fraction a, Fraction b)
	{
		if (b.Numerator == 0)
		{
			throw new DivideByZeroException("Cannot divide by zero.");
		}

		int numerator = a.Numerator * b.Denominator;
		int denominator = a.Denominator * b.Numerator;
		return new Fraction(numerator, denominator);
	}


	//comparison
	public int CompareTo(object? obj)
	{
		if (obj == null) return 1;

		if (obj is Fraction otherFraction)
		{
			return CompareTo(otherFraction);
		}
		else if (obj is int intValue)
		{
			return CompareTo(new Fraction(intValue, 1)); // Convert int to Fraction for comparison
		}
		else
		{
			throw new ArgumentException("Object is not a Fraction or int.");
		}
	}

	// Implement IComparable<Fraction> interface
	public int CompareTo(Fraction? other)
	{
		if (other == null) return 1;

		// Compare fractions based on their double values
		double thisValue = (double)Numerator / Denominator;
		double otherValue = (double)other.Numerator / other.Denominator;

		return thisValue.CompareTo(otherValue);
	}

	public static bool operator ==(Fraction a, int b)
	{
		return a.CompareTo(b) == 0;
	}

	public static bool operator !=(Fraction a, int b)
	{
		return a.CompareTo(b) != 0;
	}

	public static bool operator >(Fraction a, int b)
	{
		return a.CompareTo(b) > 0;
	}

	public static bool operator <(Fraction a, int b)
	{
		return a.CompareTo(b) < 0;
	}

	public static bool operator >=(Fraction a, int b)
	{
		return a.CompareTo(b) >= 0;
	}

	public static bool operator <=(Fraction a, int b)
	{
		return a.CompareTo(b) <= 0;
	}

	public static (long numerator, long denominator) DecimalToFraction(double value, double tolerance = 1.0E-10)
	{
		if (double.IsNaN(value) || double.IsInfinity(value))
			throw new ArgumentException("Value must be a finite number.");

		long numerator = 1, denominator = 0;
		long prevNumerator = 0, prevDenominator = 1;
		double remainder = value;

		while (true)
		{
			long integralPart = (long)Math.Floor(remainder);
			long tempNumerator = numerator;
			long tempDenominator = denominator;

			numerator = integralPart * numerator + prevNumerator;
			denominator = integralPart * denominator + prevDenominator;

			prevNumerator = tempNumerator;
			prevDenominator = tempDenominator;

			double approx = (double)numerator / denominator;
			if (Math.Abs(value - approx) < tolerance)
				break;

			remainder = 1.0 / (remainder - integralPart);
		}

		return (numerator, denominator);
	}

	public static int TryParse(string input, out Fraction result)
	{
		result = new Fraction(0, 1);
		if (string.IsNullOrWhiteSpace(input))
		{
			return 0;
		}

		input = input.Replace("\\", "/");
		int count = input.Count(c => c == '/');
		if (count > 1)
		{
			return 0;
		}
		if (input[^1] == '/' || input[0] == '/')
		{
			return 0;
		}
		string[] parts = input.Split('/');
		if (parts.Length == 1)
		{
			double numerator1;
			double denominator1 = 1;
			bool success = double.TryParse(parts[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out numerator1);
			if (success)
			{
				// Convert decimal to fraction
				int decimals = 0;
				string[] splitNum = parts[0].Split('.');
				if (splitNum.Length == 2)
				{
					decimals = splitNum[1].Length;
				}
				if (decimals > 3)
				{
					double tolerance = Math.Pow(10, -decimals);
					(numerator1, denominator1) = DecimalToFraction(numerator1, tolerance);
				}
				else
				{
					(numerator1, denominator1) = DecimalToFraction(numerator1);
				}
				if (Math.Abs(numerator1) > 2147483640 || Math.Abs(denominator1) > 2147483640)
				{
					return -1;
				}
				int numerator = (int)Math.Round(numerator1);
				int denominator = (int)Math.Round(denominator1);
				if (denominator == 0)
				{
					return -2;
				}
				result = new Fraction(numerator, denominator);
				return 1;
			}
			return 0;
		}
		else if (parts.Length == 2)
		{
			int numerator, denominator;
			if (!int.TryParse(parts[0], out numerator) || !int.TryParse(parts[1], out denominator))
			{
				double numerator1, denominator1;
				if (!double.TryParse(parts[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out numerator1) ||
					!double.TryParse(parts[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out denominator1))
				{
					return 0;
				}
				while (Math.Abs(numerator1 % 10) < double.Epsilon && Math.Abs(denominator1 % 10) < double.Epsilon && numerator1 != 0)
				{
					numerator1 /= 10;
					denominator1 /= 10;
				}
				if (Math.Abs(numerator1) > 2147483640 || Math.Abs(denominator1) > 2147483640)
				{
					return -1;
				}
				numerator = (int)Math.Round(numerator1);
				denominator = (int)Math.Round(denominator1);
				if (denominator == 0)
				{
					return -2;
				}
				result = new Fraction(numerator, denominator);
				return 1;
			}
			else if (denominator == 0)
			{
				return -2;
			}
			result = new Fraction(numerator, denominator);
			return 1;
		}
		return 0;
	}
}
