namespace Du.Blazor;

/// <summary>펼침 이벤트</summary>
public class ExpandEventArgs : EventArgs
{
	public string? Id { get; init; }
	public bool Expand { get; init; }
}

/// <summary>슬라이드 이벤트</summary>
public class SlideEventArgs : EventArgs
{
	public int From { get; init; }
	public int To { get; init; }
}
