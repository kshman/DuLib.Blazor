namespace Du.Blazor.Components;

// fa 쓸라면: <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">

public class Icon : ComponentBase
{
	/// <summary>클래스 지정</summary>
	[Parameter] public string? Class { get; set; }
	/// <summary>아이콘 이름</summary>
	[Parameter] public string? Name { get; set; }

	private string? _real_name;

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		if (Name?.Length > 3)
		{
			_real_name = Name[2] == '-' 
				? $"{Name[..2]} {Name}" // oi / bi / fa 같은거는 세번째 빼기가 있다
				: Name; // 걍 넣음
		}
		else
		{
			// 블레이저 기본으로 OI 경고
			_real_name = "oi oi-warning";
		}
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "span");
		builder.AddAttribute(1, "class", Cssc.Class(Class, _real_name));
		builder.AddAttribute(2, "aria-hidden", "true");
		builder.CloseElement();
	}
}

