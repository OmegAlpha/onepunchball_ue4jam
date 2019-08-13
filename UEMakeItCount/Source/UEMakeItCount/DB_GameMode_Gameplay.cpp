// Fill out your copyright notice in the Description page of Project Settings.


#include "DB_GameMode_Gameplay.h"
#include "DB_GameInstance.h"
#include "UEMakeItCountPlayerController.h"
#include "ConstructorHelpers.h"
#include "DB_OnlineSubSystem.h"
#include "Engine/World.h"
#include "UnrealNetwork.h"
#include "DB_GameInfoActor.h"
#include "Engine/Engine.h"
#include "GameFramework/Actor.h"
#include "GameFramework/PlayerState.h"

ADB_GameMode_Gameplay::ADB_GameMode_Gameplay()
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APlayerController> PlayerControllerBPClass(TEXT("/Game/0Game/Blueprints/bp_DB_PlayerController"));
	if (PlayerControllerBPClass.Class != NULL)
	{
		PlayerControllerClass = PlayerControllerBPClass.Class;
	}


	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnBPClass(TEXT("/Game/0Game/Blueprints/bp_PlayerCharacter"));
	if (PlayerPawnBPClass.Class != NULL)
	{
		DefaultPawnClass = PlayerPawnBPClass.Class;
	}

	

	static ConstructorHelpers::FClassFinder<APlayerState> PlayerStateBPClass(TEXT("/Game/0Game/Blueprints/bp_DB_PlayerState"));
	if (PlayerStateBPClass.Class != NULL)
	{
		PlayerStateClass = PlayerStateBPClass.Class;
	}

	PrimaryActorTick.bCanEverTick = true;
}

void ADB_GameMode_Gameplay::BeginPlay()
{
	Super::BeginPlay();

	if (HasAuthority())
	{
		CreateGameInfo();
	}

}

void ADB_GameMode_Gameplay::GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const
{
	Super::GetLifetimeReplicatedProps(OutLifetimeProps);

	DOREPLIFETIME(ADB_GameMode_Gameplay, GameInfo);
}

void ADB_GameMode_Gameplay::PostLogin(APlayerController* NewPlayer)
{
	Super::PostLogin(NewPlayer);

	if(GameInfo != nullptr)
	{
		GameInfo->AddPlayerToSession(NewPlayer);
	}	 
}

void ADB_GameMode_Gameplay::Logout(AController* Exiting)
{
	Super::Logout(Exiting);

	if (GameInfo != nullptr)
	{
		GameInfo->RemovePlayerToSession(Cast<APlayerController>(Exiting));
	}	

}

void ADB_GameMode_Gameplay::CreateGameInfo()
{
	
	if (GameInfo == nullptr)
	{
		FVector Location(0.0f, 0.0f, 0.0f);
		FRotator Rotation(0.0f, 0.0f, 0.0f);
		FActorSpawnParameters SpawnInfo;
		GameInfo = GetWorld()->SpawnActor<ADB_GameInfoActor>(Location, Rotation, SpawnInfo);
	}
}
