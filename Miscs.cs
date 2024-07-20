using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linear_Equation_Solver_High_Precision_
{
	public class Miscs
	{
		static public void ModifyKey<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey oldKey, TKey newKey)
		{
			if (dictionary.TryGetValue(oldKey, out TValue value))
			{
				dictionary.Remove(oldKey);
				dictionary[newKey] = value;
			}
		}
		static public List<int> GetUncommonElements(List<int> list1, List<int> list2)
		{
			//HashSet<int> set1 = new HashSet<int>(list1); //for later use if want to preserve
			HashSet<int> set1 = new HashSet<int>(list1);

			List<int> result = new List<int>();

			foreach (int element in list2)
			{
				if ((!set1.Contains(element)) && (!result.Contains(element)))
				{
					result.Add(element);
				}
			}

			return result;
		}
	}
}
