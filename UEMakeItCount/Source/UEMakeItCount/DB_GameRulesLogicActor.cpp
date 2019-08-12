// Fill out your copyright notice in the Description page of Project Settings.


#include "DB_GameRulesLogicActor.h"

// Sets default values
ADB_GameRulesLogicActor::ADB_GameRulesLogicActor()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
	bReplicates = true;
}

// Called when the game starts or when spawned
void ADB_GameRulesLogicActor::BeginPlay()
{
	Super::BeginPlay();
	
}

// Called every frame
void ADB_GameRulesLogicActor::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

