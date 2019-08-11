// Fill out your copyright notice in the Description page of Project Settings.


#include "DB_FunctionLibrary.h"
#include "DB_GameInstance.h"


UDB_OnlineSubSystem* UDB_FunctionLibrary::GetOnline(UObject* WorldContext)
{
	UDB_GameInstance* GameInstance = Cast<UDB_GameInstance>(WorldContext->GetWorld()->GetGameInstance());
	return GameInstance->OnlineSubsystem;
}
