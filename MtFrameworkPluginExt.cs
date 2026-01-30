using System.Collections.Generic;
using System.Drawing;
using ReClassNET.Nodes;
using ReClassNET.Plugins;

namespace MtFrameworkPlugin
{
	public class MtFrameworkPluginExt : Plugin
	{
		public override Image Icon => Properties.Resources.logo_killtrish;

		public override IReadOnlyList<INodeInfoReader> GetNodeInfoReaders()
		{
			// Register the InfoReader

			return new[] { new MtFrameworkNodeInfoReader() };
		}
	}
}
