using System.ComponentModel;
using Du.Blazor.Supp;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;

namespace Du.Blazor;

/// <summary>
/// 컴포넌트 값 변경 추적
/// </summary>
public class ComponentTracker : ComponentBase, IDisposable
{
	[Parameter] 
	public RenderFragment? ChildContent { get; set; }

	[Parameter]
	public INotifyPropertyChanged? Value { get; set; }

	[Parameter]
	public string? Desc { get; set; }

	[Inject]
	protected ILogger<ComponentTracker> Logger { get; set; } = default!;

	//
	private INotifyPropertyChanged? _prevValue;
	private string? _prevDesc;

	//
	protected override void OnParametersSet()
	{
		ThrowIf.ArgumentNull(Value, nameof(Value));

		if (!ReferenceEquals(Value, _prevValue))
		{
			Logger.LogDebug(Settings.UseLocaleMesg
				? "값 변경 감시를 시작합니다: {desc}"
				: "{desc}: Start PropertyChanged.",
				Desc ?? nameof(Value));
			Value.PropertyChanged += Value_PropertyChanged;

			if (_prevValue is not null)
			{
				Logger.LogDebug(Settings.UseLocaleMesg
					? "감시를 해제합니다: {desc}"
					: "{desc}: Stop PropertyChanged.",
					_prevDesc ?? (Settings.UseLocaleMesg ? "이전 Value" : "Previous value"));
				_prevValue.PropertyChanged -= Value_PropertyChanged;
			}
		}

		_prevValue = Value;
		_prevDesc = Desc;
	}

	//
	private void Value_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		Logger.LogDebug(Settings.UseLocaleMesg
			? "값이 바뀌었습니다: {desc}"
			: "PropertyChanged: {desc}",
			Desc ?? nameof(Value));
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		Logger.LogDebug(Settings.UseLocaleMesg
			? "렌더링합니다: {desc}"
			: "BuildRenderTree: {desc}",
			Desc ?? nameof(Value));
	}

	//
	public void Dispose() =>
		Dispose(true);

	//
	protected virtual void Dispose(bool disposing)
	{
		if (!disposing) 
			return;

		Logger.LogDebug(Settings.UseLocaleMesg
			? "개체가 종료하므로 감시를 해제합니다: {desc}"
			: "{desc}: Disposed. stopped PropertyChanged",
			_prevDesc ?? (Settings.UseLocaleMesg ? "현재 Value" : "Current value"));
		if (Value is not null)
			Value.PropertyChanged -= Value_PropertyChanged;
	}
}
