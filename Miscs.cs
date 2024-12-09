﻿/*
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linear_Equation_Solver_High_Precision_
{
	public class Miscs
	{
		static public void ModifyKey<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey oldKey, TKey newKey) where TKey : notnull
		{
			if (dictionary.TryGetValue(oldKey, out TValue? value))
			{
				if (value == null)
				{
					throw new Exception("Value of dictionary is null");
				}
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
