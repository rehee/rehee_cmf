namespace ReheeCmf.Helpers
{
	public static class WithKeyHelper
	{
		public static string? EffectiveKey(this IWithKey b) => b.KeyValue != 0 ? b.StringKeyValue : b.StringKeyValueOverride;
	}
}
