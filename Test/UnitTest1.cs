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
}
