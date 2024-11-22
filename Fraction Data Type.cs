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

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
	
		if (obj is int numerator)
		{
			return Numerator == numerator && Denominator == 1;
		}

		Fraction otherFraction = (Fraction)obj;
		return Numerator == otherFraction.Numerator && Denominator == otherFraction.Denominator;
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
	public int CompareTo(object obj)
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
	public int CompareTo(Fraction other)
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

	public static int TryParse(string input, out Fraction result)
	{
		result = null;
		
		if (string.IsNullOrWhiteSpace(input))
		{
			return 0;
		}

		input = input.Replace("\\", "/");

		int count = input.ToCharArray().Count(c => c == '/');

		if (count > 1)
		{
			return 0;
		}

		if (input[input.Length - 1] == '/' || input[0] == '/')
		{
			return 0;
		}

		string[] parts = input.Split('/');
		if (parts.Length == 1)
		{
			int numerator;
			int denominator;

			double numerator1 = 1;
			double denominator1 = 1;
			bool success = double.TryParse(parts[0], out numerator1);

			if (success)
			{
				while (Math.Round(numerator1 / 10) == numerator1 / 10 && Math.Round(denominator1 / 10) == denominator1 / 10)
				{
					numerator1 /= 10;
					denominator1 /= 10;
				}

				if (numerator1 > 2147483640 || denominator1 > 2147483640)
				{
					return -1;
				}
				while (Math.Floor(numerator1) != numerator1 && numerator1 < 2147483640 && denominator1 < 2147483640)
				{
					numerator1 *= 10;
					denominator1 *= 10;
				}

				if (numerator1 > 2147483640 || denominator1 > 2147483640)
				{
					numerator1 /= 10;
					denominator1 /= 10;
				}

				numerator = Convert.ToInt32(numerator1);
				denominator = Convert.ToInt32(denominator1);

				if (denominator == 0)
				{
					return -2;
				}

				result = new Fraction(numerator, denominator);
			}
			int a = success ? 1 : 0;
			return a;
		}
		else if (parts.Length == 2)
		{
			int numerator;
			int denominator;
			if (!int.TryParse(parts[0], out numerator) || !int.TryParse(parts[1], out denominator))
			{
				double numerator1 = 1;
				double denominator1 = 1;
				if (!double.TryParse(parts[0], out numerator1) || !double.TryParse(parts[1], out denominator1))
				{
					return 0;
				}

				while (Math.Round(numerator1 / 10) == numerator1 / 10 && Math.Round(denominator1 / 10) == denominator1 / 10)
				{
					numerator1 /= 10;
					denominator1 /= 10;
				}

				if (numerator1 > 2147483640 || denominator1 > 2147483640)
				{
					return -1;
				}

				numerator = (int)numerator1;
				denominator = (int)denominator1;

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
