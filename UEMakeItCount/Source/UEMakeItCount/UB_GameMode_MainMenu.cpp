// Fill out your copyright notice in the Description page of Project Settings.

#include "UB_GameMode_MainMenu.h"
#include "UEMakeItCountPlayerController.h"
#include "UEMakeItCountCharacter.h"
#include "UObject/ConstructorHelpers.h"
#include "Engine/World.h"
#include "Engine/Engine.h"
#include "DB_GameInstance.h"
#include "DB_OnlineSubSystem.h"
#include "UEMakeItCountPlayerController.h"
#include "GameFramework/PlayerState.h"

AUB_GameMode_MainMenu::AUB_GameMode_MainMenu()
{
	// use our custom PlayerController class
	PlayerControllerClass = AUEMakeItCountPlayerController::StaticClass();

	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnBPClass(TEXT("/Game/TopDownCPP/Blueprints/TopDownCharacter"));
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

void AUB_GameMode_MainMenu::BeginPlay()
{
	// 	FVector Location(0.0f, 0.0f, 0.0f);
	// 	FRotator Rotation(0.0f, 0.0f, 0.0f);
	// 	FActorSpawnParameters SpawnInfo;
	// 	GSObject = GetWorld()->SpawnActor<ADB_GameSparks>(Location, Rotation, SpawnInfo);

	// bUseSeamlessTravel = true;

	Cast<UDB_GameInstance>(GetGameInstance())->OnlineSubsystem->World = GetWorld();
	
}


void AUB_GameMode_MainMenu::PostLogin(APlayerController* NewPlayer)
{
	Super::PostLogin(NewPlayer);

	Cast<UDB_GameInstance>(GetGameInstance())->OnlineSubsystem->AddPlayerToSession(NewPlayer);
}

void AUB_GameMode_MainMenu::Logout(AController* Exiting)
{
	Super::Logout(Exiting);

	Cast<UDB_GameInstance>(GetGameInstance())->OnlineSubsystem->RemovePlayerFromSession(Cast<APlayerController>(Exiting));
}