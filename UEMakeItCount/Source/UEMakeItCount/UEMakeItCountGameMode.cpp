// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.

#include "UEMakeItCountGameMode.h"
#include "UEMakeItCountPlayerController.h"
#include "UEMakeItCountCharacter.h"
#include "UObject/ConstructorHelpers.h"
#include "Engine/World.h"
#include "Engine/Engine.h"
#include "DB_GameInstance.h"
#include "DB_OnlineSubSystem.h"

AUEMakeItCountGameMode::AUEMakeItCountGameMode()
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

void AUEMakeItCountGameMode::BeginPlay()
{
// 	FVector Location(0.0f, 0.0f, 0.0f);
// 	FRotator Rotation(0.0f, 0.0f, 0.0f);
// 	FActorSpawnParameters SpawnInfo;
// 	GSObject = GetWorld()->SpawnActor<ADB_GameSparks>(Location, Rotation, SpawnInfo);

	bUseSeamlessTravel = true;

	Cast<UDB_GameInstance>(GetGameInstance())->OnlineSubsystem->World = GetWorld();

}

void AUEMakeItCountGameMode::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

	int i = 0;

	for (FConstPlayerControllerIterator It
		= GetWorld()->GetPlayerControllerIterator(); It; ++It)
	{
		AUEMakeItCountPlayerController* PC = Cast<AUEMakeItCountPlayerController>(*It);
		
		
		i++;
	}

	GEngine->AddOnScreenDebugMessage(-1, 0.1f, FColor::Cyan, FString::Printf(TEXT("Lala: %s"), *FString::FromInt(i)));
}
