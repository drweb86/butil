using System;
using System.Collections.Generic;
using System.Linq;

namespace BUtil.Core.Storages
{
    class SemicolumnSeparatedStringSerializer
	{
		public static string Serialize(object settings, List<string> skipProperties)
		{
			var result = string.Join(
				";",
				settings
					.GetType()
					.GetProperties()
					.Where(p => !skipProperties.Contains(p.Name))
					.Select(x => $"{x.Name}={x.GetValue(settings)}"));
			
			return result;
        }

		public static T Deserialize<T>(string content)
			where T: class, new()
		{
			var settings = content
				.Split(";")
				.ToDictionary(
					line => line.Substring(0, line.IndexOf('=')).Trim(),
					line => line.Substring(line.IndexOf('=') + 1).Trim());

			var result = new T();

            foreach (var prop in settings
				.GetType()
                .GetProperties())
			{
				if (settings.ContainsKey(prop.Name))
				{
					prop.SetValue(content, Convert.ChangeType(settings[prop.Name], prop.PropertyType));
				}
			}

			return result;
        }
	}
}
