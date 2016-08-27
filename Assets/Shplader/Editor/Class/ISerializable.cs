using System.Text;

namespace Shplader.Core
{
	public interface ISerializable
	{
		void OnSerialize(StringBuilder stringBuilder);
	}
}
