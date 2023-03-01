using System.Diagnostics.CodeAnalysis;

namespace Du.Blazor.Supp;

internal static class ThrowIf
{
	// 컨테이너가 널이면 예외
	internal static void ContainerIsNull<TContainer, TItem>(TItem item, [NotNull] TContainer? container)
	{
		if (container is not null)
			return;

		throw new InvalidOperationException(Settings.UseLocaleMesg
			? $"{typeof(TItem)}: 컨테이너가 없어요. 이 컴포넌트는 반드시 <{typeof(TContainer)}> 컨테이너 아래 있어야 해요."
			: $"{typeof(TItem)}: No container found. This component must be contained within <{typeof(TContainer)}> container component.");
	}

	//
	internal static void ContainerIsNull<TItem>(TItem item, params object?[] containers)
	{
		if (containers.Any(c => c is not null))
			return;

		var names = from c in containers where c is not null select c.GetType().Name;
		var join = string.Join(Settings.UseLocaleMesg ? "또는 " : " or ", names);
		throw new InvalidOperationException(Settings.UseLocaleMesg
			? $"{typeof(TItem)}: 컨테이너가 없어요. 이 컴포넌트는 반드시 <{join}> 컨테이너 아래 있어야 해요."
			: $"{typeof(TItem)}: No container found. This component must be contained within <{join}> components");
	}

	//
	internal static void ItemNotNull<TItem>(TItem? item)
	{
		if (item is null)
			return;

		throw new InvalidOperationException(Settings.UseLocaleMesg
			? $"{typeof(TItem)}, {nameof(item)}: 여기서는 반드시 널이어야 해요."
			: $"{typeof(TItem)}, {nameof(item)}: Must be null here");
	}

	// 컴포넌트 캐스트가 안되면 예외
	internal static TConv CastFail<TConv>([NotNull] this object? component)
	{
		if (component is TConv converted)
			return converted;

		var nameComponent = component is null ? nameof(component) : component.GetType().ToString();
		throw new InvalidOperationException(Settings.UseLocaleMesg
			? $"{nameComponent}: 잘못된 컴포넌트 캐스팅이예요. 반드시 <{typeof(TConv)}> 이어야해요."
			: $"{nameComponent}: Invalid component casting. Must be <{typeof(TConv)}>.");
	}

	//
	internal static void ConditionFail([DoesNotReturnIf(false)] bool condition)
	{
		if (condition)
			return;

		throw new InvalidOperationException(Settings.UseLocaleMesg
			? "조건이 실패했어요!"
			: "Condition failed!");
	}

	//
	internal static void NotImplementedWithCondition<TType>([DoesNotReturnIf(false)] bool condition)
	{
		if (condition)
			return;

		throw new NotImplementedException(Settings.UseLocaleMesg
			? $"{typeof(TType)}: 이러한 조건에서 수행할 기능이 아직 만들어지지 않았어요."
			: $"{typeof(TType)}: Not implementate in such a condition yet.");
	}

	//
	internal static void ArgumentNull<TItem>([NotNull] TItem? obj, string objName)
	{
		if (obj is not null)
			return;

		throw new ArgumentNullException(Settings.UseLocaleMesg
			? $"{objName}:{typeof(TItem)} 인수는 비어있으면 안됩니다."
			: $"{objName}:{typeof(TItem)} must not be null.");
	}

	//
	[DoesNotReturn]
	internal static void ArgumentOutOfRange(string name, object value)
	{
		throw new ArgumentOutOfRangeException(name, value, Settings.UseLocaleMesg
			? "인수 값의 범위가 벗어났습니다."
			: "Aruguemt out of range.");
	}
}
