namespace ReheeCmf.Helpers
{
	public static class DictionaryHelper
	{
		public static bool TryGetValueStringKey<K>(this IDictionary<string, K?>? dictionary, string key, out K? k)
		{
			if (dictionary == null)
			{
				k = default(K);
				return false;
			}
			var dKey = dictionary.Keys.FirstOrDefault(b => String.Equals(b, key, StringComparison.OrdinalIgnoreCase));
			if (dKey == null)
			{
				k = default(K);
				return false;
			}
			return dictionary.TryGetValue(dKey, out k);
		}

		public static Dictionary<string, K?> ToDictionWithKeys<K>(this IDictionary<string, K?> dictionary, IEnumerable<string> keys)
		{
			var dKeys = dictionary.Keys.Where(b => keys.Any(k => string.Equals(k, b, StringComparison.OrdinalIgnoreCase)));
			return dictionary.Where(b => dKeys.Contains(b.Key)).ToDictionary(b => b.Key, b => b.Value);
		}
		public static bool TryAddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			if (dictionary == null)
			{
				return false;
			}
			if (key == null)
			{
				return false;
			}
			if (dictionary.ContainsKey(key))
			{
				dictionary[key] = value;
				return true;
			}
			else
			{
				dictionary.Add(key, value);
				return true;
			}
		}
		public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			if (dictionary == null)
			{
				return false;
			}
			if (key == null)
			{
				return false;
			}
			if (dictionary.ContainsKey(key))
			{
				return false;
			}
			else
			{
				dictionary.Add(key, value);
				return true;
			}
		}
		public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValue? value)
		{
			if (dictionary == null)
			{
				value = default;
				return false;
			}
			if (key == null)
			{
				value = default;
				return false;
			}
			if (dictionary.ContainsKey(key))
			{
				dictionary.Remove(key, out value);
				return true;
			}
			else
			{
				value = default;
				return false;
			}
		}
	}
}

