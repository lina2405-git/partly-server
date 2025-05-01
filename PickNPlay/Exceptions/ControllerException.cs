namespace PickNPlay.Exceptions
{

	[Serializable]
	public class ControllerException : Exception
	{
		public ControllerException() { }
		public ControllerException(string message) : base(message) { }
		public ControllerException(string message, Exception inner) : base(message, inner) { }
		protected ControllerException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
