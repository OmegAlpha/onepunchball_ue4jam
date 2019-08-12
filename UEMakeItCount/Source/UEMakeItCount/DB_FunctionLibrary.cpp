// Fill out your copyright notice in the Description page of Project Settings.


#include "DB_FunctionLibrary.h"
#include "DB_GameInstance.h"
#include "Regex.h"


UDB_OnlineSubSystem* UDB_FunctionLibrary::GetOnline(UObject* WorldContext)
{
	UDB_GameInstance* GameInstance = Cast<UDB_GameInstance>(WorldContext->GetWorld()->GetGameInstance());
	return GameInstance->OnlineSubsystem;
}

bool UDB_FunctionLibrary::CheckUserName(FString UserName)
{
	const FRegexPattern pattern(TEXT("^[a-zA-Z0-9_.-]*$"));

	FRegexMatcher matcher(pattern, UserName);

	if (matcher.FindNext())
	{
		return true;
	}

	return false;
}
