using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReheeCmf.Commons.Jsons.Options;

namespace ReheeCmf.Helpers
{
	//todo newtonjson
	public static class JsonHelper
	{
		public static T MergePatch<T>(T original, string patched)
		{
			var setting = JsonOption.DefaultOption;
			var sourceJson = System.Text.Json.JsonSerializer.Serialize(original, setting);
			var _settings = new JsonSerializerSettings() { DateParseHandling = DateParseHandling.None };
			var source = JsonConvert.DeserializeObject<JObject>(sourceJson, _settings);
			var content = JsonConvert.DeserializeObject<JObject>(patched, _settings);
			source!.Merge(content, new JsonMergeSettings()
			{
				MergeArrayHandling = MergeArrayHandling.Replace,
				MergeNullValueHandling = MergeNullValueHandling.Merge,
				PropertyNameComparison = StringComparison.OrdinalIgnoreCase
			});
			return System.Text.Json.JsonSerializer.Deserialize<T>(source.ToString(), setting)!;
		}
	}
}
