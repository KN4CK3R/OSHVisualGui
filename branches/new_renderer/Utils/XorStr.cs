using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSHVisualGui
{
	class XorStr
	{
		private static Random random = new Random();

		public static string Generate(string data)
		{
			string realData = string.Empty;

			for (var i = 0; i < data.Length; ++i)
			{
				var c = data[i];
				if (c == '\\')
				{
					++i;
					c = data[i];
					if (c == '\\')
						c = '\\';
					else if (c == 'n')
						c = '\n';
					else if (c == 'r')
						c = '\r';
					else if (c == 't')
						c = '\t';
					else if (c == 'f')
						c = '\f';
					else if (c == 'b')
						c = '\b';
					else if (c == '"')
						c = '\"';
					else if (c == 'x')
					{
						++i;
						if (i >= data.Length)
						{
							throw new Exception("invalid escape sequence: \\x");
						}
						c = char.ToLower(data[i]);
						if (!(('0' <= c && c <= '9') || ('a' <= c && c <= 'f')))
						{
							throw new Exception("invalid escape sequence: \\x" + c);
						}
						string temp = string.Empty + c;
						if (i + 1 < data.Length)
						{
							++i;
							c = char.ToLower(data[i]);
							if (!(('0' <= c && c <= '9') || ('a' <= c && c <= 'f')))
							{
								throw new Exception("invalid escape sequence: \\x" + data[i - 1] + c);
							}
							temp += c;
						}
						c = (char)int.Parse(temp, System.Globalization.NumberStyles.HexNumber);
					}
					else
					{
						throw new Exception("invalid escape sequence: \\" + c);
					}
				}
				realData += c;
			}

			var xor = random.Next(257);

			StringBuilder sb = new StringBuilder();
			sb.Append("/*" + data + "*/XorStr(" + realData.Length + ", 0x" + xor.ToString("X2") + ", ");

			int length = realData.Length / 4;
			for (var i = 0; i < length; ++i)
			{
				sb.Append("0x");
				for (var j = 0; j < 4; ++j)
				{
					var val = (int)realData[i * 4 + j];
					val ^= xor;
					xor += 127;
					xor %= 256;
					sb.Append(val.ToString("X2"));
				}
				sb.Append(", ");
			}
			realData = realData.Substring(length * 4, realData.Length % 4);
			sb.Length -= 2;

			if (realData.Length % 4 != 0)
			{
				sb.Append(", 0x");

				for (var i = 0; i < realData.Length % 4; ++i)
				{
					var val = (int)realData[i];
					val ^= xor;
					xor += 127;
					xor %= 256;
					sb.Append(val.ToString("X2"));
				}

				for (var i = 0; i < 4 - realData.Length % 4; ++i)
				{
					sb.Append("00");
				}
			}

			sb.Append(")");

			return sb.ToString();
		}
	}
}
