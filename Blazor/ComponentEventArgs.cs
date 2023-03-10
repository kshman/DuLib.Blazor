namespace Du.Blazor;

/// <summary>펼침 이벤트</summary>
public class ExpandedEventArgs : EventArgs
{
	public string? Id { get; init; }
	public bool Expanded { get; init; }
}

/// <summary>슬라이드 이벤트</summary>
public class SlideEventArgs : EventArgs
{
	public int From { get; init; }
	public int To { get; init; }
}

/// <summary>사용 상태 변경 이벤트</summary>
public class ActiveEventArgs : EventArgs
{
	public string? Id { get; init; }
	public bool Active { get; init; }
}
