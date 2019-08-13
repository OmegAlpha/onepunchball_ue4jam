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

TArray<AUEMakeItCountCharacter*> UDB_FunctionLibrary::SortByScore(TArray<AUEMakeItCountCharacter*> OrigArray)
{
	TArray<AUEMakeItCountCharacter*> RetArray;
	TArray<AUEMakeItCountCharacter*> TempArray;

	for (int i = 0; i < OrigArray.Num(); i++)
	{
		TempArray.Add(OrigArray[i]);
	}

	int tries = 0;

	while (TempArray.Num() > 0)
	{
		AUEMakeItCountCharacter* NextHighScorePlayer = nullptr;
		int NextHighScore = -999999;

		for (int i = 0; i < TempArray.Num(); i++)
		{
			if (TempArray[i]->QtyKills >= NextHighScore)
			{
				NextHighScorePlayer = TempArray[i];
				NextHighScore = TempArray[i]->QtyKills;
			}
		}

		RetArray.Add(NextHighScorePlayer);
		TempArray.Remove(NextHighScorePlayer);

		tries++;

		if(tries > 100)
			return RetArray;
	}


	return RetArray;
}
