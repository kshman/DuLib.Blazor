namespace Du.Blazor;

/// <summary>액티브 이벤트</summary>
public class ActiveEventArgs : EventArgs
{
	public ComponentProp Component { get; }
	public bool Active { get; }

	public ActiveEventArgs(ComponentProp component, bool active)
	{
		Component = component;
		Active = active;
	}
}

/// <summary>슬라이드 이벤트</summary>
public class SlideEventArgs : EventArgs
{
	public int From { get; init; }
	public int To { get; init; }
}
