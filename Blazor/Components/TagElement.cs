namespace Du.Blazor.Components;

/// <summary>태그 요소</summary>
/// <remarks>지정한 태그를 이용해서 내용을 그림</remarks>
public class TagElement : ComponentBase
{
	/// <summary>자식 콘텐트</summary>
	[Parameter] 
	public RenderFragment? ChildContent { get; set; }

	/// <summary>사용할 태그 이름</summary>
	/// <remarks>기본은 'span'</remarks>
	[Parameter] 
	public string Tag { get; set; } = "span";

	/// <summary>요소 참조</summary>
	[Parameter] 
	public ElementReference Reference { get; set; }

	/// <summary>css클래스</summary>
	[Parameter] 
	public string? Class { get; set; }
	
	/// <summary>참조된 요소가 변경될 경우 실행할 대리자</summary>
	[Parameter] 
	public Action<ElementReference>? ReferenceChanged { get; set; }

	/// <summary>마우스가 눌렸을 때 이벤트</summary>
	[Parameter] 
	public EventCallback<MouseEventArgs> OnClick { get; set; }

	/// <summary>마우스가 눌렸을때 StopPropagation 사용 여부</summary>
	[Parameter] 
	public bool OnClickStopPropagation { get; set; }

	/// <summary>마우스가 눌렸을때 PreventDefault 사용 여부</summary>
	[Parameter]
	public bool OnClickPreventDefault { get; set; }

	/// <summary>그밖에 모든 속성</summary>
	[Parameter(CaptureUnmatchedValues = true)] 
	public IDictionary<string, object>? UserAttrs { get; set; }

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, Tag);
		builder.AddAttribute(1, "class", Class);
		builder.AddAttribute(2, "onclick", InvokeOnClickAsync);
		builder.AddEventPreventDefaultAttribute(3, "onclick", OnClickPreventDefault);
		builder.AddEventStopPropagationAttribute(4, "onclick", OnClickStopPropagation);
		builder.AddMultipleAttributes(5, UserAttrs);

		builder.AddElementReferenceCapture(6, p =>
		{
			Reference = p;
			ReferenceChanged?.Invoke(p);
		});

		builder.AddContent(7, ChildContent);
		builder.CloseElement();
	}

	//
	protected virtual Task InvokeOnClickAsync(MouseEventArgs e) => OnClick.InvokeAsync(e);
}
