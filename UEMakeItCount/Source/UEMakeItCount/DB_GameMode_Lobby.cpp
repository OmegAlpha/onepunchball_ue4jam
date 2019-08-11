// Fill out your copyright notice in the Description page of Project Settings.


#include "DB_GameMode_Lobby.h"
#include "Object.h"
#include "ConstructorHelpers.h"
#include "UEMakeItCountPlayerController.h"
#include "DB_GameInstance.h"
#include "DB_OnlineSubSystem.h"

ADB_GameMode_Lobby::ADB_GameMode_Lobby()
{
	// use our custom PlayerController class
	PlayerControllerClass = AUEMakeItCountPlayerController::StaticClass();

	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnBPClass(TEXT("/Game/TopDownCPP/Blueprints/TopDownCharacter"));
	if (PlayerPawnBPClass.Class != NULL)
	{
		DefaultPawnClass = PlayerPawnBPClass.Class;
	}

	PrimaryActorTick.bCanEverTick = true;
}

void ADB_GameMode_Lobby::PostLogin(APlayerController* NewPlayer)
{
	Super::PostLogin(NewPlayer);

	Cast<UDB_GameInstance>(GetGameInstance())->OnlineSubsystem->AddPlayerToSession(NewPlayer);
}

void ADB_GameMode_Lobby::Logout(AController* Exiting)
{
	Super::Logout(Exiting);

	Cast<UDB_GameInstance>(GetGameInstance())->OnlineSubsystem->RemovePlayerFromSession(Cast<APlayerController>(Exiting));
}
