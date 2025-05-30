/*
 * Copyright 2024-2025 Himanshu Bansal
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
using Linear_Equation_Solver_High_Precision_;
using Xunit;

namespace Test
{
	public class Solve
	{
		[Fact]
		public void Zero_Coefficient_Equation()
		{
			GaussEliminationClass test = new GaussEliminationClass();
			Fraction[,] array = new Fraction[2, 3]
			{
				{ new Fraction(0, 1), new Fraction(1, 1), new Fraction(-3, 1) },
				{ new Fraction(1, 1), new Fraction(0, 1), new Fraction(-2, 1) }
			};
			var (preserved_columns, zero_columns, answer) = Main.Solve(test, array);
			
			Assert.True(answer.ContainsKey(0));
			Assert.True(answer.ContainsKey(1));

			Assert.Equal(new Fraction(2, 1), answer[0]);
			Assert.Equal(new Fraction(3, 1), answer[1]);
		}

		[Fact]
		public void Under_determined()
		{
			var test = new GaussEliminationClass();
			Fraction[,] array = new Fraction[2, 4]
			{
				{ new Fraction(2,1), new Fraction(0,1), new Fraction(-2,1), new Fraction(0,1) },
				{ new Fraction(0,1), new Fraction(2,1), new Fraction(-1,1), new Fraction(0,1) }
			};

			var (preserved_columns, zero_columns, answer) = Main.Solve(test, array);

			Assert.True(answer.ContainsKey(0));
			Assert.True(answer.ContainsKey(1));
			Assert.True(answer.ContainsKey(2));

			Assert.Equal(new Fraction(1, 1), answer[0]);
			Assert.Equal(new Fraction(1, 2), answer[1]);
			Assert.Equal(new Fraction(1, 1), answer[2]);
		}

		[Fact]
		public void Under_determined_2()
		{
			var test = new GaussEliminationClass();
			Fraction[,] array = new Fraction[4, 5]
			{
				{ new Fraction("1/1"), new Fraction("0/1"), new Fraction("-3/1"), new Fraction("0/1"), new Fraction("0/1") },
				{ new Fraction("2/1"), new Fraction("4/1"), new Fraction("-8/1"), new Fraction("-1/1"), new Fraction("0/1") },
				{ new Fraction("2/1"), new Fraction("3/1"), new Fraction("0/1"), new Fraction("-2/1"), new Fraction("0/1") },
				{ new Fraction("0/1"), new Fraction("1/1"), new Fraction("-2/1"), new Fraction("0/1"), new Fraction("0/1") }
			};

			var (preserved_columns, zero_columns, answer) = Main.Solve(test, array);

			Assert.True(answer.ContainsKey(0));
			Assert.True(answer.ContainsKey(1));
			Assert.True(answer.ContainsKey(2));
			Assert.True(answer.ContainsKey(3));

			Assert.Equal(new Fraction(3, 1), answer[0]);
			Assert.Equal(new Fraction(2, 1), answer[1]);
			Assert.Equal(new Fraction(1, 1), answer[2]);
			Assert.Equal(new Fraction(6, 1), answer[3]);
		}

		[Fact]
		public void Under_determined_3()
		{
			var test = new GaussEliminationClass();
			Fraction[,] array = new Fraction[5, 5]
			{
				{ new Fraction("1/1"), new Fraction("0/1"), new Fraction("-3/1"), new Fraction("0/1"), new Fraction("0/1") },
				{ new Fraction("2/1"), new Fraction("0/1"), new Fraction("0/1"), new Fraction("-1/1"), new Fraction("0/1") },
				{ new Fraction("0/1"), new Fraction("3/1"), new Fraction("0/1"), new Fraction("-1/1"), new Fraction("0/1") },
				{ new Fraction("0/1"), new Fraction("1/1"), new Fraction("-2/1"), new Fraction("0/1"), new Fraction("0/1") },
				{ new Fraction("0/1"), new Fraction("4/1"), new Fraction("-8/1"), new Fraction("0/1"), new Fraction("0/1") }
			};

			var (preserved_columns, zero_columns, answer) = Main.Solve(test, array);

			Assert.True(answer.ContainsKey(0));
			Assert.True(answer.ContainsKey(1));
			Assert.True(answer.ContainsKey(2));
			Assert.True(answer.ContainsKey(3));

			Assert.Equal(new Fraction(3, 1), answer[0]);
			Assert.Equal(new Fraction(2, 1), answer[1]);
			Assert.Equal(new Fraction(1, 1), answer[2]);
			Assert.Equal(new Fraction(6, 1), answer[3]);
		}

		[Fact]
		public void Complex_Equation()
		{
			var test = new GaussEliminationClass();
			Fraction[,] array = new Fraction[5, 6]
			{
				{ new Fraction("1/1"), new Fraction("1/2"), new Fraction("1/3"), new Fraction("1/4"), new Fraction("1/5"), new Fraction("-5/1") },
				{ new Fraction("1/2"), new Fraction("1/3"), new Fraction("1/4"), new Fraction("1/5"), new Fraction("1/6"), new Fraction("-71/20") },
				{ new Fraction("1/3"), new Fraction("1/4"), new Fraction("1/5"), new Fraction("1/6"), new Fraction("1/7"), new Fraction("-197/70") },
				{ new Fraction("1/4"), new Fraction("1/5"), new Fraction("1/6"), new Fraction("1/7"), new Fraction("1/8"), new Fraction("-657/280") },
				{ new Fraction("1/5"), new Fraction("1/6"), new Fraction("1/7"), new Fraction("1/8"), new Fraction("1/9"), new Fraction("-1271/630") }
			};

			var (preserved_columns, zero_columns, answer) = Main.Solve(test, array);

			Assert.True(answer.ContainsKey(0));
			Assert.True(answer.ContainsKey(1));
			Assert.True(answer.ContainsKey(2));
			Assert.True(answer.ContainsKey(3));
			Assert.True(answer.ContainsKey(4));

			Assert.Equal(new Fraction(1, 1), answer[0]);
			Assert.Equal(new Fraction(2, 1), answer[1]);
			Assert.Equal(new Fraction(3, 1), answer[2]);
			Assert.Equal(new Fraction(4, 1), answer[3]);
			Assert.Equal(new Fraction(5, 1), answer[4]);
		}
	}
	public class Fraction_Check
	{
		[Fact]
		public void HighPrecision()
		{
			Fraction f;

			Assert.Equal(1, Fraction.TryParse("0.16667", out f));
			Assert.Equal("1/6", f.ToString());

			Assert.Equal(1, Fraction.TryParse("0.501", out f));
			Assert.Equal("501/1000", f.ToString());

			Assert.Equal(1, Fraction.TryParse("0.33333", out f));
			Assert.Equal("1/3", f.ToString());

			Assert.Equal(1, Fraction.TryParse("-4697/420", out f));
			Assert.Equal("-671/60", f.ToString());

			Assert.Equal(1, Fraction.TryParse("-20759/2520", out f));
			Assert.Equal("-20759/2520", f.ToString());

			Assert.Equal(1, Fraction.TryParse("-14417/1680", out f));
			Assert.Equal("-14417/1680", f.ToString());

			Assert.Equal(1, Fraction.TryParse("-13361/1620", out f));
			Assert.Equal("-13361/1620", f.ToString());
		}

		[Fact]
		public void TryParse_NegativeCases()
		{
			Fraction f;

			// Invalid input
			Assert.Equal(0, Fraction.TryParse("abc", out f));
			// Zero denominator
			Assert.Equal(-2, Fraction.TryParse("1/0", out f));
			// Plain Large number (which should be accepted)
			Assert.Equal(1, Fraction.TryParse("2147483641/1", out f));
			// Empty string
			Assert.Equal(0, Fraction.TryParse("", out f));
			// Multiple slashes
			Assert.Equal(0, Fraction.TryParse("1/2/3", out f));
			// Leading slash
			Assert.Equal(0, Fraction.TryParse("/2", out f));
			// Trailing slash
			Assert.Equal(0, Fraction.TryParse("2/", out f));
		}

		[Fact]
		public void DivisionByZeroThrows()
		{
			var a = new Fraction(1, 2);
			var b = new Fraction(0, 1);
			Assert.Throws<DivideByZeroException>(() => a / b);
		}

		[Fact]
		public void ConstructionZeroDenominatorThrows()
		{
			Assert.Throws<ArgumentException>(() => new Fraction(1, 0));
		}
	}
}
