using System;
using UnityEngine;
using System.Collections;

namespace Shplader.Core
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class SerializeAttribute : Attribute {}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class SerializeListAttribute : Attribute {}
}
