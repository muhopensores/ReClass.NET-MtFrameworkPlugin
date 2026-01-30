using System;
using System.Text;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Nodes;

namespace MtFrameworkPlugin
{
	/// <summary>A custom node info reader which outputs Frostbite type infos.</summary>
	public class MtFrameworkNodeInfoReader : INodeInfoReader
	{
		public string ReadNodeInfo(BaseHexCommentNode node, IRemoteMemoryReader reader, MemoryBuffer memory, IntPtr nodeAddress, IntPtr nodeValue)
		{
			// 1. try the direct value
			var info = ReadPtrInfo(nodeValue, reader);
			if (!string.IsNullOrEmpty(info))
			{
				return info;
			}

			// 2. try indirect pointer
			var indirectPtr = reader.ReadRemoteIntPtr(nodeValue);
			if (indirectPtr.MayBeValid())
			{
				info = ReadPtrInfo(indirectPtr, reader);
				if (!string.IsNullOrEmpty(info))
				{
					return $"Ptr -> {info}";
				}
			}

			return null;
		}

		private static string ReadPtrInfo(IntPtr value, IRemoteMemoryReader process)
		{
			var mtVtablePtr = value;
			if (mtVtablePtr.MayBeValid())
			{
				var getDtiPtr = process.ReadRemoteIntPtr(mtVtablePtr + 0x10);
				if (getDtiPtr.MayBeValid())
				{
					var dtiOffset = process.ReadRemoteIntPtr(getDtiPtr+1);
					if (dtiOffset.MayBeValid())
					{
							var namePtr = process.ReadRemoteIntPtr(dtiOffset+0x4);
							if (namePtr.MayBeValid())
							{
								var info = process.ReadRemoteStringUntilFirstNullCharacter(namePtr, Encoding.UTF8, 64);
								if (info.Length > 0 && info[0].IsPrintable())
								{
									return info;
								}
							}
					}
				}
			}

			return null;
		}
	}
}
