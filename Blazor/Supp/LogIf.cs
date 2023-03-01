using Microsoft.Extensions.Logging;

namespace Du.Blazor.Supp;

internal static class LogIf
{
	//
	private static void ThrowBySetting()
	{
		if (Settings.ThrowOnLog)
		{
			throw new Exception(Settings.UseLocaleMesg 
				? "설정(Settings.ThrowOnLog)에 의해 예외가 발동했어요." 
				: "Throw by user setting in Settings.ThrowOnLog.");
		}
	}

	// 컨테이너가 널이면 로그
	internal static void ContainerIsNull<TItem, TContainer>(ILogger<TItem> logger, TContainer? container)
	{
		if (container is not null)
			return;

		logger.LogError(Settings.UseLocaleMesg
			? "{item}: 컨테이너가 없어요. 이 컴포넌트는 반드시 <{container}> 컨테이너 아래 있어야 해요."
			: "{item}: No container found. This component must be contained within <{container}> component.", typeof(TItem).Name, typeof(TContainer).Name);

		ThrowBySetting();
	}

	//
	internal static void ContainerIsNull<TItem>(ILogger<TItem> logger, params object?[] containers)
	{
		if (containers.Any(c => c is not null))
			return;

		var names = (from c in containers where c is not null select c.GetType().Name).ToList();
		var join = string.Join(Settings.UseLocaleMesg ? "또는 " : " or ", names);
		logger.LogError(Settings.UseLocaleMesg
			? "{item}: 컨테이너가 없어요. 이 컴포넌트는 반드시 <{containers}> 컨테이너 아래 있어야 해요."
			: "{item}: No container found. This component must be contained within <{containers}> components.", typeof(TItem).Name, join);

		ThrowBySetting();
	}

	//
	internal static void FailWithMessage<TItem>(ILogger<TItem> logger, bool condition, string message)
	{
		if (condition)
			return;

		logger.LogError(message);

		ThrowBySetting();
	}
}
