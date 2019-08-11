// Fill out your copyright notice in the Description page of Project Settings.


#include "DB_GameInstance.h"
#include "DB_OnlineSubSystem.h"

void UDB_GameInstance::Init()
{
	Super::Init();

	UE_LOG(LogTemp, Warning, TEXT("INIT GAME INSTANCE"));

	if (OnlineSubsystem == nullptr)
	{
		UE_LOG(LogTemp, Warning, TEXT("INIT ONLINE SUBSYSTEM"));
		OnlineSubsystem = NewObject<UDB_OnlineSubSystem>();
		OnlineSubsystem->Init();
	}	
}
