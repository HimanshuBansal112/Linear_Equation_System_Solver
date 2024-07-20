using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linear_Equation_Solver_High_Precision_
{
	public class GaussEliminationClass
	{
		Fraction[,] Convert_to_one(Fraction[,] matrix, int row_no, int column_count, List<int> index)
		{
			row_no -= 1;
			Fraction base_no = matrix[index[0], index[1]];

			for (int j = 0; j < column_count; j++)
			{
				matrix[row_no, j] /= base_no;
			}
			return matrix;
		}

		Fraction[,] Convert_to_zero(Fraction[,] matrix, int row_no, int column_no, int column_count, int ones_row_no)
		{
			column_no -= 1;
			row_no -= 1;
			Fraction base_no = matrix[ones_row_no, column_no];
			//element_to_be_zero=base_no*multiply_base
			Fraction multiply_base = matrix[row_no, column_no] / base_no;

			for (int j = 0; j < column_count; j++)
			{
				matrix[row_no, j] -= multiply_base * matrix[ones_row_no, j]; //column number is row number where value is one
			}
			return matrix;
		}

		static bool AreMatricesEqual(Fraction[,] matrix1, Fraction[,] matrix2)
		{
			if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1))
			{
				// Matrices have different dimensions
				return false;
			}

			for (int i = 0; i < matrix1.GetLength(0); i++)
			{
				for (int j = 0; j < matrix1.GetLength(1); j++)
				{
					// Compare corresponding elements
					if (matrix1[i, j] != matrix2[i, j])
					{
						return false;
					}
				}
			}

			// All corresponding elements are equal
			return true;
		}

		Fraction[,] GaussElimination(Fraction[,] matrix)
		{
			int column_count = matrix.GetLength(1);

			for (int i = 1; i < matrix.GetLength(0); i++)
			{
				if (column_count != matrix.GetLength(1))
				{
					throw new InvalidDataException("Column count doesn't match at every row");
				}
			}

			List<int> index_to_be_one = [0, 0];

			int last_row_no = 0;

			for (int j = 0; j < column_count; j++)
			{
				for (int i = 0; i < matrix.GetLength(0); i++)
				{
					if (index_to_be_one[0] == i && index_to_be_one[1] == j)
					{
						if (matrix[i, j] == 0)
						{
							index_to_be_one[1]++;
						}
						else if (matrix[i, j] == 1)
						{
							last_row_no = index_to_be_one[0];
							index_to_be_one[0]++;
							index_to_be_one[1]++;
						}
						else
						{
							matrix = Convert_to_one(matrix, i + 1, column_count, index_to_be_one);
							last_row_no = index_to_be_one[0];
							index_to_be_one[0]++;
							index_to_be_one[1]++;
						}
					}
					else if (j < i)
					{
						if (matrix[i, j] != 0)
						{
							matrix = Convert_to_zero(matrix, i + 1, j + 1, matrix.GetLength(1), last_row_no);
							Fraction[,] matrix_copy = matrix.Clone() as Fraction[,];
							matrix = matrix_sort(matrix);
							if (!AreMatricesEqual(matrix, matrix_copy))
							{
								matrix = GaussElimination(matrix);
							}
						}
					}
					//else
					//{
					//	Console.WriteLine("ran");
					//}
					//if (j < i)
					//{
					//	Console.WriteLine("ran");
					//};
				}
			}

			return matrix;
		}

		bool underdetermined_check(Fraction[,] matrix)
		{
			int len = matrix.GetLength(0);

			if (matrix.GetLength(0) > matrix.GetLength(1))
			{
				len = matrix.GetLength(1);
			}
			for (int i = 0; i < len; i++)
			{
				for (int j = 0; j < len; j++)
				{
					if (matrix[i, j] == 1)
					{
						j = len;
						continue;
					}
					else if (matrix[i, j] != 0)
					{
						return false;
					}
				}
			}
			return true;
		}

		public string invalid_check(Fraction[,] matrix)
		{
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				bool is_lhs_zero = true;
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					if (matrix[i, j] != 0 && j < matrix.GetLength(1) - 1)
					{
						is_lhs_zero = false;
					}
					if (j == matrix.GetLength(1) - 1 && is_lhs_zero == true)
					{
						if (matrix[i, j] != 0)
						{
							return "invalid";
						}
					}
				}
			}
			return "correct";
		}

		Fraction[,] matrix_sort(Fraction[,] matrix)
		{
			int len = matrix.GetLength(0);
			if (matrix.GetLength(0) > matrix.GetLength(1))
			{
				len = matrix.GetLength(1);
			}

			for (int i = 0; i < len; i++)
			{
				if (matrix[i, i] == 0)
				{
					for (int i2 = i + 1; i2 < len; i2++)
					{
						if (matrix[i2, i] != 0)
						{
							for (int i3 = 0; i3 < matrix.GetLength(1); i3++)
							{
								Fraction temp = new Fraction(0,1);
								temp = matrix[i2, i3];
								matrix[i2, i3] = matrix[i, i3];
								matrix[i, i3] = temp;
							}
							i2 = len;
						}
					}
				}
			}
			return matrix;
		}

		public void print_matrix(Fraction[,] array)
		{
			for (int i = 0; i < array.GetLength(0); i++)
			{
				Console.Write("[");
				for (int j = 0; j < array.GetLength(1); j++)
				{
					if (j != array.GetLength(1) - 1)
					{
						Console.Write(array[i, j] + " ");
					}
					else
					{
						Console.Write(array[i, j]);
					}
				}
				Console.WriteLine("]");
			}
		}

		public Fraction[,] clean(Fraction[,] array)
		{
			List<int> non_zero_list = new List<int>();

			for (int i = 0; i < array.GetLength(0); i++)
			{
				bool zero_row = true;
				for (int j = 0; j < array.GetLength(1); j++)
				{
					if (array[i, j] != 0)
					{
						zero_row = false;
						break;
					}
				}
				if (zero_row == false)
				{
					non_zero_list.Add(i);
				}
			}

			Fraction[,] temp12 = new Fraction[non_zero_list.Count(), array.GetLength(1)];

			for (int i = 0; i < non_zero_list.Count(); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					temp12[i, j] = array[non_zero_list[i], j];
				}
			}

			return temp12;
		}

		public (Fraction[,],List<int>,List<int>) clean_coloumn(Fraction[,] array)
		{
			List<int> non_zero_list = new List<int>();
			List<int> zero_list = new List<int>();

			for (int j = 0; j < array.GetLength(1) - 1; j++)
			{
				bool zero_column = true;
				
				for (int i = 0; i < array.GetLength(0); i++)
				{
					if (array[i, j] != 0)
					{
						zero_column = false;
						break;
					}
				}
				if (zero_column == false)
				{
					non_zero_list.Add(j);
				}
				else
				{
					zero_list.Add(j);
				}
			}
			
			non_zero_list.Add(array.GetLength(1) - 1);
			
			Fraction[,] temp12 = new Fraction[array.GetLength(0), non_zero_list.Count()];

			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < non_zero_list.Count(); j++)
				{
					temp12[i, j] = array[i, non_zero_list[j]];
				}
			}

			return (temp12, non_zero_list, zero_list);
		}

		public Fraction[,] EndResult(Fraction[,] matrix)
		{
			matrix = matrix_sort(matrix);
			matrix = GaussElimination(matrix);
			matrix = clean(matrix);
			return matrix;
		}

		public Fraction[,] TakeInput()
		{
			int row = 0, column = 0;
			string row1, column1;

			Console.Write("Enter number of equations: ");
			row1 = Console.ReadLine() ?? "a";

			while (!int.TryParse(row1, out row) || row <= 0)
			{
				Console.Write("Invalid equations count. Re-enter please: ");
				row1 = Console.ReadLine() ?? "a";
			}


			Console.Write("Enter number of variables: ");
			column1 = Console.ReadLine() ?? "a";

			while (!int.TryParse(column1, out column) || column <= 0)
			{
				Console.Write("Invalid variable count. Re-enter please: ");
				column1 = Console.ReadLine() ?? "a";
			}

			column = column + 1;

			Fraction[,] array = new Fraction[row, column];

			string temp = "a";

			Console.WriteLine("\n\nFormat is a0*x0 + a1*x1 + .... + c = 0\n");

			for (int i = 0; i < row; i++)
			{
				Console.WriteLine($"\nFor equation {i}: ");
				for (int j = 0; j < column; j++)
				{
					if (j == column - 1)
					{
						Console.Write("Write constant c: ");
					}
					else
					{
						Console.Write($"Write coefficient for variable x{j}: ");
					}
					temp = Console.ReadLine() ?? "a";

					while (Fraction.TryParse(temp, out array[i, j])!=1)
					{
						if (Fraction.TryParse(temp, out array[i, j]) == -1)
						{
							Console.Write("Large numbers not supported. Re-enter please: ");
						}
						else if (Fraction.TryParse(temp, out array[i, j]) == -2)
						{
							Console.Write("Denominator should not be 0. Re-enter please: ");
						}
						else
						{
							Console.Write("Invalid value. Re-enter please: ");
						}
						temp = Console.ReadLine() ?? "a";
					}
				}
			}
			return array;
		}

		public Fraction[,] RandomInput()
		{
			int row = 0, column = 0;
			string row1, column1;

			Console.WriteLine("This is a random constant generator for testing");
			Console.Write("Enter number of equations: ");
			row1 = Console.ReadLine() ?? "a";

			while (!int.TryParse(row1, out row) || row <= 0)
			{
				Console.Write("Invalid equations count. Re-enter please: ");
				row1 = Console.ReadLine() ?? "a";
			}


			Console.Write("Enter number of variables: ");
			column1 = Console.ReadLine() ?? "a";

			while (!int.TryParse(column1, out column) || column <= 0)
			{
				Console.Write("Invalid variable count. Re-enter please: ");
				column1 = Console.ReadLine() ?? "a";
			}

			column = column + 1;

			Fraction[,] array = new Fraction[row, column];

			var random = new Random();

			double a;

			for (int i = 0; i < row; i++)
			{
				for (int j = 0; j < column; j++)
				{
					a = random.NextDouble();
					a = a * 100;
					a = Math.Round(a, 2);

					if (Fraction.TryParse(a.ToString(), out array[i, j])!=1)
					{
						if (Fraction.TryParse(a.ToString(), out array[i, j]) == -1)
						{
							Console.WriteLine("Large numbers found.");
						}
						else if (Fraction.TryParse(a.ToString(), out array[i, j]) == -2)
						{
							Console.WriteLine("Denominator is equal to 0.");
						}
						else
						{
							Console.WriteLine("Invalid number found.");
						}
						throw new Exception("Error!!!");
					}
				}
			}
			return array;
		}
	}
}
