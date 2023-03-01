namespace Du.Blazor;

/// <summary>펼침 이벤트</summary>
public class ExpandedEventArgs : EventArgs
{
	public string Id { get; }
	public bool Expanded { get; }

	public ExpandedEventArgs(string id, bool expanded)
	{
		Id = id;
		Expanded = expanded;
	}
}

/// <summary>슬라이드 이벤트</summary>
public class SlideEventArgs : EventArgs
{
	public int From { get; }
	public int To { get; }

	public SlideEventArgs(int from, int to)
	{
		From = from;
		To = to;
	}
}
