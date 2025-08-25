namespace AgroMind.GP.APIs.Helpers
{
	public class RecommendationException:Exception
	{
		public List<string> Reasons { get; }

		public RecommendationException(string message, List<string> reasons) : base(message)
		{
			Reasons = reasons;
		}

	}
}
