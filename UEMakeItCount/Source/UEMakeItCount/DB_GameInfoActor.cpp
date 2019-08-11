// Fill out your copyright notice in the Description page of Project Settings.


#include "DB_GameInfoActor.h"
#include "UnrealNetwork.h"
#include "Engine/Engine.h"
#include "UEMakeItCountCharacter.h"
#include "UEMakeItCountPlayerController.h"

// Sets default values
ADB_GameInfoActor::ADB_GameInfoActor()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

	bReplicates = true;
	bAlwaysRelevant = true;
}

// Called when the game starts or when spawned
void ADB_GameInfoActor::BeginPlay()
{
	Super::BeginPlay();	

	GEngine->AddOnScreenDebugMessage(-1, 10.f, FColor::Emerald, TEXT("GAME INFO SPAWNED ................. :D"));

}


void ADB_GameInfoActor::GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const
{
	Super::GetLifetimeReplicatedProps(OutLifetimeProps);

	DOREPLIFETIME(ADB_GameInfoActor, PlayerInMatch);
}

// Called every frame
void ADB_GameInfoActor::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

	// GEngine->AddOnScreenDebugMessage(-1, 10.f, FColor::Emerald, *FString::FromInt(PlayerInMatch.Num()));
}

void ADB_GameInfoActor::AddPlayerToSession_Implementation(APlayerController* Controller)
{
	PlayerInMatch.Add(Controller);
}

bool ADB_GameInfoActor::AddPlayerToSession_Validate(class APlayerController* Controller)
{
	return true;
}

void ADB_GameInfoActor::RemovePlayerToSession(APlayerController* Controller)
{
	PlayerInMatch.Remove(Controller);
}

